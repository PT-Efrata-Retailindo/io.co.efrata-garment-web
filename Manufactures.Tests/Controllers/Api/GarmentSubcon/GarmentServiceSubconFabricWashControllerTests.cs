using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.Queries;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
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

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentServiceSubconFabricWashControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentServiceSubconFabricWashRepository> _mockGarmentServiceSubconFabricWashRepository;
        private Mock<IGarmentServiceSubconFabricWashItemRepository> _mockGarmentServiceSubconFabricWashItemRepository;
        private Mock<IGarmentServiceSubconFabricWashDetailRepository> _mockServiceSubconFabricWashDetailRepository;

        public GarmentServiceSubconFabricWashControllerTests() : base()
        {
            _mockGarmentServiceSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockGarmentServiceSubconFabricWashItemRepository = CreateMock<IGarmentServiceSubconFabricWashItemRepository>();
            _mockServiceSubconFabricWashDetailRepository = CreateMock<IGarmentServiceSubconFabricWashDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentServiceSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSubconFabricWashItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconFabricWashDetailRepository);
        }

        private GarmentServiceSubconFabricWashController CreateGarmentServiceSubconFabricWashController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentServiceSubconFabricWashController controller = (GarmentServiceSubconFabricWashController)Activator.CreateInstance(typeof(GarmentServiceSubconFabricWashController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();

            _mockGarmentServiceSubconFabricWashRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentServiceSubconFabricWashReadModel>().AsQueryable());

            Guid serviceSubconFabricWashGuid = Guid.NewGuid();
            _mockGarmentServiceSubconFabricWashRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconFabricWashReadModel>>()))
                .Returns(new List<GarmentServiceSubconFabricWash>()
                {
                    new GarmentServiceSubconFabricWash(serviceSubconFabricWashGuid,null,  DateTimeOffset.Now, "",false, 0, null)
                });

            Guid serviceSubconFabricWashItemGuid = Guid.NewGuid();
            GarmentServiceSubconFabricWashItem garmentServiceSubconFabricWashItem = new GarmentServiceSubconFabricWashItem(serviceSubconFabricWashItemGuid, serviceSubconFabricWashGuid, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null);

            _mockGarmentServiceSubconFabricWashItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashItemReadModel>()
                {
                    garmentServiceSubconFabricWashItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();
            Guid serviceSubconFabricWashGuid = Guid.NewGuid();
            _mockGarmentServiceSubconFabricWashRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconFabricWashReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconFabricWash>()
                {
                    new GarmentServiceSubconFabricWash(serviceSubconFabricWashGuid,null, DateTimeOffset.Now, "",false, 0, null)
                });

            Guid serviceSubconFabricWashItemGuid = Guid.NewGuid();
            _mockGarmentServiceSubconFabricWashItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconFabricWashItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconFabricWashItem>()
                {
                    new GarmentServiceSubconFabricWashItem(serviceSubconFabricWashItemGuid, serviceSubconFabricWashGuid, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null)
                });
            _mockServiceSubconFabricWashDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconFabricWashDetailReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconFabricWashDetail>()
                {
                    new GarmentServiceSubconFabricWashDetail(new Guid(), serviceSubconFabricWashItemGuid, new ProductId(1), null, null, null, null, 1, new UomId(1), null)
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
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();
            Guid serviceSubconFabricWashGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSubconFabricWashCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSubconFabricWash(serviceSubconFabricWashGuid, null, DateTimeOffset.Now, "", false, 0, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSubconFabricWashCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();
            Guid serviceSubconFabricWashGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentServiceSubconFabricWashCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentServiceSubconFabricWashCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();
            Guid serviceSubconFabricWashGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentServiceSubconFabricWashCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSubconFabricWash(serviceSubconFabricWashGuid, null, DateTimeOffset.Now, "", false, 0, null));
            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentServiceSubconFabricWashCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();
            Guid serviceSubconFabricWashGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentServiceSubconFabricWashCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentServiceSubconFabricWash(serviceSubconFabricWashGuid, null, DateTimeOffset.Now, "", false, 0, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();
            Guid id = Guid.NewGuid();
            _mockGarmentServiceSubconFabricWashRepository
              .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new List<GarmentServiceSubconFabricWashReadModel>().AsQueryable());


            _mockGarmentServiceSubconFabricWashRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconFabricWashReadModel>>()))
                .Returns(new List<GarmentServiceSubconFabricWash>()
                {
                    new GarmentServiceSubconFabricWash(id, null, DateTimeOffset.Now, "",false, 0, null)
                });

            GarmentServiceSubconFabricWashItem garmentServiceSubconFabricWashItem = new GarmentServiceSubconFabricWashItem(id, id, null, DateTimeOffset.Now, new UnitSenderId(1), null, null, new UnitRequestId(1), null, null);
            GarmentServiceSubconFabricWashDetail garmentServiceSubconFabricWashDetail = new GarmentServiceSubconFabricWashDetail(new Guid(), new Guid(), new ProductId(1), null, null, null, "ColorD", 1, new UomId(1), null);

            _mockGarmentServiceSubconFabricWashItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashItemReadModel>()
                {
                    garmentServiceSubconFabricWashItem.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconFabricWashDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashDetailReadModel>() {
                    garmentServiceSubconFabricWashDetail.GetReadModel()
                }.AsQueryable());

            _mockGarmentServiceSubconFabricWashItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconFabricWashItemReadModel>>()))
                .Returns(new List<GarmentServiceSubconFabricWashItem>()
                {
                    new GarmentServiceSubconFabricWashItem(id, id,  null, DateTimeOffset.Now,new UnitSenderId(1),null,null, new UnitRequestId(1), null, null)
                });
            _mockServiceSubconFabricWashDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentServiceSubconFabricWashDetailReadModel>>()))
                .Returns(new List<GarmentServiceSubconFabricWashDetail>()
                {
                    new GarmentServiceSubconFabricWashDetail(id, id, new ProductId(1), null, null, null, "ColorD", 1, new UomId(1), null)
                });

            // Act
            var orderData = new
            {
                ServiceSubconFabricWashDate = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsServiceSubconFabricWashQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXls(DateTime.Now, DateTime.Now, "token", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetXLS_Return_InternalServerError()
        {
            var unitUnderTest = CreateGarmentServiceSubconFabricWashController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsServiceSubconFabricWashQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXls(DateTime.Now, DateTime.Now, "token", 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }
    }
}
