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

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconFabricWashs
{
    public class UpdateGarmentServiceSubconFabricWashCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockServiceSubconFabricWashRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashItemRepository> _mockServiceSubconFabricWashItemRepository;

        public UpdateGarmentServiceSubconFabricWashCommandHandlerTests()
        {
            _mockServiceSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockServiceSubconFabricWashItemRepository = CreateMock<IGarmentServiceSubconFabricWashItemRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashItemRepository);
        }

        private UpdateGarmentServiceSubconFabricWashCommandHandler CreateUpdateGarmentServiceSubconFabricWashCommandHandler()
        {
            return new UpdateGarmentServiceSubconFabricWashCommandHandler(_MockStorage.Object);
        }

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    Guid serviceSubconFabricWashGuid = Guid.NewGuid();
        //    Guid serviceSubconFabricWashItemGuid = Guid.NewGuid();
        //    UpdateGarmentServiceSubconFabricWashCommandHandler unitUnderTest = CreateUpdateGarmentServiceSubconFabricWashCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    UpdateGarmentServiceSubconFabricWashCommand UpdateGarmentServiceSubconFabricWashCommand = new UpdateGarmentServiceSubconFabricWashCommand()
        //    {
        //        Items = new List<GarmentServiceSubconFabricWashItemValueObject>
        //        {
        //            new GarmentServiceSubconFabricWashItemValueObject
        //            {
        //                UnitExpenditureNo = "unitExpenditureNo",
        //                ExpenditureDate = DateTimeOffset.Now,
        //                UnitSender = new UnitSender(1, "UnitSenderCode", "UnitSenderName"),
        //                UnitRequest = new UnitRequest(1, "UnitRequestCode", "UnitRequestName"),
        //                Details = new List<GarmentServiceSubconFabricWashDetailValueObject>
        //                {
        //                    new GarmentServiceSubconFabricWashDetailValueObject
        //                    {
        //                        Product = new Product(1, "ProductCode", "ProductName","roductRemark"),
        //                        Uom = new Uom(1, "UomUnit"),
        //                        IsSave=true,
        //                        Quantity=1,
        //                        DesignColor= "ColorD",
        //                    }
        //                }

        //            }
        //        },

        //    };

        //    UpdateGarmentServiceSubconFabricWashCommand.SetIdentity(serviceSubconFabricWashGuid);

        //    _mockServiceSubconFabricWashRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSubconFabricWashReadModel>()
        //        {
        //            new GarmentServiceSubconFabricWashReadModel(serviceSubconFabricWashGuid)
        //        }.AsQueryable());

        //    _mockServiceSubconFabricWashRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWash>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWash>()));

        //    _MockStorage
        //        .Setup(x => x.Save())
        //        .Verifiable();

        //    // Act
        //    var result = await unitUnderTest.Handle(UpdateGarmentServiceSubconFabricWashCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}
    }
}
