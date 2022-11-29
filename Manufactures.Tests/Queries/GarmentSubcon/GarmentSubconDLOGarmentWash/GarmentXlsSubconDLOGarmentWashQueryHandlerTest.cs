using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOGarmentWashReport;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOGarmentWash
{
    public class GarmentXlsSubconDLOGarmentWashQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockgarmentSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockgarmentSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockgarmentSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconSewingItemRepository> _mockgarmentSubconSewingItemRepository;
        private readonly Mock<IGarmentServiceSubconSewingDetailRepository> _mockgarmentSubconSewingDetailRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconDLOGarmentWashQueryHandlerTest()
        {

            _mockgarmentSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockgarmentSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockgarmentSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockgarmentSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();
            _mockgarmentSubconSewingDetailRepository = CreateMock<IGarmentServiceSubconSewingDetailRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconSewingRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconSewingItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconSewingDetailRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsGarmentSubconDLOGarmentWashReportQueryHandler CreateGetGetXlsGarmentSubconDLOGarmentWashReportQueryHandler()
        {
            return new GetXlsGarmentSubconDLOGarmentWashReportQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsGarmentSubconDLOGarmentWashReportQueryHandler unitUnderTest = CreateGetGetXlsGarmentSubconDLOGarmentWashReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconDLO = Guid.NewGuid();
            Guid guidSubconDLOItem = Guid.NewGuid();
            Guid guidSubconSewing = Guid.NewGuid();
            Guid guidSubconSewingItem = Guid.NewGuid();
            Guid guidSubconSewingDetail = Guid.NewGuid();


            GetXlsGarmentSubconDLOGarmentWashReportQuery getMonitoring = new GetXlsGarmentSubconDLOGarmentWashReportQuery(1, 25, "", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2));


            _mockgarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
                {
                        new GarmentSubconDeliveryLetterOut(guidSubconDLO, "dLNo", "dLType", "SUBCON JASA", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON JASA GARMENT WASH",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel(),

                }.AsQueryable());

            _mockgarmentSubconDeliveryLetterOutItemRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
                    {
                    new GarmentSubconDeliveryLetterOutItem(guidSubconDLOItem,guidSubconDLO,1,new ProductId(1),"ProductCode","ProductName","ProductRemark","DesignColor",1,new UomId(1),"UomUnit", new UomId(1),"uomOutUnit","fabricType", guidSubconSewing, "roNo", "poSerialNumber", "SubconNo", It.IsAny<int>(),"").GetReadModel()
                    }.AsQueryable());

            _mockgarmentSubconSewingRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>
                {
                    new GarmentServiceSubconSewing(guidSubconSewing, "serviceSubconSewingNo", DateTimeOffset.Now, true, new BuyerId(1), "buyerCode", "buyerName", 5, "uomUnit").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconSewingItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingItemReadModel>
                {
                    new GarmentServiceSubconSewingItem(guidSubconSewingItem, guidSubconSewing, "rONo", "article", new GarmentComodityId(1), "comodityCode", "comodityName", new BuyerId(1), "buyerCode", "buyerName",  new UnitDepartmentId(1), "unitName", "unitCode").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconSewingDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingDetailReadModel>
                {
                    new GarmentServiceSubconSewingDetail(guidSubconSewingDetail, guidSubconSewingItem, Guid.Empty, Guid.Empty, new ProductId(1), "productCode", "productName", "designColor", 2, new UomId(1), "uomUnit", new UnitDepartmentId(1), "unitCode", "unitName", "remark", "color").GetReadModel()
                }.AsQueryable());


            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task Handle_StateUnderTest_Expected()
        {
            GetXlsGarmentSubconDLOGarmentWashReportQueryHandler unitUnderTest = CreateGetGetXlsGarmentSubconDLOGarmentWashReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconDLO = Guid.NewGuid();
            Guid guidSubconDLOItem = Guid.NewGuid();
            Guid guidSubconSewing = Guid.NewGuid();
            Guid guidSubconSewingItem = Guid.NewGuid();
            Guid guidSubconSewingDetail = Guid.NewGuid();


            GetXlsGarmentSubconDLOGarmentWashReportQuery getMonitoring = new GetXlsGarmentSubconDLOGarmentWashReportQuery(1, 25, "", DateTime.Now.AddDays(2), DateTime.Now);


            _mockgarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
                {
                        new GarmentSubconDeliveryLetterOut(guidSubconDLO, "dLNo", "dLType", "SUBCON JASA", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON JASA GARMENT WASH",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel(),

                }.AsQueryable());

            _mockgarmentSubconDeliveryLetterOutItemRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
                    {
                    new GarmentSubconDeliveryLetterOutItem(guidSubconDLOItem,guidSubconDLO,1,new ProductId(1),"ProductCode","ProductName","ProductRemark","DesignColor",1,new UomId(1),"UomUnit", new UomId(1),"uomOutUnit","fabricType", guidSubconSewing, "roNo", "poSerialNumber", "SubconNo", It.IsAny<int>(),"").GetReadModel()
                    }.AsQueryable());

            _mockgarmentSubconSewingRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>
                {
                    new GarmentServiceSubconSewing(guidSubconSewing, "serviceSubconSewingNo", DateTimeOffset.Now, true, new BuyerId(1), "buyerCode", "buyerName", 5, "uomUnit").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconSewingItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingItemReadModel>
                {
                    new GarmentServiceSubconSewingItem(guidSubconSewingItem, guidSubconSewing, "rONo", "article", new GarmentComodityId(1), "comodityCode", "comodityName", new BuyerId(1), "buyerCode", "buyerName",new UnitDepartmentId(1), "unitCode", "unitName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconSewingDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingDetailReadModel>
                {
                    new GarmentServiceSubconSewingDetail(guidSubconSewingItem, guidSubconSewingItem, Guid.Empty, Guid.Empty, new ProductId(1), "productCode", "productName", "designColor", 2, new UomId(1), "uomUnit", new UnitDepartmentId(1), "unitCode", "unitName", "remark", "color").GetReadModel()
                }.AsQueryable());


            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
