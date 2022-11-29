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
    public class UpdateGarmentSampleDeliveryReturnCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleDeliveryReturnRepository> _mockGarmentSampleDeliveryReturnRepository;
        private readonly Mock<IGarmentSampleDeliveryReturnItemRepository> _mockGarmentSampleDeliveryReturnItemRepository;
        private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;

        public UpdateGarmentSampleDeliveryReturnCommandHandlerTests()
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

        private UpdateGarmentSampleDeliveryReturnCommandHandler CreateUpdateGarmentSampleDeliveryReturnCommandHandler()
        {
            return new UpdateGarmentSampleDeliveryReturnCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_when_item_save_is_true()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateGarmentSampleDeliveryReturnCommandHandler unitUnderTest = CreateUpdateGarmentSampleDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            var garmentSampleDeliveryReturn = new GarmentSampleDeliveryReturn(id, "drNo", "roNo", "article", 1, "unitDONo", 1, "preparingId", DateTimeOffset.Now, "returnType", new UnitDepartmentId(1), "unitCode", "unitName", new StorageId(1), "storageName", "storageCode", true);

            _mockGarmentSampleDeliveryReturnRepository
           .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
           .Returns(new List<GarmentSampleDeliveryReturn>()
            {
              garmentSampleDeliveryReturn
            });

            _mockGarmentSampleDeliveryReturnItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnItemReadModel, bool>>>()))
            .Returns(new List<GarmentSampleDeliveryReturnItem>()
             {
               new GarmentSampleDeliveryReturnItem(id,id,1,1,"preparingItemId",new ProductId(1),"productCode","FABRIC","designColor","roNo",1,new UomId(1),"uomUnit")
             });

            GarmentSamplePreparingItem garmentSamplePreparingItem = new GarmentSamplePreparingItem(id, 1, new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), "productCode", "productName", "designColor", 1, new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1), "uomUnit", "fabricType", 1, 1, id, null);
            _mockGarmentSamplePreparingItemRepository
            .Setup(s => s.Query)
            .Returns(new List<GarmentSamplePreparingItemReadModel>()
            {
                garmentSamplePreparingItem.GetReadModel()
            }.AsQueryable());

            _mockGarmentSampleDeliveryReturnItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleDeliveryReturnItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleDeliveryReturnItem>()));

            _mockGarmentSamplePreparingItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentSamplePreparingItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparingItem>()));

            _mockGarmentSampleDeliveryReturnRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleDeliveryReturn>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleDeliveryReturn>()));

            _MockStorage
            .Setup(x => x.Save())
            .Verifiable();

            UpdateGarmentSampleDeliveryReturnCommand request = new UpdateGarmentSampleDeliveryReturnCommand()
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
                UnitDOId = 1,
                UnitDONo = "UnitDONo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSampleDeliveryReturnItemValueObject>()
                {
                    new GarmentSampleDeliveryReturnItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        DRId =id,
                        Id =id,
                        IsSave =true,
                        PreparingItemId =id.ToString(),
                        Product =new Product()
                        {
                            Id =1,
                            Code ="Code",
                            Name ="FABRIC"
                        },
                        Quantity =1,
                        QuantityUENItem =1,
                        RemainingQuantityPreparingItem =1,
                        RONo ="RONo",
                        UENItemId =1,
                        UnitDOItemId =1,
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit ="Unit"
                        },

                    }
                }

            };
            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_when_item_save_is_false()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateGarmentSampleDeliveryReturnCommandHandler unitUnderTest = CreateUpdateGarmentSampleDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            var garmentSampleDeliveryReturn = new GarmentSampleDeliveryReturn(id, "drNo", "roNo", "article", 1, "unitDONo", 1, "preparingId", DateTimeOffset.Now, "returnType", new UnitDepartmentId(1), "unitCode", "unitName", new StorageId(1), "storageName", "storageCode", true);

            _mockGarmentSampleDeliveryReturnRepository
           .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
           .Returns(new List<GarmentSampleDeliveryReturn>()
            {
              garmentSampleDeliveryReturn
            });

            _mockGarmentSampleDeliveryReturnItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnItemReadModel, bool>>>()))
            .Returns(new List<GarmentSampleDeliveryReturnItem>()
             {
               new GarmentSampleDeliveryReturnItem(id,id,1,1,"preparingItemId",new ProductId(1),"productCode","FABRIC","designColor","roNo",1,new UomId(1),"uomUnit")
             });

            GarmentSamplePreparingItem garmentSamplePreparingItem = new GarmentSamplePreparingItem(id, 1, new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), "productCode", "productName", "designColor", 1, new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1), "uomUnit", "fabricType", 1, 1, id, null);
            _mockGarmentSamplePreparingItemRepository
            .Setup(s => s.Query)
            .Returns(new List<GarmentSamplePreparingItemReadModel>()
            {
                garmentSamplePreparingItem.GetReadModel()
            }.AsQueryable());

            _mockGarmentSampleDeliveryReturnItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleDeliveryReturnItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleDeliveryReturnItem>()));

            _mockGarmentSamplePreparingItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentSamplePreparingItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparingItem>()));

            _mockGarmentSampleDeliveryReturnRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleDeliveryReturn>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleDeliveryReturn>()));

            _MockStorage
            .Setup(x => x.Save())
            .Verifiable();

            UpdateGarmentSampleDeliveryReturnCommand request = new UpdateGarmentSampleDeliveryReturnCommand()
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
                UnitDOId = 1,
                UnitDONo = "UnitDONo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSampleDeliveryReturnItemValueObject>()
                {
                    new GarmentSampleDeliveryReturnItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        DRId =id,
                        Id =id,
                        IsSave =false,
                        PreparingItemId =id.ToString(),
                        Product =new Product()
                        {
                            Id =1,
                            Code ="Code",
                            Name ="FABRIC"
                        },
                        Quantity =1,
                        QuantityUENItem =1,
                        RemainingQuantityPreparingItem =1,
                        RONo ="RONo",
                        UENItemId =1,
                        UnitDOItemId =1,
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit ="Unit"
                        },

                    }
                }

            };
            // Act
            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_Throws_ValidationException()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateGarmentSampleDeliveryReturnCommandHandler unitUnderTest = CreateUpdateGarmentSampleDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSampleDeliveryReturnRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentSampleDeliveryReturn>()
            {

            });

            UpdateGarmentSampleDeliveryReturnCommand request = new UpdateGarmentSampleDeliveryReturnCommand()
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
                UnitDOId = 1,
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSampleDeliveryReturnItemValueObject>()
                {
                    new GarmentSampleDeliveryReturnItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        DRId =id,
                        Id =id,
                        IsSave =true,
                        PreparingItemId =id.ToString(),
                        Product =new Product()
                        {
                            Id =1,
                            Code ="Code",
                            Name ="Name"
                        },
                        Quantity =1,
                        QuantityUENItem =1,
                        RemainingQuantityPreparingItem =1,
                        RONo ="RONo",
                        UENItemId =1,
                        UnitDOItemId =1,
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit ="Unit"
                        }
                    }
                }

            };
            // Act
            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => unitUnderTest.Handle(request, cancellationToken));
        }
    }
}
