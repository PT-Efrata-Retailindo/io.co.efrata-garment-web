using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.Monitoring;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleCuttingOuts.Monitoring
{
    public class GetSampleCuttingMonitoringQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleCuttingOutRepository> _mockGarmentSampleCuttingOutRepository;
        private readonly Mock<IGarmentSampleCuttingOutItemRepository> _mockGarmentSampleCuttingOutItemRepository;
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockGarmentSampleCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockGarmentSampleCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockGarmentSampleCuttingInDetailRepository;
        private readonly Mock<IGarmentSampleAvalComponentRepository> _mockGarmentSampleAvalComponentRepository;
        private readonly Mock<IGarmentSampleAvalComponentItemRepository> _mockGarmentSampleAvalComponentItemRepository;
        private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;
        //private readonly Mock<IGarmentBalanceCuttingRepository> _mockGarmentBalanceCuttingRepository;
        protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;
        private readonly Mock<IGarmentSampleRequestRepository> _mockGarmentSampleRequestRepository;
        private readonly Mock<IGarmentSampleRequestProductRepository> _mockGarmentSampleRequestProductRepository;

        public GetSampleCuttingMonitoringQueryHandlerTest()
        {
            _mockGarmentSampleCuttingOutRepository = CreateMock<IGarmentSampleCuttingOutRepository>();
            _mockGarmentSampleCuttingOutItemRepository = CreateMock<IGarmentSampleCuttingOutItemRepository>();
            _mockGarmentSampleCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockGarmentSampleCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockGarmentSampleCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockGarmentSampleCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockGarmentSampleAvalComponentRepository = CreateMock<IGarmentSampleAvalComponentRepository>();
            _mockGarmentSampleAvalComponentItemRepository = CreateMock<IGarmentSampleAvalComponentItemRepository>();
            _mockGarmentSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();
            //_mockGarmentBalanceCuttingRepository = CreateMock<IGarmentBalanceCuttingRepository>();
            _mockGarmentSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
            _mockGarmentSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalComponentItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestProductRepository);
            //_MockStorage.SetupStorage(_mockGarmentBalanceCuttingRepository);

            //SalesDataSettings.Endpoint = "https://com-danliris-service-sales.azurewebsites.net/v1/";
            serviceProviderMock = new Mock<IServiceProvider>();
            _mockhttpService = CreateMock<IHttpClientService>();


            //List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel> {
            //    new CostCalViewModel
            //    {
            //        ro="ro",
            //        comodityName="comodityName",
            //        buyerCode="buyerCode",
            //        hours=10
            //    }
            //};

            //_mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
            //    .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
            serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);

        }
        private GetSampleCuttingMonitoringQueryHandler CreateGetSampleCuttingMonitoringQueryHandler()
        {
            return new GetSampleCuttingMonitoringQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            try
            {
                // Arrange
                GetSampleCuttingMonitoringQueryHandler unitUnderTest = CreateGetSampleCuttingMonitoringQueryHandler();
                CancellationToken cancellationToken = CancellationToken.None;

                Guid guidPrepare = Guid.NewGuid();
                Guid guidPrepareItem = Guid.NewGuid();
                Guid guidCuttingIn = Guid.NewGuid();
                Guid guidCuttingInItem = Guid.NewGuid();
                Guid guidCuttingInDetail = Guid.NewGuid();
                Guid guidAvalComponent = Guid.NewGuid();
                Guid guidAvalComponentItem = Guid.NewGuid();
                Guid guidCuttingOut = Guid.NewGuid();
                Guid guidCuttingOutItem = Guid.NewGuid();
                Guid guidCuttingOutDetail = Guid.NewGuid();
                GetSampleCuttingMonitoringQuery getMonitoring = new GetSampleCuttingMonitoringQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");


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
                    new GarmentSampleCuttingIn(guidCuttingIn,"cutInNo","Main Fabric","cuttingFrom","ro","article",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,4.5).GetReadModel()
                    }.AsQueryable());

                _mockGarmentSampleCuttingInDetailRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSampleCuttingInDetailReadModel>
                    {
                    new GarmentSampleCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"productCode","productName","designColor","fabricType",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
                    }.AsQueryable());

                _mockGarmentSampleAvalComponentItemRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSampleAvalComponentItemReadModel>
                    {
                    new GarmentSampleAvalComponentItem(guidAvalComponentItem,guidAvalComponent,guidCuttingInDetail,new Guid(),new Guid(),new Domain.Shared.ValueObjects.ProductId(1),"productCode","productName","designColor","color",10,0, new Domain.Shared.ValueObjects.SizeId(1),"sizeName",100,100).GetReadModel()
                    }.AsQueryable());
                _mockGarmentSampleAvalComponentRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSampleAvalComponentReadModel>
                    {
                    new GarmentSampleAvalComponent(guidAvalComponent,"avalComponentNo",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"unitCode","unitName","avalComponentType","ro1","article",new Domain.Shared.ValueObjects.GarmentComodityId(1),"comodityCode","comodityName",DateTimeOffset.Now, false).GetReadModel()
                    }.AsQueryable());

                _mockGarmentSampleCuttingOutItemRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSampleCuttingOutItemReadModel>
                    {
                    new GarmentSampleCuttingOutItem(guidCuttingOutItem,guidCuttingIn,guidCuttingInDetail,guidCuttingOut,new Domain.Shared.ValueObjects.ProductId(1),"productCode","productName","designColor",100).GetReadModel()
                    }.AsQueryable());
                _mockGarmentSampleCuttingOutRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSampleCuttingOutReadModel>
                    {
                     new GarmentSampleCuttingOut(guidCuttingOut,"cutOutNo", "cuttingOutType",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"unitFromCode","unitFromName",DateTime.Now,"ro","article",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",false).GetReadModel()
                    }.AsQueryable());

                var guidGarmentSamplePreparing = Guid.NewGuid();
                _mockGarmentSamplePreparingRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSamplePreparingReadModel>
                    {
                     new Domain.GarmentSample.SamplePreparings.GarmentSamplePreparing(guidGarmentSamplePreparing,1, "UENNo", new UnitDepartmentId(1), "UnitCode", "UnitName", DateTimeOffset.Now, "ro", "Article", true,new Domain.Shared.ValueObjects.BuyerId(1), null,null).GetReadModel()
                    }.AsQueryable());

                var garmentPreparingItem = Guid.NewGuid();
                _mockGarmentSamplePreparingItemRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSamplePreparingItemReadModel>
                    {
                     new GarmentSamplePreparingItem(guidGarmentSamplePreparing, 0, new ProductId(1), null, null, null, 0, new UomId(1), null, null, 0, 0, Guid.Empty,null).GetReadModel()
                    }.AsQueryable());
                var guidSampleReqId = Guid.NewGuid();
                _mockGarmentSampleRequestProductRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleRequestProductReadModel>
               {
                    new Domain.GarmentSample.SampleRequests.GarmentSampleRequestProduct (guidSampleReqId,guidSampleReqId,"","","",new Domain.Shared.ValueObjects.SizeId(1),"","",10,1).GetReadModel()
               }.AsQueryable());
                _mockGarmentSampleRequestRepository
                    .Setup(s => s.Query)
                    .Returns(new List<GarmentSampleRequestReadModel>
                    {
                    new Domain.GarmentSample.SampleRequests.GarmentSampleRequest (guidSampleReqId,"","","ro","ro",DateTimeOffset.Now,new Domain.Shared.ValueObjects.BuyerId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","","","",DateTimeOffset.Now,"","","",true,true,DateTimeOffset.Now,"",false,DateTimeOffset.Now,"","",false,DateTimeOffset.Now,"","","","","","",new Domain.Shared.ValueObjects.SectionId(1),"", null).GetReadModel()
                    }.AsQueryable());

                // Act
                var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

                // Assert
                result.Should().NotBeNull();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
