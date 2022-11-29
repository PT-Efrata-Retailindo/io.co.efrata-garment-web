using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.Queries.GetGarmentAvalComponents;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentAvalComponentControllerTests : BaseControllerUnitTest
    {
        private GarmentAvalComponentController CreateGarmentAvalComponentController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentAvalComponentController controller = new GarmentAvalComponentController(_MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentAvalComponentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllGarmentAvalComponentsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentAvalComponentsListViewModel
                {
                    GarmentAvalComponents = new List<GarmentAvalComponentDto>()
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAvalComponentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetGarmentAvalComponentQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentAvalComponentViewModel(new GarmentAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false)));

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAvalComponentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentAvalComponentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentAvalComponentCommand>());

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAvalComponentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentAvalComponentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

    }
}
