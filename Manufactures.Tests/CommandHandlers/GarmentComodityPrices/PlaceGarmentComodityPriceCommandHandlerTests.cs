using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentComodityPrices.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Commands;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentComodityPrices.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentComodityPrices
{
    public class PlaceGarmentComodityPriceCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public PlaceGarmentComodityPriceCommandHandlerTests()
        {
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockComodityPriceRepository);
        }

        private PlaceGarmentComodityPriceCommandHandler CreatePlaceGarmentComodityPriceCommandHandler()
        {
            return new PlaceGarmentComodityPriceCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid preparingItemGuid = Guid.NewGuid();
            PlaceGarmentComodityPriceCommandHandler unitUnderTest = CreatePlaceGarmentComodityPriceCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
			PlaceGarmentComodityPriceCommand placeGarmentCuttingInCommand = new PlaceGarmentComodityPriceCommand()
			{
				Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
				Date = DateTimeOffset.Now,
				Items = new List<GarmentComodityPriceItemValueObject>
				{
					new GarmentComodityPriceItemValueObject
					{
						Comodity=new GarmentComodity(1, "comoCode", "ComoName"),
						Unit=new UnitDepartment(1, "UnitCode", "UnitName"),
						Date=DateTimeOffset.Now,
						Price=1000,
						NewPrice=0,
						IsValid=true

                    }
                },

            };

            _mockComodityPriceRepository
                .Setup(s => s.Update(It.IsAny<GarmentComodityPrice>()))
                .Returns(Task.FromResult(It.IsAny<GarmentComodityPrice>()));
            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentCuttingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
