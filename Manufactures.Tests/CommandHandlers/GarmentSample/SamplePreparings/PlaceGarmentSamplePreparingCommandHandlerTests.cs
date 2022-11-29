using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SamplePreparings.CommandHandlers;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SamplePreparings
{
    public class PlaceGarmentSamplePreparingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSamplePreparingRepository> _mockSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockSamplePreparingItemRepository;

        public PlaceGarmentSamplePreparingCommandHandlerTests()
        {
            _mockSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockSamplePreparingItemRepository);
        }

        private PlaceGarmentSamplePreparingCommandHandler CreatePlaceGarmentSamplePreparingCommandHandler()
        {
            return new PlaceGarmentSamplePreparingCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid preparingItemGuid = Guid.NewGuid();
            PlaceGarmentSamplePreparingCommandHandler unitUnderTest = CreatePlaceGarmentSamplePreparingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSamplePreparingCommand placeGarmentSamplePreparingCommand = new PlaceGarmentSamplePreparingCommand()
            {
                UENId = 1,
                UENNo = "test",
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Test",
                ProcessDate = DateTimeOffset.Now,
                IsCuttingIn = false,
                Buyer = new Domain.Shared.ValueObjects.Buyer(1, "buy", "buy"),
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
                        ROSource="ro"
                    }
                },
            };

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
            var result = await unitUnderTest.Handle(placeGarmentSamplePreparingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
