using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconGarmentWashReport;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconGarmentWashReport
{
    public class GarmentXlsSubconGarmentWashQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockgarmentSubconGarmentWashRepository;
        private readonly Mock<IGarmentServiceSubconSewingItemRepository> _mockgarmentSubconGarmentWashItemRepository;
        private readonly Mock<IGarmentServiceSubconSewingDetailRepository> _mockgarmentSubconGarmentWashDetailRepository;
    
        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconGarmentWashQueryHandlerTest()
        {

            _mockgarmentSubconGarmentWashRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockgarmentSubconGarmentWashItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();
            _mockgarmentSubconGarmentWashDetailRepository = CreateMock<IGarmentServiceSubconSewingDetailRepository>();
        
            _MockStorage.SetupStorage(_mockgarmentSubconGarmentWashRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconGarmentWashItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconGarmentWashDetailRepository);
       
            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsGarmentSubconGarmentWashReportQueryHandler CreateGetXlsGarmentSubconGarmentWashReportQueryHandler()
        {
            return new GetXlsGarmentSubconGarmentWashReportQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsGarmentSubconGarmentWashReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconGarmentWashReportQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidSubconSewing = Guid.NewGuid();
            Guid guidSubconSewingItem = Guid.NewGuid();
            Guid guidSubconSewingDetail = Guid.NewGuid();
            Guid guidSubconSewingIn = Guid.NewGuid();
            Guid guidSubconSewingInItem = Guid.NewGuid();

            GetXlsGarmentSubconGarmentWashReporQuery getMonitoring = new GetXlsGarmentSubconGarmentWashReporQuery(1, 25, "{}", DateTime.Now, DateTime.Now);

            _mockgarmentSubconGarmentWashRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>
                {
                        new GarmentServiceSubconSewing(guidSubconSewing, "SJS001", DateTimeOffset.Now, false, new BuyerId (1), "BuyerCode", "BuyerName", 5, "uomUnit").GetReadModel()
     
                }.AsQueryable());

            _mockgarmentSubconGarmentWashItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingItemReadModel>
                {
                    new GarmentServiceSubconSewingItem(guidSubconSewingItem, guidSubconSewing, "RONo", "Article", new GarmentComodityId (1), "ComodityCode", "ComodityName", new BuyerId (1), "BuyerCode", "BuyerName",new UnitDepartmentId(1), "unitCode", "unitName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconGarmentWashDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingDetailReadModel>
                {
                    new GarmentServiceSubconSewingDetail(guidSubconSewingDetail, guidSubconSewingItem, guidSubconSewingIn, guidSubconSewingInItem, new ProductId (1), "ProductCode", "ProductName", "DesignColor", 10, new UomId (1), "UomUnit", new UnitDepartmentId (1), "UnitNCode", "UnitName", "Remark", "Color").GetReadModel()

                }.AsQueryable());
           
            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);
        
            // Assert
            result.Should().NotBeNull();           

        }
    }
}
