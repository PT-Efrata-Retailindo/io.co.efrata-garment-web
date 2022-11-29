using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOCuttingSewingReport;
using System.Threading.Tasks;
using System.Threading;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.GarmentExpenditureNoteReport;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOCuttingSewing
{
    public class GarmentXlsSubconDLOCuttingSewingQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSubconDeliveryLetterOutRepository> _mockgarmentSubconDeliveryLetterOutRepository;
        private readonly Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockgarmentSubconDeliveryLetterOutItemRepository;
		protected readonly Mock<IHttpClientService> _mockhttpService;
		private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconDLOCuttingSewingQueryHandlerTest()
        {

            _mockgarmentSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockgarmentSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutRepository);
			_MockStorage.SetupStorage(_mockgarmentSubconDeliveryLetterOutItemRepository);

            serviceProviderMock = new Mock<IServiceProvider>();
			_mockhttpService = CreateMock<IHttpClientService>();
			PurchasingDataSettings.Endpoint = "https://com-danliris-service-purchasing.azurewebsites.net/v1/";

			List<UENViewModel> uENViewModel = new List<UENViewModel> {
				new UENViewModel
				{
					UENNo="UENNo",
					UENDate= DateTimeOffset.Now,
					UnitRequestName="UnitRequestName",
					UnitSenderName="UnitSenderName",
					FabricType="Type",
					RONo="RONo",
					Quntity=1,
					UOMUnit="UomUnit"
				}
			};

			_mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
				.ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(uENViewModel) + "}") });
			serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);

		}

        private GetXlsGarmentSubconDLOSewingReportQueryHandler CreateGetXlsGarmentSubconDLOSewingReportQueryHandler()
        {
            return new GetXlsGarmentSubconDLOSewingReportQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			
				// Arrange
				GetXlsGarmentSubconDLOSewingReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconDLOSewingReportQueryHandler();
				CancellationToken cancellationToken = CancellationToken.None;

				Guid guidDeliveryLetterOut = Guid.NewGuid();
				Guid guidDeliveryLetterOutItem = Guid.NewGuid();
				GetXlsGarmentSubconDLOCuttingSewingReportQuery getMonitoring = new GetXlsGarmentSubconDLOCuttingSewingReportQuery(1, 25, "{}", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(2));


				_mockgarmentSubconDeliveryLetterOutItemRepository
					.Setup(s => s.Query)
					.Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
					{
					new GarmentSubconDeliveryLetterOutItem(guidDeliveryLetterOutItem,guidDeliveryLetterOut,1,new ProductId(1),"ProductCode","ProductName","ProductRemark","DesignColor",1,new UomId(1),"UomUnit", new UomId(1),"uomOutUnit","fabricType", Guid.Empty, "roNo", "poSerialNumber", "SubconNo", It.IsAny<int>(),"").GetReadModel()
					}.AsQueryable());

				_mockgarmentSubconDeliveryLetterOutRepository
					.Setup(s => s.Query)
					.Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
					{
					new GarmentSubconDeliveryLetterOut(guidDeliveryLetterOut,"dLNo","dLType","SUBCON GARMENT", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", false, "serviceType", "SUBCON CUTTING SEWING",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()
					}.AsQueryable());

				// Act
				var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

				// Assert
				result.Should().NotBeNull();
		}

		[Fact]
		public async Task Handle_StateUnderTest_UnExpectedBehavior()
		{

			// Arrange
			GetXlsGarmentSubconDLOSewingReportQueryHandler unitUnderTest = CreateGetXlsGarmentSubconDLOSewingReportQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidDeliveryLetterOut = Guid.NewGuid();
			Guid guidDeliveryLetterOutItem = Guid.NewGuid();
			GetXlsGarmentSubconDLOCuttingSewingReportQuery getMonitoring = new GetXlsGarmentSubconDLOCuttingSewingReportQuery(1, 25, "{}", DateTime.Now.AddDays(2), DateTime.Now);


			_mockgarmentSubconDeliveryLetterOutItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>
				{
					new GarmentSubconDeliveryLetterOutItem(guidDeliveryLetterOutItem,guidDeliveryLetterOut,1,new ProductId(1),"ProductCode","ProductName","ProductRemark","DesignColor",1,new UomId(1),"UomUnit", new UomId(1),"uomOutUnit","fabricType", Guid.Empty, "roNo", "poSerialNumber", "SubconNo", It.IsAny<int>(),"").GetReadModel()
				}.AsQueryable());

			_mockgarmentSubconDeliveryLetterOutRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSubconDeliveryLetterOutReadModel>
				{
					new GarmentSubconDeliveryLetterOut(guidDeliveryLetterOut,"dLNo","dLType","SUBCON GARMENT", DateTimeOffset.Now, 1, "uENNo", "pONo", 1, "remark", false, "serviceType", "SUBCON CUTTING SEWING",It.IsAny<int>(),"",It.IsAny<int>(),"").GetReadModel()
				}.AsQueryable());

			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
