using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.MonitoringSewing;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleSewingOuts
{
    public class XlsSampleSewingCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockGarmentSewingOutRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
        private readonly Mock<IGarmentSampleSewingInRepository> _mockGarmentSewingInRepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockGarmentSewingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockGarmentCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
        private readonly Mock<IGarmentSampleRequestProductRepository> _mockGarmentSampleRequestProductRepository;
        private readonly Mock<IGarmentSampleRequestRepository> _mockGarmentSampleRequestRepository;
		private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
		private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;

		protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;
        public XlsSampleSewingCommandHandlerTest()
        {
            _mockGarmentSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
            _mockGarmentSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
            _mockGarmentCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockGarmentSewingInRepository = CreateMock<IGarmentSampleSewingInRepository>();
            _mockGarmentSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();
            _mockGarmentSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();
            _mockGarmentSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
			_mockGarmentSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
			_mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

			_MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestProductRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestRepository);
			_MockStorage.SetupStorage(_mockGarmentSamplePreparingRepository);
			_MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);

			serviceProviderMock = new Mock<IServiceProvider>();
            _mockhttpService = CreateMock<IHttpClientService>();


        }
        private GetXlsSampleSewingQueryHandler CreateGetMonitoringSewingQueryHandler()
        {
            return new GetXlsSampleSewingQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetXlsSampleSewingQueryHandler unitUnderTest = CreateGetMonitoringSewingQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidLoading = Guid.NewGuid();
            Guid guidLoadingItem = Guid.NewGuid();
            Guid guidSewingOut = Guid.NewGuid();
            Guid guidSewingOutItem = Guid.NewGuid();
            Guid guidCuttingIn = Guid.NewGuid();
            Guid guidCuttingInItem = Guid.NewGuid();
            Guid guidCuttingInDetail = Guid.NewGuid();
            Guid guidPrepare = Guid.NewGuid();
            Guid guidPrepareItem = Guid.NewGuid();
            Guid guidFinishingOut = Guid.NewGuid();
            Guid guidFinishingOutItem = Guid.NewGuid();

            GetXlsSampleSewingQuery getMonitoring = new GetXlsSampleSewingQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2),"CUTTING", "token");

            _mockGarmentCuttingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleCuttingInItemReadModel>
               {
                    new GarmentSampleCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"uENNo",Guid.Empty,"sewingOutNo").GetReadModel()
               }.AsQueryable());

            _mockGarmentCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInReadModel>
                {
                    new GarmentSampleCuttingIn(guidCuttingIn,"cutInNo","Main Fabric","cuttingFrom","ro","article",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,4.5).GetReadModel()
                }.AsQueryable());

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInDetailReadModel>
                {
                    new GarmentSampleCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"productCode","productName","designColor","fabricType",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
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
                    new GarmentSampleSewingOut(guidSewingOut,"",new Domain.Shared.ValueObjects.BuyerId(1),"","",new UnitDepartmentId(1),"","","CUTTING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
                }.AsQueryable());
            _mockGarmentSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInReadModel>
                {
                    new GarmentSampleSewingIn(guidSewingOut,"","",new Guid(),"",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","rono","article",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
                }.AsQueryable());
            _mockGarmentSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInItemReadModel>
                {
                    new GarmentSampleSewingInItem(new Guid(),guidSewingOut,new Guid(),new Guid(),Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"","","",new Domain.Shared.ValueObjects.SizeId(1),"",1,new Domain.Shared.ValueObjects.UomId(1),"","",1,1,1).GetReadModel()
                }.AsQueryable());
            _mockGarmentSampleRequestProductRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestProductReadModel>
                {
                    new GarmentSampleRequestProduct(guidSewingOutItem,guidSewingOutItem,"","","",new Domain.Shared.ValueObjects.SizeId(1),"","", 100,1).GetReadModel()
                }.AsQueryable());
            _mockGarmentSampleRequestRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestReadModel>
                {
                    new Manufactures.Domain.GarmentSample.SampleRequests.GarmentSampleRequest(guidSewingOutItem,"","","ro","",DateTimeOffset.Now,new Domain.Shared.ValueObjects.BuyerId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","","","",DateTimeOffset.Now,"","","",true,true,DateTimeOffset.Now,"",false,null,"","",false,null,"","","","","","",new Domain.Shared.ValueObjects.SectionId(1),"",null).GetReadModel()
                }.AsQueryable());

			var guidGarmentSamplePreparing = Guid.NewGuid();
			_mockGarmentSamplePreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingReadModel>
				{
					 new Domain.GarmentSample.SamplePreparings.GarmentSamplePreparing(guidGarmentSamplePreparing,1, "UENNo", new Domain.GarmentSample.SamplePreparings.ValueObjects.UnitDepartmentId(1), "UnitCode", "UnitName", DateTimeOffset.Now, "ro", "Article", true,new Domain.Shared.ValueObjects.BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			var garmentPreparingItem = Guid.NewGuid();
			_mockGarmentSamplePreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingItemReadModel>
				{
					 new GarmentSamplePreparingItem(guidGarmentSamplePreparing, 0, new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), null, null, null, 0, new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1), null, null, 0, 0, Guid.Empty,null).GetReadModel()
				}.AsQueryable());
			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
		[Fact]
		public async Task Handle_StateUnderTestBookeeping_ExpectedBehavior()
		{
			// Arrange
			GetXlsSampleSewingQueryHandler unitUnderTest = CreateGetMonitoringSewingQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidLoading = Guid.NewGuid();
			Guid guidLoadingItem = Guid.NewGuid();
			Guid guidSewingOut = Guid.NewGuid();
			Guid guidSewingOutItem = Guid.NewGuid();
			Guid guidCuttingIn = Guid.NewGuid();
			Guid guidCuttingInItem = Guid.NewGuid();
			Guid guidCuttingInDetail = Guid.NewGuid();
			Guid guidPrepare = Guid.NewGuid();
			Guid guidPrepareItem = Guid.NewGuid();
			Guid guidFinishingOut = Guid.NewGuid();
			Guid guidFinishingOutItem = Guid.NewGuid();

			GetXlsSampleSewingQuery getMonitoring = new GetXlsSampleSewingQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "BOOKKEEPING", "token");

			_mockGarmentCuttingInItemRepository
			   .Setup(s => s.Query)
			   .Returns(new List<GarmentSampleCuttingInItemReadModel>
			   {
					new GarmentSampleCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"uENNo",Guid.Empty,"sewingOutNo").GetReadModel()
			   }.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInReadModel>
				{
					new GarmentSampleCuttingIn(guidCuttingIn,"cutInNo","Main Fabric","cuttingFrom","ro","article",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,4.5).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInDetailReadModel>
				{
					new GarmentSampleCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"productCode","productName","designColor","fabricType",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
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
					new GarmentSampleSewingOut(guidSewingOut,"",new BuyerId(1),"","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","","CUTTING",DateTimeOffset.Now,"ro","",new UnitDepartmentId(1),"","",new GarmentComodityId(1),"","",true).GetReadModel()
				}.AsQueryable());
			_mockGarmentSewingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingInReadModel>
				{
					new GarmentSampleSewingIn(guidSewingOut,"","",new Guid(),"",new UnitDepartmentId(1),"","",new UnitDepartmentId(1),"","","rono","article",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",DateTimeOffset.Now).GetReadModel()
				}.AsQueryable());
			_mockGarmentSewingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleSewingInItemReadModel>
				{
					new GarmentSampleSewingInItem(new Guid(),guidSewingOut,new Guid(),new Guid(),Guid.Empty,Guid.Empty,new ProductId(1),"","","",new SizeId(1),"",1,new UomId(1),"","",1,1,1).GetReadModel()
				}.AsQueryable());
			_mockGarmentSampleRequestProductRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleRequestProductReadModel>
				{
					new GarmentSampleRequestProduct(guidSewingOutItem,guidSewingOutItem,"","","",new SizeId(1),"","", 100,1).GetReadModel()
				}.AsQueryable());
			_mockGarmentSampleRequestRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleRequestReadModel>
				{
					new Manufactures.Domain.GarmentSample.SampleRequests.GarmentSampleRequest(guidSewingOutItem,"","","ro","",DateTimeOffset.Now,new BuyerId(1),"","",new GarmentComodityId(1),"","","","",DateTimeOffset.Now,"","","",true,true,DateTimeOffset.Now,"",false,null,"","",false,null,"","","","","","",new SectionId(1),"",null).GetReadModel()
				}.AsQueryable());
			var guidGarmentSamplePreparing = Guid.NewGuid();
			_mockGarmentSamplePreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingReadModel>
				{
					 new Domain.GarmentSample.SamplePreparings.GarmentSamplePreparing(guidGarmentSamplePreparing,1, "UENNo", new Domain.GarmentSample.SamplePreparings.ValueObjects.UnitDepartmentId(1), "UnitCode", "UnitName", DateTimeOffset.Now, "ro", "Article", true,new Domain.Shared.ValueObjects.BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			var garmentPreparingItem = Guid.NewGuid();
			_mockGarmentSamplePreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingItemReadModel>
				{
					 new GarmentSamplePreparingItem(guidGarmentSamplePreparing, 0, new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), null, null, null, 0, new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1), null, null, 0, 0, Guid.Empty,null).GetReadModel()
				}.AsQueryable());
			// Act
			var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
