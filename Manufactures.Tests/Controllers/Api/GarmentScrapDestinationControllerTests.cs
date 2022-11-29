using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentScrapDestinations;
using Manufactures.Domain.GarmentScrapDestinations.Commands;
using Manufactures.Domain.GarmentScrapDestinations.ReadModels;
using Manufactures.Domain.GarmentScrapDestinations.Repositories;
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
	public class GarmentScrapDestinationControllerTests : BaseControllerUnitTest
	{
		private readonly Mock<IGarmentScrapDestinationRepository> _mockGarmentScrapDestinationRepository;


		public GarmentScrapDestinationControllerTests() : base()
		{
			_mockGarmentScrapDestinationRepository = CreateMock<IGarmentScrapDestinationRepository>();
			_MockStorage.SetupStorage(_mockGarmentScrapDestinationRepository);
		}

		private GarmentScrapDestinationController CreateGarmentScrapDestinationController()
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);
			GarmentScrapDestinationController controller = (GarmentScrapDestinationController)Activator.CreateInstance(typeof(GarmentScrapDestinationController), _MockServiceProvider.Object);
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
			var unitUnderTest = CreateGarmentScrapDestinationController();

			_mockGarmentScrapDestinationRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentScrapDestinationReadModel>().AsQueryable());


			_mockGarmentScrapDestinationRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentScrapDestinationReadModel>>()))
				.Returns(new List<GarmentScrapDestination>()
				{
					new GarmentScrapDestination(new Guid(),"","","")
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
            var unitUnderTest = CreateGarmentScrapDestinationController();
            Guid identity = Guid.NewGuid();
            _mockGarmentScrapDestinationRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapDestinationReadModel, bool>>>()))
                .Returns(new List<GarmentScrapDestination>()
                {
                    new GarmentScrapDestination(identity,"code","name","description")
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
            var unitUnderTest = CreateGarmentScrapDestinationController();
            Guid identity = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentScrapDestinationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentScrapDestination(identity, "code", "name", "description"));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentScrapDestinationCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentScrapDestinationController();
            Guid identity = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentScrapDestinationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentScrapDestination(identity, "code", "name", "description"));

            // Act
            var result = await unitUnderTest.Put(identity.ToString(), new UpdateGarmentScrapDestinationCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentScrapDestinationController();
            Guid identity = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentScrapDestinationCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentScrapDestination(identity, "code", "name", "description"));

            // Act
            var result = await unitUnderTest.Delete(identity.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
