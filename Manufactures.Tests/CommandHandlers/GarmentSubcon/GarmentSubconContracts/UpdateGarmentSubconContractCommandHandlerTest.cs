using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.GarmentSubconContracts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using System.Linq;
using FluentAssertions;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ValueObjects;
using System.Linq.Expressions;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconContracts
{
    public class UpdateGarmentSubconContractCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconContractRepository> _mockSubconContractRepository;
        private readonly Mock<IGarmentSubconContractItemRepository> _mockSubconContractItemRepository;

        public UpdateGarmentSubconContractCommandHandlerTest()
        {
            _mockSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();
            _mockSubconContractItemRepository = CreateMock<IGarmentSubconContractItemRepository>();

            _MockStorage.SetupStorage(_mockSubconContractRepository);
            _MockStorage.SetupStorage(_mockSubconContractItemRepository);
        }
        private UpdateGarmentSubconContractCommandHandler CreateUpdateGarmentSubconContractCommandHandler()
        {
            return new UpdateGarmentSubconContractCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSubconContractCommandHandler unitUnderTest = CreateUpdateGarmentSubconContractCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconContractCommand UpdateGarmentSubconContractCommand = new UpdateGarmentSubconContractCommand()
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
            UpdateGarmentSubconContractCommand.SetIdentity(SubconContractGuid);

            _mockSubconContractRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractReadModel>()
                {
                    new GarmentSubconContractReadModel(SubconContractGuid)
                }.AsQueryable());


            _mockSubconContractItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconContractItem>()
                {
                    new GarmentSubconContractItem(Guid.Empty,SubconContractGuid,new ProductId(1),"code","name",1,new UomId(1),"unit", 1)
                });

            _mockSubconContractItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContractItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContractItem>()));

            _mockSubconContractRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconContract>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconContract>()));


            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconContractCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}