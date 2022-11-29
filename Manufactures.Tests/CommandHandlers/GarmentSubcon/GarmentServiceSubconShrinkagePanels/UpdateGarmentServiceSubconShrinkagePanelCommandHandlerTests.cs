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
    public class UpdateGarmentServiceSubconShrinkagePanelCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockServiceSubconShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelItemRepository> _mockServiceSubconShrinkagePanelItemRepository;

        public UpdateGarmentServiceSubconShrinkagePanelCommandHandlerTests()
        {
            _mockServiceSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();
            _mockServiceSubconShrinkagePanelItemRepository = CreateMock<IGarmentServiceSubconShrinkagePanelItemRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelItemRepository);
        }

        private UpdateGarmentServiceSubconShrinkagePanelCommandHandler CreateUpdateGarmentServiceSubconShrinkagePanelCommandHandler()
        {
            return new UpdateGarmentServiceSubconShrinkagePanelCommandHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid serviceSubconShrinkagePanelGuid = Guid.NewGuid();
            Guid serviceSubconShrinkagePanelItemGuid = Guid.NewGuid();
            UpdateGarmentServiceSubconShrinkagePanelCommandHandler unitUnderTest = CreateUpdateGarmentServiceSubconShrinkagePanelCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentServiceSubconShrinkagePanelCommand UpdateGarmentServiceSubconShrinkagePanelCommand = new UpdateGarmentServiceSubconShrinkagePanelCommand()
            {
                Items = new List<GarmentServiceSubconShrinkagePanelItemValueObject>
                {
                    new GarmentServiceSubconShrinkagePanelItemValueObject
                    {
                        UnitExpenditureNo = "unitExpenditureNo",
                        ExpenditureDate = DateTimeOffset.Now,
                        UnitSender = new UnitSender(1, "UnitSenderCode", "UnitSenderName"),
                        UnitRequest = new UnitRequest(1, "UnitRequestCode", "UnitRequestName"),
                        Details = new List<GarmentServiceSubconShrinkagePanelDetailValueObject>
                        {
                            new GarmentServiceSubconShrinkagePanelDetailValueObject
                            {
                                Product = new Product(1, "ProductCode", "ProductName","roductRemark"),
                                Uom = new Uom(1, "UomUnit"),
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                            }
                        }

                    }
                },

            };

            UpdateGarmentServiceSubconShrinkagePanelCommand.SetIdentity(serviceSubconShrinkagePanelGuid);

            _mockServiceSubconShrinkagePanelRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>()
                {
                    new GarmentServiceSubconShrinkagePanelReadModel(serviceSubconShrinkagePanelGuid)
                }.AsQueryable());

            _mockServiceSubconShrinkagePanelRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanel>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanel>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentServiceSubconShrinkagePanelCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
