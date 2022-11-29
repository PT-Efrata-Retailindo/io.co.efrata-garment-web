using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
	public class GarmentScrapStockControllerTests : BaseControllerUnitTest
	{
		private readonly Mock<IGarmentScrapStockRepository> _mockGarmentScrapStockRepository;
		public GarmentScrapStockControllerTests() : base()
		{
			_mockGarmentScrapStockRepository = CreateMock<IGarmentScrapStockRepository>();
			_MockStorage.SetupStorage(_mockGarmentScrapStockRepository);
		}

		private GarmentScrapStockController CreateGarmentScrapStockController()
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);
			GarmentScrapStockController controller = (GarmentScrapStockController)Activator.CreateInstance(typeof(GarmentScrapStockController), _MockServiceProvider.Object);
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
			var unitUnderTest = CreateGarmentScrapStockController();

			_mockGarmentScrapStockRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentScrapStockReadModel>().AsQueryable());


			_mockGarmentScrapStockRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentScrapStockReadModel>>()))
				.Returns(new List<GarmentScrapStock>()
				{
					new GarmentScrapStock(new Guid(),new Guid(),"",new Guid(),"",100,1,"unit")
				});


			// Act
			var result = await unitUnderTest.Get();

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}
		[Fact]
		public async Task Get_Remaining_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var unitUnderTest = CreateGarmentScrapStockController();

			_mockGarmentScrapStockRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentScrapStockReadModel>().AsQueryable());


			_mockGarmentScrapStockRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentScrapStockReadModel>>()))
				.Returns(new List<GarmentScrapStock>()
				{
					new GarmentScrapStock(new Guid(),new Guid(),"",new Guid(),"",100,1,"unit")
				});


			// Act
			var result = await unitUnderTest.GetRemaining();

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}

	}
}
