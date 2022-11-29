using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconShrinkagePanels;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.ExcelTemplates;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentServiceSubconShrinkagePanels
{
    public class GarmentXlsSubconShrinkagePanelQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconShrinkagePanelRepository> _mockgarmentSubconShrinkagePanelRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelItemRepository> _mockgarmentSubconShrikagePanelItemRepository;
        private readonly Mock<IGarmentServiceSubconShrinkagePanelDetailRepository> _mockgarmentSubconShrinkagePanelDetailRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconShrinkagePanelQueryHandlerTest()
        {

            _mockgarmentSubconShrinkagePanelRepository = CreateMock<IGarmentServiceSubconShrinkagePanelRepository>();
            _mockgarmentSubconShrikagePanelItemRepository = CreateMock<IGarmentServiceSubconShrinkagePanelItemRepository>();
            _mockgarmentSubconShrinkagePanelDetailRepository = CreateMock<IGarmentServiceSubconShrinkagePanelDetailRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconShrinkagePanelRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconShrikagePanelItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconShrinkagePanelDetailRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsServiceSubconShrinkagePanelsQueryHandler CreateGetXlsServiceSubconShrinkagePanelsQueryHandler()
        {
            return new GetXlsServiceSubconShrinkagePanelsQueryHandler(_MockStorage.Object/*, serviceProviderMock.Object*/);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsServiceSubconShrinkagePanelsQueryHandler unitUnderTest = CreateGetXlsServiceSubconShrinkagePanelsQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconShrinkagePanel = Guid.NewGuid();
            Guid guidSubconShrinkagePanelItem = Guid.NewGuid();
            Guid guidSubconShrinkagePanelDetail = Guid.NewGuid();


            GetXlsSubconServiceSubconShrinkagePanelsQuery getMonitoring = new GetXlsSubconServiceSubconShrinkagePanelsQuery(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2), "token");




            _mockgarmentSubconShrinkagePanelRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>
                {
                    new GarmentServiceSubconShrinkagePanel(guidSubconShrinkagePanel, "serviceSubconShrinkagePanelNo", DateTimeOffset.Now, "remark", true, 5, "uomUnit").GetReadModel()

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

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task Handle_StateUnderTest_Expected()
        {
            GetXlsServiceSubconShrinkagePanelsQueryHandler unitUnderTest = CreateGetXlsServiceSubconShrinkagePanelsQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconShrinkagePanel = Guid.NewGuid();
            Guid guidSubconShrinkagePanelItem = Guid.NewGuid();
            Guid guidSubconShrinkagePanelDetail = Guid.NewGuid();


            GetXlsSubconServiceSubconShrinkagePanelsQuery getMonitoring = new GetXlsSubconServiceSubconShrinkagePanelsQuery(DateTime.Now.AddDays(2), DateTime.Now,  "token");




            _mockgarmentSubconShrinkagePanelRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconShrinkagePanelReadModel>
                {
                    new GarmentServiceSubconShrinkagePanel(guidSubconShrinkagePanel, "serviceSubconShrinkagePanelNo", DateTimeOffset.Now, "remark", true, 5, "uomUnit").GetReadModel()

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

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
