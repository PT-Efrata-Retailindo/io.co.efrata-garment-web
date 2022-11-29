using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleExpenditureGoods
{
    public class GarmentSampleExpenditureGoodQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleExpenditureGoodRepository> _mockGarmentSampleExpenditureGoodRepository;
        private readonly Mock<IGarmentSampleExpenditureGoodItemRepository> _mockGarmentSampleExpenditureGoodItemRepository;
        private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockGarmentSampleCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockGarmentSampleCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockGarmentSampleCuttingInDetailRepository;
        protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;
        private readonly Mock<IGarmentSampleRequestRepository> _mockGarmentSampleRequestRepository;
        private readonly Mock<IGarmentSampleRequestProductRepository> _mockGarmentSampleRequestProductRepository;
        public GarmentSampleExpenditureGoodQueryHandlerTest()
        {
            _mockGarmentSampleExpenditureGoodRepository = CreateMock<IGarmentSampleExpenditureGoodRepository>();
            _mockGarmentSampleExpenditureGoodItemRepository = CreateMock<IGarmentSampleExpenditureGoodItemRepository>();
            _mockGarmentSampleCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockGarmentSampleCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockGarmentSampleCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockGarmentSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();
            _mockGarmentSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
            _mockGarmentSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleExpenditureGoodItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestProductRepository);

            serviceProviderMock = new Mock<IServiceProvider>();
            _mockhttpService = CreateMock<IHttpClientService>();

            List<PEBResultViewModel> pEBResultViews = new List<PEBResultViewModel> {
                new PEBResultViewModel
                {
                    BCDate = DateTime.MinValue,
                    BCNo = "",
                    BonNo = ""
                }
            };

            _mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.Is<string>(s => s.Contains("customs-reports/getPEB")), It.IsAny<string>(), It.IsAny<HttpContent>()))
              .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(pEBResultViews) + "}") });

            serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
        }
        private GarmentSampleExpenditureGoodQueryHandler CreateGetMonitoringExpenditureGoodQueryHandler()
        {
            return new GarmentSampleExpenditureGoodQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GarmentSampleExpenditureGoodQueryHandler unitUnderTest = CreateGetMonitoringExpenditureGoodQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidExpenditureGood = Guid.NewGuid();
            Guid guidExpenditureGoodItem = Guid.NewGuid();
            Guid guidSewingOut = Guid.NewGuid();
            Guid guidSewingOutItem = Guid.NewGuid();
            Guid guidCuttingIn = Guid.NewGuid();
            Guid guidCuttingInItem = Guid.NewGuid();
            Guid guidCuttingInDetail = Guid.NewGuid();
            Guid guidPrepare = Guid.NewGuid();
            Guid guidPrepareItem = Guid.NewGuid();

            GetMonitoringSampleExpenditureGoodQuery getMonitoring = new GetMonitoringSampleExpenditureGoodQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");

            _mockGarmentSampleCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInItemReadModel>
                {
                    new GarmentSampleCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"uENNo",Guid.Empty,"sewingOutNo").GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInReadModel>
                {
                    new GarmentSampleCuttingIn(guidCuttingIn,"cutInNo","Main Fabric","cuttingFrom","ro","article",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,4.5).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInDetailReadModel>
                {
                    new GarmentSampleCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"productCode","productName","designColor","fabricType",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>
                {
                    new GarmentSampleExpenditureGoodItem(guidExpenditureGoodItem,guidExpenditureGood,new Guid(),new SizeId(1),"",10,0,new UomId(1),"","",10,10).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>
                {
                    new GarmentSampleExpenditureGood(guidExpenditureGood,"","",new UnitDepartmentId(1),"","","ro","",new GarmentComodityId(1),"","",new BuyerId(1),"","",DateTimeOffset.Now,"","",10,"",true,1).GetReadModel()
                }.AsQueryable());


            _mockGarmentSamplePreparingRepository
                 .Setup(s => s.Query)
                 .Returns(new List<GarmentSamplePreparingReadModel>
                 {
                    new Domain.GarmentSample.SamplePreparings.GarmentSamplePreparing(guidPrepare,1,"uenNo",new Domain.GarmentSample.SamplePreparings.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true,new BuyerId(1), null,null).GetReadModel()
                 }.AsQueryable());

            _mockGarmentSamplePreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSamplePreparingItemReadModel>
                {
                    new GarmentSamplePreparingItem(guidPrepareItem,1,new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,guidPrepareItem,null).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleRequestRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestReadModel>
                {
                    new Manufactures.Domain.GarmentSample.SampleRequests.GarmentSampleRequest(guidSewingOutItem,"","","ro","",DateTimeOffset.Now,new BuyerId(1),"","",new GarmentComodityId(1),"","","","",DateTimeOffset.Now,"","","",true,true,DateTimeOffset.Now,"",false,null,"","",false,null,"","","","","","",new SectionId(1),"", null).GetReadModel()
                }.AsQueryable());
            // Act
            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}