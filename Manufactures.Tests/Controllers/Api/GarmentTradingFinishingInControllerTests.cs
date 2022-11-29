using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentTradingFinishingIns.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentTradingFinishingInControllerTests : BaseControllerUnitTest
    {
        public GarmentTradingFinishingInControllerTests() : base()
        {
        }

        private GarmentTradingFinishingInController CreateGarmentTradingFinishingInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentTradingFinishingInController controller = new GarmentTradingFinishingInController(_MockServiceProvider.Object);
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
        public async Task Post_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentTradingFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentTradingFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingIn(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentTradingFinishingInCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentTradingFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentTradingFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            //Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentTradingFinishingInCommand>()));
        }

        [Fact]
        public async Task Delete_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentTradingFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentTradingFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingIn(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
