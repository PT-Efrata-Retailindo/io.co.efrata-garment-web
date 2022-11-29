using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentFinishingOuts.Queries;
using Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
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
    public class GarmentFinishingOutControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentFinishingOutRepository> _mockGarmentFinishingOutRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockGarmentFinishingOutItemRepository;
        private readonly Mock<IGarmentFinishingOutDetailRepository> _mockGarmentFinishingOutDetailRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;

        public GarmentFinishingOutControllerTests() : base()
        {
            _mockGarmentFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
            _mockGarmentFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();
            _mockGarmentFinishingOutDetailRepository = CreateMock<IGarmentFinishingOutDetailRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentFinishingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentFinishingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentFinishingOutDetailRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
        }

        private GarmentFinishingOutController CreateGarmentFinishingOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentFinishingOutController controller = (GarmentFinishingOutController)Activator.CreateInstance(typeof(GarmentFinishingOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentFinishingOutController();

            _mockGarmentFinishingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentFinishingOutReadModel>().AsQueryable());

            Guid finishingOutGuid = Guid.NewGuid();
            _mockGarmentFinishingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingOutReadModel>>()))
                .Returns(new List<GarmentFinishingOut>()
                {
                    new GarmentFinishingOut(finishingOutGuid, null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            GarmentFinishingOutItem garmentFinishingOutItem = new GarmentFinishingOutItem(finishingOutItemGuid, finishingOutGuid, finishingInGuid, finishingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1, 1, 1);
            _mockGarmentFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutItemReadModel>()
                {
                    garmentFinishingOutItem.GetReadModel()
                }.AsQueryable());

            GarmentFinishingOutDetail garmentFinishingOutDetail = new GarmentFinishingOutDetail(Guid.NewGuid(), finishingOutItemGuid, new SizeId(1), null, 1, new UomId(1), null);
            _mockGarmentFinishingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutDetailReadModel>()
                {
                    garmentFinishingOutDetail.GetReadModel()
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
            var unitUnderTest = CreateGarmentFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _mockGarmentFinishingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOut>()
                {
                    new GarmentFinishingOut(finishingOutGuid, null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            _mockGarmentFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutItem>()
                {
                    new GarmentFinishingOutItem(finishingOutItemGuid, finishingOutGuid, finishingInGuid, finishingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1,1,1)
                });

            _mockGarmentFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutDetail>()
                {
                    new GarmentFinishingOutDetail(Guid.NewGuid(), finishingOutItemGuid, new SizeId(1), null, 1, new UomId(1), null)
                });

            //_mockFinishingInItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentFinishingInItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _mockGarmentFinishingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOut>()
                {
                    new GarmentFinishingOut(finishingOutGuid, null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", "art", new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            _mockGarmentFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutItem>()
                {
                    new GarmentFinishingOutItem(finishingOutItemGuid, finishingOutGuid, finishingInGuid, finishingInItemGuid, new ProductId(1), null, null, "design", new SizeId(1), "size", 1, new UomId(1), null, "color", 1,1,1)
                });

            _mockGarmentFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutDetail>()
                {
                    new GarmentFinishingOutDetail(Guid.NewGuid(), finishingOutItemGuid, new SizeId(1), "size", 1, new UomId(1), null)
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
            var unitUnderTest = CreateGarmentFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentFinishingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingOut(finishingOutGuid, null,  new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentFinishingOutCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentFinishingOutCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());


            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentFinishingOutCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentFinishingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingOut(finishingOutGuid, null,  new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentFinishingOutCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentFinishingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingOut(finishingOutGuid, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
		[Fact]
		public async Task GetMonitoringBehavior()
		{
			var unitUnderTest = CreateGarmentFinishingOutController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringFinishingQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringFinishingListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetXLSBehavior()
		{
			var unitUnderTest = CreateGarmentFinishingOutController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsFinishingQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "",1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
			;
		}

        [Fact]
        public async Task GetXLS_Throws_Exception()
        {
            var unitUnderTest = CreateGarmentFinishingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsFinishingQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Assert
            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");
           Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
           
        }


        [Fact]
        public async Task GetColor_Return_Exception()
        {
            var unitUnderTest = CreateGarmentFinishingOutController();
            var id = Guid.NewGuid();
            _mockGarmentFinishingOutRepository
               .Setup(s => s.ReadColor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns(new List<GarmentFinishingOutReadModel>() { new GarmentFinishingOutReadModel(id) }.AsQueryable());

            _mockGarmentFinishingOutRepository
               .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingOutReadModel>>()))
               .Returns(new List<GarmentFinishingOut>()
               {
                    new GarmentFinishingOut(id,"finishingOutNo",new UnitDepartmentId(1),"unitToCode","unitToName","Finishing",DateTimeOffset.Now, "RONo","article", new UnitDepartmentId(1),"unitCode", "unitName",new GarmentComodityId(1),"comodityCode","comodityName",true)
               });

            _mockGarmentFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutItemReadModel>()
                {
                   new GarmentFinishingOutItemReadModel(id)
                }.AsQueryable());

            // Act
            var orderData = new
            {
                Name = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetColor(1,25, order,new List<string>(),"","{}");

            // Assert
            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));

        }

        [Fact]
        public async Task GetComplete_Success()
        {
            var unitUnderTest = CreateGarmentFinishingOutController();

            var id = Guid.NewGuid();
            _mockGarmentFinishingOutRepository
                 .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(new List<GarmentFinishingOutReadModel>().AsQueryable());

            //_mockGarmentFinishingOutRepository
            //  .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingOutReadModel>>()))
            //  .Returns(new List<GarmentFinishingOut>()
            //  {
            //        new GarmentFinishingOut(id,"finishingOutNo",new UnitDepartmentId(1),"unitToCode","unitToName","Finishing",DateTimeOffset.Now, "RONo","article", new UnitDepartmentId(1),"unitCode", "unitName",new GarmentComodityId(1),"comodityCode","comodityName",true)
            //  });

            //_mockGarmentFinishingOutItemRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentFinishingOutItemReadModel>()
            //   {
            //       new GarmentFinishingOutItemReadModel(id)
            //   }.AsQueryable());

            //_mockGarmentFinishingOutItemRepository
            // .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingOutItemReadModel>>()))
            // .Returns(new List<GarmentFinishingOutItem>() { new GarmentFinishingOutItem(id,id,id,id,new ProductId(1),"productCode", "productName","designColor",new SizeId(1),"sizeName",1,new UomId(1),"uomUnit","color",1,1,1) });

            //_mockGarmentFinishingOutDetailRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentFinishingOutDetailReadModel>()
            //    {
            //        new GarmentFinishingOutDetailReadModel(id)
            //    }.AsQueryable());

            //_mockGarmentFinishingOutDetailRepository
            //   .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingOutDetailReadModel>>()))
            //   .Returns(new List<GarmentFinishingOutDetail>()
            //   {
            //        new GarmentFinishingOutDetail(id, id, new SizeId(1), "size", 1, new UomId(1), null)
            //   });

            _mockGarmentFinishingOutRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentFinishingOutReadModel>>()))
                .Returns(new List<object>()
                .AsQueryable());

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
		public async Task GetXLSBookkeepingBehavior()
		{
			var unitUnderTest = CreateGarmentFinishingOutController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsFinishingQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "bookkeeping", 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
			;
		}

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentFinishingOutCommand command = new UpdateDatesGarmentFinishingOutCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentFinishingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentFinishingOutCommand command = new UpdateDatesGarmentFinishingOutCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentFinishingOutCommand command2 = new UpdateDatesGarmentFinishingOutCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }

        [Fact]
        public async Task GetForTraceableBehavior()
        {
            var unitUnderTest = CreateGarmentFinishingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetTotalQuantityTraceableQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentTotalQtyTraceableListViewModel());

            // Act
            var result = await unitUnderTest.ForTraceable("RO1,RO2");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }


    }
}