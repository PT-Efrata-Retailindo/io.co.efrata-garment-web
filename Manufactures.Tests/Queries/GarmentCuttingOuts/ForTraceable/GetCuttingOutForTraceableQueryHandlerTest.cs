using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentCuttingOuts.Queries.GetCuttingOutForTraceable;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentCuttingOuts.ForTraceable
{
    public class GetCuttingOutForTraceableQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentCuttingOutRepository> _mockgarmentCuttingOutRepository;
        private readonly Mock<IGarmentCuttingOutItemRepository> _mockgarmentCuttingOutItemRepository;
        private readonly Mock<IGarmentCuttingOutDetailRepository> _mockgarmentCuttingOutDetailRepository;

        //protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;

        public GetCuttingOutForTraceableQueryHandlerTest()
        {
            _mockgarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockgarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockgarmentCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();

            _MockStorage.SetupStorage(_mockgarmentCuttingOutRepository);
            _MockStorage.SetupStorage(_mockgarmentCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockgarmentCuttingOutDetailRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetCuttingOutForTraceableQueryHandler CreateGetCuttingOutForTraceableQueryHandler()
        {
            return new GetCuttingOutForTraceableQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetCuttingOutForTraceableQueryHandler unitUnderTest = CreateGetCuttingOutForTraceableQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

           
            Guid guidCuttingOut = Guid.NewGuid();
            Guid guidCuttingOutItem = Guid.NewGuid();
            Guid guidCuttingOutDetail = Guid.NewGuid();


            GetCuttingOutForTraceableQuery getMonitoring = new GetCuttingOutForTraceableQuery(new List<string> { "ro" }, "token");

            
            _mockgarmentCuttingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>
            {
                    new GarmentCuttingOutDetail(new Guid(),guidCuttingOutItem,new Domain.Shared.ValueObjects.SizeId(1),"","",100,100,new Domain.Shared.ValueObjects.UomId(1),"",10,10).GetReadModel()
            }.AsQueryable());

            _mockgarmentCuttingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>
                {
                    new GarmentCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new Domain.Shared.ValueObjects.ProductId(1),"","","",100).GetReadModel()
                }.AsQueryable());
            _mockgarmentCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                     new GarmentCuttingOut(guidCuttingOut, "", "SEWING",new UnitDepartmentId(1),"","",DateTime.Now,"ro","article",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"cm","cmo",false).GetReadModel()
                }.AsQueryable());

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
