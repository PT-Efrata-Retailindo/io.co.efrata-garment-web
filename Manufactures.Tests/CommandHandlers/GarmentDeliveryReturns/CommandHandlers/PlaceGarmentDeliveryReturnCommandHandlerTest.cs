using Barebone.Tests;
using Castle.DynamicProxy.Generators;
using FluentAssertions;
using Manufactures.Application.GarmentDeliveryReturns.CommandHandlers;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentDeliveryReturns.CommandHandlers
{
    public class PlaceGarmentDeliveryReturnCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentDeliveryReturnRepository> _mockGarmentDeliveryReturnRepository;
        private readonly Mock<IGarmentDeliveryReturnItemRepository> _mockGarmentDeliveryReturnItemRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

        public PlaceGarmentDeliveryReturnCommandHandlerTest()
        {
            _mockGarmentDeliveryReturnRepository = CreateMock<IGarmentDeliveryReturnRepository>();
            _mockGarmentDeliveryReturnItemRepository = CreateMock<IGarmentDeliveryReturnItemRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentDeliveryReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentDeliveryReturnItemRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);
        }

        private PlaceGarmentDeliveryReturnCommandHandler CreatePlaceGarmentDeliveryReturnCommandHandler()
        {
            return new PlaceGarmentDeliveryReturnCommandHandler(_MockStorage.Object);
        }



        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            PlaceGarmentDeliveryReturnCommandHandler unitUnderTest = CreatePlaceGarmentDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentDeliveryReturn>()
              {

              });

            _mockGarmentDeliveryReturnItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentDeliveryReturnItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentDeliveryReturnItem>()));


            GarmentPreparingItem garmentPreparingItem = new GarmentPreparingItem(id, 1, new Domain.GarmentPreparings.ValueObjects.ProductId(1), "productCode", "productName", "designColor", 1, new Domain.GarmentPreparings.ValueObjects.UomId(1), "uomUnit", "FABRIC", 1, 1, id,null, "fasilitas");

            _mockGarmentPreparingItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingItemReadModel, bool>>>()))
            .Returns(new List<GarmentPreparingItem>()
            {
                    garmentPreparingItem
            });

            _mockGarmentPreparingItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentPreparingItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentPreparingItem>()));

            _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Update(It.IsAny<GarmentDeliveryReturn>()))
            .Returns(Task.FromResult(It.IsAny<GarmentDeliveryReturn>()));

            GarmentDeliveryReturn garmentDeliveryReturn = new GarmentDeliveryReturn(id, "drNo", "roNo", "article", 1, "unitDONo", 1, "preparingId", DateTimeOffset.Now, "returnType", new UnitDepartmentId(1), "unitCode", "unitName", new StorageId(1), "storageName", "storageCode", true);
            _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Query)
             .Returns(new List<GarmentDeliveryReturnReadModel>()
             {
                garmentDeliveryReturn.GetReadModel()
             }.AsQueryable());


            _MockStorage
             .Setup(x => x.Save())
             .Verifiable();

            PlaceGarmentDeliveryReturnCommand request = new PlaceGarmentDeliveryReturnCommand()
            {
                Article = "Article",
                DRNo = "DRNo",
                IsUsed = true,
                PreparingId = id.ToString(),
                ReturnDate = DateTimeOffset.Now,
                ReturnType = "ReturnType",
                RONo = "RONo",
                Storage = new Domain.GarmentDeliveryReturns.ValueObjects.Storage()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UENId = 1,
                Unit = new Domain.GarmentDeliveryReturns.ValueObjects.UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UnitDOId = 1,
                UnitDONo = "UnitDONo",
                Items = new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject()
                    {
                        Id =id,
                        DesignColor ="DesignColor",
                        DRId =id,
                        IsSave =true,
                        PreparingItemId =id.ToString(),
                        Product =new Product()
                        {
                            Id =1,
                            Code ="Code",
                            Name ="FABRIC"
                        },
                        RONo ="RONo",
                        Quantity =1,
                        QuantityUENItem =1,
                        RemainingQuantityPreparingItem =1,
                        UENItemId =1,
                        UnitDOItemId=1,
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit ="Unit"
                        }

                    }
                }
            };
            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();


        }


    }
}
