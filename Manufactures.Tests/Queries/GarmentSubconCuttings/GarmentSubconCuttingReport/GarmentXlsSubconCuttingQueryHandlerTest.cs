using System;
using System.Collections.Generic;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.Queries;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubconCuttings.GarmentSubconCuttingReport
{
    public class GarmentXlsSubconCuttingQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconCuttingRepository> _mockgarmentSubconCuttingRepository;
        private readonly Mock<IGarmentServiceSubconCuttingItemRepository> _mockgarmentSubconCuttingItemRepository;
        private readonly Mock<IGarmentServiceSubconCuttingDetailRepository> _mockgarmentSubconCutingDetailRepository;
        private readonly Mock<IGarmentServiceSubconCuttingSizeRepository> _mockgarmentSubconCuttingSizeRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconCuttingQueryHandlerTest()
        {

            _mockgarmentSubconCuttingRepository = CreateMock<IGarmentServiceSubconCuttingRepository>();
            _mockgarmentSubconCuttingItemRepository = CreateMock<IGarmentServiceSubconCuttingItemRepository>();
            _mockgarmentSubconCutingDetailRepository = CreateMock<IGarmentServiceSubconCuttingDetailRepository>();
            _mockgarmentSubconCuttingSizeRepository = CreateMock<IGarmentServiceSubconCuttingSizeRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCuttingItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCutingDetailRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconCuttingSizeRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsServiceSubconCuttingQueryHandler CreateGetXlsServiceSubconCuttingQueryHandler()
        {
            return new GetXlsServiceSubconCuttingQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsServiceSubconCuttingQueryHandler unitUnderTest = CreateGetXlsServiceSubconCuttingQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidSubconCutting = Guid.NewGuid();
            Guid guidSubconCuttingItem = Guid.NewGuid();
            Guid guidSubconCuttingDetail = Guid.NewGuid();
            Guid guidSubconCuttingSize = Guid.NewGuid();
            Guid guidSubconCuttingIn = Guid.NewGuid();
            Guid guidSubconCuttingInDetail = Guid.NewGuid();
            
            GetXlsServiceSubconCuttingQuery getMonitoring = new GetXlsServiceSubconCuttingQuery(1, 25, "{}", DateTime.Now, DateTime.Now, "token");

            _mockgarmentSubconCuttingRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingReadModel>
                {
                        new GarmentServiceSubconCutting(guidSubconCutting, "SJCB001", "SCCuttingType", new UnitDepartmentId (1), "UnitCode", "UnitName", DateTimeOffset.Now, false, new BuyerId (1), "BuyerCode", "BuyerName", new UomId(1), "uomUnit", 1).GetReadModel()
     
                }.AsQueryable());

            _mockgarmentSubconCuttingItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingItemReadModel>
                {
                    new GarmentServiceSubconCuttingItem(guidSubconCuttingItem, guidSubconCutting, "RONo", "Article", new GarmentComodityId (1), "ComodityCode", "ComodityName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconCutingDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingDetailReadModel>
                {
                    new GarmentServiceSubconCuttingDetail(guidSubconCuttingDetail, guidSubconCuttingItem, "DesignColor", 10).GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconCuttingSizeRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconCuttingSizeReadModel>
                {
                    new GarmentServiceSubconCuttingSize(guidSubconCuttingSize, new SizeId (1), "SizeName", 10, new UomId (1), "UomUnit", "Color", guidSubconCuttingDetail, guidSubconCuttingIn, guidSubconCuttingInDetail, new ProductId (1), "ProductCode", "ProductName").GetReadModel(),
                }.AsQueryable());
           
            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);
        
            // Assert
            result.Should().NotBeNull();           

        }
    }
}
