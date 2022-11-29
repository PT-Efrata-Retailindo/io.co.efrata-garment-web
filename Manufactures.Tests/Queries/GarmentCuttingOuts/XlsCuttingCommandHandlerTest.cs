using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentCuttingOuts.Queries;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
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

namespace Manufactures.Tests.Queries.GarmentCuttingOuts
{
	public class XlsCuttingCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentCuttingOutRepository> _mockGarmentCuttingOutRepository;
		private readonly Mock<IGarmentCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
		private readonly Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
		private readonly Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
		private readonly Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
		private readonly Mock<IGarmentAvalComponentRepository> _mockGarmentAvalComponentRepository;
		private readonly Mock<IGarmentAvalComponentItemRepository> _mockGarmentAvalComponentItemRepository;
		private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
		private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;
        private readonly Mock<IGarmentBalanceCuttingRepository> _mockGarmentBalanceCuttingRepository;
        protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;

		public XlsCuttingCommandHandlerTest()
		{
            _mockGarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockGarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockGarmentAvalComponentRepository = CreateMock<IGarmentAvalComponentRepository>();
            _mockGarmentAvalComponentItemRepository = CreateMock<IGarmentAvalComponentItemRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
            _mockGarmentBalanceCuttingRepository = CreateMock<IGarmentBalanceCuttingRepository>();

            _MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalComponentItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);
            _MockStorage.SetupStorage(_mockGarmentBalanceCuttingRepository);

            SalesDataSettings.Endpoint = "https://com-danliris-service-sales.azurewebsites.net/v1/";

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
        private GetXlsCuttingQueryHandler CreateGetXlsCuttingQueryHandler()
			{
				return new GetXlsCuttingQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
			}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			//Arrange
			GetXlsCuttingQueryHandler unitUnderTest = CreateGetXlsCuttingQueryHandler();
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

			GetXlsCuttingQuery getMonitoring = new GetXlsCuttingQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "", "token");



			_mockGarmentCuttingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInItemReadModel>
				{
					new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"",Guid.Empty,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInReadModel>
				{
					new GarmentCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,4.5).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInDetailReadModel>
				{
					new GarmentCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"","","","",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentAvalComponentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalComponentItemReadModel>
				{
					new GarmentAvalComponentItem(guidAvalComponentItem,guidAvalComponent,guidCuttingInDetail,new Guid(),new Guid(),new ProductId(1),"","","","",10,0, new SizeId(1),"",100,100).GetReadModel()
				}.AsQueryable());
			_mockGarmentAvalComponentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalComponentReadModel>
				{
					new GarmentAvalComponent(guidAvalComponent,"",new UnitDepartmentId(1),"","","","ro","",new GarmentComodityId(1),"","",DateTimeOffset.Now, false).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutItemReadModel>
				{
					new GarmentCuttingOutItem(guidCuttingOutItem,guidCuttingIn,guidCuttingInDetail,guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
				}.AsQueryable());
			_mockGarmentCuttingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutReadModel>
				{
					 new GarmentCuttingOut(guidCuttingOut, "", "",new UnitDepartmentId(1),"","",DateTime.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparing = Guid.NewGuid();
			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					 new GarmentPreparing(guidGarmentPreparing,1,"uenNo",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true, new BuyerId(1), null, null).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparingItem = Guid.NewGuid();
			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					 new GarmentPreparingItem(guidGarmentPreparingItem,1,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentPreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,guidGarmentPreparing,null,"fasilitas").GetReadModel()
				}.AsQueryable());
			var garmentBalanceCutting = Guid.NewGuid();
			_mockGarmentBalanceCuttingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentBalanceCuttingReadModel>
				{
					 new GarmentBalanceCutting(garmentBalanceCutting,"ro","article",1,"unitCode","unitName","buyerCode",1,"comodityName",2,1,1,2,2,9,9,100,100).GetReadModel()
				}.AsQueryable());
			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert

			result.Should().NotBeNull();
		}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior_bookkeeping()
		{
			 //Arrange
			GetXlsCuttingQueryHandler unitUnderTest = CreateGetXlsCuttingQueryHandler();
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

			GetXlsCuttingQuery getMonitoring = new GetXlsCuttingQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "bookkeeping", "token");



			_mockGarmentCuttingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInItemReadModel>
				{
					new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"",Guid.Empty,"").GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInReadModel>
				{
					new GarmentCuttingIn(guidCuttingIn,"","Main Fabric","","ro","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,4.5).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingInDetailReadModel>
				{
					new GarmentCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"","","","",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentAvalComponentItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalComponentItemReadModel>
				{
					new GarmentAvalComponentItem(guidAvalComponentItem,guidAvalComponent,guidCuttingInDetail,new Guid(),new Guid(),new ProductId(1),"","","","",10,0, new SizeId(1),"",100,100).GetReadModel()
				}.AsQueryable());

			
			_mockGarmentAvalComponentRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentAvalComponentReadModel>
				{
					new GarmentAvalComponent(guidAvalComponent,"",new UnitDepartmentId(1),"","","","ro","",new GarmentComodityId(1),"","",DateTimeOffset.Now, false).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutItemReadModel>
				{
					new GarmentCuttingOutItem(guidCuttingOutItem,guidCuttingIn,guidCuttingInDetail,guidCuttingOut,new ProductId(1),"","","",100).GetReadModel()
				}.AsQueryable());
			_mockGarmentCuttingOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentCuttingOutReadModel>
				{
					 new GarmentCuttingOut(guidCuttingOut, "", "",new UnitDepartmentId(1),"","",DateTime.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparing = Guid.NewGuid();
			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingReadModel>
				{
					 new GarmentPreparing(guidGarmentPreparing,1,"uenNo",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true, new BuyerId(1), null, null).GetReadModel()
				}.AsQueryable());

			var guidGarmentPreparingItem = Guid.NewGuid();
			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentPreparingItemReadModel>
				{
					 new GarmentPreparingItem(guidGarmentPreparingItem,1,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentPreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,guidGarmentPreparing,null,"fasilitas").GetReadModel()
				}.AsQueryable());
            var garmentBalanceCutting = Guid.NewGuid();
            _mockGarmentBalanceCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentBalanceCuttingReadModel>
                {
                     new GarmentBalanceCutting(garmentBalanceCutting,"ro","article",1,"unitCode","unitName","buyerCode",1,"comodityName",2,1,1,2,2,9,9,100,100).GetReadModel()
                }.AsQueryable());

             //Act
            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
