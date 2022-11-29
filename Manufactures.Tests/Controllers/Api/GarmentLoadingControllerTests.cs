using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentLoadings.Queries;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
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
    public class GarmentLoadingControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentLoadingRepository> _mockLoadingRepository;
        private readonly Mock<IGarmentLoadingItemRepository> _mockLoadingItemRepository;
        private readonly Mock<IGarmentSewingDOItemRepository> _mockSewingDOItemRepository;

        public GarmentLoadingControllerTests() : base()
        {
            _mockLoadingRepository = CreateMock<IGarmentLoadingRepository>();
            _mockLoadingItemRepository = CreateMock<IGarmentLoadingItemRepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockLoadingRepository);
            _MockStorage.SetupStorage(_mockLoadingItemRepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
        }

        private GarmentLoadingController CreateGarmentLoadingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentLoadingController controller = (GarmentLoadingController)Activator.CreateInstance(typeof(GarmentLoadingController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentLoadingController();

            _mockLoadingRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentLoadingReadModel>().AsQueryable());

            _mockLoadingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentLoadingReadModel>>()))
                .Returns(new List<GarmentLoading>()
                {
                    new GarmentLoading(Guid.NewGuid(), null , Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, "RONo",null,new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null)
                });

            _mockLoadingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentLoadingItemReadModel>()
                {
                    new GarmentLoadingItemReadModel(Guid.NewGuid())
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
            var unitUnderTest = CreateGarmentLoadingController();

            _mockLoadingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentLoadingReadModel, bool>>>()))
                .Returns(new List<GarmentLoading>()
                {
                    new GarmentLoading(Guid.NewGuid(), null , Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, "RONo",null,new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null)
                });

            _mockLoadingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentLoadingItemReadModel, bool>>>()))
                .Returns(new List<GarmentLoadingItem>()
                {
                    new GarmentLoadingItem(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(),new SizeId(1), null, new ProductId(1), null, null, null, 1,1,10,new UomId(1),null, null,1)
                });

            //_mockSewingDOItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingDOItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentLoadingController();

            _mockLoadingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentLoadingReadModel, bool>>>()))
                .Returns(new List<GarmentLoading>()
                {
                    new GarmentLoading(Guid.NewGuid(), "no" , Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, "RONo","art",new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null)
                });

            _mockLoadingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentLoadingItemReadModel, bool>>>()))
                .Returns(new List<GarmentLoadingItem>()
                {
                    new GarmentLoadingItem(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(),new SizeId(1), "size", new ProductId(1), null, null, "design", 1,1,10,new UomId(1),null, "color",1)
                });

            //_mockSewingDOItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingDOItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString(), "buyerCode");

            // Assert
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentLoadingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentLoadingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentLoading(Guid.NewGuid(), null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null));

            // Act
            var command = new PlaceGarmentLoadingCommand()
            {
                LoadingNo = "LoadingNo",
                SewingDONo = "SewingDONo",
                Price = 1
            };
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentLoadingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentLoadingCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            var command = new PlaceGarmentLoadingCommand();
            
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(command));
           
        }

        [Fact]
        public async Task GetComplete_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentLoadingController();
            var id = Guid.NewGuid();
            _mockLoadingRepository
               .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns(new List<GarmentLoadingReadModel>()
               {
                   new GarmentLoadingReadModel(id)
               }
               .AsQueryable());

            //_mockLoadingRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentLoadingReadModel>>()))
            //    .Returns(new List<GarmentLoading>()
            //    {
            //        new GarmentLoading(id, null , id, null, new UnitDepartmentId(1), null, null, "RONo",null,new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null)
            //    });

            //_mockLoadingItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentLoadingItemReadModel>()
            //    {
            //        new GarmentLoadingItemReadModel(id)
            //    }.AsQueryable());

            //_mockLoadingItemRepository
            //  .Setup(s => s.Find(It.IsAny<IQueryable<GarmentLoadingItemReadModel>>()))
            //  .Returns(new List<GarmentLoadingItem>()
            //  {
            //        new GarmentLoadingItem(id, id,id,new SizeId(1), "size", new ProductId(1), null, null, "design", 1,1,10,new UomId(1),null, "color",1)
            //  });

            _mockLoadingRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentLoadingReadModel>>()))
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
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentLoadingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentLoadingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentLoading(Guid.NewGuid(), null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentLoadingCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentLoadingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentLoadingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentLoading(Guid.NewGuid(), null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

		//[Fact]
		//public async Task GetLoaderByRO_StateUnderTest_ExpectedBehavior()
		//{
		//    // Arrange
		//    var unitUnderTest = CreateGarmentLoadingController();

		//    _mockLoadingRepository
		//        .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
		//        .Returns(new List<GarmentLoadingReadModel>().AsQueryable());

		//    _mockLoadingRepository
		//        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentLoadingReadModel>>()))
		//        .Returns(new List<GarmentLoading>()
		//        {
		//            new GarmentLoading(Guid.NewGuid(), null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null)
		//        });

		//    // Act
		//    var result = await unitUnderTest.GetLoaderByRO(It.IsAny<string>());

		//    // Assert
		//    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		//}

		//[Fact]
		//public async Task GetComplete_StateUnderTest_ExpectedBehavior()
		//{
		//    // Arrange
		//    var unitUnderTest = CreateGarmentLoadingController();

		//    _mockLoadingRepository
		//        .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
		//        .Returns(new List<GarmentLoadingReadModel>().AsQueryable());

		//    _mockLoadingRepository
		//        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentLoadingReadModel>>()))
		//        .Returns(new List<GarmentLoading>()
		//        {
		//            new GarmentLoading(Guid.NewGuid(), null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null)
		//        });

		//    _mockLoadingItemRepository
		//        .Setup(s => s.Query)
		//        .Returns(new List<GarmentLoadingItemReadModel>()
		//        {
		//            new GarmentLoadingItemReadModel(Guid.NewGuid())
		//        }.AsQueryable());

		//    _mockLoadingItemRepository
		//        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentLoadingItemReadModel>>()))
		//        .Returns(new List<GarmentLoadingItem>()
		//        {
		//            new GarmentLoadingItem(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(),new SizeId(1), null, new ProductId(1), null, null, null, 1,1,10,new UomId(1),null, null)
		//        });

		//    // Act
		//    var result = await unitUnderTest.GetComplete();

		//    // Assert
		//    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		//}

		[Fact]
		public async Task GetMonitoringBehavior()
		{
			var unitUnderTest = CreateGarmentLoadingController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringLoadingQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringLoadingListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentLoadingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsLoadingQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "",1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

       
        [Fact]
		public async Task GetXLSBookkeepingBehavior()
		{
			var unitUnderTest = CreateGarmentLoadingController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsLoadingQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "bookkeeping", 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

		}

        [Fact]
        public async Task GetXLS_Throws_Exception()
        {
            var unitUnderTest = CreateGarmentLoadingController();

            _MockMediator
                 .Setup(s => s.Send(It.IsAny<GetXlsLoadingQuery>(), It.IsAny<CancellationToken>()))
                 .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "bookkeeping", 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.InternalServerError);

        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentLoadingController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentLoadingCommand command = new UpdateDatesGarmentLoadingCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentLoadingController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentLoadingCommand command = new UpdateDatesGarmentLoadingCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentLoadingCommand command2 = new UpdateDatesGarmentLoadingCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }
    }
}
