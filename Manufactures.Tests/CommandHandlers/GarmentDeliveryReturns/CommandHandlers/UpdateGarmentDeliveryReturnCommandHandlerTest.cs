﻿using Barebone.Tests;
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
    public class UpdateGarmentDeliveryReturnCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentDeliveryReturnRepository> _mockGarmentDeliveryReturnRepository;
        private readonly Mock<IGarmentDeliveryReturnItemRepository> _mockGarmentDeliveryReturnItemRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

        public UpdateGarmentDeliveryReturnCommandHandlerTest()
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

        private UpdateGarmentDeliveryReturnCommandHandler CreateUpdateGarmentDeliveryReturnCommandHandler()
        {
            return new UpdateGarmentDeliveryReturnCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_when_item_save_is_true()
        {
             // Arrange
             Guid id = Guid.NewGuid();
             UpdateGarmentDeliveryReturnCommandHandler unitUnderTest = CreateUpdateGarmentDeliveryReturnCommandHandler();
             CancellationToken cancellationToken = CancellationToken.None;

             var garmentDeliveryReturn = new GarmentDeliveryReturn(id, "drNo", "roNo", "article", 1, "unitDONo", 1, "preparingId", DateTimeOffset.Now, "returnType", new UnitDepartmentId(1), "unitCode", "unitName", new StorageId(1), "storageName", "storageCode", true);
        
             _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentDeliveryReturn>()
             {
              garmentDeliveryReturn
             });

            _mockGarmentDeliveryReturnItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnItemReadModel, bool>>>()))
            .Returns(new List<GarmentDeliveryReturnItem>()
             {
               new GarmentDeliveryReturnItem(id,id,1,1,"preparingItemId",new ProductId(1),"productCode","FABRIC","designColor","roNo",1,new UomId(1),"uomUnit","","","","","")
             });

             GarmentPreparingItem garmentDeliveryReturnItem = new GarmentPreparingItem(id, 1, new Domain.GarmentPreparings.ValueObjects.ProductId(1), "productCode", "productName", "designColor", 1, new Domain.GarmentPreparings.ValueObjects.UomId(1), "uomUnit", "fabricType", 1, 1, id,null, "fasilitas");
             _mockGarmentPreparingItemRepository
             .Setup(s => s.Query)
             .Returns(new List<GarmentPreparingItemReadModel>()
             {
                garmentDeliveryReturnItem.GetReadModel()
             }.AsQueryable());

            _mockGarmentDeliveryReturnItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentDeliveryReturnItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentDeliveryReturnItem>()));

            _mockGarmentPreparingItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentPreparingItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentPreparingItem>()));

            _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Update(It.IsAny<GarmentDeliveryReturn>()))
            .Returns(Task.FromResult(It.IsAny<GarmentDeliveryReturn>()));

            _MockStorage
            .Setup(x => x.Save())
            .Verifiable();

            UpdateGarmentDeliveryReturnCommand request = new UpdateGarmentDeliveryReturnCommand()
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
                Items = new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject()
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
            UpdateGarmentDeliveryReturnCommandHandler unitUnderTest = CreateUpdateGarmentDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            var garmentDeliveryReturn = new GarmentDeliveryReturn(id, "drNo", "roNo", "article", 1, "unitDONo", 1, "preparingId", DateTimeOffset.Now, "returnType", new UnitDepartmentId(1), "unitCode", "unitName", new StorageId(1), "storageName", "storageCode", true);

            _mockGarmentDeliveryReturnRepository
           .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
           .Returns(new List<GarmentDeliveryReturn>()
            {
              garmentDeliveryReturn
            });

            _mockGarmentDeliveryReturnItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnItemReadModel, bool>>>()))
            .Returns(new List<GarmentDeliveryReturnItem>()
             {
               new GarmentDeliveryReturnItem(id,id,1,1,"preparingItemId",new ProductId(1),"productCode","FABRIC","designColor","roNo",1,new UomId(1),"uomUnit","","","","","")
             });

            GarmentPreparingItem garmentDeliveryReturnItem = new GarmentPreparingItem(id, 1, new Domain.GarmentPreparings.ValueObjects.ProductId(1), "productCode", "productName", "designColor", 1, new Domain.GarmentPreparings.ValueObjects.UomId(1), "uomUnit", "fabricType", 1, 1, id,null, "fasilitas");
            _mockGarmentPreparingItemRepository
            .Setup(s => s.Query)
            .Returns(new List<GarmentPreparingItemReadModel>()
            {
                garmentDeliveryReturnItem.GetReadModel()
            }.AsQueryable());

            _mockGarmentDeliveryReturnItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentDeliveryReturnItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentDeliveryReturnItem>()));

            _mockGarmentPreparingItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentPreparingItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentPreparingItem>()));

            _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Update(It.IsAny<GarmentDeliveryReturn>()))
            .Returns(Task.FromResult(It.IsAny<GarmentDeliveryReturn>()));

            _MockStorage
            .Setup(x => x.Save())
            .Verifiable();

            UpdateGarmentDeliveryReturnCommand request = new UpdateGarmentDeliveryReturnCommand()
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
                Items = new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject()
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
            UpdateGarmentDeliveryReturnCommandHandler unitUnderTest = CreateUpdateGarmentDeliveryReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentDeliveryReturnRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
            .Returns(new List<GarmentDeliveryReturn>()
            {

            });

            UpdateGarmentDeliveryReturnCommand request = new UpdateGarmentDeliveryReturnCommand()
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
                Items = new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject()
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
