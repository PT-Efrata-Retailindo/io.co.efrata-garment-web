using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods;
using Manufactures.Application.GarmentExpenditureGoods.Queries.GetReportExpenditureGoods;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.MonitoringProductionStockFlow;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentExpenditureGoods.GarmentMutationExpenditureGood
{
    public class XlsExpenditureGoodReportQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockgarmentExpenditureGoodRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockgarmentExpenditureGoodItemRepository;
        private readonly Mock<IGarmentCuttingOutRepository> _mockgarmentCuttingOutRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public XlsExpenditureGoodReportQueryHandlerTest()
        {
            _mockgarmentExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
            _mockgarmentExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();
            _MockStorage.SetupStorage(_mockgarmentExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockgarmentExpenditureGoodItemRepository);


            _mockgarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _MockStorage.SetupStorage(_mockgarmentCuttingOutRepository);


            serviceProviderMock = new Mock<IServiceProvider>();
        }

        private GetXlsReportExpenditureGoodsQueryHandler CreateGetXlsMutationQueryHandler()
        {
            return new GetXlsReportExpenditureGoodsQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }


        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsReportExpenditureGoodsQueryHandler unitUnderTest = CreateGetXlsMutationQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidExpenditureGood = Guid.NewGuid();
            Guid guidExpenditureGoodItem = Guid.NewGuid();
            Guid guidCuttingOut = Guid.NewGuid();

            GetXlsReportExpenditureGoodsQuery getXlsMutation = new GetXlsReportExpenditureGoodsQuery(1, 25, "{}", DateTime.Now, DateTime.Now.AddDays(5), "token");

            _mockgarmentExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>
                {
                    new Domain.GarmentExpenditureGoods.GarmentExpenditureGoodItem(guidExpenditureGoodItem,guidExpenditureGood,Guid.NewGuid(),new Domain.Shared.ValueObjects.SizeId(1),"",1,0,new Domain.Shared.ValueObjects.UomId(1),"","",0,0).GetReadModel()
                }.AsQueryable());

            _mockgarmentExpenditureGoodRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReadModel>
                {
                    new Domain.GarmentExpenditureGoods.GarmentExpenditureGood(guidExpenditureGood, "","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","","213","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"BR","",new Domain.Shared.ValueObjects.BuyerId(1),"","",DateTimeOffset.Now,"","",0,"",false,0).GetReadModel()
                }.AsQueryable());

            _mockgarmentCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                     new GarmentCuttingOut(guidCuttingOut, "", "SEWING",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",DateTime.Now,"213","article",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"BR","cmo",false).GetReadModel()
                }.AsQueryable());



            var result = await unitUnderTest.Handle(getXlsMutation, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }*/
    }
}
