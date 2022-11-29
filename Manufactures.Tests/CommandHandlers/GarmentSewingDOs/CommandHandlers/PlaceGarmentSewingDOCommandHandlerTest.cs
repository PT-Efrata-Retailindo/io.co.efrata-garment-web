using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingDOs.CommandHandlers;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Commands;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingDOs.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSewingDOs.CommandHandlers
{
    public class PlaceGarmentSewingDOCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingDORepository> _mockGarmentSewingDORepository;
        private readonly Mock<IGarmentSewingDOItemRepository> _mockGarmentSewingDOItemRepository;
       

        public PlaceGarmentSewingDOCommandHandlerTest()
        {
            _mockGarmentSewingDORepository = CreateMock<IGarmentSewingDORepository>();
            _mockGarmentSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();
           
            _MockStorage.SetupStorage(_mockGarmentSewingDORepository);
            _MockStorage.SetupStorage(_mockGarmentSewingDOItemRepository);
           
        }

        private PlaceGarmentSewingDOCommandHandler CreatePlaceGarmentSewingDOCommandHandler()
        {
            return new PlaceGarmentSewingDOCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            PlaceGarmentSewingDOCommandHandler unitUnderTest = CreatePlaceGarmentSewingDOCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSewingDOItemRepository
              .Setup(s => s.Update(It.IsAny<GarmentSewingDOItem>()))
              .Returns(Task.FromResult(It.IsAny<GarmentSewingDOItem>()));

            GarmentSewingDO garmentSewingDO = new GarmentSewingDO(id, "sewingDONo", id, new UnitDepartmentId(1), "unitFromCode", "unitFromName", new UnitDepartmentId(1), "unitCode", "unitName", "roNo", "article", new GarmentComodityId(1), "comodityCode", "comodityName", DateTimeOffset.Now);
            _mockGarmentSewingDORepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSewingDOReadModel>
               {
                    garmentSewingDO.GetReadModel()
               }.AsQueryable());

            _mockGarmentSewingDORepository
              .Setup(s => s.Update(It.IsAny<GarmentSewingDO>()))
              .Returns(Task.FromResult(It.IsAny<GarmentSewingDO>()));

            _MockStorage
               .Setup(x => x.Save())
               .Verifiable();

            // Act
            PlaceGarmentSewingDOCommand request = new PlaceGarmentSewingDOCommand()
            {
                Article = "Article",
                Comodity =new GarmentComodity()
                {
                    Id=1,
                    Code ="Code",
                    Name ="Name"
                },
                CuttingOutId =id,
                RONo = "RONo",
                SewingDODate =DateTimeOffset.Now,
                SewingDONo = "SewingDONo",
                Unit =new UnitDepartment()
                {
                    Id =1,
                    Code ="Code",
                    Name ="Name"
                },
                UnitFrom =new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items =new List<GarmentSewingDOItemValueObject>()
                {
                    new GarmentSewingDOItemValueObject()
                    {
                        Color ="Color",
                        BasicPrice =1,
                        CuttingOutDetailId =id,
                        CuttingOutItemId =id,
                        DesignColor ="DesignColor",
                        Id =id,
                        Price =1,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        RemainingQuantity =1,
                        SewingDOId =id,
                        Size =new SizeValueObject()
                        {
                            Id =1,
                            Size ="1"
                        },
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit ="Unit"
                        }
                    }
                }
            };
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
        }
}
