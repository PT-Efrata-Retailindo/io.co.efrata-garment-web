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
    public class PlaceGarmentPreparingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentPreparingRepository> _mockPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockPreparingItemRepository;

        public PlaceGarmentPreparingCommandHandlerTests()
        {
            _mockPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockPreparingRepository);
            _MockStorage.SetupStorage(_mockPreparingItemRepository);
        }

        private PlaceGarmentPreparingCommandHandler CreatePlaceGarmentPreparingCommandHandler()
        {
            return new PlaceGarmentPreparingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid preparingItemGuid = Guid.NewGuid();
            PlaceGarmentPreparingCommandHandler unitUnderTest = CreatePlaceGarmentPreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentPreparingCommand placeGarmentPreparingCommand = new PlaceGarmentPreparingCommand()
            {
                UENId = 1,
                UENNo = "test",
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Test",
                ProcessDate = DateTimeOffset.Now,
                IsCuttingIn = false,
                Buyer = new Domain.Shared.ValueObjects.Buyer(1, "buy", "buy"),
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
                        ROSource="ro"
                    }
                },
            };

            //_mockPreparingRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
            //    .Returns(new List<GarmentPreparing>());

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
            var result = await unitUnderTest.Handle(placeGarmentPreparingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}