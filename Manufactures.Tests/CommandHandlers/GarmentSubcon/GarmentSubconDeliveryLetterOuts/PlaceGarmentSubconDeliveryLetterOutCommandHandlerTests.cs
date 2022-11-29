using Barebone.Tests;
using Manufactures.Application.GarmentSubcon.GarmentSubconDeliveryLetterOuts.CommandHandlers;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;
using Xunit;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using FluentAssertions;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class PlaceGarmentSubconDeliveryLetterOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockServiceSubconFabricWashRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockServiceSubconShrinkagePanelRepository;
        private readonly Mock<IGarmentSubconContractRepository> _mockSubconContractRepository;

        public PlaceGarmentSubconDeliveryLetterOutCommandHandlerTests()
        {
            _mockSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockSubconCuttingOutRepository = CreateMock<IGarmentSubconCuttingOutRepository>();
            _mockSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockServiceSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockServiceSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();
            _mockSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();

            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockSubconSewingRepository);
            _MockStorage.SetupStorage(_mockSubconContractRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockServiceSubconShrinkagePanelRepository);
        }
        private PlaceGarmentSubconDeliveryLetterOutCommandHandler CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler()
        {
            return new PlaceGarmentSubconDeliveryLetterOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_BB()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                EPOId = It.IsAny<int>(),
                EPONo = "EPO",
                ContractType = "SUBCON BAHAN BAKU",
                DLDate = DateTimeOffset.Now,
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
                        ContractQuantity=1,
                        QtyPacking=1,
                        UomSatuanUnit=""
                    }
                },

            };

            _mockSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
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
            var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior_CT()
        //{
        //    // Arrange
        //    Guid subconCuttingOutGuid = Guid.NewGuid();
        //    PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
        //    {
        //        EPOId = It.IsAny<int>(),
        //        EPONo = "EPO",
        //        ContractType = "SUBCON CUTTING",
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
        //                SubconId=subconCuttingOutGuid,
        //                SubconNo="no",
        //                POSerialNumber="poNo",
        //                RONo="RONo",
        //                QtyPacking=1,
        //                UomSatuanUnit=""
        //            }
        //        },

        //    };
        //    GarmentSubconCuttingOut garmentSubconCuttingOut = new GarmentSubconCuttingOut(subconCuttingOutGuid, "no", "", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, "ro", "", new GarmentComodityId(1), "", "", 1, 1, "", false);

        //    _mockSubconCuttingOutRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentCuttingOutReadModel>
        //        {
        //            garmentSubconCuttingOut.GetReadModel()
        //        }.AsQueryable());

        //    _mockSubconDeliveryLetterOutRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
        //    _mockSubconDeliveryLetterOutRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOut>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOut>()));
        //    _mockSubconDeliveryLetterOutItemRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSubconDeliveryLetterOutItem>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSubconDeliveryLetterOutItem>()));

        //    _mockSubconCuttingOutRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentSubconCuttingOut>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentSubconCuttingOut>()));
        //    _MockStorage
        //        .Setup(x => x.Save())
        //        .Verifiable();

        //    // Act
        //    var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior_JS_CUTTING()
        //{
        //    // Arrange
        //    Guid subconCUTTINGGuid = Guid.NewGuid();
        //    Guid sewingInItemGuid = Guid.NewGuid();
        //    Guid SubconContractGuid = Guid.NewGuid();
        //    PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
        //    {
        //        EPOId = It.IsAny<int>(),
        //        EPONo = "EPO",
        //        ContractType = "SUBCON JASA",
        //        ServiceType = "SUBCON JASA KOMPONEN",
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
        //        Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
        //        {
        //            new GarmentSubconDeliveryLetterOutItemValueObject
        //            {
        //                Product = new Product(1, "ProductCode", "ProductName"),
        //                SubconId=subconCUTTINGGuid,
        //                SubconNo="no",
        //                Quantity=1,
        //                DesignColor= "ColorD",
        //                SubconDeliveryLetterOutId=Guid.NewGuid(),
        //                FabricType="test",
        //                ProductRemark="test",
        //                UENItemId=1,
        //                Uom=new Uom(1,"UomUnit"),
        //                UomOut=new Uom(1,"UomUnit"),
        //                ContractQuantity=1,
        //                QtyPacking=1,
        //                UomSatuanUnit=""
        //            }
        //        },

        //    };
        //    GarmentServiceSubconCutting garmentSubconCutting = new GarmentServiceSubconCutting(subconCUTTINGGuid, "no", "BORDIR", new UnitDepartmentId(1), "", "", DateTimeOffset.Now, false, new BuyerId(1), "", "", new UomId(1),"", 1);

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
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
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
        //    var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior_JS_SEWING()
        //{
        //    // Arrange
        //    Guid subconSewingGuid = Guid.NewGuid();
        //    Guid sewingInItemGuid = Guid.NewGuid();
        //    Guid SubconContractGuid = Guid.NewGuid();
        //    PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
        //    {
        //        EPOId = It.IsAny<int>(),
        //        EPONo = "EPO",
        //        ContractType = "SUBCON JASA",
        //        ServiceType = "SUBCON JASA GARMENT WASH",
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
        //        Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
        //        {
        //            new GarmentSubconDeliveryLetterOutItemValueObject
        //            {
        //                Product = new Product(1, "ProductCode", "ProductName"),
        //                SubconId=subconSewingGuid,
        //                SubconNo="no",
        //                Quantity=1,
        //                DesignColor= "ColorD",
        //                SubconDeliveryLetterOutId=Guid.NewGuid(),
        //                FabricType="test",
        //                ProductRemark="test",
        //                UENItemId=1,
        //                Uom=new Uom(1,"UomUnit"),
        //                UomOut=new Uom(1,"UomUnit"),
        //                ContractQuantity=1,
        //                QtyPacking=1,
        //                UomSatuanUnit=""
        //            }
        //        },

        //    };
        //    GarmentServiceSubconSewing garmentSubconSewing = new GarmentServiceSubconSewing(subconSewingGuid, "no", DateTimeOffset.Now, false, new BuyerId(1), "", "", 1, "");

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
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
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
        //    var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior_JS_SHRINKAGE()
        //{
        //    // Arrange
        //    Guid subconGuid = Guid.NewGuid();
        //    Guid SubconContractGuid = Guid.NewGuid();
        //    PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
        //    {
        //        EPOId = It.IsAny<int>(),
        //        EPONo = "EPO",
        //        ContractType = "SUBCON JASA",
        //        ServiceType = "SUBCON JASA SHRINKAGE PANEL",
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
        //        Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
        //        {
        //            new GarmentSubconDeliveryLetterOutItemValueObject
        //            {
        //                Product = new Product(1, "ProductCode", "ProductName"),
        //                SubconId=subconGuid,
        //                SubconNo="no",
        //                Quantity=1,
        //                DesignColor= "ColorD",
        //                SubconDeliveryLetterOutId=Guid.NewGuid(),
        //                FabricType="test",
        //                ProductRemark="test",
        //                UENItemId=1,
        //                Uom=new Uom(1,"UomUnit"),
        //                UomOut=new Uom(1,"UomUnit"),
        //                ContractQuantity=1,
        //                QtyPacking=1,
        //                UomSatuanUnit=""

        //            }
        //        },

        //    };
        //    GarmentServiceSubconShrinkagePanel garmentSubconShrinkage = new GarmentServiceSubconShrinkagePanel(subconGuid, "", DateTimeOffset.Now, "", false, 1, "");

        //    _mockServiceSubconShrinkagePanelRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>
        //        {
        //            garmentSubconShrinkage.GetReadModel()
        //        }.AsQueryable());

        //    _mockServiceSubconShrinkagePanelRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentServiceSubconShrinkagePanel>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconShrinkagePanel>()));

        //    _mockSubconDeliveryLetterOutRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
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
        //    var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}

        //[Fact]
        //public async Task Handle_StateUnderTest_ExpectedBehavior_JS_FABRIC()
        //{
        //    // Arrange
        //    Guid subconGuid = Guid.NewGuid();
        //    Guid SubconContractGuid = Guid.NewGuid();
        //    PlaceGarmentSubconDeliveryLetterOutCommandHandler unitUnderTest = CreatePlaceGarmentSubconDeliveryLetterOutCommandHandler();
        //    CancellationToken cancellationToken = CancellationToken.None;
        //    PlaceGarmentSubconDeliveryLetterOutCommand placeGarmentSubconDeliveryLetterOutCommand = new PlaceGarmentSubconDeliveryLetterOutCommand()
        //    {
        //        EPOId = It.IsAny<int>(),
        //        EPONo = "EPO",
        //        ContractType = "SUBCON JASA",
        //        ServiceType = "SUBCON JASA FABRIC WASH",
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
        //        Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>
        //        {
        //            new GarmentSubconDeliveryLetterOutItemValueObject
        //            {
        //                Product = new Product(1, "ProductCode", "ProductName"),
        //                SubconId=subconGuid,
        //                SubconNo="no",
        //                Quantity=1,
        //                DesignColor= "ColorD",
        //                SubconDeliveryLetterOutId=Guid.NewGuid(),
        //                FabricType="test",
        //                ProductRemark="test",
        //                UENItemId=1,
        //                Uom=new Uom(1,"UomUnit"),
        //                UomOut=new Uom(1,"UomUnit"),
        //                ContractQuantity=1,
        //                QtyPacking=1,
        //                UomSatuanUnit=""
        //            }
        //        },

        //    };
        //    GarmentServiceSubconFabricWash garmentSubconFabric = new GarmentServiceSubconFabricWash(subconGuid, "", DateTimeOffset.Now,"", false, 1, "");

        //    _mockServiceSubconFabricWashRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentServiceSubconFabricWashReadModel>
        //        {
        //            garmentSubconFabric.GetReadModel()
        //        }.AsQueryable());

        //    _mockServiceSubconFabricWashRepository
        //        .Setup(s => s.Update(It.IsAny<GarmentServiceSubconFabricWash>()))
        //        .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconFabricWash>()));

        //    _mockSubconDeliveryLetterOutRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());
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
        //    var result = await unitUnderTest.Handle(placeGarmentSubconDeliveryLetterOutCommand, cancellationToken);

        //    // Assert
        //    result.Should().NotBeNull();
        //}
    }
}
