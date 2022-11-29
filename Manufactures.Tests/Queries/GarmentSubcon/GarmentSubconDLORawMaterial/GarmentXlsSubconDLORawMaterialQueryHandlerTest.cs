using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLORawMaterial
{
    public class GarmentXlsSubconDLORawMaterialQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockgarmentSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockgarmentSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockgarmentSubconShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelItemRepository> _mockgarmentSubconShrikagePanelItemRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelDetailRepository> _mockgarmentSubconShrinkagePanelDetailRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockgarmentSubconFabricWashRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashItemRepository> _mockgarmentSubconFabricWashItemRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashDetailRepository> _mockgarmentSubconFabricWashDetailRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconDLORawMaterialQueryHandlerTest()
        {

            _mockgarmentSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockgarmentSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockgarmentSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();
            _mockgarmentSubconShrikagePanelItemRepository = CreateMock<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _mockgarmentSubconShrinkagePanelDetailRepository = CreateMock<IGarmentServiceSubconShrinkagePanelDetailRepository>();
            _mockgarmentSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockgarmentSubconFabricWashItemRepository = CreateMock<IGarmentServiceSubconFabricWashItemRepository>();
            _mockgarmentSubconFabricWashDetailRepository = CreateMock<IGarmentServiceSubconFabricWashDetailRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconShrikagePanelItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconShrinkagePanelDetailRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconFabricWashItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconFabricWashDetailRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsGarmentSubconDLORawMaterialReportQueryHandler CreateGetXlsGarmentSubconDLORawMaterialReportQueryHandler()
        {
            return new GetXlsGarmentSubconDLORawMaterialReportQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsGarmentSubconDLORawMaterialReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconDLORawMaterialReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconDLO = Guid.NewGuid();
            Guid guidSubconDLO2 = Guid.NewGuid();
            Guid guidSubconDLOItem = Guid.NewGuid();
            Guid guidSubconDLOItem2 = Guid.NewGuid();
            Guid guidSubconShrinkagePanel = Guid.NewGuid();
            Guid guidSubconShrinkagePanelItem = Guid.NewGuid();
            Guid guidSubconShrinkagePanelDetail = Guid.NewGuid();
            Guid guidSubconFabricWash = Guid.NewGuid();
            Guid guidSubconFabricWashItem = Guid.NewGuid();
            Guid guidSubconFabricWashDetail = Guid.NewGuid();


            GetXlsGarmentSubconDLORawMaterialReportQuery getMonitoring = new GetXlsGarmentSubconDLORawMaterialReportQuery(1, 25, "", DateTime.Now.AddDays(-2), DateTime.Now.AddDays(2));


            _mockgarmentSubconDeliveryLetterOutRepository
                 .Setup(s => s.Query)
                 .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
                 {
                        new GarmentSubconDeliveryLetterOut(guidSubconDLO, "dLNo", "dLType", "SUBCON BAHAN BAKU", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON BB SHRINKAGE/PANEL",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel(),
                         new GarmentSubconDeliveryLetterOut(guidSubconDLO2, "dLNo", "dLType", "SUBCON BAHAN BAKU", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON BB FABRIC WASH/PRINT",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel(),
                 }.AsQueryable());

            _mockgarmentSubconDeliveryLetterOutItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
                {   
                    new GarmentSubconDeliveryLetterOutItem(guidSubconDLOItem, guidSubconDLO, 1, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uomUnit", new UomId(1), "uomOutUnit", "fabricType", guidSubconShrinkagePanel, "roNo", "poSerialNumber", "subconNo", It.IsAny<int>(),"").GetReadModel(),
                    new GarmentSubconDeliveryLetterOutItem(guidSubconDLOItem, guidSubconDLO2, 1, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uomUnit", new UomId(1), "uomOutUnit", "fabricType", guidSubconShrinkagePanel, "roNo", "poSerialNumber", "subconNo", It.IsAny<int>(),"").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconShrinkagePanelRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>
                {
                    new GarmentServiceSubconShrinkagePanel(guidSubconShrinkagePanel, "serviceSubconShrinkagePanelNo", DateTimeOffset.Now, "remark", true, 0, null).GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconShrikagePanelItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelItemReadModel>
                {
                    new GarmentServiceSubconShrinkagePanelItem(guidSubconShrinkagePanelItem, guidSubconShrinkagePanel, "unitExpenditureNo", DateTimeOffset.Now, new UnitSenderId(1), "unitSenderCode", "unitSenderName", new UnitRequestId(1), "unitRequestCode", "unitRequestName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconShrinkagePanelDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelDetailReadModel>
                {
                    new GarmentServiceSubconShrinkagePanelDetail(guidSubconShrinkagePanelDetail, guidSubconShrinkagePanelItem, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uomUnit").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconFabricWashRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashReadModel>
                {
                    new GarmentServiceSubconFabricWash(guidSubconShrinkagePanel, "serviceSubconFabricWashNo", DateTimeOffset.Now, "remark", true, 0, null).GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconFabricWashItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashItemReadModel>
                {
                    new GarmentServiceSubconFabricWashItem(guidSubconFabricWashItem, guidSubconShrinkagePanel, "unitExpenditureNo", DateTimeOffset.Now, new UnitSenderId(1), "unitSenderCode", "unitSenderName", new UnitRequestId(1), "unitRequestCode", "unitRequestName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconFabricWashDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashDetailReadModel>
                {
                    new GarmentServiceSubconFabricWashDetail(guidSubconFabricWashItem, guidSubconFabricWashItem, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uom" ).GetReadModel()
                }.AsQueryable());

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task Handle_StateUnderTest_UnExpectedBehavior()
        {
            GetXlsGarmentSubconDLORawMaterialReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconDLORawMaterialReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconDLO = Guid.NewGuid();
            Guid guidSubconDLO2 = Guid.NewGuid();
            Guid guidSubconDLOItem = Guid.NewGuid();
            Guid guidSubconDLOItem2 = Guid.NewGuid();
            Guid guidSubconShrinkagePanel = Guid.NewGuid();
            Guid guidSubconShrinkagePanelItem = Guid.NewGuid();
            Guid guidSubconShrinkagePanelDetail = Guid.NewGuid();
            Guid guidSubconFabricWash = Guid.NewGuid();
            Guid guidSubconFabricWashItem = Guid.NewGuid();
            Guid guidSubconFabricWashDetail = Guid.NewGuid();


            GetXlsGarmentSubconDLORawMaterialReportQuery getMonitoring = new GetXlsGarmentSubconDLORawMaterialReportQuery(1, 25, "", DateTime.Now.AddDays(2), DateTime.Now);


            _mockgarmentSubconDeliveryLetterOutRepository
                 .Setup(s => s.Query)
                 .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
                 {
                        new GarmentSubconDeliveryLetterOut(guidSubconDLO, "dLNo", "dLType", "SUBCON BAHAN BAKU", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON BB SHRINKAGE/PANEL",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel(),
                         new GarmentSubconDeliveryLetterOut(guidSubconDLO2, "dLNo", "dLType", "SUBCON BAHAN BAKU", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON BB FABRIC WASH/PRINT",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel(),
                 }.AsQueryable());

            _mockgarmentSubconDeliveryLetterOutItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
                {
                    new GarmentSubconDeliveryLetterOutItem(guidSubconDLOItem, guidSubconDLO, 1, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uomUnit", new UomId(1), "uomOutUnit", "fabricType", guidSubconShrinkagePanel, "roNo", "poSerialNumber", "subconNo", It.IsAny<int>(),"").GetReadModel(),
                    new GarmentSubconDeliveryLetterOutItem(guidSubconDLOItem, guidSubconDLO2, 1, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uomUnit", new UomId(1), "uomOutUnit", "fabricType", guidSubconShrinkagePanel, "roNo", "poSerialNumber", "subconNo", It.IsAny<int>(),"").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconShrinkagePanelRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>
                {
                    new GarmentServiceSubconShrinkagePanel(guidSubconShrinkagePanel, "serviceSubconShrinkagePanelNo", DateTimeOffset.Now, "remark", true, 0, null).GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconShrikagePanelItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelItemReadModel>
                {
                    new GarmentServiceSubconShrinkagePanelItem(guidSubconShrinkagePanelItem, guidSubconShrinkagePanel, "unitExpenditureNo", DateTimeOffset.Now, new UnitSenderId(1), "unitSenderCode", "unitSenderName", new UnitRequestId(1), "unitRequestCode", "unitRequestName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconShrinkagePanelDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelDetailReadModel>
                {
                    new GarmentServiceSubconShrinkagePanelDetail(guidSubconShrinkagePanelDetail, guidSubconShrinkagePanelItem, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uomUnit").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconFabricWashRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashReadModel>
                {
                    new GarmentServiceSubconFabricWash(guidSubconShrinkagePanel, "serviceSubconFabricWashNo", DateTimeOffset.Now, "remark", true, 0, null).GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconFabricWashItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashItemReadModel>
                {
                    new GarmentServiceSubconFabricWashItem(guidSubconFabricWashItem, guidSubconShrinkagePanel, "unitExpenditureNo", DateTimeOffset.Now, new UnitSenderId(1), "unitSenderCode", "unitSenderName", new UnitRequestId(1), "unitRequestCode", "unitRequestName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconFabricWashDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashDetailReadModel>
                {
                    new GarmentServiceSubconFabricWashDetail(guidSubconFabricWashItem, guidSubconFabricWashItem, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uom" ).GetReadModel()
                }.AsQueryable());

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
