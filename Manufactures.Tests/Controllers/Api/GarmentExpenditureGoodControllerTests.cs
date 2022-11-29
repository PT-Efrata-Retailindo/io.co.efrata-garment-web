using Barebone.Tests;
using Manufactures.Application.GarmentExpenditureGoods.Queries;
using Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods;
using Manufactures.Application.GarmentExpenditureGoods.Queries.GetReportExpenditureGoods;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
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

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentExpenditureGoodControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockGarmentExpenditureGoodRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockGarmentExpenditureGoodItemRepository;

        public GarmentExpenditureGoodControllerTests() : base()
        {
            _mockGarmentExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
            _mockGarmentExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockGarmentExpenditureGoodItemRepository);
        }

        private GarmentExpenditureGoodController CreateGarmentExpenditureGoodController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentExpenditureGoodController controller = (GarmentExpenditureGoodController)Activator.CreateInstance(typeof(GarmentExpenditureGoodController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1,0, new UomId(1), null, null, 1, 1);
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>()
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
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodItemReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGoodItem>()
                {
                    new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1,0, new UomId(1), null, null, 1, 1)
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
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodItemReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGoodItem>()
                {
                    new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), "size", 1,0, new UomId(1), null, "desc", 1, 1)
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
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false, 1));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentExpenditureGoodCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentExpenditureGoodCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false, 1)
                );

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentExpenditureGoodCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false, 1)
                );

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_IsReceived_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateIsReceivedGarmentExpenditureGoodCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentExpenditureGood(ExpenditureGoodGuid, null, null, new UnitDepartmentId(1), null, null, "RONo", "article", new GarmentComodityId(1), null, null, new BuyerId(1), null, null, DateTimeOffset.Now, null, null, 0, null, false, 1)
                );
            // Act
            var result = await unitUnderTest.Patch(Guid.NewGuid().ToString(), false);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentExpenditureGoodCommand command = new UpdateDatesGarmentExpenditureGoodCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentExpenditureGoodCommand command = new UpdateDatesGarmentExpenditureGoodCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentExpenditureGoodCommand command2 = new UpdateDatesGarmentExpenditureGoodCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }

        [Fact]
        public async Task GetMonitoring_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetMonitoringExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentMonitoringExpenditureGoodListViewModel());

            // Act
            var result = await unitUnderTest.GetMonitoring(DateTime.Now.AddDays(-1),DateTime.Now.AddDays(1),1,25,"{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReadModel>() { 
                    new GarmentExpenditureGoodReadModel(ExpenditureGoodGuid) 
                }
                .AsQueryable());

            //_mockGarmentExpenditureGoodRepository
            //   .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReadModel>>()))
            //   .Returns(new List<GarmentExpenditureGood>()
            //   {
            //        new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
            //   });

            //_mockGarmentExpenditureGoodItemRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentExpenditureGoodItemReadModel>()
            //   {
            //       new GarmentExpenditureGoodItemReadModel(ExpenditureGoodItemGuid)
            //   }
            //   .AsQueryable());

            //_mockGarmentExpenditureGoodItemRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodItemReadModel>>()))
            //    .Returns(new List<GarmentExpenditureGoodItem>() { 
            //        new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid,ExpenditureGoodItemGuid,Guid.NewGuid(),new SizeId(1),"sizeName",1,1,new UomId(1),"uomUnit","description",1,1)
            //    });

            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentExpenditureGoodReadModel>>()))
                .Returns(new List<object>()
                .AsQueryable());


            // Act
            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete( 1, 25, order,new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            var result = await unitUnderTest.GetXls( DateTime.Now, DateTime.Now, "", 1, 25, "{}");
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLS_Throws_InternalServerError()
        {
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsExpenditureGoodQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var result = await unitUnderTest.GetXls( DateTime.Now, DateTime.Now, "", 1, 25, "{}");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }


        [Fact]
        public async Task GetMutation_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetMutationExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentMutationExpenditureGoodListViewModel());

            // Act
            var result = await unitUnderTest.GetMutation(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetXLSMutationBehavior()
        {
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsMutationExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            var result = await unitUnderTest.GetXlsMutation(DateTime.Now, DateTime.Now, 1, 25, "{}");
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetMutationXLS_Throws_InternalServerError()
        {
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsMutationExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var result = await unitUnderTest.GetXlsMutation(DateTime.Now, DateTime.Now, 1, 25, "{}");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task GetReport_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetReportExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentReportExpenditureGoodListViewModel());

            // Act
            var result = await unitUnderTest.GetReport(DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1), 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetXLSReportBehavior()
        {
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsReportExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            var result = await unitUnderTest.GetXlsReport(DateTime.Now, DateTime.Now, 1, 25, "{}");
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetReportXLS_Throws_InternalServerError()
        {
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsReportExpenditureGoodsQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            var result = await unitUnderTest.GetXlsReport(DateTime.Now, DateTime.Now, 1, 25, "{}");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task GetForTraceable_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>()
                {
                    garmentExpenditureGoodItem.GetReadModel()
                }.AsQueryable());


            // Act
            var result = await unitUnderTest.GetTraceablebyRONo("RONo");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetBasicPrice_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            //_mockGarmentExpenditureGoodRepository
            //    .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns(new List<GarmentExpenditureGoodReadModel>() {
            //        new GarmentExpenditureGoodReadModel(ExpenditureGoodGuid)
            //    }
            //    .AsQueryable());

            _mockGarmentExpenditureGoodRepository
               .Setup(s => s.BasicPriceByRO(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(It.IsAny<double>());

            //_mockGarmentExpenditureGoodItemRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentExpenditureGoodItemReadModel>()
            //   {
            //       new GarmentExpenditureGoodItemReadModel(ExpenditureGoodItemGuid)
            //   }
            //   .AsQueryable());

            //_mockGarmentExpenditureGoodItemRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodItemReadModel>>()))
            //    .Returns(new List<GarmentExpenditureGoodItem>() {
            //        new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid,ExpenditureGoodItemGuid,Guid.NewGuid(),new SizeId(1),"sizeName",1,1,new UomId(1),"uomUnit","description",1,1)
            //    });


            // Act
            var filterData = new
            {
                RONo = "RONo",
                UnitId = 1
            };

            string filter = JsonConvert.SerializeObject(filterData);
            var result = await unitUnderTest.GetBasicPriceByRONo("", filter);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetForTraceablebyInvoice_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>()
                {
                    garmentExpenditureGoodItem.GetReadModel()
                }.AsQueryable());


            // Act
            var result = await unitUnderTest.GetTraceablebyRONo("invoice");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetForOmzet_Success_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>()
                {
                    garmentExpenditureGoodItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentExpenditureGoodController();

            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReadModel>().AsQueryable());

            Guid ExpenditureGoodGuid = Guid.NewGuid();
            _mockGarmentExpenditureGoodRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReadModel>>()))
                .Returns(new List<GarmentExpenditureGood>()
                {
                    new GarmentExpenditureGood(ExpenditureGoodGuid, null,null,new UnitDepartmentId(1),null,null,"RONo","article",new GarmentComodityId(1),null,null,new BuyerId(1), null, null,DateTimeOffset.Now,  null,null,0,null,false,1)
                });

            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, new Guid(), new SizeId(1), null, 1, 0, new UomId(1), null, null, 1, 1);
            _mockGarmentExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>()
                {
                    garmentExpenditureGoodItem.GetReadModel()
                }.AsQueryable());


            // Act
            var result = await unitUnderTest.GetExpenditureForAnnualOmzet(DateTime.Now, DateTime.Now, 7);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
