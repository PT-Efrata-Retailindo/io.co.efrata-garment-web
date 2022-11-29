using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentSubconCustomsIns.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconCustomsIns
{
    public class PlaceGarmentSubconCustomsInCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconCustomsInRepository> _mockSubconCustomsInRepository;
        private readonly Mock<IGarmentSubconCustomsInItemRepository> _mockSubconCustomsInItemRepository;
        
        public PlaceGarmentSubconCustomsInCommandHandlerTest()
        {
            _mockSubconCustomsInRepository = CreateMock<IGarmentSubconCustomsInRepository>();
            _mockSubconCustomsInItemRepository = CreateMock<IGarmentSubconCustomsInItemRepository>();

            _MockStorage.SetupStorage(_mockSubconCustomsInRepository);
            _MockStorage.SetupStorage(_mockSubconCustomsInItemRepository);
        }
        private PlaceGarmentSubconCustomsInCommandHandler CreatePlaceGarmentSubconCustomsInCommandHandler()
        {
            return new PlaceGarmentSubconCustomsInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SubconCustomsOutGuid = Guid.NewGuid();
            Guid SubconDLOutGuid = Guid.NewGuid();
            PlaceGarmentSubconCustomsInCommandHandler unitUnderTest = CreatePlaceGarmentSubconCustomsInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconCustomsInCommand placeGarmentSubconCustomsInCommand = new PlaceGarmentSubconCustomsInCommand()
            {
                BcDate = DateTimeOffset.Now,
                BcNo = "bcNo",
                BcType = "bcType",
                IsUsed = false,
                Remark = "Remark",
                SubconCategory = "Category",
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
                       RemainingQuantity = 1,
                       SubconCustomsInId = Guid.NewGuid(),
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
            _mockSubconCustomsInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCustomsIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCustomsIn>()));

            _mockSubconCustomsInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCustomsInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCustomsInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSubconCustomsInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
