using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentAdjustments.CommandHandlers;
using Manufactures.Application.GarmentSewingOuts.Queries.MonitoringSewing;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentAdjustments.ValueObjects;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.MonitoringProductionStockFlow;
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
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;

namespace Manufactures.Tests.Queries.GarmentSewingOuts
{
	public class MonitoringSewingCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentSewingOutRepository> _mockGarmentSewingOutRepository;
		private readonly Mock<IGarmentSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
        private readonly Mock<IGarmentSewingInRepository> _mockGarmentSewingInRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockGarmentSewingInItemRepository;
		private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
		private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;
        private readonly Mock<IGarmentBalanceMonitoringProductionStockFlowRepository> _mockGarmentBalanceSewingRepository;
        private readonly Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
        private readonly Mock<IGarmentAdjustmentRepository> _mockGarmentAdjustmentRepository;
        private readonly Mock<IGarmentAdjustmentItemRepository> _mockGarmentAdjustmentItemRepository;

        protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;
		public MonitoringSewingCommandHandlerTest()
		{
			 _mockGarmentSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();
            _mockGarmentSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
              _mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockGarmentAdjustmentRepository = CreateMock<IGarmentAdjustmentRepository>();
            _mockGarmentAdjustmentItemRepository = CreateMock<IGarmentAdjustmentItemRepository>();
            _mockGarmentBalanceSewingRepository = CreateMock<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            _mockGarmentSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockGarmentSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
             
			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentAdjustmentRepository);
            _MockStorage.SetupStorage(_mockGarmentAdjustmentItemRepository);
            _MockStorage.SetupStorage(_mockGarmentBalanceSewingRepository);

            serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();

            List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel> {
                new CostCalViewModel
                {
                    ro="ro",
                    comodityName="",
                    buyerCode="",
                    hours=10
                }
            };
			_mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
			   .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
			serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
		}
		private GetMonitoringSewingQueryHandler CreateGetMonitoringSewingQueryHandler()
		{
			return new GetMonitoringSewingQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GetMonitoringSewingQueryHandler unitUnderTest = CreateGetMonitoringSewingQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidLoading = Guid.NewGuid();
			Guid guidLoadingItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();
            Guid guidCuttingIn = Guid.NewGuid();
            Guid guidCuttingInItem = Guid.NewGuid();
            Guid guidCuttingInDetail = Guid.NewGuid();
            Guid guidPrepare = Guid.NewGuid();
            Guid guidPrepareItem = Guid.NewGuid();
            Guid guidFinishingOut = Guid.NewGuid();
            Guid guidFinishingOutItem = Guid.NewGuid();

            GetMonitoringSewingQuery getMonitoring = new GetMonitoringSewingQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");

            _mockGarmentCuttingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentCuttingInItemReadModel>
               {
                    new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"uENNo",Guid.Empty,"sewingOutNo").GetReadModel()
               }.AsQueryable());

            _mockGarmentCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>
                {
                    new GarmentCuttingIn(guidCuttingIn,"cutInNo","Main Fabric","cuttingFrom","ro","article",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,4.5).GetReadModel()
                }.AsQueryable());

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    new GarmentCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"productCode","productName","designColor","fabricType",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
                }.AsQueryable());

          
			_mockGarmentSewingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingOutItemReadModel>
				{
					new GarmentSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSewingOutReadModel>
				{
					new GarmentSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());
            _mockGarmentSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>
                {
                    new GarmentSewingIn(guidSewingOut,"","",new Guid(),"",new UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","rono","article",new GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
                }.AsQueryable());
            _mockGarmentSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>
                {
                    new GarmentSewingInItem(new Guid(),guidSewingOut,new Guid(),new Guid(),new Guid(),new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"","",1,1,1).GetReadModel()
                }.AsQueryable());

            _mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					new GarmentPreparing(guidPrepare,1,"uenNo",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true,new BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			 
			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					new GarmentPreparingItem(guidPrepareItem,1,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentPreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,guidPrepare,null,"fasilitas").GetReadModel()
				}.AsQueryable());

            var garmentBalanceSewing = Guid.NewGuid();
            _mockGarmentBalanceSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentBalanceMonitoringProductionStockReadModel>
                {
                     new GarmentBalanceMonitoringProductionStocFlow("ro","buyer","article","comodityName",1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,garmentBalanceSewing).GetReadModel()
                }.AsQueryable());
            var garmentadjustmentId = Guid.NewGuid();
            _mockGarmentAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>
                {
                     new GarmentAdjustment(garmentadjustmentId,"no","type","ro","article",new UnitDepartmentId(1),"unitcode","unitname",DateTimeOffset.Now,new GarmentComodityId(1),"comoditycode","comodityname","").GetReadModel()
                }.AsQueryable());
            _mockGarmentAdjustmentItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentAdjustmentItemReadModel>
               {
                     new GarmentAdjustmentItem(new Guid(),new Guid(),new Guid(),new Guid(),new Guid(),new Guid(),new SizeId(1),"sizeName",new ProductId(1),"productCode","productName","",1,1,new UomId(1),"uom","color",10).GetReadModel()
               }.AsQueryable());
            // Act
            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}

        private PlaceGarmentAdjustmentCommandHandler CreatePlaceGarmentAdjustmentCommandHandler()
        {
            return new PlaceGarmentAdjustmentCommandHandler(_MockStorage.Object);
        }

    }
}
