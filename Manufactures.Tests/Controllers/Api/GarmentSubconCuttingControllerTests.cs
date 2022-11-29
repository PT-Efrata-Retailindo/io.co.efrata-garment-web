using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubconCuttings.Queries.GetAllGarmentSubconCuttings;
using Manufactures.Controllers.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentSubconCuttingControllerTests : BaseControllerUnitTest
    {
        private GarmentSubconCuttingController CreateController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconCuttingController controller = new GarmentSubconCuttingController(_MockServiceProvider.Object);
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
        public async Task Get_All_Success()
        {
            // Arrange
            var unitUnderTest = CreateController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllGarmentSubconCuttingsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetGarmentSubconCuttingListViewModel
                {
                    data = new List<GarmentSubconCuttingDto>()
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }


    }
}
