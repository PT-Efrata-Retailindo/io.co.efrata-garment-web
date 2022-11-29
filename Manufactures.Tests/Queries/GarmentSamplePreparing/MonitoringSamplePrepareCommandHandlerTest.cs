using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentSample.SamplePreparings.Queries.GetMonitoringPrepareSample;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSamplePreparing
{
    public class MonitoringReceiptSampleCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentPreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentPreparingItemRepository;
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockGarmentCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
		private readonly Mock<IGarmentSampleAvalProductRepository> _mockGarmentAvalProductRepository;
		private readonly Mock<IGarmentSampleAvalProductItemRepository> _mockGarmentAvalProductItemRepository;
		private readonly Mock<IGarmentSampleDeliveryReturnRepository> _mockGarmentDeliveryReturnRepository;
		private readonly Mock<IGarmentSampleDeliveryReturnItemRepository> _mockGarmentDeliveryReturnItemRepository;
		protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;

        public MonitoringReceiptSampleCommandHandlerTest()
        {
            _mockGarmentPreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);

            _mockGarmentCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();

			_mockGarmentAvalProductRepository = CreateMock<IGarmentSampleAvalProductRepository>();
			_mockGarmentAvalProductItemRepository = CreateMock<IGarmentSampleAvalProductItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentAvalProductRepository);
			_MockStorage.SetupStorage(_mockGarmentAvalProductItemRepository);

			_mockGarmentDeliveryReturnRepository = CreateMock<IGarmentSampleDeliveryReturnRepository>();
			_mockGarmentDeliveryReturnItemRepository = CreateMock<IGarmentSampleDeliveryReturnItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentDeliveryReturnRepository);
			_MockStorage.SetupStorage(_mockGarmentDeliveryReturnItemRepository);

			_MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
            serviceProviderMock = new Mock<IServiceProvider>();
            _mockhttpService = CreateMock<IHttpClientService>();

        }

        private GetMonitoringSamplePrepareQueryHandler CreateGetMonitoringPrepareQueryHandler()
        {
            return new GetMonitoringSamplePrepareQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			GetMonitoringSamplePrepareQueryHandler unitUnderTest = CreateGetMonitoringPrepareQueryHandler();
			CancellationToken cancellationToken = CancellationToken.None;

			Guid guidPrepare = Guid.NewGuid();
			Guid guidPrepareItem = Guid.NewGuid();
			Guid guidCuttingIn = Guid.NewGuid();
			Guid guidCuttingInItem = Guid.NewGuid();
			Guid guidCuttingInDetail = Guid.NewGuid();
			Guid guidAvalProduct = Guid.NewGuid();
			Guid guidAvalProductItem = Guid.NewGuid();
			Guid guidDeliveryReturn = Guid.NewGuid();
			Guid guidDeliveryReturnItem = Guid.NewGuid();
			GetMonitoringSamplePrepareQuery getXlsPrepareQuery = new GetMonitoringSamplePrepareQuery(1, 25, "{}", 1, DateTime.Now, DateTime.Now.AddDays(2), "token");

			_mockGarmentPreparingItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingItemReadModel>
				{
					new GarmentSamplePreparingItem(guidPrepareItem, 1,new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1), "", "", "", 0, new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1), "", "", 0, 50, guidPrepare,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentPreparingRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSamplePreparingReadModel>
				{
					new Domain.GarmentSample.SamplePreparings.GarmentSamplePreparing(guidPrepare,1,"",new Domain.GarmentSample.SamplePreparings.ValueObjects.UnitDepartmentId(1),"","",DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified),"roNo","",true,new BuyerId(1), null,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInItemReadModel>
				{
					new GarmentSampleCuttingInItem(guidCuttingInItem,guidCuttingIn,guidPrepare,1,"",Guid.Empty,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInReadModel>
				{
					new GarmentSampleCuttingIn(guidCuttingIn,"","Main Fabric","","","",new UnitDepartmentId(1),"","",DateTimeOffset.Now,4.5).GetReadModel()
				}.AsQueryable());

			_mockGarmentCuttingInDetailRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleCuttingInDetailReadModel>
				{
					new GarmentSampleCuttingInDetail(guidCuttingInDetail,guidCuttingInItem,guidPrepareItem,Guid.Empty,Guid.Empty,new Domain.Shared.ValueObjects.ProductId(1),"","","","",9,new Domain.Shared.ValueObjects.UomId(1),"",4,new Domain.Shared.ValueObjects.UomId(1),"",1,100,100,5.5,null).GetReadModel()
				}.AsQueryable());

			_mockGarmentAvalProductItemRepository
			   .Setup(s => s.Query)
			   .Returns(new List<GarmentSampleAvalProductItemReadModel>
			   {
					new GarmentSampleAvalProductItem(guidAvalProductItem,guidAvalProduct,new Domain.GarmentSample.SampleAvalProducts.ValueObjects.GarmentSamplePreparingId(guidPrepare.ToString()),new Domain.GarmentSample.SampleAvalProducts.ValueObjects.GarmentSamplePreparingItemId(guidPrepareItem.ToString()),new Domain.GarmentSample.SampleAvalProducts.ValueObjects.ProductId(1),"","","",9,new Domain.GarmentSample.SampleAvalProducts.ValueObjects.UomId(1),"",100,false).GetReadModel()
			   }.AsQueryable());
			_mockGarmentAvalProductRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleAvalProductReadModel>
				{
					new GarmentSampleAvalProduct(guidAvalProduct,"","",DateTimeOffset.Now, new UnitDepartmentId(1), null, null).GetReadModel()
				}.AsQueryable());
			_mockGarmentDeliveryReturnItemRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleDeliveryReturnItemReadModel>
				{
					new GarmentSampleDeliveryReturnItem(guidDeliveryReturnItem,guidDeliveryReturn,1,1,guidPrepareItem.ToString(), new Domain.GarmentSample.SampleDeliveryReturns.ValueObjects.ProductId(1),"","","","",9, new Domain.GarmentSample.SampleDeliveryReturns.ValueObjects.UomId(1),"").GetReadModel()
				}.AsQueryable());
			_mockGarmentDeliveryReturnRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentSampleDeliveryReturnReadModel>
				{
					 new GarmentSampleDeliveryReturn(guidDeliveryReturn, "", "", "", 1, "", 1, guidPrepare.ToString(), DateTimeOffset.Now, "", new  Domain.GarmentSample.SampleDeliveryReturns.ValueObjects.UnitDepartmentId(1), "", "", new Domain.GarmentSample.SampleDeliveryReturns.ValueObjects.StorageId(1), "", "", true).GetReadModel()
				}.AsQueryable());

			// Act
			var result = await unitUnderTest.Handle(getXlsPrepareQuery, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}

	}
}