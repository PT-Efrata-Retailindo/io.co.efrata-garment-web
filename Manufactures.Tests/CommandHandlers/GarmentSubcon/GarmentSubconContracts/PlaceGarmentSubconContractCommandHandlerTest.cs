using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconContracts
{
    public class PlaceGarmentSubconContractCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconContractRepository> _mockSubconContractRepository;
        private readonly Mock<IGarmentSubconContractItemRepository> _mockSubconContractItemRepository;
        public PlaceGarmentSubconContractCommandHandlerTest()
        {
            _mockSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();
            _mockSubconContractItemRepository = CreateMock<IGarmentSubconContractItemRepository>();

            _MockStorage.SetupStorage(_mockSubconContractRepository);
            _MockStorage.SetupStorage(_mockSubconContractItemRepository);
        }
        private PlaceGarmentSubconContractCommandHandler CreatePlaceGarmentSubconContractCommandHandler()
        {
            return new PlaceGarmentSubconContractCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid subconContractGuid = Guid.NewGuid();
            PlaceGarmentSubconContractCommandHandler unitUnderTest = CreatePlaceGarmentSubconContractCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconContractCommand placeGarmentSubconContractCommand = new PlaceGarmentSubconContractCommand()
            {
                AgreementNo = "test",
                BPJNo = "test",
                ContractNo = "test",
                ContractType = "test",
                DueDate = DateTimeOffset.Now,
                FinishedGoodType = "test",
                JobType = "test",
                Quantity = 1,
                Supplier = new Supplier
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                Buyer = new Buyer
                {
                    Id = 1,
                    Code = "Buyercode",
                    Name = "BuyerName"
                },
                Uom = new Uom
                {
                    Id = 1,
                    Unit = "unit"
                },
                SKEPNo = "no",
                AgreementDate = DateTimeOffset.Now,
                SubconCategory = "SUBCON",
                ContractDate = DateTimeOffset.Now,
                Items = new List<GarmentSubconContractItemValueObject>()
                {
                    new GarmentSubconContractItemValueObject
                    {
                        Uom=new Uom
                        {
                            Id=1,
                            Unit="unit"
                        },
                        Product=new Product
                        {
                            Id=1,
                            Name="name",
                            Code="code"
                        }
                    }
                }
            };
            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>().AsQueryable());
            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));

            _mockSubconContractItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContractItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContractItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSubconContractCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}