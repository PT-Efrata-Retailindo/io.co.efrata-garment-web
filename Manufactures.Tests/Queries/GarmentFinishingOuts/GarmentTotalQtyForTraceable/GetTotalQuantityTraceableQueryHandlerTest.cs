using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentFinishingOuts.GarmentTotalQtyForTraceable
{
    public class GetTotalQuantityTraceableQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingOutRepository> _mockgarmentFinishingOutRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockgarmentFinishingOutItemRepository;
        private readonly Mock<IGarmentFinishingInRepository> _mockgarmentFinishingInRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockgarmentFinishingInItemRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GetTotalQuantityTraceableQueryHandlerTest()
        {
            _mockgarmentFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
            _mockgarmentFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();
            _mockgarmentFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
            _mockgarmentFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockgarmentFinishingOutRepository);
            _MockStorage.SetupStorage(_mockgarmentFinishingOutItemRepository);
            _MockStorage.SetupStorage(_mockgarmentFinishingInRepository);
            _MockStorage.SetupStorage(_mockgarmentFinishingInItemRepository);

            serviceProviderMock = new Mock<IServiceProvider>();


        }

        private GetTotalQuantityTraceableQueryHandler CreateGetTotalQuantityTraceableQueryHandler()
        {
            return new GetTotalQuantityTraceableQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetTotalQuantityTraceableQueryHandler unitUnderTest = CreateGetTotalQuantityTraceableQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidFinishingIn = Guid.NewGuid();
            Guid guidFinishingInItem = Guid.NewGuid();
            Guid guidFinishingOut = Guid.NewGuid();
            Guid guidFinishingOutItem = Guid.NewGuid();


            GetTotalQuantityTraceableQuery getMonitoring = new GetTotalQuantityTraceableQuery("ro", "token");

            _mockgarmentFinishingInItemRepository
              .Setup(s => s.Query)
              .Returns(new List<GarmentFinishingInItemReadModel>
              {
                    new GarmentFinishingInItem(guidFinishingInItem, guidFinishingIn, new Guid(), new Guid(), new Guid(), new SizeId(1), "", new ProductId(1), "", "","",1,1,new UomId(1), "","",1,1).GetReadModel()
              }.AsQueryable());

            _mockgarmentFinishingInRepository
              .Setup(s => s.Query)
              .Returns(new List<GarmentFinishingInReadModel>
              {
                    new GarmentFinishingIn(guidFinishingIn, "", "", new UnitDepartmentId(1), "","","","",new UnitDepartmentId(1),"","",DateTimeOffset.Now, new GarmentComodityId(1), "","",1,"","").GetReadModel()
              }.AsQueryable());


            _mockgarmentFinishingOutItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentFinishingOutItemReadModel>
               {
                    new GarmentFinishingOutItem(guidFinishingOutItem,guidFinishingOut,guidFinishingIn,guidFinishingInItem,new Domain.Shared.ValueObjects.ProductId(1),"","","",new SizeId(1),"",10, new Domain.Shared.ValueObjects.UomId(1),"","",10,10,10).GetReadModel()
               }.AsQueryable());

            _mockgarmentFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>
                {
                    new GarmentFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
                }.AsQueryable());


            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
