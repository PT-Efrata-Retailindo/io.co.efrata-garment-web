using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
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

namespace Manufactures.Tests.Queries.GarmentMonitoringProductionStockFlows
{
	public class XlsMonitoringProductionStockFlowCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentCuttingOutRepository> _mockGarmentCuttingOutRepository;
		private readonly Mock<IGarmentCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
		private readonly Mock<IGarmentCuttingOutDetailRepository> _mockGarmentCuttingOutDetailRepository;
		private readonly Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
		private readonly Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
		private readonly Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
		private readonly Mock<IGarmentLoadingRepository> _mockGarmentLoadingRepository;
		private readonly Mock<IGarmentLoadingItemRepository> _mockGarmentLoadingItemRepository;
		private readonly Mock<IGarmentSewingOutRepository> _mockGarmentSewingOutRepository;
		private readonly Mock<IGarmentSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
		private readonly Mock<IGarmentFinishingOutRepository> _mockGarmentFinishingOutRepository;
		private readonly Mock<IGarmentFinishingOutItemRepository> _mockGarmentFinishingOutItemRepository;
		 protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;

		public XlsMonitoringProductionStockFlowCommandHandlerTest()
		{

			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentLoadingRepository);
			_MockStorage.SetupStorage(_mockGarmentLoadingItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutDetailRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);

			serviceProviderMock = new Mock<IServiceProvider>();
            _mockhttpService = CreateMock<IHttpClientService>();

            List<CostCalViewModel> costCalViewModels = new List<CostCalViewModel> {
                new CostCalViewModel
                {
                    ro="ro",
                    comodityName="",
                    buyerCode="buyer",
                    hours=10
                }
            };

