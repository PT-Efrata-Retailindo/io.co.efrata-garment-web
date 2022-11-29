using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentSample.GarmentMonitoringSampleFlows.Queries;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
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
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;

namespace Manufactures.Tests.GarmentSample.Queries.GarmentMonitoringProductionFlows
{
	public class XlsMonitoringSampleFlowCommandHandlerTest : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentSampleCuttingOutRepository> _mockGarmentCuttingOutRepository;
		private readonly Mock<IGarmentSampleCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
		private readonly Mock<IGarmentSampleCuttingOutDetailRepository> _mockGarmentCuttingOutDetailRepository;
		private readonly Mock<IGarmentSampleSewingOutRepository> _mockGarmentSewingOutRepository;
		private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
		private readonly Mock<IGarmentSampleSewingOutDetailRepository> _mockGarmentSewingOutDetailRepository;
		private readonly Mock<IGarmentSampleFinishingOutRepository> _mockGarmentFinishingOutRepository;
		private readonly Mock<IGarmentSampleFinishingOutItemRepository> _mockGarmentFinishingOutItemRepository;
		private readonly Mock<IGarmentSampleFinishingOutDetailRepository> _mockGarmentFinishingOutDetailRepository;
		private readonly Mock<IGarmentSampleRequestRepository> _mockGarmentSampleRequestRepository;
		private readonly Mock<IGarmentSampleRequestProductRepository> _mockGarmentSampleRequestProductRepository;
		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;

		public XlsMonitoringSampleFlowCommandHandlerTest()
		{
			_mockGarmentFinishingOutRepository = CreateMock<IGarmentSampleFinishingOutRepository>();
			_mockGarmentFinishingOutItemRepository = CreateMock<IGarmentSampleFinishingOutItemRepository>();
			_mockGarmentFinishingOutDetailRepository = CreateMock<IGarmentSampleFinishingOutDetailRepository>();
			_MockStorage.SetupStorage(_mockGarmentFinishingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentFinishingOutDetailRepository);

			_mockGarmentSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
			_mockGarmentSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
			_mockGarmentSewingOutDetailRepository = CreateMock<IGarmentSampleSewingOutDetailRepository>();
			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentSewingOutDetailRepository);

			_mockGarmentCuttingOutRepository = CreateMock<IGarmentSampleCuttingOutRepository>();
			_mockGarmentCuttingOutItemRepository = CreateMock<IGarmentSampleCuttingOutItemRepository>();
			_mockGarmentCuttingOutDetailRepository = CreateMock<IGarmentSampleCuttingOutDetailRepository>();
			_MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
			_MockStorage.SetupStorage(_mockGarmentCuttingOutDetailRepository);

			_mockGarmentSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
			_mockGarmentSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();
			_MockStorage.SetupStorage(_mockGarmentSampleRequestRepository);
			_MockStorage.SetupStorage(_mockGarmentSampleRequestProductRepository);

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
			List<HOrderViewModel> HOrderViewModels = new List<HOrderViewModel> {
				new HOrderViewModel
				{
					No="ro",
					Codeby="buyer",
					Qty=100,
					Kode="dd"
				}
			};
			_mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
			serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
		}

		private GetXlsMonitoringSampleFlowQueryHandler CreateGetXlsMonitoringSampleFlowQueryHandler()
		{
			return new GetXlsMonitoringSampleFlowQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
		}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GetXlsMonitoringSampleFlowQueryHandler unitUnderTest = CreateGetXlsMonitoringSampleFlowQueryHandler();
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
			Guid guidSampleReqId = Guid.NewGuid();
			GetXlsMonitoringSampleFlowQuery getMonitoring = new GetXlsMonitoringSampleFlowQuery(1, 25, "{}", 1, DateTime.Now, null, "token");

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
					 new GarmentSampleCuttingOut(guidCuttingOut, "", "",new UnitDepartmentId(1),"","",DateTime.Now.AddDays(-1),"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());

			_mockGarmentFinishingOutDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleFinishingOutDetailReadModel>
				{
					new GarmentSampleFinishingOutDetail(new Guid(),guidFinishingOutItem,new SizeId(1),"",10, new UomId(1),"").GetReadModel()
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
					new GarmentSampleFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
				}.AsQueryable());

			_mockGarmentSewingOutDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingOutDetailReadModel>
				{
					new GarmentSampleSewingOutDetail(new Guid(),guidSewingOutItem,new SizeId(1),"",0, new UomId(1),"").GetReadModel()
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
					new GarmentSampleSewingOut(guidSewingOut,"",new BuyerId(1),"","",new UnitDepartmentId(1),"","","FINISHING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());

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
	}
}
