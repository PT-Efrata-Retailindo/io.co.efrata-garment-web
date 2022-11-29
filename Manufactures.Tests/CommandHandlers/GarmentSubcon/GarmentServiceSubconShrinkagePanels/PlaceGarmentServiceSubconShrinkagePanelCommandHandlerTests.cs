using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconShrinkagePanels
{
    public class PlaceGarmentServiceSubconShrinkagePanelCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockServiceSubconShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelItemRepository> _mockServiceSubconShrinkagePanelItemRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelDetailRepository> _mockServiceSubconShrinkagePanelDetailRepository;

        public PlaceGarmentServiceSubconShrinkagePanelCommandHandlerTests()
        {
            _mockServiceSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();
            _mockServiceSubconShrinkagePanelItemRepository = CreateMock<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _mockServiceSubconShrinkagePanelDetailRepository = CreateMock<IGarmentServiceSubconShrinkagePanelDetailRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelDetailRepository);
        }

        private PlaceGarmentServiceSubconShrinkagePanelCommandHandler CreatePlaceGarmentServiceSubconShrinkagePanelCommandHandler()
        {
            return new PlaceGarmentServiceSubconShrinkagePanelCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            PlaceGarmentServiceSubconShrinkagePanelCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconShrinkagePanelCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconShrinkagePanelCommand placeGarmentServiceSubconShrinkagePanelCommand = new PlaceGarmentServiceSubconShrinkagePanelCommand()
            {
                ServiceSubconShrinkagePanelDate = DateTimeOffset.Now,
                IsSave = true,
                Items = new List<GarmentServiceSubconShrinkagePanelItemValueObject>
                {
                    new GarmentServiceSubconShrinkagePanelItemValueObject
                    {
                        ExpenditureDate = DateTimeOffset.Now,
                        UnitExpenditureNo = "no",
                        UnitRequest = new UnitRequest(1, "UnitRequestCode", "UnitRequestName"),
                        UnitSender = new UnitSender(1, "UnitSenderCode", "UnitSenderName"),
                        Details = new List<GarmentServiceSubconShrinkagePanelDetailValueObject>
                        {
                            new GarmentServiceSubconShrinkagePanelDetailValueObject
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
            
            _mockServiceSubconShrinkagePanelRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>().AsQueryable());


            _mockServiceSubconShrinkagePanelRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanel>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanel>()));
            _mockServiceSubconShrinkagePanelItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanelItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanelItem>()));
            _mockServiceSubconShrinkagePanelDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanelDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanelDetail>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconShrinkagePanelCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