            _mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
             .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
            serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
        }
        private GetXlsMonitoringProductionStockFlowQueryHandler CreateGetMonitoringProductionFlowQueryHandler()
		{
			return new GetXlsMonitoringProductionStockFlowQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}

		//[Fact]
		//public async Task Handle_StateUnderTest_ExpectedBehavior_bookkeeping()
		//{
		//	// Arrange
		//	GetXlsMonitoringProductionStockFlowQueryHandler unitUnderTest = CreateGetMonitoringProductionFlowQueryHandler();
		//	CancellationToken cancellationToken = CancellationToken.None;

		//	Guid guidLoading = Guid.NewGuid();
		//	Guid guidLoadingItem = Guid.NewGuid();
		//	Guid guidCuttingOut = Guid.NewGuid();
		//	Guid guidCuttingOutItem = Guid.NewGuid();
		//	Guid guidCuttingOutDetail = Guid.NewGuid();
		//	Guid guidFinishingOut = Guid.NewGuid();
		//	Guid guidFinishingOutItem = Guid.NewGuid();
		//	Guid guidSewingOut = Guid.NewGuid();
		//	Guid guidSewingOutItem = Guid.NewGuid();
		//	Guid guidSewingIn = Guid.NewGuid();
		//	Guid guidSewingInItem = Guid.NewGuid();
		//	Guid guidSewingDO = Guid.NewGuid();
		//	Guid guidSewingDOItem = Guid.NewGuid();
		//	Guid guidCuttingIn = Guid.NewGuid();
		//	Guid guidCuttingInItem = Guid.NewGuid();
		//	Guid guidFinishingIn = Guid.NewGuid();
		//	Guid guidFinishingInItem = Guid.NewGuid();
		//	Guid guidAdjustment = Guid.NewGuid();
		//	Guid guidAval = Guid.NewGuid();
		//	Guid guidExpenditure = Guid.NewGuid();
		//	Guid guidExpenditureReturn = Guid.NewGuid();
		//	Guid guidAdjustmentItem = Guid.NewGuid();
		//	GetXlsMonitoringProductionStockFlowQuery getMonitoring = new GetXlsMonitoringProductionStockFlowQuery(1, 25, "{}", 1, "ro", DateTime.Now.AddDays(-5), DateTime.Now, "bookkeeping", "token");

		//	_mockGarmentLoadingItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentLoadingItemReadModel>
		//		{
		//			new GarmentLoadingItem(guidLoadingItem,guidLoading,new Guid(),new SizeId(1),"",new ProductId(1),"","","",0,0,0, new UomId(1),"","",10).GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentLoadingRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentLoadingReadModel>
		//		{
		//			new GarmentLoading(guidLoading,"",new Guid(),"",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","").GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentCuttingOutDetailRepository
		//	.Setup(s => s.Query)
		//	.Returns(new List<GarmentCuttingOutDetailReadModel>
		//	{
		//			new GarmentCuttingOutDetail(new Guid(),guidCuttingOutItem,new SizeId(1),"","",100,100,new UomId(1),"",10,10).GetReadModel()
		//	}.AsQueryable());

		//	_mockGarmentCuttingOutItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingOutItemReadModel>
		//		{
		//			new GarmentCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
		//		}.AsQueryable());
		//	_mockGarmentCuttingOutRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingOutReadModel>
		//		{
		//			 new GarmentCuttingOut(guidCuttingOut, "", "SEWING",new UnitDepartmentId(1),"","",DateTime.Now.AddDays(-1),"ro","article",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"cm","cmo",false).GetReadModel()
		//		}.AsQueryable());



		//	_mockGarmentFinishingOutItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentFinishingOutItemReadModel>
		//		{
		//			new GarmentFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",10, new UomId(1),"","",10,10,10).GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentFinishingOutRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentFinishingOutReadModel>
		//		{
		//			new GarmentFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now.AddDays(-1),"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
		//		}.AsQueryable());


		//	_mockGarmentSewingOutItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentSewingOutItemReadModel>
		//		{
		//			new GarmentSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentSewingOutRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentSewingOutReadModel>
		//		{
		//			new GarmentSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","FINISHING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
		//		}.AsQueryable());


		//	_mockGarmentCuttingInDetailRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingInDetailReadModel>
		//		{
		//			new GarmentCuttingInDetail(new Guid(),guidCuttingInItem,new Guid(),new Guid(),new Guid(),new ProductId(1),"","","","Main Fabric",10,new UomId(1),"",10,new UomId(1),"",10,100,100,1,"").GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentCuttingInItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingInItemReadModel>
		//		{
		//			new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,new Guid(),1,"",guidSewingOut,"").GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentCuttingInRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingInReadModel>
		//		{
		//			new GarmentCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now.AddDays(-1),1).GetReadModel()
		//		}.AsQueryable());


		//	// Act
		//	var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

		//	// Assert
		//	result.Should().NotBeNull();
		//}

		//[Fact]
		//public async Task Handle_StateUnderTest_ExpectedBehavior()
		//{
		//	// Arrange
		//	GetXlsMonitoringProductionStockFlowQueryHandler unitUnderTest = CreateGetMonitoringProductionFlowQueryHandler();
		//	CancellationToken cancellationToken = CancellationToken.None;

		//	Guid guidLoading = Guid.NewGuid();
		//	Guid guidLoadingItem = Guid.NewGuid();
		//	Guid guidCuttingOut = Guid.NewGuid();
		//	Guid guidCuttingOutItem = Guid.NewGuid();
		//	Guid guidCuttingOutDetail = Guid.NewGuid();
		//	Guid guidFinishingOut = Guid.NewGuid();
		//	Guid guidFinishingOutItem = Guid.NewGuid();
		//	Guid guidSewingOut = Guid.NewGuid();
		//	Guid guidSewingOutItem = Guid.NewGuid();
		//	Guid guidSewingIn = Guid.NewGuid();
		//	Guid guidSewingInItem = Guid.NewGuid();
		//	Guid guidSewingDO = Guid.NewGuid();
		//	Guid guidSewingDOItem = Guid.NewGuid();
		//	Guid guidCuttingIn = Guid.NewGuid();
		//	Guid guidCuttingInItem = Guid.NewGuid();
		//	Guid guidFinishingIn = Guid.NewGuid();
		//	Guid guidFinishingInItem = Guid.NewGuid();
		//	Guid guidAdjustment = Guid.NewGuid();
		//	Guid guidAval = Guid.NewGuid();
		//	Guid guidExpenditure = Guid.NewGuid();
		//	Guid guidExpenditureReturn = Guid.NewGuid();
		//	Guid guidAdjustmentItem = Guid.NewGuid();
		//	GetXlsMonitoringProductionStockFlowQuery getMonitoring = new GetXlsMonitoringProductionStockFlowQuery(1, 25, "{}", 1, "ro", DateTime.Now.AddDays(-5), DateTime.Now, "", "token");

		//	_mockGarmentLoadingItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentLoadingItemReadModel>
		//		{
		//			new GarmentLoadingItem(guidLoadingItem,guidLoading,new Guid(),new SizeId(1),"",new ProductId(1),"","","",0,0,0, new UomId(1),"","",10).GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentLoadingRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentLoadingReadModel>
		//		{
		//			new GarmentLoading(guidLoading,"",new Guid(),"",new UnitDepartmentId(1),"","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,new GarmentComodityId(1),"","").GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentCuttingOutDetailRepository
		//	.Setup(s => s.Query)
		//	.Returns(new List<GarmentCuttingOutDetailReadModel>
		//	{
		//			new GarmentCuttingOutDetail(new Guid(),guidCuttingOutItem,new SizeId(1),"","",100,100,new UomId(1),"",10,10).GetReadModel()
		//	}.AsQueryable());

		//	_mockGarmentCuttingOutItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingOutItemReadModel>
		//		{
		//			new GarmentCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
		//		}.AsQueryable());
		//	_mockGarmentCuttingOutRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingOutReadModel>
		//		{
		//			 new GarmentCuttingOut(guidCuttingOut, "", "SEWING",new UnitDepartmentId(1),"","",DateTime.Now.AddDays(-1),"ro","article",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"cm","cmo",false).GetReadModel()
		//		}.AsQueryable());



		//	_mockGarmentFinishingOutItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentFinishingOutItemReadModel>
		//		{
		//			new GarmentFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new ProductId(1),"","","",new SizeId(1),"",10, new UomId(1),"","",10,10,10).GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentFinishingOutRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentFinishingOutReadModel>
		//		{
		//			new GarmentFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now.AddDays(-1),"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
		//		}.AsQueryable());


		//	_mockGarmentSewingOutItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentSewingOutItemReadModel>
		//		{
		//			new GarmentSewingOutItem(guidSewingOutItem,guidSewingOut,new Guid(),new Guid(), new ProductId(1),"","","",new SizeId(1),"",0, new UomId(1),"","",10,100,100).GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentSewingOutRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentSewingOutReadModel>
		//		{
		//			new GarmentSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","FINISHING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
		//		}.AsQueryable());


		//	_mockGarmentCuttingInDetailRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingInDetailReadModel>
		//		{
		//			new GarmentCuttingInDetail(new Guid(),guidCuttingInItem,new Guid(),new Guid(),new Guid(),new ProductId(1),"","","","Main Fabric",10,new UomId(1),"",10,new UomId(1),"",10,100,100,1,"").GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentCuttingInItemRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingInItemReadModel>
		//		{
		//			new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,new Guid(),1,"",guidSewingOut,"").GetReadModel()
		//		}.AsQueryable());

		//	_mockGarmentCuttingInRepository
		//		.Setup(s => s.Query)
		//		.Returns(new List<GarmentCuttingInReadModel>
		//		{
		//			new GarmentCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now.AddDays(-1),1).GetReadModel()
		//		}.AsQueryable());


		//	// Act
		//	var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

		//	// Assert
		//	result.Should().NotBeNull();
		//}

	}
}
