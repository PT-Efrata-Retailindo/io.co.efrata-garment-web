using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentCuttingIns.CommandHandlers;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingIns.ValueObjects;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.CuttingIn
{
    public class RemoveGarmentCuttingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockPreparingItemRepository;

        public RemoveGarmentCuttingInCommandHandlerTests()
        {
            _mockCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockPreparingItemRepository);
        }

        private RemoveGarmentCuttingInCommandHandler CreateRemoveGarmentCuttingInCommandHandler()
        {
            return new RemoveGarmentCuttingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid preparingItemGuid = Guid.NewGuid();
            RemoveGarmentCuttingInCommandHandler unitUnderTest = CreateRemoveGarmentCuttingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentCuttingInCommand RemoveGarmentCuttingInCommand = new RemoveGarmentCuttingInCommand(cuttingInGuid);

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>()
                {
                    new GarmentCuttingInReadModel(cuttingInGuid)
                }.AsQueryable());
            _mockCuttingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingInItem>()
                {
                    new GarmentCuttingInItem(Guid.Empty, Guid.Empty, Guid.Empty, 0, null, Guid.Empty,null)
                });
            _mockCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingInDetailReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingInDetail>()
                {
                    new GarmentCuttingInDetail(Guid.Empty, Guid.Empty, preparingItemGuid,Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, null, 0, new UomId(1), null, 0, new UomId(1), null, 0, 0,0,1,null)
                });

            _mockPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>
                {
                    new GarmentPreparingItemReadModel(preparingItemGuid)
                }.AsQueryable());

            _mockCuttingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingIn>()));
            _mockCuttingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingInItem>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingInDetail>()));
            _mockPreparingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentPreparingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentPreparingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentCuttingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
