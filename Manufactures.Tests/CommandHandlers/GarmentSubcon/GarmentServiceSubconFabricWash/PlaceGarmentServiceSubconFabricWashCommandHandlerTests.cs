using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconFabricWashes
{
    public class PlaceGarmentServiceSubconFabricWashCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockServiceSubconFabricWashRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashItemRepository> _mockServiceSubconFabricWashItemRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashDetailRepository> _mockServiceSubconFabricWashDetailRepository;

        public PlaceGarmentServiceSubconFabricWashCommandHandlerTests()
        {
            _mockServiceSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockServiceSubconFabricWashItemRepository = CreateMock<IGarmentServiceSubconFabricWashItemRepository>();
            _mockServiceSubconFabricWashDetailRepository = CreateMock<IGarmentServiceSubconFabricWashDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashDetailRepository);
        }

        private PlaceGarmentServiceSubconFabricWashCommandHandler CreatePlaceGarmentServiceSubconFabricWashCommandHandler()
        {
            return new PlaceGarmentServiceSubconFabricWashCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            PlaceGarmentServiceSubconFabricWashCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconFabricWashCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconFabricWashCommand placeGarmentServiceSubconFabricWashCommand = new PlaceGarmentServiceSubconFabricWashCommand()
            {
                ServiceSubconFabricWashDate = DateTimeOffset.Now,
                IsSave = true,
                Items = new List<GarmentServiceSubconFabricWashItemValueObject>
                {
                    new GarmentServiceSubconFabricWashItemValueObject
                    {
                        ExpenditureDate = DateTimeOffset.Now,
                        UnitExpenditureNo = "no",
                        UnitRequest = new UnitRequest(1, "UnitRequestCode", "UnitRequestName"),
                        UnitSender = new UnitSender(1, "UnitSenderCode", "UnitSenderName"),
                        Details = new List<GarmentServiceSubconFabricWashDetailValueObject>
                        {
                            new GarmentServiceSubconFabricWashDetailValueObject
                            {
                                Product = new Product(1, "ProductCode", "ProductName"),
                                Uom = new Uom(1, "UomUnit"),
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "DesignColor"
                            }
                        }
                    }
                },

            };
            
            _mockServiceSubconFabricWashRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashReadModel>().AsQueryable());


            _mockServiceSubconFabricWashRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWash>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWash>()));
            _mockServiceSubconFabricWashItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWashItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWashItem>()));
            _mockServiceSubconFabricWashDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWashDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWashDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconFabricWashCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
