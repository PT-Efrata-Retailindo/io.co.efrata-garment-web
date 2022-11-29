using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOComponentServiceReport;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOComponent
{
    public class GarmentXlsSubconDLOComponentQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockgarmentSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockgarmentSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockgarmentSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconCuttingItemRepository> _mockgarmentSubconCuttingItemRepository;
        private readonly Mock<IGarmentServiceSubconCuttingDetailRepository> _mockgarmentSubconCuttingDetailRepository;
        private readonly Mock<IGarmentServiceSubconCuttingSizeRepository> _mockgarmentSubconCuttingSizeRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconDLOComponentQueryHandlerTest()
        {

            _mockgarmentSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockgarmentSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockgarmentSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockgarmentSubconCuttingItemRepository = CreateMock<IGarmentServiceSubconCuttingItemRepository>();
            _mockgarmentSubconCuttingDetailRepository = CreateMock<IGarmentServiceSubconCuttingDetailRepository>();
            _mockgarmentSubconCuttingSizeRepository = CreateMock<IGarmentServiceSubconCuttingSizeRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCuttingItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCuttingDetailRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCuttingSizeRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsGarmentSubconDLOComponentServiceReportQueryHandler CreateGetXlsGarmentSubconDLOComponentServiceReportQueryHandler()
        {
            return new GetXlsGarmentSubconDLOComponentServiceReportQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsGarmentSubconDLOComponentServiceReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconDLOComponentServiceReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidDeliveryLetterOut = Guid.NewGuid();
            Guid guidDeliveryLetterOutItem = Guid.NewGuid();
            Guid guidSubconCutting = Guid.NewGuid();
            Guid guidSubconCuttingItem = Guid.NewGuid();
            Guid guidSubconCuttingDetail = Guid.NewGuid();
            Guid guidSubconCuttingSize = Guid.NewGuid();


            GetXlsGarmentSubconDLOComponentServiceReportQuery getMonitoring = new GetXlsGarmentSubconDLOComponentServiceReportQuery(1, 25, "{}", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2));
            //GetXlsGarmentRealizationSubconReportQuery getMonitoring2 = new GetXlsGarmentRealizationSubconReportQuery(1, 25, "", "subconcontract2", "token");


            _mockgarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
                {
                        new GarmentSubconDeliveryLetterOut(guidDeliveryLetterOut, "dLNo", "dLType","SUBCON JASA", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON JASA KOMPONEN",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconDeliveryLetterOutItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
                {
                    new GarmentSubconDeliveryLetterOutItem(guidDeliveryLetterOutItem, guidDeliveryLetterOut, 1, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uomUnit", new UomId(1), "uomOutUnit", "fabricType", guidSubconCutting, "roNo", "poSerialNumber", "subconNo", It.IsAny<int>(),"").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconCuttingRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>
                {
                    new GarmentServiceSubconCutting(guidSubconCutting, "subconNo", "subconType", new UnitDepartmentId(1), "unitCode", "unitName", DateTimeOffset.Now, true, new BuyerId(1), "buyerCode", "buyerName", new UomId(1), "uomUnit", 1).GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconCuttingItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingItemReadModel>
                {
                    new GarmentServiceSubconCuttingItem(guidSubconCuttingItem, guidSubconCutting , "rONo", "article", new GarmentComodityId(1), "comodityCode", "comodityName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconCuttingDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingDetailReadModel>
                {
                    new GarmentServiceSubconCuttingDetail(guidSubconCuttingDetail, guidSubconCuttingItem, "designColor", 1).GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconCuttingSizeRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingSizeReadModel>
                {
                    new GarmentServiceSubconCuttingSize(guidSubconCuttingSize, new SizeId(1), "sizeName", 1, new UomId(1), "uomUnit", "color", guidSubconCuttingDetail, new Guid(), new Guid(), new ProductId(1), "productCode", "productName").GetReadModel(),
                }.AsQueryable());

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task Handle_StateUnderTest_Behavior()
        {
            GetXlsGarmentSubconDLOComponentServiceReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconDLOComponentServiceReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidDeliveryLetterOut = Guid.NewGuid();
            Guid guidDeliveryLetterOutItem = Guid.NewGuid();
            Guid guidSubconCutting = Guid.NewGuid();
            Guid guidSubconCuttingItem = Guid.NewGuid();
            Guid guidSubconCuttingDetail = Guid.NewGuid();
            Guid guidSubconCuttingSize = Guid.NewGuid();


            GetXlsGarmentSubconDLOComponentServiceReportQuery getMonitoring = new GetXlsGarmentSubconDLOComponentServiceReportQuery(1, 25, "{}", DateTime.Now.AddDays(2), DateTime.Now);
            //GetXlsGarmentRealizationSubconReportQuery getMonitoring2 = new GetXlsGarmentRealizationSubconReportQuery(1, 25, "", "subconcontract2", "token");


            _mockgarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
                {
                        new GarmentSubconDeliveryLetterOut(guidDeliveryLetterOut, "dLNo", "dLType", "SUBCON JASA", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON JASA KOMPONEN",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconDeliveryLetterOutItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
                {
                    new GarmentSubconDeliveryLetterOutItem(guidDeliveryLetterOutItem, guidDeliveryLetterOut, 1, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uomUnit", new UomId(1), "uomOutUnit", "fabricType", guidSubconCutting, "roNo", "poSerialNumber", "subconNo", It.IsAny<int>(),"").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconCuttingRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>
                {
                    new GarmentServiceSubconCutting(guidSubconCutting, "subconNo", "subconType", new UnitDepartmentId(1), "unitCode", "unitName", DateTimeOffset.Now, true, new BuyerId(1), "buyerCode", "buyerName", new UomId(1), "uomUnit", 1).GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconCuttingItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingItemReadModel>
                {
                    new GarmentServiceSubconCuttingItem(guidSubconCuttingItem, guidSubconCutting , "rONo", "article", new GarmentComodityId(1), "comodityCode", "comodityName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconCuttingDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingDetailReadModel>
                {
                    new GarmentServiceSubconCuttingDetail(guidSubconCuttingDetail, guidSubconCuttingItem, "designColor", 1).GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconCuttingSizeRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingSizeReadModel>
                {
                    new GarmentServiceSubconCuttingSize(guidSubconCuttingSize, new SizeId(1), "sizeName", 1, new UomId(1), "uomUnit", "color", guidSubconCuttingDetail, new Guid(), new Guid(), new ProductId(1), "productCode", "productName").GetReadModel(),
                }.AsQueryable());

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
