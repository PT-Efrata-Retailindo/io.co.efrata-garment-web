using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconGarmentWashReport;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Xunit;

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentServiceSubconSewingControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentServiceSubconSewingRepository> _mockGarmentServiceSubconSewingRepository;
        private Mock<IGarmentServiceSubconSewingItemRepository> _mockGarmentServiceSubconSewingItemRepository;
        private Mock<IGarmentServiceSubconSewingDetailRepository> _mockServiceSubconSewingDetailRepository;

        public GarmentServiceSubconSewingControllerTests() : base()
        {
            _mockGarmentServiceSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockGarmentServiceSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();
            _mockServiceSubconSewingDetailRepository = CreateMock<IGarmentServiceSubconSewingDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentServiceSubconSewingRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSubconSewingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconSewingDetailRepository);
        }

        private GarmentServiceSubconSewingController CreateGarmentServiceSubconSewingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentServiceSubconSewingController controller = (GarmentServiceSubconSewingController)Activator.CreateInstance(typeof(GarmentServiceSubconSewingController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentServiceSubconSewingController();

            _mockGarmentServiceSubconSewingRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentServiceSubconSewingReadModel>().AsQueryable());

            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _mockGarmentServiceSubconSewingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconSewingReadModel>>()))
                .Returns(new List<GarmentServiceSubconSewing>()
                {
                    new GarmentServiceSubconSewing(serviceSubconSewingGuid,null,  DateTimeOffset.Now, false, new BuyerId(1), null, null,0,null)
                });
            //, new UnitDepartmentId(1),null,null
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid serviceSubconSewingItemGuid = Guid.NewGuid();
            GarmentServiceSubconSewingItem garmentServiceSubconSewingItem = new GarmentServiceSubconSewingItem(serviceSubconSewingItemGuid, serviceSubconSewingGuid, null, null, new GarmentComodityId(1), null, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null);

            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingItemReadModel>()
                {
                    garmentServiceSubconSewingItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _mockGarmentServiceSubconSewingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewing>()
                {
                    new GarmentServiceSubconSewing(serviceSubconSewingGuid,null, DateTimeOffset.Now, false, new BuyerId(1), null, null,0,null)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid serviceSubconSewingItemGuid = Guid.NewGuid();
            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewingItem>()
                {
                    new GarmentServiceSubconSewingItem(serviceSubconSewingItemGuid, serviceSubconSewingGuid, null, null,new GarmentComodityId(1),null,null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null)
                });
            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewingDetail>()
                {
                    new GarmentServiceSubconSewingDetail(serviceSubconSewingItemGuid, serviceSubconSewingItemGuid,  new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null)
                });
            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSubconSewingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSubconSewing(serviceSubconSewingGuid, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, 0, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSubconSewingCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSubconSewingCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSubconSewingCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentServiceSubconSewingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSubconSewing(serviceSubconSewingGuid, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, 0, null));
            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentServiceSubconSewingCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid serviceSubconSewingGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentServiceSubconSewingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSubconSewing(serviceSubconSewingGuid, null, DateTimeOffset.Now, false, new BuyerId(1), null, null, 0, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid id = Guid.NewGuid();
            _mockGarmentServiceSubconSewingRepository
              .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new List<GarmentServiceSubconSewingReadModel>().AsQueryable());


            _mockGarmentServiceSubconSewingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconSewingReadModel>>()))
                .Returns(new List<GarmentServiceSubconSewing>()
                {
                    new GarmentServiceSubconSewing(id, null, DateTimeOffset.Now, false, new BuyerId(1), null, null,0,null)
                });

            GarmentServiceSubconSewingItem garmentServiceSubconSewingItem = new GarmentServiceSubconSewingItem(id, id,  null, null,new GarmentComodityId(1),null, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null);
            GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail = new GarmentServiceSubconSewingDetail(new Guid(), new Guid(), new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null);
            //id, id, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1),
            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingItemReadModel>()
                {
                    garmentServiceSubconSewingItem.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingDetailReadModel>() {
                    garmentServiceSubconSewingDetail.GetReadModel()
                }.AsQueryable());

            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconSewingItemReadModel>>()))
                .Returns(new List<GarmentServiceSubconSewingItem>()
                {
                    new GarmentServiceSubconSewingItem(id, id,  null, null,new GarmentComodityId(1),null,null, new BuyerId(1), null, null,  new UnitDepartmentId(1), null, null)
                });
            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconSewingDetailReadModel>>()))
                .Returns(new List<GarmentServiceSubconSewingDetail>()
                {
                    new GarmentServiceSubconSewingDetail(id, id,  new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null)
                });

            // Act
            var orderData = new
            {
                article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetItem_Return_Success()
        {
            var unitUnderTest = CreateGarmentServiceSubconSewingController();
            Guid id = Guid.NewGuid();
            _mockGarmentServiceSubconSewingItemRepository
              .Setup(s => s.ReadItem(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new List<GarmentServiceSubconSewingItemReadModel>().AsQueryable());
            GarmentServiceSubconSewingItem garmentServiceSubconSewingItem = new GarmentServiceSubconSewingItem(id, id, null, null, new GarmentComodityId(1), null, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null);
            GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail = new GarmentServiceSubconSewingDetail(new Guid(), new Guid(), new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null);
            //id, id, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1),
            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingItemReadModel>()
                {
                    garmentServiceSubconSewingItem.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingDetailReadModel>() {
                    garmentServiceSubconSewingDetail.GetReadModel()
                }.AsQueryable());

            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconSewingItemReadModel>>()))
                .Returns(new List<GarmentServiceSubconSewingItem>()
                {
                    new GarmentServiceSubconSewingItem(id, id,  null, null,new GarmentComodityId(1),null,null, new BuyerId(1), null, null,  new UnitDepartmentId(1), null, null)
                });
            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconSewingDetailReadModel>>()))
                .Returns(new List<GarmentServiceSubconSewingDetail>()
                {
                    new GarmentServiceSubconSewingDetail(id, id,  new Guid(), new Guid(), new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null, null, null)
                });

            // Act
            var orderData = new
            {
                article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetItems(1, 25, order, new List<string>(), "", "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }
        //
        [Fact]
        public async Task GetXLSubconSewingBehavior()
        {
            var unitUnderTest = CreateGarmentServiceSubconSewingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconGarmentWashReporQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act

            var result = await unitUnderTest.GetXlsSubconGarmentWashReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLSSubconSewingReturn_InternalServerError()
        {
            var unitUnderTest = CreateGarmentServiceSubconSewingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconGarmentWashReporQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act

            var result = await unitUnderTest.GetXlsSubconGarmentWashReport(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>(), 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }
    }
}
