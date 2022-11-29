using Barebone.Tests;
using Manufactures.Application.GarmentSample.SampleExpenditureGoods.Queries;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentSampleExpenditureGoodControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentSampleExpenditureGoodRepository> _mockGarmentSampleExpenditureGoodRepository;
        private readonly Mock<IGarmentSampleExpenditureGoodItemRepository> _mockGarmentSampleExpenditureGoodItemRepository;

        public GarmentSampleExpenditureGoodControllerTests() : base()
        {
            _mockGarmentSampleExpenditureGoodRepository = CreateMock<IGarmentSampleExpenditureGoodRepository>();
            _mockGarmentSampleExpenditureGoodItemRepository = CreateMock<IGarmentSampleExpenditureGoodItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleExpenditureGoodItemRepository);
        }

        private GarmentSampleExpenditureGoodController CreateGarmentSampleExpenditureGoodController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleExpenditureGoodController controller = (GarmentSampleExpenditureGoodController)Activator.CreateInstance(typeof(GarmentSampleExpenditureGoodController), _MockServiceProvider.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        [Fact]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentSampleExpenditureGoodItem garmentExpenditureGoodItem = new GarmentSampleExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentSampleExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>()
                {
                    garmentExpenditureGoodItem.GetReadModel()
                }.AsQueryable());


            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleExpenditureGoodReadModel, bool>>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleExpenditureGoodItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleExpenditureGoodItem>()
                {
                    new GarmentSampleExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1,0, new UomId(1), null, null, 1, 1)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleExpenditureGoodReadModel, bool>>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleExpenditureGoodItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleExpenditureGoodItem>()
                {
                    new GarmentSampleExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), "size", 1,0, new UomId(1), null, "desc", 1, 1)
                });

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString(), "buyerCode");

            // Assert
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }



        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false, 1));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleExpenditureGoodCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSampleExpenditureGoodCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSampleExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false, 1)
                );
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleExpenditureGoodReadModel, bool>>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });
            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSampleExpenditureGoodCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false, 1)
                );
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleExpenditureGoodReadModel, bool>>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });
            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task Put_IsReceived_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
        //    Guid ExpenditureGoodGuid = Guid.NewGuid();
        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<UpdateIsReceivedGarmentSampleExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false, 1)
        //        );
        //    // Act
        //    var result = await unitUnderTest.Patch(Guid.NewGuid().ToString(), false);

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleExpenditureGoodCommand command = new UpdateDatesGarmentSampleExpenditureGoodCommand(ids, DateTimeOffset.Now);
            _MockMediator
                .Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior_BadRequest()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleExpenditureGoodCommand command = new UpdateDatesGarmentSampleExpenditureGoodCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentSampleExpenditureGoodCommand command2 = new UpdateDatesGarmentSampleExpenditureGoodCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }

        //[Fact]
        //public async Task GetMonitoring_Return_Success()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
        //    Guid ExpenditureGoodGuid = Guid.NewGuid();
        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<GetMonitoringExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new GarmentMonitoringExpenditureGoodListViewModel());

        //    // Act
        //    var result = await unitUnderTest.GetMonitoring(1, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), 1, 25, "{}");

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>() {
                    new GarmentSampleExpenditureGoodReadModel(ExpenditureGoodGuid)
                }
                .AsQueryable());

            //_mockGarmentSampleExpenditureGoodRepository
            //   .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleExpenditureGoodReadModel>>()))
            //   .Returns(new List<GarmentSampleExpenditureGood>()
            //   {
            //        new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
            //   });

            //_mockGarmentSampleExpenditureGoodItemRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>()
            //   {
            //       new GarmentSampleExpenditureGoodItemReadModel(ExpenditureGoodItemGuid)
            //   }
            //   .AsQueryable());

            //_mockGarmentSampleExpenditureGoodItemRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleExpenditureGoodItemReadModel>>()))
            //    .Returns(new List<GarmentSampleExpenditureGoodItem>() { 
            //        new GarmentSampleExpenditureGoodItem(ExpenditureGoodItemGuid,ExpenditureGoodItemGuid,Guid.NewGuid(),new SizeId(1),"sizeName",1,1,new UomId(1),"uomUnit","description",1,1)
            //    });

            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentSampleExpenditureGoodReadModel>>()))
                .Returns(new List<object>()
                .AsQueryable());


            // Act
            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLS_Throws_InternalServerError()
        {
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }


        //[Fact]
        //public async Task GetMutation_Return_Success()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
        //    Guid ExpenditureGoodGuid = Guid.NewGuid();
        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<GetMutationExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new GarmentMutationExpenditureGoodListViewModel());

        //    // Act
        //    var result = await unitUnderTest.GetMutation(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), 1, 25, "{}");

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        //[Fact]
        //public async Task GetXLSMutationBehavior()
        //{
        //    var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<GetXlsMutationExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new MemoryStream());

        //    var result = await unitUnderTest.GetXlsMutation(DateTime.Now, DateTime.Now, 1, 25, "{}");
        //    Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        //}

        //[Fact]
        //public async Task GetMutationXLS_Throws_InternalServerError()
        //{
        //    var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<GetXlsMutationExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
        //        .Throws(new Exception());

        //    var result = await unitUnderTest.GetXlsMutation(DateTime.Now, DateTime.Now, 1, 25, "{}");
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        //}

        [Fact]
        public async Task GetReport_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetMonitoringSampleExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentMonitoringSampleExpenditureGoodListViewModel());

            // Act
            var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task GetXLSReportBehavior()
        //{
        //    var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<GetXlsSampleExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new MemoryStream());

        //    var result = await unitUnderTest.GetXlsReport(DateTime.Now, DateTime.Now, 1, 25, "{}");
        //    Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        //}

        //[Fact]
        //public async Task GetReportXLS_Throws_InternalServerError()
        //{
        //    var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<GetXlsSampleExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
        //        .Throws(new Exception());

        //    var result = await unitUnderTest.GetXls(DateTime.Now, DateTime.Now, 1, 25, "{}");
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        //}

        [Fact]
        public async Task GetForTraceable_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentSampleExpenditureGoodItem garmentExpenditureGoodItem = new GarmentSampleExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentSampleExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>()
                {
                    garmentExpenditureGoodItem.GetReadModel()
                }.AsQueryable());


            // Act
            var result = await unitUnderTest.GetTraceablebyRONo("RONo");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task GetBasicPrice_Return_Success()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentSampleExpenditureGoodController();
        //    Guid ExpenditureGoodGuid = Guid.NewGuid();
        //    Guid ExpenditureGoodItemGuid = Guid.NewGuid();
        //    //_mockGarmentSampleExpenditureGoodRepository
        //    //    .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //    //    .Returns(new List<GarmentSampleExpenditureGoodReadModel>() {
        //    //        new GarmentSampleExpenditureGoodReadModel(ExpenditureGoodGuid)
        //    //    }
        //    //    .AsQueryable());

        //    _mockGarmentSampleExpenditureGoodRepository
        //       .Setup(s => s.BasicPriceByRO(It.IsAny<string>(), It.IsAny<string>()))
        //       .Returns(It.IsAny<double>());

        //    //_mockGarmentSampleExpenditureGoodItemRepository
        //    //   .Setup(s => s.Query)
        //    //   .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>()
        //    //   {
        //    //       new GarmentSampleExpenditureGoodItemReadModel(ExpenditureGoodItemGuid)
        //    //   }
        //    //   .AsQueryable());

        //    //_mockGarmentSampleExpenditureGoodItemRepository
        //    //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleExpenditureGoodItemReadModel>>()))
        //    //    .Returns(new List<GarmentSampleExpenditureGoodItem>() {
        //    //        new GarmentSampleExpenditureGoodItem(ExpenditureGoodItemGuid,ExpenditureGoodItemGuid,Guid.NewGuid(),new SizeId(1),"sizeName",1,1,new UomId(1),"uomUnit","description",1,1)
        //    //    });


        //    // Act
        //    var filterData = new
        //    {
        //        RONo = "RONo",
        //        UnitId = 1
        //    };

        //    string filter = JsonConvert.SerializeObject(filterData);
        //    var result = await unitUnderTest.GetBasicPriceByRONo("", filter);

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        [Fact]
        public async Task GetForTraceablebyInvoice_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentSampleExpenditureGoodItem garmentExpenditureGoodItem = new GarmentSampleExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentSampleExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>()
                {
                    garmentExpenditureGoodItem.GetReadModel()
                }.AsQueryable());


            // Act
            var result = await unitUnderTest.GetTraceablebyRONo("invoice");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
        //
        [Fact]
        public async Task GetForOmzet_Success_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>().AsQueryable());

            Guid SampleExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(SampleExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid SampleExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentSampleExpenditureGoodItem garmentSamppleExpenditureGoodItem = new GarmentSampleExpenditureGoodItem(SampleExpenditureGoodItemGuid, SampleExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentSampleExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>()
                {
                    garmentSamppleExpenditureGoodItem.GetReadModel()
                }.AsQueryable());


            // Act
            var result = await unitUnderTest.GetExpenditureForOmzet(DateTime.Now, DateTime.Now, "", 7);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetForAnnualOmzet_Success_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleExpenditureGoodController();

            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>().AsQueryable());

            Guid SampleExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentSampleExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentSampleExpenditureGood>()
                {
                    new GarmentSampleExpenditureGood(SampleExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid SampleExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentSampleExpenditureGoodItem garmentSamppleExpenditureGoodItem = new GarmentSampleExpenditureGoodItem(SampleExpenditureGoodItemGuid, SampleExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentSampleExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>()
                {
                    garmentSamppleExpenditureGoodItem.GetReadModel()
                }.AsQueryable());


            // Act
            var result = await unitUnderTest.GetExpenditureForAnnualOmzet(DateTime.Now, DateTime.Now, 7);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}