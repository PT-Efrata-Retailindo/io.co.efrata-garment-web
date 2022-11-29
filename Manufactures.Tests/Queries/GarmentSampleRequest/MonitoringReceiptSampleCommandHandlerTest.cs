using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
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
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.GarmentSectionResult;

namespace Manufactures.Tests.Queries.GarmentSampleRequest
{
    public class MonitoringReceiptSampleCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleRequestRepository> _mockGarmentSampleRequestRepository;
        private readonly Mock<IGarmentSampleRequestProductRepository> _mockGarmentSampleRequestProductRepository;
        protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;
        public MonitoringReceiptSampleCommandHandlerTest()
        {
            _mockGarmentSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
            _mockGarmentSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();
            _MockStorage.SetupStorage(_mockGarmentSampleRequestRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestProductRepository);

            serviceProviderMock = new Mock<IServiceProvider>();
            _mockhttpService = CreateMock<IHttpClientService>();
            GarmentSectionViewModel costCalViewModels = new GarmentSectionViewModel {
                    Id=1,
                    Name="haha"
                
            };

            _mockhttpService.Setup(x => x.GetAsync( It.IsAny<string>(), It.IsAny<string>()))
             .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(costCalViewModels) + "}") });
            serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);


        }
        private GetMonitoringReceiptSampleQueryHandler CreateGetMonitoringReceiptSampleQueryHandler()
        {
            return new GetMonitoringReceiptSampleQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetMonitoringReceiptSampleQueryHandler unitUnderTest = CreateGetMonitoringReceiptSampleQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            GetMonitoringReceiptSampleQuery getMonitoringReceiptSample = new GetMonitoringReceiptSampleQuery(1, 25, "{}", DateTime.Now, DateTime.Now.AddDays(2), "token");
            Guid guidSampleReqId = Guid.NewGuid();

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
                    new Domain.GarmentSample.SampleRequests.GarmentSampleRequest (guidSampleReqId,"","","","",DateTimeOffset.Now,new Domain.Shared.ValueObjects.BuyerId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","","","",DateTimeOffset.Now,"","","",true,true,DateTimeOffset.Now,"",false,DateTimeOffset.Now,"","",false,DateTimeOffset.Now,"","","","","","",new Domain.Shared.ValueObjects.SectionId(1),"", null).GetReadModel()
                }.AsQueryable());

            var result = await unitUnderTest.Handle(getMonitoringReceiptSample, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
