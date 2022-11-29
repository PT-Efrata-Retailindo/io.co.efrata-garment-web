using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentSubconCustomsIns.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconCustomsIns
{
    public class UpdateGarmentSubconCustomsInCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconCustomsInRepository> _mockSubconCustomsInRepository;
        private readonly Mock<IGarmentSubconCustomsInItemRepository> _mockSubconCustomsInItemRepository;
        
        public UpdateGarmentSubconCustomsInCommandHandlerTest()
        {
            _mockSubconCustomsInRepository = CreateMock<IGarmentSubconCustomsInRepository>();
            _mockSubconCustomsInItemRepository = CreateMock<IGarmentSubconCustomsInItemRepository>();
            
            _MockStorage.SetupStorage(_mockSubconCustomsInRepository);
            _MockStorage.SetupStorage(_mockSubconCustomsInItemRepository);
        }

        private UpdateGarmentSubconCustomsInCommandHandler CreateUpdateGarmentSubconCustomsInCommandHandler()
        {
            return new UpdateGarmentSubconCustomsInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SubconCustomsInGuid = Guid.NewGuid();
            UpdateGarmentSubconCustomsInCommandHandler unitUnderTest = CreateUpdateGarmentSubconCustomsInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconCustomsInCommand updateGarmentSubconCustomsInCommand = new UpdateGarmentSubconCustomsInCommand()
            {
                BcDate = DateTimeOffset.Now,
                BcNo = "no",
                BcType = "type",
                IsUsed = false,
                Remark = "remark",
                SubconContractId = Guid.NewGuid(),
                SubconContractNo = "no", 
                Supplier = new Supplier
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                SubconType = "type",
                Items = new List<GarmentSubconCustomsInItemValueObject>()
                {
                    new GarmentSubconCustomsInItemValueObject
                    {
                       Quantity=1,
                       DoId = 1,
                       DoNo = "no",
                       Id = Guid.Empty,
                       RemainingQuantity = 1,
                       SubconCustomsInId = SubconCustomsInGuid,
                       Supplier = new Supplier
                       {
                           Code = "test",
                           Id = 1,
                           Name = "test"
                       },
                       TotalQty = 1,
                    }
                }

            };
            updateGarmentSubconCustomsInCommand.SetIdentity(SubconCustomsInGuid);

            _mockSubconCustomsInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCustomsInReadModel>()
                {
                    new GarmentSubconCustomsInReadModel(SubconCustomsInGuid)
                }.AsQueryable());


            _mockSubconCustomsInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCustomsInItem>()
                {
                    new GarmentSubconCustomsInItem(Guid.Empty, SubconCustomsInGuid, new SupplierId(1), "code", "name", 1, "no", 1)
                }); ;

            _mockSubconCustomsInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCustomsInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCustomsInItem>()));

            _mockSubconCustomsInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCustomsIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCustomsIn>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(updateGarmentSubconCustomsInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
