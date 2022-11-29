using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentScrapClassifications;
using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.ReadModels;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
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
	public class GarmentScrapClassificationControllerTests : BaseControllerUnitTest
	{
		private readonly Mock<IGarmentScrapClassificationRepository> _mockGarmentScrapClassificationRepository;
		

		public GarmentScrapClassificationControllerTests() : base()
		{
			_mockGarmentScrapClassificationRepository = CreateMock<IGarmentScrapClassificationRepository>();			
			_MockStorage.SetupStorage(_mockGarmentScrapClassificationRepository);
		}

		private GarmentScrapClassificationController CreateGarmentScrapClassificationController()
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);
			GarmentScrapClassificationController controller = (GarmentScrapClassificationController)Activator.CreateInstance(typeof(GarmentScrapClassificationController), _MockServiceProvider.Object);
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
			var unitUnderTest = CreateGarmentScrapClassificationController();

			_mockGarmentScrapClassificationRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentScrapClassificationReadModel>().AsQueryable());

			 
			_mockGarmentScrapClassificationRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentScrapClassificationReadModel>>()))
				.Returns(new List<GarmentScrapClassification>()
				{
					new GarmentScrapClassification(new Guid(),"code","name","description")
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
			var unitUnderTest = CreateGarmentScrapClassificationController();
			Guid identity = Guid.NewGuid();
			_mockGarmentScrapClassificationRepository
				.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapClassificationReadModel, bool>>>()))
				.Returns(new List<GarmentScrapClassification>()
				{
					new GarmentScrapClassification(identity,"code","name","description")
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
			var unitUnderTest = CreateGarmentScrapClassificationController();
			Guid identity = Guid.NewGuid();
			_MockMediator
				.Setup(s => s.Send(It.IsAny<PlaceGarmentScrapClassificationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentScrapClassification(identity, "code", "name", "description"));

			// Act
			var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentScrapClassificationCommand>());

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}

		[Fact]
		public async Task Put_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var unitUnderTest = CreateGarmentScrapClassificationController();
			Guid identity = Guid.NewGuid();
			_MockMediator
				.Setup(s => s.Send(It.IsAny<UpdateGarmentScrapClassificationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentScrapClassification(identity, "code", "name", "description"));

			// Act
			var result = await unitUnderTest.Put(identity.ToString(), new UpdateGarmentScrapClassificationCommand());

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}

		[Fact]
		public async Task Delete_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var unitUnderTest = CreateGarmentScrapClassificationController();
			Guid identity = Guid.NewGuid();
			_MockMediator
				.Setup(s => s.Send(It.IsAny<RemoveGarmentScrapClassificationCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentScrapClassification(identity, "code", "name", "description"));

			// Act
			var result = await unitUnderTest.Delete(identity.ToString());

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}
	}
}
