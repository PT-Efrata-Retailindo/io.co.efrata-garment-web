using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentSample.GarmentMonitoringSampleStockFlows.Queries;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.GarmentMonitoringSampleStockFlows
{
	public class XlsMonitoringSampleStockFlowCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentSampleCuttingOutRepository> _mockGarmentCuttingOutRepository;
		private readonly Mock<IGarmentSampleCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
		private readonly Mock<IGarmentSampleCuttingOutDetailRepository> _mockGarmentCuttingOutDetailRepository;
		private readonly Mock<IGarmentSampleCuttingInRepository> _mockGarmentCuttingInRepository;
		private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
		private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
		private readonly Mock<IGarmentSampleSewingOutRepository> _mockGarmentSewingOutRepository;
		private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
		private readonly Mock<IGarmentSampleFinishingOutRepository> _mockGarmentFinishingOutRepository;
		private readonly Mock<IGarmentSampleFinishingOutItemRepository> _mockGarmentFinishingOutItemRepository;
		private readonly Mock<IGarmentSampleSewingInRepository> _mockSewingInRepository;
		private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingInItemRepository;
		private readonly Mock<IGarmentSampleAvalComponentRepository> _mockGarmentSampleAvalComponentRepository;
		private readonly Mock<IGarmentSampleAvalComponentItemRepository> _mockGarmentSampleAvalComponentItemRepository;
		private readonly Mock<IGarmentSampleFinishingInRepository> _mockFinishingInRepository;
		private readonly Mock<IGarmentSampleFinishingInItemRepository> _mockFinishingInItemRepository;
		private readonly Mock<IGarmentSampleExpenditureGoodRepository> _mockExpenditureGoodRepository;
		private readonly Mock<IGarmentSampleExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;
		private readonly Mock<IGarmentSamplePreparingRepository> _mockSamplePreparingRepository;
		private readonly Mock<IGarmentSamplePreparingItemRepository> _mockSamplePreparingItemRepository;
		private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
		private readonly Mock<IGarmentSampleRequestRepository> _mockGarmentSampleRequestRepository;
		private readonly Mock<IGarmentSampleRequestProductRepository> _mockGarmentSampleRequestProductRepository;


		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;

		public XlsMonitoringSampleStockFlowCommandHandlerTest()
		{

			_mockGarmentFinishingOutRepository = CreateMock<IGarmentSampleFinishingOutRepository>();
			_mockGarmentFinishingOutItemRepository = CreateMock<IGarmentSampleFinishingOutItemRepository>();
			_mockGarmentSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
			_mockGarmentSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
			_mockGarmentCuttingOutRepository = CreateMock<IGarmentSampleCuttingOutRepository>();
			_mockGarmentCuttingOutItemRepository = CreateMock<IGarmentSampleCuttingOutItemRepository>();
			_mockGarmentCuttingOutDetailRepository = CreateMock<IGarmentSampleCuttingOutDetailRepository>();
			_mockGarmentSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
			_mockGarmentSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
			_mockGarmentCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
			_mockGarmentCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
			_mockGarmentCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
			_mockGarmentSampleAvalComponentRepository = CreateMock<IGarmentSampleAvalComponentRepository>();
			_mockGarmentSampleAvalComponentItemRepository = CreateMock<IGarmentSampleAvalComponentItemRepository>();
			_mockSewingInRepository = CreateMock<IGarmentSampleSewingInRepository>();
			_mockSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();
			_mockFinishingInRepository = CreateMock<IGarmentSampleFinishingInRepository>();
			_mockFinishingInItemRepository = CreateMock<IGarmentSampleFinishingInItemRepository>();
			_mockExpenditureGoodRepository = CreateMock<IGarmentSampleExpenditureGoodRepository>();
			_mockExpenditureGoodItemRepository = CreateMock<IGarmentSampleExpenditureGoodItemRepository>();
			_mockExpenditureGoodItemRepository = CreateMock<IGarmentSampleExpenditureGoodItemRepository>();
			_mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
			_mockSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
			_mockSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();
			_mockGarmentSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
			_mockGarmentSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();
			_MockStorage.SetupStorage(_mockGarmentSampleRequestRepository);
			_MockStorage.SetupStorage(_mockGarmentSampleRequestProductRepository);

			_MockStorage.SetupStorage(_mockSamplePreparingRepository);
			_MockStorage.SetupStorage(_mockSamplePreparingItemRepository);
			_MockStorage.SetupStorage(_mockComodityPriceRepository);
			_MockStorage.SetupStorage(_mockExpenditureGoodRepository);
			_MockStorage.SetupStorage(_mockExpenditureGoodItemRepository);

			_MockStorage.SetupStorage(_mockFinishingInRepository);
			_MockStorage.SetupStorage(_mockFinishingInItemRepository);
			_MockStorage.SetupStorage(_mockSewingInRepository);
			_MockStorage.SetupStorage(_mockSewingInItemRepository);
			_MockStorage.SetupStorage(_mockGarmentSampleAvalComponentRepository);
			_MockStorage.SetupStorage(_mockGarmentSampleAvalComponentItemRepository);

			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);

			_MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutDetailRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
			serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();
			 
            //List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel> {
            //    new CostCalViewModel
            //    {
            //        ro="ro",
            //        comodityName="",
            //        buyerCode="buyer",
            //        hours=10
            //    }
            //};

            //_mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
            // .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
            serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
        }
        private GetXlsMonitoringSampleStockFlowQueryHandler CreateGetMonitoringSampleFlowQueryHandler()
		{
			return new GetXlsMonitoringSampleStockFlowQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior_bookkeeping()
		{
			// Arrange
			GetXlsMonitoringSampleStockFlowQueryHandler unitUnderTest = CreateGetMonitoringSampleFlowQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidLoading = Guid.NewGuid();
			Guid guidLoadingItem = Guid.NewGuid();
			Guid guidCuttingOut = Guid.NewGuid();
			Guid guidCuttingOutItem = Guid.NewGuid();
			Guid guidCuttingOutDetail = Guid.NewGuid();
			Guid guidFinishingOut = Guid.NewGuid();
			Guid guidFinishingOutItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();
			Guid guidSewingIn = Guid.NewGuid();
			Guid guidSewingInItem = Guid.NewGuid();
			Guid guidSewingDO = Guid.NewGuid();
			Guid guidSewingDOItem = Guid.NewGuid();
			Guid guidCuttingIn = Guid.NewGuid();
			Guid guidCuttingInItem = Guid.NewGuid();
			Guid guidFinishingIn = Guid.NewGuid();
			Guid guidFinishingInItem = Guid.NewGuid();
			Guid guidAdjustment = Guid.NewGuid();
			Guid guidAval = Guid.NewGuid();
			Guid guidExpenditure = Guid.NewGuid();
			Guid guidExpenditureReturn = Guid.NewGuid();
			Guid guidAdjustmentItem = Guid.NewGuid();
			GetXlsMonitoringSampleStockFlowQuery getMonitoring = new GetXlsMonitoringSampleStockFlowQuery(1, 25, "{}", 1, null, DateTime.Now.AddDays(-5), DateTime.Now,"bookkeeping", "token");


			_mockSamplePreparingItemRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentSamplePreparingItemReadModel>
			{
					new GarmentSamplePreparingItem(guidCuttingIn,1,new  Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), "", "","",10,new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1),"","",10,10,guidCuttingIn,"ro").GetReadModel()
			}.AsQueryable());

			_mockSamplePreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingReadModel>
				{
						new Domain.GarmentSample.SamplePreparings.GarmentSamplePreparing(guidCuttingIn,1,"",new Domain.GarmentSample.SamplePreparings.ValueObjects.UnitDepartmentId(1),"","",DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),"ro","",true,new BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingOutDetailRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentSampleCuttingOutDetailReadModel>
			{
					 new GarmentSampleCuttingOutDetail(new Guid(),guidCuttingOutItem,new SizeId(1),"","",100,100,new UomId(1),"",10,10).GetReadModel()
			}.AsQueryable());

			_mockGarmentCuttingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingOutItemReadModel>
				{
					 new GarmentSampleCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
				}.AsQueryable());
			_mockGarmentCuttingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingOutReadModel>
				{
					  new GarmentSampleCuttingOut(guidCuttingOut, "", "SEWING",new UnitDepartmentId(1),"","",DateTime.Now.AddDays(-1),"ro","article",new UnitDepartmentId(1),"","", new GarmentComodityId(1),"cm","cmo",false).GetReadModel()
				}.AsQueryable());



			_mockGarmentFinishingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleFinishingOutItemReadModel>
				{
					 new GarmentSampleFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",10, new UomId(1),"","",10,10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleFinishingOutReadModel>
				{
					 new GarmentSampleFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now.AddDays(-1),"ro","",new UnitDepartmentId(1),"","", new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());


			_mockGarmentSewingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingOutItemReadModel>
				{
					 new GarmentSampleSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingOutReadModel>
				{
					 new GarmentSampleSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","FINISHING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","", new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());




			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInDetailReadModel>
				{
					 new GarmentSampleCuttingInDetail(new Guid(),guidCuttingInItem,new Guid(),new Guid(),new Guid(),new ProductId(1),"","","","Main Fabric",10,new UomId(1),"",10,new UomId(1),"",10,100,100,1,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInItemReadModel>
				{
					 new GarmentSampleCuttingInItem(guidCuttingInItem,guidCuttingIn,new Guid(),1,"",guidSewingOut,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInReadModel>
				{
					 new GarmentSampleCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now.AddDays(-1),1).GetReadModel()
				}.AsQueryable());

			_mockGarmentSampleAvalComponentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleAvalComponentItemReadModel>
				{
					 new GarmentSampleAvalComponentItem(guidCuttingInItem,guidCuttingIn,new Guid(),new Guid(),new Guid(),new ProductId(1),"","","","",10,10,new SizeId(1),"",10,10).GetReadModel()

				}.AsQueryable());
			_mockGarmentSampleAvalComponentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleAvalComponentReadModel>
				{
					 new GarmentSampleAvalComponent(guidCuttingInItem,"",new UnitDepartmentId(1),"","","","ro","", new GarmentComodityId(1),"","",DateTimeOffset.Now,true).GetReadModel()

				}.AsQueryable());

			_mockExpenditureGoodItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleExpenditureGoodItemReadModel>
				{
					 new GarmentSampleExpenditureGoodItem(guidCuttingInItem,guidCuttingIn,new Guid(),new SizeId(1),"",10,10,new UomId(1),"","",10,10).GetReadModel()

				}.AsQueryable());
			_mockExpenditureGoodRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleExpenditureGoodReadModel>
				{
					 new GarmentSampleExpenditureGood(guidCuttingInItem,"","",new UnitDepartmentId(1),"","","ro","", new GarmentComodityId(1),"","",new BuyerId(1),"","", DateTimeOffset.Now,"","",9,"",true,9).GetReadModel()

				}.AsQueryable());


			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInReadModel>
				{
					 new GarmentSampleCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now.AddDays(-1),1).GetReadModel()
				}.AsQueryable());

			_mockComodityPriceRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentComodityPriceReadModel>
				{
					 new GarmentComodityPrice(guidCuttingIn,true,DateTime.Now,new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",100).GetReadModel()
				}.AsQueryable());
			_mockSewingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingInReadModel>
				{
					new GarmentSampleSewingIn(guidSewingOut,"","",new Guid(),"",new UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","rono","article",new GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
				}.AsQueryable());
			_mockSewingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingInItemReadModel>
				{
					new GarmentSampleSewingInItem(new Guid(),guidSewingOut,new Guid(),new Guid(),Guid.Empty,Guid.Empty,new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"","",1,1,1).GetReadModel()
				}.AsQueryable());

			_mockFinishingInRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentSampleFinishingInReadModel>
			{
					 new GarmentSampleFinishingIn(guidCuttingIn,"","Main Fabric",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","", DateTimeOffset.Now.AddDays(-1),new GarmentComodityId(1),"","",1,"","").GetReadModel()
			}.AsQueryable());


			_mockFinishingInItemRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentSampleFinishingInItemReadModel>
			{
					 new GarmentSampleFinishingInItem(guidCuttingIn,guidCuttingIn,guidSewingOutItem,guidSewingOutItem,guidSewingOut,new SizeId(1),"",new ProductId(1),"","","",10,10,new UomId(1),"","",10,10,1).GetReadModel()
			}.AsQueryable());

			_mockComodityPriceRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentComodityPriceReadModel>
			{
					 new GarmentComodityPrice(guidCuttingIn,true,DateTime.Now,new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",100).GetReadModel()
			}.AsQueryable());

			_mockGarmentSampleRequestProductRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleRequestProductReadModel>
				{
					new GarmentSampleRequestProduct(guidSewingOutItem,guidSewingOutItem,"","","",new SizeId(1),"","", 100,1).GetReadModel()
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

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GetXlsMonitoringSampleStockFlowQueryHandler unitUnderTest = CreateGetMonitoringSampleFlowQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidLoading = Guid.NewGuid();
			Guid guidLoadingItem = Guid.NewGuid();
			Guid guidCuttingOut = Guid.NewGuid();
			Guid guidCuttingOutItem = Guid.NewGuid();
			Guid guidCuttingOutDetail = Guid.NewGuid();
			Guid guidFinishingOut = Guid.NewGuid();
			Guid guidFinishingOutItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();
			Guid guidSewingIn = Guid.NewGuid();
			Guid guidSewingInItem = Guid.NewGuid();
			Guid guidSewingDO = Guid.NewGuid();
			Guid guidSewingDOItem = Guid.NewGuid();
			Guid guidCuttingIn = Guid.NewGuid();
			Guid guidCuttingInItem = Guid.NewGuid();
			Guid guidFinishingIn = Guid.NewGuid();
			Guid guidFinishingInItem = Guid.NewGuid();
			Guid guidAdjustment = Guid.NewGuid();
			Guid guidAval = Guid.NewGuid();
			Guid guidExpenditure = Guid.NewGuid();
			Guid guidExpenditureReturn = Guid.NewGuid();
			Guid guidAdjustmentItem = Guid.NewGuid();
			GetXlsMonitoringSampleStockFlowQuery getMonitoring = new GetXlsMonitoringSampleStockFlowQuery(1, 25, "{}", 1, "ro", DateTime.Now.AddDays(-5), DateTime.Now, "", "token");

			_mockSamplePreparingItemRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentSamplePreparingItemReadModel>
			{
					new GarmentSamplePreparingItem(guidCuttingIn,1,new  Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), "", "","",10,new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1),"","",10,10,guidCuttingIn,"ro").GetReadModel()
			}.AsQueryable());

			_mockSamplePreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingReadModel>
				{
						new Domain.GarmentSample.SamplePreparings.GarmentSamplePreparing(guidCuttingIn,1,"",new Domain.GarmentSample.SamplePreparings.ValueObjects.UnitDepartmentId(1),"","",DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),"ro","",true,new BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingOutDetailRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentSampleCuttingOutDetailReadModel>
			{
					 new GarmentSampleCuttingOutDetail(new Guid(),guidCuttingOutItem,new SizeId(1),"","",100,100,new UomId(1),"",10,10).GetReadModel()
			}.AsQueryable());

			_mockGarmentCuttingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingOutItemReadModel>
				{
					 new GarmentSampleCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
				}.AsQueryable());
			_mockGarmentCuttingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingOutReadModel>
				{
					  new GarmentSampleCuttingOut(guidCuttingOut, "", "SEWING",new UnitDepartmentId(1),"","",DateTime.Now.AddDays(-1),"ro","article",new UnitDepartmentId(1),"","", new GarmentComodityId(1),"cm","cmo",false).GetReadModel()
				}.AsQueryable());



			_mockGarmentFinishingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleFinishingOutItemReadModel>
				{
					 new GarmentSampleFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",10, new UomId(1),"","",10,10,10).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleFinishingOutReadModel>
				{
					 new GarmentSampleFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now.AddDays(-1),"ro","",new UnitDepartmentId(1),"","", new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());


			_mockGarmentSewingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingOutItemReadModel>
				{
					 new GarmentSampleSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingOutReadModel>
				{
					 new GarmentSampleSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","FINISHING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","", new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());




			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInDetailReadModel>
				{
					 new GarmentSampleCuttingInDetail(new Guid(),guidCuttingInItem,new Guid(),new Guid(),new Guid(),new ProductId(1),"","","","Main Fabric",10,new UomId(1),"",10,new UomId(1),"",10,100,100,1,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInItemReadModel>
				{
					 new GarmentSampleCuttingInItem(guidCuttingInItem,guidCuttingIn,new Guid(),1,"",guidSewingOut,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInReadModel>
				{
					 new GarmentSampleCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now.AddDays(-1),1).GetReadModel()
				}.AsQueryable());

			_mockGarmentSampleAvalComponentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleAvalComponentItemReadModel>
				{
					 new GarmentSampleAvalComponentItem(guidCuttingInItem,guidCuttingIn,new Guid(),new Guid(),new Guid(),new ProductId(1),"","","","",10,10,new SizeId(1),"",10,10).GetReadModel()

				}.AsQueryable());
			_mockGarmentSampleAvalComponentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleAvalComponentReadModel>
				{
					 new GarmentSampleAvalComponent(guidCuttingInItem,"",new UnitDepartmentId(1),"","","","ro","", new GarmentComodityId(1),"","",DateTimeOffset.Now,true).GetReadModel()

				}.AsQueryable());

			_mockExpenditureGoodItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleExpenditureGoodItemReadModel>
				{
					 new GarmentSampleExpenditureGoodItem(guidCuttingInItem,guidCuttingIn,new Guid(),new SizeId(1),"",10,10,new UomId(1),"","",10,10).GetReadModel()

				}.AsQueryable());
			_mockExpenditureGoodRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleExpenditureGoodReadModel>
				{
					 new GarmentSampleExpenditureGood(guidCuttingInItem,"","",new UnitDepartmentId(1),"","","ro","", new GarmentComodityId(1),"","",new BuyerId(1),"","", DateTimeOffset.Now,"","",9,"",true,9).GetReadModel()

				}.AsQueryable());


			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInReadModel>
				{
					 new GarmentSampleCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now.AddDays(-1),1).GetReadModel()
				}.AsQueryable());

			_mockComodityPriceRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentComodityPriceReadModel>
				{
					 new GarmentComodityPrice(guidCuttingIn,true,DateTime.Now,new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",100).GetReadModel()
				}.AsQueryable());
			_mockSewingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingInReadModel>
				{
					new GarmentSampleSewingIn(guidSewingOut,"","",new Guid(),"",new UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","rono","article",new GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
				}.AsQueryable());
			_mockSewingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingInItemReadModel>
				{
					new GarmentSampleSewingInItem(new Guid(),guidSewingOut,new Guid(),new Guid(),Guid.Empty,Guid.Empty,new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"","",1,1,1).GetReadModel()
				}.AsQueryable());

			_mockFinishingInRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentSampleFinishingInReadModel>
			{
					 new GarmentSampleFinishingIn(guidCuttingIn,"","Main Fabric",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","", DateTimeOffset.Now.AddDays(-1),new GarmentComodityId(1),"","",1,"","").GetReadModel()
			}.AsQueryable());


			_mockFinishingInItemRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentSampleFinishingInItemReadModel>
			{
					 new GarmentSampleFinishingInItem(guidCuttingIn,guidCuttingIn,guidSewingOutItem,guidSewingOutItem,guidSewingOut,new SizeId(1),"",new ProductId(1),"","","",10,10,new UomId(1),"","",10,10,1).GetReadModel()
			}.AsQueryable());

			_mockComodityPriceRepository
			.Setup(s => s.Query)
			.Returns(new List<GarmentComodityPriceReadModel>
			{
					 new GarmentComodityPrice(guidCuttingIn,true,DateTime.Now,new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",100).GetReadModel()
			}.AsQueryable());

			_mockGarmentSampleRequestProductRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleRequestProductReadModel>
				{
					new GarmentSampleRequestProduct(guidSewingOutItem,guidSewingOutItem,"","","",new SizeId(1),"","", 100,1).GetReadModel()
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
