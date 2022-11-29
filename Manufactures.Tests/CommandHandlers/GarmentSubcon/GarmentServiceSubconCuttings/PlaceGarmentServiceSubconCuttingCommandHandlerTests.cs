using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.CommandHandlers;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconCuttings
{
    public class PlaceGarmentServiceSubconCuttingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockServiceSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconCuttingItemRepository> _mockServiceSubconCuttingItemRepository;
        private readonly Mock<IGarmentServiceSubconCuttingDetailRepository> _mockServiceSubconCuttingDetailRepository;
        private readonly Mock<IGarmentServiceSubconCuttingSizeRepository> _mockServiceSubconCuttingSizeRepository;
        private readonly Mock<IGarmentCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;

        public PlaceGarmentServiceSubconCuttingCommandHandlerTests()
        {
            _mockServiceSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockServiceSubconCuttingItemRepository = CreateMock<IGarmentServiceSubconCuttingItemRepository>();
            _mockServiceSubconCuttingDetailRepository = CreateMock<IGarmentServiceSubconCuttingDetailRepository>();
            _mockServiceSubconCuttingSizeRepository = CreateMock<IGarmentServiceSubconCuttingSizeRepository>();
            _mockCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();

            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingSizeRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconCuttingDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
        }
        private PlaceGarmentServiceSubconCuttingCommandHandler CreatePlaceGarmentServiceSubconCuttingCommandHandler()
        {
            return new PlaceGarmentServiceSubconCuttingCommandHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BORDIR()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid cuttingInDetailGuid= Guid.NewGuid();
            Guid cuttingInItemGuid = Guid.NewGuid();
            Guid subconCuttingDetailGuid = Guid.NewGuid();
            PlaceGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconCuttingCommand placeGarmentServiceSubconCuttingCommand = new PlaceGarmentServiceSubconCuttingCommand()
            {
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                SubconDate = DateTimeOffset.Now,
                SubconType="BORDIR",
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Items = new List<GarmentServiceSubconCuttingItemValueObject>
                {
                    new GarmentServiceSubconCuttingItemValueObject
                    {
                        
                        Article = "Article",
                        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                        RONo = "RONo",
                        ServiceSubconCuttingId=Guid.NewGuid(),
                        Details= new List<GarmentServiceSubconCuttingDetailValueObject>
                        {
                            new GarmentServiceSubconCuttingDetailValueObject
                            {
                                IsSave=true,
                                Quantity=20,
                                DesignColor= "ColorD",
                                CuttingInQuantity=20,
                                Sizes= new List<GarmentServiceSubconCuttingSizeValueObject>
                                {
                                    new GarmentServiceSubconCuttingSizeValueObject
                                    {
                                        Product = new Product(1, "ProductCode", "ProductName"),
                                        CuttingInDetailId=cuttingInDetailGuid,
                                        CuttingInId=cuttingInGuid,
                                        Size=new SizeValueObject
                                        {
                                            Size="a",
                                            Id=1,
                                        },
                                        Uom=new Uom
                                        {
                                            Unit="a",
                                            Id=1
                                        },
                                        Color="aa",
                                        Quantity=1,
                                        
                                    }
                                }
                            }
                        }
                    }
                },

            };

            GarmentCuttingIn garmentCuttingIn = new GarmentCuttingIn(cuttingInGuid, "", "", "", "RONo", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 1);
            GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(cuttingInItemGuid, cuttingInGuid, new Guid(), 1, "", new Guid(), "");
            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(cuttingInDetailGuid, cuttingInItemGuid, new Guid(), new Guid(), new Guid(), new ProductId(1), "", "", "ColorD", "", 1, new UomId(1), "", 10, new UomId(1), "", 10, 1, 1, 1, "");

            GarmentServiceSubconCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSubconCuttingDetail(subconCuttingDetailGuid, new Guid(),  "ColorD", 1);
            GarmentServiceSubconCuttingSize garmentServiceSubconCuttingSize = new GarmentServiceSubconCuttingSize(new Guid(),new SizeId(1),"",1,new UomId(1),"", "ColorD", subconCuttingDetailGuid, cuttingInGuid, cuttingInDetailGuid,new ProductId(1), "", "");
            //cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "",
            _mockServiceSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>().AsQueryable());
            //_mockServiceSubconCuttingDetailRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentServiceSubconCuttingDetailReadModel>
            //   {
            //       garmentServiceSubconCuttingDetail.GetReadModel()
            //   }.AsQueryable()
            //   );
            _mockServiceSubconCuttingSizeRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentServiceSubconCuttingSizeReadModel>
               {
                   garmentServiceSubconCuttingSize.GetReadModel()
               }.AsQueryable());

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>
                {
                    garmentCuttingIn.GetReadModel()
                }.AsQueryable());
            _mockCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>
                {
                    garmentCuttingInItem.GetReadModel()
                }.AsQueryable());
            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    garmentCuttingInDetail.GetReadModel(),
                    garmentCuttingInDetail.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingItem>()));
            _mockServiceSubconCuttingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingDetail>()));
            _mockServiceSubconCuttingSizeRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingSize>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingSize>()));

            _mockGarmentPreparingRepository
                .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>(), string.Empty))
                .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();
            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_PRINT()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid cuttingInDetailGuid = Guid.NewGuid();
            Guid cuttingInItemGuid = Guid.NewGuid();
            Guid subconCuttingDetailGuid = Guid.NewGuid();
            PlaceGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconCuttingCommand placeGarmentServiceSubconCuttingCommand = new PlaceGarmentServiceSubconCuttingCommand()
            {
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                SubconDate = DateTimeOffset.Now,
                SubconType = "PRINT",
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Items = new List<GarmentServiceSubconCuttingItemValueObject>
                {
                    new GarmentServiceSubconCuttingItemValueObject
                    {

                        Article = "Article",
                        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                        RONo = "RONo",
                        ServiceSubconCuttingId=Guid.NewGuid(),
                        Details= new List<GarmentServiceSubconCuttingDetailValueObject>
                        {
                            new GarmentServiceSubconCuttingDetailValueObject
                            {
                                IsSave=true,
                                Quantity=20,
                                DesignColor= "ColorD",
                                CuttingInQuantity=20,
                                Sizes= new List<GarmentServiceSubconCuttingSizeValueObject>
                                {
                                    new GarmentServiceSubconCuttingSizeValueObject
                                    {
                                        Product = new Product(1, "ProductCode", "ProductName"),
                                        CuttingInDetailId=cuttingInDetailGuid,
                                        CuttingInId=cuttingInGuid,
                                        Size=new SizeValueObject
                                        {
                                            Size="a",
                                            Id=1,
                                        },
                                        Uom=new Uom
                                        {
                                            Unit="a",
                                            Id=1
                                        },
                                        Color="aa",
                                        Quantity=1,
                                    }
                                }
                            }
                        }
                    }
                },
            };
            GarmentCuttingIn garmentCuttingIn = new GarmentCuttingIn(cuttingInGuid, "", "", "", "RONo", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 1);
            GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(cuttingInItemGuid, cuttingInGuid, new Guid(), 1, "", new Guid(), "");
            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(cuttingInDetailGuid, cuttingInItemGuid, new Guid(), new Guid(), new Guid(), new ProductId(1), "", "", "ColorD", "", 1, new UomId(1), "", 10, new UomId(1), "", 10, 1, 1, 1, "");

            GarmentServiceSubconCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSubconCuttingDetail(subconCuttingDetailGuid, new Guid(), "ColorD", 1);
            GarmentServiceSubconCuttingSize garmentServiceSubconCuttingSize = new GarmentServiceSubconCuttingSize(new Guid(), new SizeId(1), "", 1, new UomId(1), "", "ColorD", subconCuttingDetailGuid, cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "");
            //cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "",
            _mockServiceSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>().AsQueryable());
            //_mockServiceSubconCuttingDetailRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentServiceSubconCuttingDetailReadModel>
            //   {
            //       garmentServiceSubconCuttingDetail.GetReadModel()
            //   }.AsQueryable()
            //   );
            _mockServiceSubconCuttingSizeRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentServiceSubconCuttingSizeReadModel>
               {
                   garmentServiceSubconCuttingSize.GetReadModel()
               }.AsQueryable()
               );

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>
                {
                    garmentCuttingIn.GetReadModel()
                }.AsQueryable());
            _mockCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>
                {
                    garmentCuttingInItem.GetReadModel()
                }.AsQueryable());
            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    garmentCuttingInDetail.GetReadModel(),
                    garmentCuttingInDetail.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingItem>()));
            _mockServiceSubconCuttingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingDetail>()));
            _mockServiceSubconCuttingSizeRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingSize>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingSize>()));

            _mockGarmentPreparingRepository
                .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>(), string.Empty))
                .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_PLISKET()
        {
            // Arrange
            Guid cuttingInGuid = Guid.NewGuid();
            Guid cuttingInDetailGuid = Guid.NewGuid();
            Guid cuttingInItemGuid = Guid.NewGuid();
            Guid subconCuttingDetailGuid = Guid.NewGuid();
            PlaceGarmentServiceSubconCuttingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconCuttingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconCuttingCommand placeGarmentServiceSubconCuttingCommand = new PlaceGarmentServiceSubconCuttingCommand()
            {
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                SubconDate = DateTimeOffset.Now,
                SubconType = "PLISKET",
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Items = new List<GarmentServiceSubconCuttingItemValueObject>
                {
                    new GarmentServiceSubconCuttingItemValueObject
                    {

                        Article = "Article",
                        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                        RONo = "RONo",
                        ServiceSubconCuttingId=Guid.NewGuid(),
                        Details= new List<GarmentServiceSubconCuttingDetailValueObject>
                        {
                            new GarmentServiceSubconCuttingDetailValueObject
                            {
                                //Product = new Product(1, "ProductCode", "ProductName"),
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                                CuttingInQuantity=1,
                               // CuttingInDetailId=Guid.NewGuid(),
                                Sizes= new List<GarmentServiceSubconCuttingSizeValueObject>
                                {
                                    new GarmentServiceSubconCuttingSizeValueObject
                                    {
                                        Product = new Product(1, "ProductCode", "ProductName"),
                                        CuttingInDetailId=cuttingInDetailGuid,
                                        CuttingInId=cuttingInGuid,
                                        Size=new SizeValueObject
                                        {
                                            Size="a",
                                            Id=1,
                                        },
                                        Uom=new Uom
                                        {
                                            Unit="a",
                                            Id=1
                                        },
                                        Color="aa",
                                        Quantity=1,
                                    }
                                }
                            }
                        }
                    }
                },
            };
            GarmentCuttingIn garmentCuttingIn = new GarmentCuttingIn(cuttingInGuid, "", "", "", "RONo", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, 1);
            GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(cuttingInItemGuid, cuttingInGuid, new Guid(), 1, "", new Guid(), "");
            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(cuttingInDetailGuid, cuttingInItemGuid, new Guid(), new Guid(), new Guid(), new ProductId(1), "", "", "ColorD", "", 1, new UomId(1), "", 10, new UomId(1), "", 10, 1, 1, 1, "");

            GarmentServiceSubconCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSubconCuttingDetail(subconCuttingDetailGuid, new Guid(), "ColorD", 1);
            GarmentServiceSubconCuttingSize garmentServiceSubconCuttingSize = new GarmentServiceSubconCuttingSize(new Guid(), new SizeId(1), "", 1, new UomId(1), "", "ColorD", subconCuttingDetailGuid, cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "");
            //cuttingInGuid, cuttingInDetailGuid, new ProductId(1), "", "",
            _mockServiceSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>().AsQueryable());
            //_mockServiceSubconCuttingDetailRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentServiceSubconCuttingDetailReadModel>
            //   {
            //       garmentServiceSubconCuttingDetail.GetReadModel()
            //   }.AsQueryable()
            //   );
            _mockServiceSubconCuttingSizeRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentServiceSubconCuttingSizeReadModel>
               {
                   garmentServiceSubconCuttingSize.GetReadModel()
               }.AsQueryable()
               );

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>
                {
                    garmentCuttingIn.GetReadModel()
                }.AsQueryable());
            _mockCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>
                {
                    garmentCuttingInItem.GetReadModel()
                }.AsQueryable());
            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    garmentCuttingInDetail.GetReadModel(),
                    garmentCuttingInDetail.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));
            _mockServiceSubconCuttingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingItem>()));
            _mockServiceSubconCuttingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingDetail>()));
            _mockServiceSubconCuttingSizeRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCuttingSize>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCuttingSize>()));

            _mockGarmentPreparingRepository
                .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>(), string.Empty))
                .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconCuttingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
