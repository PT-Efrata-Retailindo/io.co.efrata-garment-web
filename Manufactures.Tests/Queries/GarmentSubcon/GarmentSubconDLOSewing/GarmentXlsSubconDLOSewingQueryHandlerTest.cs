using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOSewingReport;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOSewing
{
    public class GarmentXlsSubconDLOGarmentWashQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockgarmentSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockgarmentSubconDeliveryLetterOutItemRepository;
        private readonly Mock<IGarmentCuttingOutRepository> _mockgarmentCuttingOutRepository;
        private readonly Mock<IGarmentCuttingOutItemRepository> _mockgarmentCuttingOutItemRepository;
        private readonly Mock<IGarmentCuttingOutDetailRepository> _mockgarmentCuttingOutDetailRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconDLOGarmentWashQueryHandlerTest()
        {

            _mockgarmentSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockgarmentSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockgarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockgarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockgarmentCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockgarmentCuttingOutRepository);
            _MockStorage.SetupStorage(_mockgarmentCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockgarmentCuttingOutDetailRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsGarmentSubconDLOSewingReportQueryHandler CreateGetXlsGarmentSubconDLOSewingReportQueryHandler()
        {
            return new GetXlsGarmentSubconDLOSewingReportQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsGarmentSubconDLOSewingReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconDLOSewingReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconDLO = Guid.NewGuid();
            Guid guidSubonDLOItem = Guid.NewGuid();
            Guid guidCuttingOut = Guid.NewGuid();
            Guid guidCuttingOutItem = Guid.NewGuid();
            Guid guidCuttingOutDetail = Guid.NewGuid();


            GetXlsGarmentSubconDLOSewingReportQuery getMonitoring = new GetXlsGarmentSubconDLOSewingReportQuery(1, 25, "", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2));


            _mockgarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
                {
                        new GarmentSubconDeliveryLetterOut(guidSubconDLO, "dLNo", "dLType", "SUBCON GARMENT", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON SEWING",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconDeliveryLetterOutItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
                {
                    new GarmentSubconDeliveryLetterOutItem(guidSubonDLOItem, guidSubconDLO, 1, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 1, new UomId(1), "uomUnit", new UomId(1), "uomOutUnit", "fabricType", guidCuttingOut, "roNo", "poSerialNumber", "subconNo", It.IsAny<int>(),"").GetReadModel()
                }.AsQueryable());

            _mockgarmentCuttingOutRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                    new GarmentCuttingOut(guidCuttingOut, "cutOutNo", "cuttingOutType", new UnitDepartmentId(1), "unitFromCode", "unitFromName", DateTimeOffset.Now, "rONo", "article", new UnitDepartmentId(1), "untCode", "unitName", new GarmentComodityId(1), "comodityCode", "comodityName", true).GetReadModel()

                }.AsQueryable());

            _mockgarmentCuttingOutItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>
                {
                    new GarmentCuttingOutItem(guidCuttingOutItem, Guid.Empty, Guid.Empty, guidCuttingOut, new ProductId(1), "productCode", "productName", "designColor", 2).GetReadModel()
                }.AsQueryable());

            _mockgarmentCuttingOutDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>
                {
                    new GarmentCuttingOutDetail(guidCuttingOutDetail, guidCuttingOutItem, new SizeId(1), "sizeName", "color", 2, 2, new UomId(1), "cuttingOutUomUnit", 1, 1).GetReadModel()
                }.AsQueryable());



            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task Handle_StateUnderTest_Expected()
        {
            GetXlsGarmentSubconDLOSewingReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconDLOSewingReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconDLO = Guid.NewGuid();
            Guid guidSubonDLOItem = Guid.NewGuid();
            Guid guidCuttingOut = Guid.NewGuid();
            Guid guidCuttingOutItem = Guid.NewGuid();
            Guid guidCuttingOutDetail = Guid.NewGuid();


            GetXlsGarmentSubconDLOSewingReportQuery getMonitoring = new GetXlsGarmentSubconDLOSewingReportQuery(1, 25, "", DateTime.Now.AddDays(2), DateTime.Now);


            _mockgarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
                {
                        new GarmentSubconDeliveryLetterOut(guidSubconDLO, "dLNo", "dLType", "SUBCON GARMENT", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", true, "serviceType", "SUBCON SEWING",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconDeliveryLetterOutItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
                {
                    new GarmentSubconDeliveryLetterOutItem(guidSubonDLOItem, guidSubconDLO, 1, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 1, new UomId(1), "uomUnit", new UomId(1), "uomOutUnit", "fabricType", guidCuttingOut, "roNo", "poSerialNumber", "subconNo", It.IsAny<int>(),"").GetReadModel()
                }.AsQueryable());

            _mockgarmentCuttingOutRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                    new GarmentCuttingOut(guidCuttingOut, "cutOutNo", "cuttingOutType", new UnitDepartmentId(1), "unitFromCode", "unitFromName", DateTimeOffset.Now, "rONo", "article", new UnitDepartmentId(1), "untCode", "unitName", new GarmentComodityId(1), "comodityCode", "comodityName", true).GetReadModel()

                }.AsQueryable());

            _mockgarmentCuttingOutItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>
                {
                    new GarmentCuttingOutItem(guidCuttingOutItem, Guid.Empty, Guid.Empty, guidCuttingOut, new ProductId(1), "productCode", "productName", "designColor", 2).GetReadModel()
                }.AsQueryable());

            _mockgarmentCuttingOutDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>
                {
                    new GarmentCuttingOutDetail(guidCuttingOutDetail, guidCuttingOutItem, new SizeId(1), "sizeName", "color", 2, 2, new UomId(1), "cuttingOutUomUnit", 1, 1).GetReadModel()
                }.AsQueryable());



            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
