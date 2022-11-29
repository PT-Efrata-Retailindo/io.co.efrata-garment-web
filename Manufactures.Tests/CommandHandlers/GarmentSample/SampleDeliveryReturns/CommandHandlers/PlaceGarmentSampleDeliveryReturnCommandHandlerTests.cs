using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleDeliveryReturns.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleDeliveryReturns.CommandHandlers
{
    public class PlaceGarmentSampleDeliveryReturnCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleDeliveryReturnRepository> _mockGarmentSampleDeliveryReturnRepository;
        private readonly Mock<IGarmentSampleDeliveryReturnItemRepository> _mockGarmentSampleDeliveryReturnItemRepository;
        private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;

        public PlaceGarmentSampleDeliveryReturnCommandHandlerTests()
        {
            _mockGarmentSampleDeliveryReturnRepository = CreateMock<IGarmentSampleDeliveryReturnRepository>();
            _mockGarmentSampleDeliveryReturnItemRepository = CreateMock<IGarmentSampleDeliveryReturnItemRepository>();
            _mockGarmentSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleDeliveryReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleDeliveryReturnItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);
        }

        private PlaceGarmentSampleDeliveryReturnCommandHandler CreatePlaceGarmentSampleDeliveryReturnCommandHandler()
        {
            return new PlaceGarmentSampleDeliveryReturnCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            PlaceGarmentSampleDeliveryReturnCommandHandler unitUnderTest = CreatePlaceGarmentSampleDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            _mockGarmentSampleDeliveryReturnRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentSampleDeliveryReturn>()
            {

            });

            _mockGarmentSampleDeliveryReturnItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleDeliveryReturnItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleDeliveryReturnItem>()));


            GarmentSamplePreparingItem garmentSamplePreparingItem = new GarmentSamplePreparingItem(id, 1, new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), "productCode", "productName", "designColor", 1, new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1), "uomUnit", "FABRIC", 1, 1, id, null);

            _mockGarmentSamplePreparingItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingItemReadModel, bool>>>()))
            .Returns(new List<GarmentSamplePreparingItem>()
            {
                    garmentSamplePreparingItem
            });

            _mockGarmentSamplePreparingItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentSamplePreparingItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparingItem>()));

            _mockGarmentSampleDeliveryReturnRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleDeliveryReturn>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleDeliveryReturn>()));

            GarmentSampleDeliveryReturn garmentSampleDeliveryReturn = new GarmentSampleDeliveryReturn(id, "drNo", "roNo", "article", 1, "unitDONo", 1, "preparingId", DateTimeOffset.Now, "returnType", new UnitDepartmentId(1), "unitCode", "unitName", new StorageId(1), "storageName", "storageCode", true);
            _mockGarmentSampleDeliveryReturnRepository
            .Setup(s => s.Query)
             .Returns(new List<GarmentSampleDeliveryReturnReadModel>()
             {
                garmentSampleDeliveryReturn.GetReadModel()
             }.AsQueryable());


            _MockStorage
             .Setup(x => x.Save())
             .Verifiable();

            PlaceGarmentSampleDeliveryReturnCommand request = new PlaceGarmentSampleDeliveryReturnCommand()
            {
                Article = "Article",
                DRNo = "DRNo",
                IsUsed = true,
                PreparingId = id.ToString(),
                ReturnDate = DateTimeOffset.Now,
                ReturnType = "ReturnType",
                RONo = "RONo",
                Storage = new Storage()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UENId = 1,
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UnitDOId = 1,
                UnitDONo = "UnitDONo",
                Items = new List<GarmentSampleDeliveryReturnItemValueObject>()
                {
                    new GarmentSampleDeliveryReturnItemValueObject()
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
