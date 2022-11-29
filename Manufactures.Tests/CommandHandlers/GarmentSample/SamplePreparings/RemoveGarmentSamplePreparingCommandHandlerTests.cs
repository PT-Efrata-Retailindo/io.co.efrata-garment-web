using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SamplePreparings.CommandHandlers;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SamplePreparings
{
    public class RemoveGarmentSamplePreparingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSamplePreparingRepository> _mockSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockSamplePreparingItemRepository;

        public RemoveGarmentSamplePreparingCommandHandlerTests()
        {
            _mockSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockSamplePreparingItemRepository);
        }

        private RemoveGarmentSamplePreparingCommandHandler CreateRemoveGarmentSamplePreparingCommandHandler()
        {
            return new RemoveGarmentSamplePreparingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid preparingGuid = Guid.NewGuid();
            Guid preparingItemGuid = Guid.NewGuid();
            RemoveGarmentSamplePreparingCommandHandler unitUnderTest = CreateRemoveGarmentSamplePreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSamplePreparingCommand RemoveGarmentSamplePreparingCommand = new RemoveGarmentSamplePreparingCommand();
            RemoveGarmentSamplePreparingCommand.SetId(preparingGuid);

            _mockSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>()
                {
                    new GarmentSamplePreparing(preparingGuid, 1, "UENNo", new UnitDepartmentId(1), "UnitCode", "UnitName", DateTimeOffset.Now, "RONo", "Article", true,new Domain.Shared.ValueObjects.BuyerId(1), null,null)
                });

            _mockSamplePreparingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingItemReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparingItem>()
                {
                    new GarmentSamplePreparingItem(preparingItemGuid, 0, new ProductId(1), null, null, null, 0, new UomId(1), null, null, 0, 0, Guid.Empty,null)
                });

            _mockSamplePreparingRepository
                .Setup(s => s.Update(It.IsAny<GarmentSamplePreparing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparing>()));

            _mockSamplePreparingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSamplePreparingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSamplePreparingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_PreparingNotFound_Error()
        {
            // Arrange
            Guid preparingGuid = Guid.NewGuid();
            RemoveGarmentSamplePreparingCommandHandler unitUnderTest = CreateRemoveGarmentSamplePreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSamplePreparingCommand RemoveGarmentSamplePreparingCommand = new RemoveGarmentSamplePreparingCommand();
            RemoveGarmentSamplePreparingCommand.SetId(preparingGuid);

            _mockSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>());

            // Act
            var result = await Assert.ThrowsAnyAsync<Exception>(async () => await unitUnderTest.Handle(RemoveGarmentSamplePreparingCommand, cancellationToken));

            // Assert
            result.Should().NotBeNull();
        }
    }
}
