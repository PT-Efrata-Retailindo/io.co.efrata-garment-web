using Barebone.Tests;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers;
using Xunit;
using System.Threading.Tasks;
using System.Threading;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System.Linq;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using FluentAssertions;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class UpdateGarmentSubconDeliveryLetterOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockServiceSubconFabricWashRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockServiceSubconShrinkagePanelRepository;

        public UpdateGarmentSubconDeliveryLetterOutCommandHandlerTests()
        {
            _mockSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockSubconCuttingOutRepository = CreateMock<IGarmentSubconCuttingOutRepository>();
            _mockSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockServiceSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockServiceSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();

            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockSubconSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelRepository);
        }

        private UpdateGarmentSubconDeliveryLetterOutCommandHandler CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler()
        {
            return new UpdateGarmentSubconDeliveryLetterOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BB()
        {
            // Arrange
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {

                ContractType = "SUBCON BAHAN BAKU",
                DLDate = DateTimeOffset.Now.AddDays(1),
                DLType = "PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","","SUBCON BAHAN BAKU",DateTimeOffset.Now,1,"","",1,"",false,"","",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",new Guid(),"","","", It.IsAny<int>(),"")
                });

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_CT()
        {
            // Arrange
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid subconCuttingOutGuid = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                ContractType = "SUBCON GARMENT",
                SubconCategory = "SUBCON SEWING",
                DLDate = DateTimeOffset.Now,
                DLType = "RE PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                UENId = 1,
                UENNo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1,
                        SubconId=subconCuttingOutGuid,
                        RONo="ro",
                        POSerialNumber="aa",
                        SubconNo="no"
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","","SUBCON CUTTING",DateTimeOffset.Now,1,"","",1,"",false,"","",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconCuttingOutGuid,"","","", It.IsAny<int>(),"")
                });

            GarmentSubconCuttingOut garmentSubconCuttingOut = new GarmentSubconCuttingOut(subconCuttingOutGuid, "no", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, "ro", "", new GarmentComodityId(1), "", "", 1, 1, "", false);

            _mockSubconCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                    garmentSubconCuttingOut.GetReadModel()
                }.AsQueryable());


            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _mockSubconCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingOut>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_JS_CUTTING()
        {
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid subconCuttingOutGuid = Guid.NewGuid();
            Guid subconGuid = Guid.NewGuid();
            UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                ContractType = "SUBCON JASA",
                DLDate = DateTimeOffset.Now,
                DLType = "RE PROSES",
                EPOItemId = 1,
                IsUsed = false,
                PONo = "test",
                Remark = "test",
                TotalQty = 10,
                UsedQty = 10,
                UENId = 1,
                UENNo = "test",
                SubconCategory = "SUBCON JASA KOMPONEN",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Quantity=1,
                        DesignColor= "ColorD",
                        SubconDeliveryLetterOutId=Guid.NewGuid(),
                        FabricType="test",
                        ProductRemark="test",
                        UENItemId=1,
                        Uom=new Uom(1,"UomUnit"),
                        UomOut=new Uom(1,"UomUnit"),
                        ContractQuantity=1,
                        SubconId=subconGuid,
                        RONo="ro",
                        POSerialNumber="aa",
                        SubconNo="no"
                    }
                },
            };
            UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

            _mockSubconDeliveryLetterOutRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
               {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA KOMPONEN","",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()
               }.AsQueryable());
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconCuttingOutGuid,"","","", It.IsAny<int>(),"")
                });

            GarmentServiceSubconCutting garmentSubconCutting = new GarmentServiceSubconCutting(subconGuid, "no", "BORDIR", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, false, new BuyerId(1), "", "", new UomId(1), "UomUnit", 1);

            _mockSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>
                {
                    garmentSubconCutting.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));


            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
            _mockSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior_JS_SEWING()
        //{
        //    Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
        //    Guid subconCuttingOutGuid = Guid.NewGuid();
        //    Guid subconGuid = Guid.NewGuid();
        //    UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
        //    {
        //        EPOId = 1,
        //        EPONo = "EPO",
        //        ContractType = "SUBCON JASA",
        //        DLDate = DateTimeOffset.Now,
        //        DLType = "RE PROSES",
        //        EPOItemId = 1,
        //        IsUsed = false,
        //        PONo = "test",
        //        Remark = "test",
        //        TotalQty = 10,
        //        UsedQty = 10,
        //        UENId = 1,
        //        UENNo = "test",
        //        ServiceType = "SUBCON JASA GARMENT WASH",
        //        Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
        //        {
        //            new GarmentSubconDeliveryLetterOutItemValueObject
        //            {
        //                Product = new Product(1, "ProductCode", "ProductName"),
        //                Quantity=1,
        //                DesignColor= "ColorD",
        //                SubconDeliveryLetterOutId=Guid.NewGuid(),
        //                FabricType="test",
        //                ProductRemark="test",
        //                UENItemId=1,
        //                Uom=new Uom(1,"UomUnit"),
        //                UomOut=new Uom(1,"UomUnit"),
        //                ContractQuantity=1,
        //                SubconId=subconGuid,
        //                RONo="ro",
        //                POSerialNumber="aa",
        //                SubconNo="no",
        //                QtyPacking = 1,
        //                UomSatuanUnit = ""
        //            }
        //        },
        //    };
        //    UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

        //    _mockSubconDeliveryLetterOutRepository
        //       .Setup(s => s.Query)
        //       .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
        //       {
        //            new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA KOMPONEN","",1,"",1,"").GetReadModel()
        //       }.AsQueryable());
        //    _mockSubconDeliveryLetterOutItemRepository
        //        .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
        //        .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
        //        {
        //            new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconCuttingOutGuid,"","","",1,"")
        //        });

        //    GarmentServiceSubconCutting garmentSubconCutting = new GarmentServiceSubconCutting(subconGuid, "no", "BORDIR", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, false, new BuyerId(1), "", "",new UomId(1), "", 1);

        //    _mockSubconCuttingRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSubconCuttingReadModel>
        //        {
        //            garmentSubconCutting.GetReadModel()
        //        }.AsQueryable());

        //    _mockSubconCuttingRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentServiceSubconCutting>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconCutting>()));


        //    _mockSubconDeliveryLetterOutRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
        //    _mockSubconDeliveryLetterOutItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

        //    _MockStorage
        //        .Setup(x => x.Save())
        //        .Verifiable();

        //    // Act
        //    var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior_JS_SEWING()
        //{
        //    Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
        //    Guid subconCuttingOutGuid = Guid.NewGuid();
        //    Guid subconGuid = Guid.NewGuid();
        //    UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
        //    {
        //        ContractNo = "test",
        //        ContractType = "SUBCON JASA",
        //        DLDate = DateTimeOffset.Now,
        //        DLType = "RE PROSES",
        //        EPOItemId = 1,
        //        IsUsed = false,
        //        PONo = "test",
        //        Remark = "test",
        //        TotalQty = 10,
        //        UsedQty = 10,
        //        SubconContractId = new Guid(),
        //        UENId = 1,
        //        UENNo = "test",
        //        ServiceType = "SUBCON JASA GARMENT WASH",
        //        Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
        //        {
        //            new GarmentSubconDeliveryLetterOutItemValueObject
        //            {
        //                Product = new Product(1, "ProductCode", "ProductName"),
        //                Quantity=1,
        //                DesignColor= "ColorD",
        //                SubconDeliveryLetterOutId=Guid.NewGuid(),
        //                FabricType="test",
        //                ProductRemark="test",
        //                UENItemId=1,
        //                Uom=new Uom(1,"UomUnit"),
        //                UomOut=new Uom(1,"UomUnit"),
        //                ContractQuantity=1,
        //                SubconId=subconGuid,
        //                RONo="ro",
        //                POSerialNumber="aa",
        //                SubconNo="no"
        //            }
        //        },
        //    };
        //    UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

        //    _mockSubconDeliveryLetterOutRepository
        //       .Setup(s => s.Query)
        //       .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
        //       {
        //            new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA KOMPONEN","",1,"",1,"").GetReadModel()
        //       }.AsQueryable());
        //    _mockSubconDeliveryLetterOutItemRepository
        //        .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
        //        .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
        //        {
        //            new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconCuttingOutGuid,"","","",1,"")
        //        });

        //    GarmentServiceSubconSewing garmentSubconSewing = new GarmentServiceSubconSewing(subconGuid, "no", DateTimeOffset.Now, false, new BuyerId(1), "", "",1,"");

        //    _mockSubconSewingRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSubconSewingReadModel>
        //        {
        //            garmentSubconSewing.GetReadModel()
        //        }.AsQueryable());

        //    _mockSubconSewingRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewing>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewing>()));


        //    _mockSubconDeliveryLetterOutRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
        //    _mockSubconDeliveryLetterOutItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

        //    _MockStorage
        //        .Setup(x => x.Save())
        //        .Verifiable();

        //    // Act
        //    var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}

        //    [Fact]
        //    public async Task Handle_StateUnderTest_ExpectedBehavior_JS_FABRIC()
        //    {
        //        Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
        //        Guid subconCuttingOutGuid = Guid.NewGuid();
        //        Guid subconGuid = Guid.NewGuid();
        //        UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
        //        CancellationToken cancellationToken = CancellationToken.None;
        //        UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
        //        {
        //            EPOId = 1,
        //            EPONo = "EPO",
        //            ContractType = "SUBCON JASA",
        //            DLDate = DateTimeOffset.Now,
        //            DLType = "RE PROSES",
        //            EPOItemId = 1,
        //            IsUsed = false,
        //            PONo = "test",
        //            Remark = "test",
        //            TotalQty = 10,
        //            UsedQty = 10,
        //            UENId = 1,
        //            UENNo = "test",
        //            ServiceType = "SUBCON JASA FABRIC WASH",
        //            Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
        //            {
        //                new GarmentSubconDeliveryLetterOutItemValueObject
        //                {
        //                    Product = new Product(1, "ProductCode", "ProductName"),
        //                    Quantity=1,
        //                    DesignColor= "ColorD",
        //                    SubconDeliveryLetterOutId=Guid.NewGuid(),
        //                    FabricType="test",
        //                    ProductRemark="test",
        //                    UENItemId=1,
        //                    Uom=new Uom(1,"UomUnit"),
        //                    UomOut=new Uom(1,"UomUnit"),
        //                    ContractQuantity=1,
        //                    SubconId=subconGuid,
        //                    RONo="ro",
        //                    POSerialNumber="aa",
        //                    SubconNo="no",
        //                    QtyPacking = 1,
        //                    UomSatuanUnit = ""
        //                }
        //            },
        //        };
        //        UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

        //        _mockSubconDeliveryLetterOutRepository
        //           .Setup(s => s.Query)
        //           .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
        //           {
        //                new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA KOMPONEN","",1,"",1,"").GetReadModel()
        //           }.AsQueryable());
        //        _mockSubconDeliveryLetterOutItemRepository
        //            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
        //            .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
        //            {
        //                new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconGuid,"","","",1,"")
        //            });

        //        GarmentServiceSubconFabricWash garmentSubconFabric = new GarmentServiceSubconFabricWash(subconGuid, "", DateTimeOffset.Now,"", false,1,"");

        //        _mockServiceSubconFabricWashRepository
        //            .Setup(s => s.Query)
        //            .Returns(new List<GarmentServiceSubconFabricWashReadModel>
        //            {
        //                garmentSubconFabric.GetReadModel()
        //            }.AsQueryable());

        //        _mockServiceSubconFabricWashRepository
        //            .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWash>()))
        //            .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWash>()));


        //        _mockSubconDeliveryLetterOutRepository
        //            .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
        //            .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
        //        _mockSubconDeliveryLetterOutItemRepository
        //            .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
        //            .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

        //        _MockStorage
        //            .Setup(x => x.Save())
        //            .Verifiable();

        //        // Act
        //        var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //        // Assert
        //        result.Should().NotBeNull();
        //    }

        //    [Fact]
        //    public async Task Handle_StateUnderTest_ExpectedBehavior_JS_SHRINKAGE()
        //    {
        //        Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
        //        Guid subconCuttingOutGuid = Guid.NewGuid();
        //        Guid subconGuid = Guid.NewGuid();
        //        UpdateGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreateUpdateGarmentSubconDeliveryLetterOutCommandHandler();
        //        CancellationToken cancellationToken = CancellationToken.None;
        //        UpdateGarmentSubconDeliveryLetterOutCommand UpdateGarmentSubconDeliveryLetterOutCommand = new UpdateGarmentSubconDeliveryLetterOutCommand()
        //        {
        //            EPOId = 1,
        //            EPONo = "EPO",
        //            ContractType = "SUBCON JASA",
        //            DLDate = DateTimeOffset.Now,
        //            DLType = "RE PROSES",
        //            EPOItemId = 1,
        //            IsUsed = false,
        //            PONo = "test",
        //            Remark = "test",
        //            TotalQty = 10,
        //            UsedQty = 10,
        //            UENId = 1,
        //            UENNo = "test",
        //            ServiceType = "SUBCON JASA SHRINKAGE PANEL",
        //            Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
        //            {
        //                new GarmentSubconDeliveryLetterOutItemValueObject
        //                {
        //                    Product = new Product(1, "ProductCode", "ProductName"),
        //                    Quantity=1,
        //                    DesignColor= "ColorD",
        //                    SubconDeliveryLetterOutId=Guid.NewGuid(),
        //                    FabricType="test",
        //                    ProductRemark="test",
        //                    UENItemId=1,
        //                    Uom=new Uom(1,"UomUnit"),
        //                    UomOut=new Uom(1,"UomUnit"),
        //                    ContractQuantity=1,
        //                    SubconId=subconGuid,
        //                    RONo="ro",
        //                    POSerialNumber="aa",
        //                    SubconNo="no",
        //                    QtyPacking = 1,
        //                    UomSatuanUnit = ""
        //                }
        //            },
        //        };
        //        UpdateGarmentSubconDeliveryLetterOutCommand.SetIdentity(SubconDeliveryLetterOutGuid);

        //        _mockSubconDeliveryLetterOutRepository
        //           .Setup(s => s.Query)
        //           .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>()
        //           {
        //                new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid,"","","SUBCON JASA",DateTimeOffset.Now,1,"","",1,"",false,"SUBCON JASA KOMPONEN","",1,"",1,"").GetReadModel()
        //           }.AsQueryable());
        //        _mockSubconDeliveryLetterOutItemRepository
        //            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
        //            .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
        //            {
        //                new GarmentSubconDeliveryLetterOutItem(Guid.Empty,SubconDeliveryLetterOutGuid,1,new ProductId(1),"code","name","remark","color",1,new UomId(1),"unit",new UomId(1),"unit","fabType",subconGuid,"","","",1,"")
        //            });

        //        GarmentServiceSubconShrinkagePanel garmentSubconShrinkage = new GarmentServiceSubconShrinkagePanel(subconGuid, "", DateTimeOffset.Now, null, false,1,"");

        //        _mockServiceSubconShrinkagePanelRepository
        //            .Setup(s => s.Query)
        //            .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>
        //            {
        //                garmentSubconShrinkage.GetReadModel()
        //            }.AsQueryable());

        //        _mockServiceSubconShrinkagePanelRepository
        //            .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanel>()))
        //            .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanel>()));


        //        _mockSubconDeliveryLetterOutRepository
        //            .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
        //            .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
        //        _mockSubconDeliveryLetterOutItemRepository
        //            .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
        //            .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

        //        _MockStorage
        //            .Setup(x => x.Save())
        //            .Verifiable();

        //        // Act
        //        var result = await unitUnderTest.Handle(UpdateGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //        // Assert
        //        result.Should().NotBeNull();
        //    }
        //}
    }
}
