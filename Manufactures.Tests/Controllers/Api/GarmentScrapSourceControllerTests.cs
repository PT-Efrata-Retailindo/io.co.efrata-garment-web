using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
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

namespace Manufactures.Tests.Controllers.Api
{
	public class GarmentScrapSourceControllerTests  : BaseControllerUnitTest
	{
		private readonly Mock<IGarmentScrapSourceRepository> _mockGarmentScrapSourceRepository;
		

		public GarmentScrapSourceControllerTests() : base()
		{
			_mockGarmentScrapSourceRepository = CreateMock<IGarmentScrapSourceRepository>();
			_MockStorage.SetupStorage(_mockGarmentScrapSourceRepository);
		}

		private GarmentScrapSourceController CreateGarmentScrapSourceController()
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);
			GarmentScrapSourceController controller = (GarmentScrapSourceController)Activator.CreateInstance(typeof(GarmentScrapSourceController), _MockServiceProvider.Object);
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
			var unitUnderTest = CreateGarmentScrapSourceController();

			_mockGarmentScrapSourceRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentScrapSourceReadModel>().AsQueryable());


			_mockGarmentScrapSourceRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentScrapSourceReadModel>>()))
				.Returns(new List<GarmentScrapSource>()
				{
					new GarmentScrapSource(new Guid(),"","","")
				});


			// Act
			var result = await unitUnderTest.Get();

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentScrapSourceController();
            Guid identity = Guid.NewGuid();
            _mockGarmentScrapSourceRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapSourceReadModel, bool>>>()))
                .Returns(new List<GarmentScrapSource>()
                {
                    new GarmentScrapSource(identity,"code","name","description")
                });

            // Act
            var result = await unitUnderTest.Get(identity.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentScrapSourceController();
            Guid identity = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentScrapSourceCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentScrapSource(identity, "code", "name", "description"));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentScrapSourceCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentScrapSourceController();
            Guid identity = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentScrapSourceCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentScrapSource(identity, "code", "name", "description"));

            // Act
            var result = await unitUnderTest.Put(identity.ToString(), new UpdateGarmentScrapSourceCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentScrapSourceController();
            Guid identity = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentScrapSourceCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentScrapSource(identity, "code", "name", "description"));

            // Act
            var result = await unitUnderTest.Delete(identity.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
