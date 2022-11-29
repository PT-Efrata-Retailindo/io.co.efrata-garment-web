using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetAllGarmentSampleAvalComponents;
using Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetGarmentSampleAvalComponents;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands;
using Manufactures.Domain.Shared.ValueObjects;
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

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentSampleAvalComponentControllerTests : BaseControllerUnitTest
    {
        private GarmentSampleAvalComponentController CreateGarmentSampleAvalComponentController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleAvalComponentController controller = new GarmentSampleAvalComponentController(_MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSampleAvalComponentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllGarmentSampleAvalComponentsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleAvalComponentsListViewModel
                {
                    GarmentSampleAvalComponents = new List<GarmentSampleAvalComponentDto>()
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
            var unitUnderTest = CreateGarmentSampleAvalComponentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetGarmentSampleAvalComponentQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleAvalComponentViewModel(new GarmentSampleAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false)));

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleAvalComponentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleAvalComponentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleAvalComponentCommand>());

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleAvalComponentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleAvalComponentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

    }
}
