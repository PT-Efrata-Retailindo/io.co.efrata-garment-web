using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods;
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
    public class MutationExpenditureGoodQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentBalanceMonitoringProductionStockFlowRepository> _mockgarmentBalanceMonitoringProductionStockFlowRepository;
        private readonly Mock<IGarmentAdjustmentRepository> _mockgarmentAdjustmentRepository;
        private readonly Mock<IGarmentAdjustmentItemRepository> _mockgarmentAdjustmentItemRepository;
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockgarmentExpenditureGoodRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockgarmentExpenditureGoodItemRepository;
        private readonly Mock<IGarmentExpenditureGoodReturnRepository> _mockgarmentExpenditureGoodReturnRepository;
        private readonly Mock<IGarmentExpenditureGoodReturnItemRepository> _mockgarmentExpenditureGoodReturnItemRepository;
        private readonly Mock<IGarmentFinishingOutRepository> _mockgarmentFinishingOutRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockgarmentFinishingOutItemRepository;
        private readonly Mock<IGarmentCuttingOutRepository> _mockgarmentCuttingOutRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public MutationExpenditureGoodQueryHandlerTest()
        {
            _mockgarmentBalanceMonitoringProductionStockFlowRepository = CreateMock<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            _MockStorage.SetupStorage(_mockgarmentBalanceMonitoringProductionStockFlowRepository);


            _mockgarmentAdjustmentRepository = CreateMock<IGarmentAdjustmentRepository>();
            _mockgarmentAdjustmentItemRepository = CreateMock<IGarmentAdjustmentItemRepository>();
            _MockStorage.SetupStorage(_mockgarmentAdjustmentRepository);
            _MockStorage.SetupStorage(_mockgarmentAdjustmentItemRepository);

            _mockgarmentExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
            _mockgarmentExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();
            _MockStorage.SetupStorage(_mockgarmentExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockgarmentExpenditureGoodItemRepository);

            _mockgarmentExpenditureGoodReturnRepository = CreateMock<IGarmentExpenditureGoodReturnRepository>();
            _mockgarmentExpenditureGoodReturnItemRepository = CreateMock<IGarmentExpenditureGoodReturnItemRepository>();
            _MockStorage.SetupStorage(_mockgarmentExpenditureGoodReturnRepository);
            _MockStorage.SetupStorage(_mockgarmentExpenditureGoodReturnItemRepository);

            _mockgarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _MockStorage.SetupStorage(_mockgarmentCuttingOutRepository);

            _mockgarmentFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
            _mockgarmentFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();
            _MockStorage.SetupStorage(_mockgarmentFinishingOutRepository);
            _MockStorage.SetupStorage(_mockgarmentFinishingOutItemRepository);


            serviceProviderMock = new Mock<IServiceProvider>();
        }

        private GarmentMutationExpenditureGoodQueryHandler CreateGetMutationQueryHandler()
        {
            return new GarmentMutationExpenditureGoodQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GarmentMutationExpenditureGoodQueryHandler unitUnderTest = CreateGetMutationQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidAdjustment = Guid.NewGuid();
            Guid guidAdjustmentItem = Guid.NewGuid();
            Guid guidExpenditureGood = Guid.NewGuid();
            Guid guidExpenditureGoodItem = Guid.NewGuid();
            Guid guidExpenditureGoodReturn = Guid.NewGuid();
            Guid guidExpenditureGoodReturnItem = Guid.NewGuid();
            Guid guidCuttingOut = Guid.NewGuid();
            Guid guidFinishingOut = Guid.NewGuid();
            Guid guidFinishingOutItem = Guid.NewGuid();
            Guid guidbalance = Guid.NewGuid();

            GetMutationExpenditureGoodsQuery getMutation = new GetMutationExpenditureGoodsQuery(1, 25, "{}", DateTime.Now, DateTime.Now.AddDays(5), "token");

            _mockgarmentBalanceMonitoringProductionStockFlowRepository
                .Setup(s=>s.Query)
                .Returns(new List<GarmentBalanceMonitoringProductionStockReadModel> {
                    new GarmentBalanceMonitoringProductionStocFlow(new GarmentBalanceMonitoringProductionStockReadModel(Guid.NewGuid())).GetReadModel()
                }.AsQueryable());

            _mockgarmentAdjustmentItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentItemReadModel>
                {
                    new Domain.GarmentAdjustments.GarmentAdjustmentItem(guidAdjustmentItem, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new Domain.Shared.ValueObjects.SizeId(1), "", new Domain.Shared.ValueObjects.ProductId(1), "","","",0,0,new Domain.Shared.ValueObjects.UomId(1),"","",0).GetReadModel()
                }.AsQueryable());

            _mockgarmentAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>
                {
                    new Domain.GarmentAdjustments.GarmentAdjustment(guidAdjustment,"","","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",DateTimeOffset.Now,new Domain.Shared.ValueObjects.GarmentComodityId(1),"","","").GetReadModel()
                }.AsQueryable());

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

            _mockgarmentExpenditureGoodReturnItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReturnItemReadModel>
                {
                    new GarmentExpenditureGoodReturnItem(new Guid(),guidExpenditureGoodReturnItem,guidExpenditureGood,new Guid(),new Guid(), new Domain.Shared.ValueObjects.SizeId(1),"",100,new Domain.Shared.ValueObjects.UomId(1),"","",100,100).GetReadModel()
                }.AsQueryable());

            _mockgarmentExpenditureGoodReturnRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReturnReadModel>
                {
                    new GarmentExpenditureGoodReturn(guidExpenditureGoodReturn,"np","SAMPLE",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","","ro","article",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.BuyerId(1),"","",DateTimeOffset.Now,"","").GetReadModel()
                }.AsQueryable());

            _mockgarmentCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                     new GarmentCuttingOut(guidCuttingOut, "", "SEWING",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",DateTime.Now,"ro","article",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"cm","cmo",false).GetReadModel()
                }.AsQueryable());


            _mockgarmentFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutItemReadModel>
                {
                    new GarmentFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new Domain.Shared.ValueObjects.ProductId(1),"","","",new Domain.Shared.ValueObjects.SizeId(1),"",10, new Domain.Shared.ValueObjects.UomId(1),"","",10,10,10).GetReadModel()
                }.AsQueryable());

            _mockgarmentFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>
                {
                    new GarmentFinishingOut(guidFinishingOut,"",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now,"ro","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",false).GetReadModel()
                }.AsQueryable());



            var result = await unitUnderTest.Handle(getMutation, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }*/
    }
}
