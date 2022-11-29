using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentPreparings.CommandHandlers;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentPreparings
{
    public class UpdateGarmentPreparingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentPreparingRepository> _mockPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockPreparingItemRepository;

        public UpdateGarmentPreparingCommandHandlerTests()
        {
            _mockPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockPreparingRepository);
            _MockStorage.SetupStorage(_mockPreparingItemRepository);
        }

        private UpdateGarmentPreparingCommandHandler CreateUpdateGarmentPreparingCommandHandler()
        {
            return new UpdateGarmentPreparingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid preparingGuid = Guid.NewGuid();
            UpdateGarmentPreparingCommandHandler unitUnderTest = CreateUpdateGarmentPreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentPreparingCommand UpdateGarmentPreparingCommand = new UpdateGarmentPreparingCommand()
            {
                UENId = 1,
                UENNo = "test",
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Test",
                ProcessDate = DateTimeOffset.Now,
                IsCuttingIn = false,
                Items = new List<GarmentPreparingItemValueObject>
                {
                    new GarmentPreparingItemValueObject
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

            UpdateGarmentPreparingCommand.SetId(preparingGuid);

            _mockPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(preparingGuid, 1, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, true,new Domain.Shared.ValueObjects.BuyerId(1),
                    null,null)
                });

            _mockPreparingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingItemReadModel, bool>>>()))
                .Returns(new List<GarmentPreparingItem>()
                {
                    new GarmentPreparingItem(Guid.Empty, 0, new ProductId(1), null, null, null, 0, new UomId(1), null, null, 0, 0, Guid.Empty,null,"fasilitas")
                });

            _mockPreparingRepository
                .Setup(s => s.Update(It.IsAny<GarmentPreparing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentPreparing>()));

            _mockPreparingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentPreparingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentPreparingItem>()));
            
            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentPreparingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_PreparingNotFound_Error()
        {
            // Arrange
            Guid preparingGuid = Guid.NewGuid();
            UpdateGarmentPreparingCommandHandler unitUnderTest = CreateUpdateGarmentPreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentPreparingCommand UpdateGarmentPreparingCommand = new UpdateGarmentPreparingCommand();

            UpdateGarmentPreparingCommand.SetId(preparingGuid);

            _mockPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>());

            // Act
            var result = await Assert.ThrowsAnyAsync<Exception>(async () => await unitUnderTest.Handle(UpdateGarmentPreparingCommand, cancellationToken));

            // Assert
            result.Should().NotBeNull();
        }
    }
}