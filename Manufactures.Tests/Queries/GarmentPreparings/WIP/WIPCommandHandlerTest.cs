using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Manufactures.Application.GarmentPreparings.Queries.GetWIP;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentPreparings.WIP
{
    public class WIPCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;
        private readonly Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
        private readonly Mock<IGarmentAvalProductRepository> _mockGarmentAvalProductRepository;
        private readonly Mock<IGarmentAvalProductItemRepository> _mockGarmentAvalProductItemRepository;
        //private readonly Mock<IGarmentDeliveryReturnRepository> _mockGarmentDeliveryReturnRepository;
        private readonly Mock<IGarmentDeliveryReturnItemRepository> _mockGarmentDeliveryReturnItemRepository;
        private readonly Mock<IGarmentCuttingOutRepository> _mockGarmentCuttingOutRepository;
        private readonly Mock<IGarmentCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
        private readonly Mock<IGarmentCuttingOutDetailRepository> _mockGarmentCuttingOutDetailRepository;
        private readonly Mock<IGarmentFinishingOutRepository> _mockGarmentFinishingOutRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockGarmentFinishingOutItemRepository;

        protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;

        public WIPCommandHandlerTest()
        {
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);

            _mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);

            _mockGarmentAvalProductRepository = CreateMock<IGarmentAvalProductRepository>();
            _mockGarmentAvalProductItemRepository = CreateMock<IGarmentAvalProductItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalProductItemRepository);

            _mockGarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockGarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockGarmentCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();
            _MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutDetailRepository);

            _mockGarmentFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
            _mockGarmentFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentFinishingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentFinishingOutItemRepository);

            //_mockGarmentDeliveryReturnRepository = CreateMock<IGarmentDeliveryReturnRepository>();
            _mockGarmentDeliveryReturnItemRepository = CreateMock<IGarmentDeliveryReturnItemRepository>();
            //_MockStorage.SetupStorage(_mockGarmentDeliveryReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentDeliveryReturnItemRepository);

            serviceProviderMock = new Mock<IServiceProvider>();
            _mockhttpService = CreateMock<IHttpClientService>();

            List<GarmentProductViewModel> garmentProductViewModels = new List<GarmentProductViewModel> {
                new GarmentProductViewModel
                {
                    Code = "test",
                    Composition = "",
                    Const = "",
                    Id = 1,
                    Name = "test",
                    ProductType = "",
                    Remark ="",
                    Tags = "",
                    UOM = new Infrastructure.External.DanLirisClient.Microservice.MasterResult.Uom
                    {
                        Id  = 1,
                        Unit = ""
                    },
                    Width = "",
                    Yarn = ""

                }
            };

            _mockhttpService.Setup(x => x.SendAsync(It.IsAny<HttpMethod>(),It.IsAny<string>(), It.IsAny<string>(), It.IsAny<HttpContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"data\": " + JsonConvert.SerializeObject(garmentProductViewModels) + "}") });
            serviceProviderMock.Setup(x => x.GetService(typeof(IHttpClientService))).Returns(_mockhttpService.Object);
        }

        private GetWIPQueryHandler CreateGetWIPQueryHandler()
        {
            return new GetWIPQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetWIPQueryHandler unitUnderTest = CreateGetWIPQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidPreparing = Guid.NewGuid();
            Guid guidPreparingItem = Guid.NewGuid();
            Guid guidCuttingIn = Guid.NewGuid();
            Guid guidCuttingInItem = Guid.NewGuid();
            Guid guidCuttingInDetail = Guid.NewGuid();
            Guid guidAvalProduct = Guid.NewGuid();
            Guid guidAvalProductItem = Guid.NewGuid();
            Guid guidCuttingOut = Guid.NewGuid();
            Guid guidCuttingOutItem = Guid.NewGuid();
            Guid guidCuttingOutDetail = Guid.NewGuid();
            Guid guidFinishingOut = Guid.NewGuid();
            Guid guidFinishingOutItem = Guid.NewGuid();
            Guid guidFinishingOutDetail = Guid.NewGuid();
            Guid guidDeliveryReturn = Guid.NewGuid();
            Guid guidDeliveryReturnItem = Guid.NewGuid();

            GetWIPQuery getMonitoring = new GetWIPQuery(1, 25, "{}", DateTime.Now.AddDays(5), "token");

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>
                {
                    new Domain.GarmentPreparings.GarmentPreparingItem(guidPreparingItem, 1, new Domain.GarmentPreparings.ValueObjects.ProductId(1), "1", "", "", 1, new Domain.GarmentPreparings.ValueObjects.UomId(1), "", "", 0, 50, guidPreparing,null,"fasilitas").GetReadModel()
                }.AsQueryable());

            _mockGarmentPreparingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingReadModel>
                {
                    new Domain.GarmentPreparings.GarmentPreparing(guidPreparing,1,"",new Domain.GarmentPreparings.ValueObjects.UnitDepartmentId(1),"","",DateTimeOffset.Now,"roNo","",true,new BuyerId(1), null,null).GetReadModel()
                }.AsQueryable());

            _mockGarmentCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>
                {
                    new GarmentCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPreparing,1,"",Guid.Empty,null).GetReadModel()
                }.AsQueryable());

            _mockGarmentCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>
                {
                    new GarmentCuttingIn(guidCuttingIn,"","Main Fabric","","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",DateTimeOffset.Now,4.5).GetReadModel()
                }.AsQueryable());

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    new GarmentCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPreparingItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"","","","",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
                }.AsQueryable());

            _mockGarmentAvalProductItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalProductItemReadModel>
                {
                    new GarmentAvalProductItem(guidAvalProductItem,guidAvalProduct,new GarmentPreparingId(guidPreparing.ToString()),new GarmentPreparingItemId(guidPreparingItem.ToString()),new Domain.GarmentAvalProducts.ValueObjects.ProductId(1),"","","",9,new Domain.GarmentAvalProducts.ValueObjects.UomId(1),"",1,false).GetReadModel()
                }.AsQueryable());
            _mockGarmentAvalProductRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalProductReadModel>
                {
                    new GarmentAvalProduct(guidAvalProduct,"","",DateTimeOffset.Now,new UnitDepartmentId (1),"","").GetReadModel()
                }.AsQueryable());
            _mockGarmentCuttingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>
            {
                    new GarmentCuttingOutDetail(new Guid(),guidCuttingOutItem,new SizeId(1),"","",100,100,new Domain.Shared.ValueObjects.UomId(1),"",10,10).GetReadModel()
            }.AsQueryable());

            _mockGarmentCuttingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>
                {
                    new GarmentCuttingOutItem(guidCuttingOutItem,new Guid() ,new Guid(),guidCuttingOut,new Domain.Shared.ValueObjects.ProductId(1),"","","",100).GetReadModel()
                }.AsQueryable());
            _mockGarmentCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutReadModel>
                {
                     new GarmentCuttingOut(guidCuttingOut, "", "SEWING",new UnitDepartmentId(1),"","",DateTime.Now,"ro","article",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"cm","cmo",false).GetReadModel()
                }.AsQueryable());


            _mockGarmentFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutItemReadModel>
                {
                    new GarmentFinishingOutItem(guidFinishingOutItem,guidFinishingOut,new Guid(),new Guid(),new Domain.Shared.ValueObjects.ProductId(1),"","","",new SizeId(1),"",10, new Domain.Shared.ValueObjects.UomId(1),"","",10,10,10).GetReadModel()
                }.AsQueryable());

            _mockGarmentFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>
                {
                    new GarmentFinishingOut(guidFinishingOut,"",new UnitDepartmentId(1),"","","GUDANG JADI",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",false).GetReadModel()
                }.AsQueryable());

            _mockGarmentDeliveryReturnItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentDeliveryReturnItemReadModel>
                {
                    new GarmentDeliveryReturnItem(guidDeliveryReturnItem,guidDeliveryReturn,1,1,guidPreparingItem.ToString(), new Domain.GarmentDeliveryReturns.ValueObjects.ProductId(1),"","","","",9, new Domain.GarmentDeliveryReturns.ValueObjects.UomId(1),"","","","","","").GetReadModel()
                }.AsQueryable());


            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
