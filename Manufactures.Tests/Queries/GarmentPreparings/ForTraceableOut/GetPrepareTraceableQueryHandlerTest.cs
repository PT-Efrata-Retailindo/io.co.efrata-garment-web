using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentPreparings.Queries.GetPrepareTraceable;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentPreparings.ForTraceableOut
{
    public class GetPrepareTraceableQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentPreparingRepository> _mockgarmentPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockgarmentPreparingItemRepository;

        //protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;

        public GetPrepareTraceableQueryHandlerTest()
        {
            _mockgarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockgarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockgarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockgarmentPreparingItemRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetPrepareTraceableQueryHandler CreateGetPrepareTraceableQueryHandler()
        {
            return new GetPrepareTraceableQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetPrepareTraceableQueryHandler unitUnderTest = CreateGetPrepareTraceableQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidPreparing = Guid.NewGuid();
            Guid guidPreparingItem = Guid.NewGuid();


            GetPrepareTraceableQuery getMonitoring = new GetPrepareTraceableQuery("ro1,ro2", "token");


            _mockgarmentPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>
                {
                        new GarmentPreparingItem(guidPreparingItem,0,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productcode","name","",20,new Domain.GarmentPreparings.ValueObjects.UomId(1),"","",0,0,guidPreparing,"ro1","fasilitas").GetReadModel()

                }.AsQueryable());

            _mockgarmentPreparingRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingReadModel>
                {
                    new GarmentPreparing(guidPreparing,0,"",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1), "", "", DateTimeOffset.Now, "ro1","",true,new Domain.Shared.ValueObjects.BuyerId(1),"","").GetReadModel()
                }.AsQueryable());
           

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
