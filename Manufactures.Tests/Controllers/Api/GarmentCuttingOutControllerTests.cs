using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Application.GarmentCuttingOuts.Queries;
using System.IO;
using FluentAssertions;
using Newtonsoft.Json;
using Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts;
using Manufactures.Application.GarmentCuttingOuts.Queries.GetCuttingOutForTraceable;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentCuttingOutControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentCuttingOutRepository> _mockGarmentCuttingOutRepository;
        private Mock<IGarmentCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
        private Mock<IGarmentCuttingOutDetailRepository> _mockGarmentCuttingOutDetailRepository;
        private Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
        private Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
        private Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
        private Mock<IGarmentSewingDORepository> _mockGarmentSewingDORepository;
        private Mock<IGarmentSewingDOItemRepository> _mockGarmentSewingDOItemRepository;

        public GarmentCuttingOutControllerTests() : base()
        {
            _mockGarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockGarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockGarmentCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();
            _mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockGarmentSewingDORepository = CreateMock<IGarmentSewingDORepository>();
            _mockGarmentSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingDORepository);
            _MockStorage.SetupStorage(_mockGarmentSewingDOItemRepository);
        }

        private GarmentCuttingOutController CreateGarmentCuttingOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentCuttingOutController controller = (GarmentCuttingOutController)Activator.CreateInstance(typeof(GarmentCuttingOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllCuttingOutQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CuttingOutListViewModel
                {
                    data = new List<GarmentCuttingOutListDto>()
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Get_with_order()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllCuttingOutQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CuttingOutListViewModel
                {
                    data = new List<GarmentCuttingOutListDto>()
                });

            // Act
            var orderData = new
            {
                article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1,1, order,new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Get_with_Keyword_and_Order()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllCuttingOutQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CuttingOutListViewModel
                {
                    data = new List<GarmentCuttingOutListDto>()
                });

            // Act
            var orderData = new
            {
                article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1,25,order,new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Get_with_Keyword_No_Order()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllCuttingOutQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CuttingOutListViewModel
                {
                    data = new List<GarmentCuttingOutListDto>()
                });

            // Act
            var result = await unitUnderTest.Get(1, 25, "{}", new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _mockGarmentCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOut>()
                {
                    new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null,false)
                });

            Guid cuttingOutItemGuid = Guid.NewGuid();
            _mockGarmentCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOutItem>()
                {
                    new GarmentCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null, 1)
                });

            _mockGarmentCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOutDetail>()
                {
                    new GarmentCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), null, null, 1, 1, new UomId(1), null, 1, 1)
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
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _mockGarmentCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOut>()
                {
                    new GarmentCuttingOut(cuttingOutGuid,"cutOutNo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", "art", new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null,false)
                });

            Guid cuttingOutItemGuid = Guid.NewGuid();
            _mockGarmentCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOutItem>()
                {
                    new GarmentCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1),"productCode", "productName", "design", 1)
                });

            _mockGarmentCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingOutDetail>()
                {
                    new GarmentCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), "size", "color", 1, 1, new UomId(1), "uom", 1, 1)
                });

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString(), "buyerCode");

            // Assert
            //Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, false));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentCuttingOutCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentCuttingOutCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, false));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentCuttingOutCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_Return_BadRequest()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();

            _mockGarmentSewingDORepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOReadModel>()
                {
                    new GarmentSewingDO(Guid.NewGuid(), null, cuttingOutGuid, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now).GetReadModel()
                }.AsQueryable());

            _mockGarmentSewingDOItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOItemReadModel, bool>>>()))
               .Returns(new List<GarmentSewingDOItem>()
               {
                    new GarmentSewingDOItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new ProductId(1),"producCode","productName","designColor",new SizeId(1),"sizeName",5,new UomId(1),"uomUnit","color",1,1,1)
               });

           
            // Act
            var result = await unitUnderTest.Delete(cuttingOutGuid.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();

            _mockGarmentSewingDORepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOReadModel>()
                {
                    new GarmentSewingDO(Guid.NewGuid(), null, cuttingOutGuid, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now).GetReadModel()
                }.AsQueryable());

            _mockGarmentSewingDOItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOItemReadModel, bool>>>()))
               .Returns(new List<GarmentSewingDOItem>()
               {
                    new GarmentSewingDOItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new ProductId(1),"producCode","productName","designColor",new SizeId(1),"sizeName",1,new UomId(1),"uomUnit","color",1,1,1)
               });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, false));

            // Act
            var result = await unitUnderTest.Delete(cuttingOutGuid.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
		public async Task GetMonitoringBehavior()
		{
			var unitUnderTest = CreateGarmentCuttingOutController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringCuttingQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringCuttingListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}

        [Fact]
        public async Task GetXLSBehavior_Throws_Exception()
        {
            var unitUnderTest = CreateGarmentCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsCuttingQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());
            var result =await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.InternalServerError);

        }

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsCuttingQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetComplete_ExpectedBehavior()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentCuttingOutController();
            _mockGarmentCuttingOutRepository
                 .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(new List<GarmentCuttingOutReadModel>() { new GarmentCuttingOutReadModel(id)}.AsQueryable());

            //_mockGarmentCuttingOutRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutReadModel>>()))
            //    .Returns(new List<GarmentCuttingOut>()
            //    {
            //        new GarmentCuttingOut(id, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null,false)
            //    });

            //_mockGarmentCuttingOutItemRepository
            //   .Setup(s => s.Query)
            //   .Returns(new List<GarmentCuttingOutItemReadModel>()
            //   {
            //       new GarmentCuttingOutItemReadModel(id)
            //   }.AsQueryable());

            //_mockGarmentCuttingOutItemRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutItemReadModel>>()))
            //    .Returns(new List<GarmentCuttingOutItem>()
            //    {
            //        new GarmentCuttingOutItem(id,id,id,id,new ProductId(1),"productCode","productName","designColor",1)
            //    });

            //_mockGarmentCuttingOutDetailRepository
            // .Setup(s => s.Query)
            // .Returns(new List<GarmentCuttingOutDetailReadModel>()
            // {
            //       new GarmentCuttingOutDetailReadModel(id)
            // }.AsQueryable());

            //_mockGarmentCuttingOutDetailRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutDetailReadModel>>()))
            //    .Returns(new List<GarmentCuttingOutDetail>()
            //    {
            //        new GarmentCuttingOutDetail(id,id,new SizeId(1),"sizeName","color",1,1,new UomId(1),"cuttingOutUoamUnit",1,1)
            //    }) ;

            _mockGarmentCuttingOutRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentCuttingOutReadModel>>()))
                .Returns(new List<object>()
                .AsQueryable());

            // Act
            var orderData = new
            {
                CuttingOutType = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1,25,order,new List<string>(),"","{}");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
		public async Task GetXLSBookKeepingBehavior()
		{
			var unitUnderTest = CreateGarmentCuttingOutController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsCuttingQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "bookkeeping", 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

		}

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentCuttingOutCommand command = new UpdateDatesGarmentCuttingOutCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentCuttingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentCuttingOutCommand command = new UpdateDatesGarmentCuttingOutCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentCuttingOutCommand command2 = new UpdateDatesGarmentCuttingOutCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }

        [Fact]
        public async Task GetTraceabeBehavior()
        {
            var unitUnderTest = CreateGarmentCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetCuttingOutForTraceableQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetCuttingOutForTraceableListViewModel());

            // Act
            var result = await unitUnderTest.GetForTraceable("ro1,ro2");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }


    }
}