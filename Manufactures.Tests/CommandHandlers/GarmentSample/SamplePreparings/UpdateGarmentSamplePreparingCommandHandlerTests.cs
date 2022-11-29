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
    public class UpdateGarmentSamplePreparingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSamplePreparingRepository> _mockSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockSamplePreparingItemRepository;

        public UpdateGarmentSamplePreparingCommandHandlerTests()
        {
            _mockSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockSamplePreparingItemRepository);
        }

        private UpdateGarmentSamplePreparingCommandHandler CreateUpdateGarmentSamplePreparingCommandHandler()
        {
            return new UpdateGarmentSamplePreparingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid preparingGuid = Guid.NewGuid();
            UpdateGarmentSamplePreparingCommandHandler unitUnderTest = CreateUpdateGarmentSamplePreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSamplePreparingCommand UpdateGarmentSamplePreparingCommand = new UpdateGarmentSamplePreparingCommand()
            {
                UENId = 1,
                UENNo = "test",
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Test",
                ProcessDate = DateTimeOffset.Now,
                IsCuttingIn = false,
                Items = new List<GarmentSamplePreparingItemValueObject>
                {
                    new GarmentSamplePreparingItemValueObject
                    {
                        UENItemId = 1,
                        Product = new Product(1, "ProductCode", "ProductName"),
                        DesignColor = "test",
                        Quantity = 1,
                        Uom = new Uom(1, "UomUnit"),
                        FabricType = "Test",
                        RemainingQuantity = 1,
                        BasicPrice = 1,
                    }
                },
            };

            UpdateGarmentSamplePreparingCommand.SetId(preparingGuid);

            _mockSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>()
                {
                    new GarmentSamplePreparing(preparingGuid, 1, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, true,new Domain.Shared.ValueObjects.BuyerId(1),
                    null,null)
                });

            _mockSamplePreparingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingItemReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparingItem>()
                {
                    new GarmentSamplePreparingItem(Guid.Empty, 0, new ProductId(1), null, null, null, 0, new UomId(1), null, null, 0, 0, Guid.Empty,null)
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
            var result = await unitUnderTest.Handle(UpdateGarmentSamplePreparingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_PreparingNotFound_Error()
        {
            // Arrange
            Guid preparingGuid = Guid.NewGuid();
            UpdateGarmentSamplePreparingCommandHandler unitUnderTest = CreateUpdateGarmentSamplePreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSamplePreparingCommand UpdateGarmentSamplePreparingCommand = new UpdateGarmentSamplePreparingCommand();

            UpdateGarmentSamplePreparingCommand.SetId(preparingGuid);

            _mockSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>());

            // Act
            var result = await Assert.ThrowsAnyAsync<Exception>(async () => await unitUnderTest.Handle(UpdateGarmentSamplePreparingCommand, cancellationToken));

            // Assert
            result.Should().NotBeNull();
        }
    }
}
