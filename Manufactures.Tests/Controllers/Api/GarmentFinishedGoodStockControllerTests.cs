using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
	public class GarmentFinishedGoodStockControllerTests : BaseControllerUnitTest
	{
		private readonly Mock<IGarmentFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
		public GarmentFinishedGoodStockControllerTests() : base()
		{
			_mockFinishedGoodStockRepository = CreateMock<IGarmentFinishedGoodStockRepository>();

			_MockStorage.SetupStorage(_mockFinishedGoodStockRepository);

		}

		private GarmentFinishedGoodStockController CreateGarmentFinishedGoodStockController()
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);
			GarmentFinishedGoodStockController controller = (GarmentFinishedGoodStockController)Activator.CreateInstance(typeof(GarmentFinishedGoodStockController), _MockServiceProvider.Object);
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
			var unitUnderTest = CreateGarmentFinishedGoodStockController();

			_mockFinishedGoodStockRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentFinishedGoodStockReadModel>().AsQueryable());

			_mockFinishedGoodStockRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishedGoodStockReadModel>>()))
				.Returns(new List<GarmentFinishedGoodStock>()
				{
					new GarmentFinishedGoodStock(Guid.NewGuid(),"","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.SizeId(1),"",new Domain.Shared.ValueObjects.UomId(1),"",10,10,10)
				});

			// Act
			var result = await unitUnderTest.Get();

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}

		
		[Fact]
		public async Task GetList_Success()
		{
			// Arrange
			var id = Guid.NewGuid();
			var unitUnderTest = CreateGarmentFinishedGoodStockController();
			_mockFinishedGoodStockRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentFinishedGoodStockReadModel>().AsQueryable());

			_mockFinishedGoodStockRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishedGoodStockReadModel>>()))
				.Returns(new List<GarmentFinishedGoodStock>()
				{
					new GarmentFinishedGoodStock(id,"","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.SizeId(1),"",new Domain.Shared.ValueObjects.UomId(1),"",10,10,10)
				});
			var orderData = new
			{
				UnitName = "desc",
			};

			string order = JsonConvert.SerializeObject(orderData);
			var result = await unitUnderTest.GetList(1, 25, order, new List<string>(), "", "{}");
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}

		[Fact]
		public async Task GetComplete_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var unitUnderTest = CreateGarmentFinishedGoodStockController();

			_mockFinishedGoodStockRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentFinishedGoodStockReadModel>().AsQueryable());

			_mockFinishedGoodStockRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishedGoodStockReadModel>>()))
				.Returns(new List<GarmentFinishedGoodStock>()
				{
					new GarmentFinishedGoodStock(Guid.NewGuid(),"","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.SizeId(1),"",new Domain.Shared.ValueObjects.UomId(1),"",10,10,10)
				});

			// Act

			var orderData = new
			{
				UnitName = "desc",
			};

			string order = JsonConvert.SerializeObject(orderData);
			var result = await unitUnderTest.GetComplete(1,25,order,new List<string>(),"","{}");

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}
		[Fact]
		public async Task GetSingle_StateUnderTest_ExpectedBehavior()
		{
			var unitUnderTest = CreateGarmentFinishedGoodStockController();
			Guid identity = Guid.NewGuid();
		 

			_mockFinishedGoodStockRepository
			   .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishedGoodStockReadModel, bool>>>()))
			   .Returns(new List<GarmentFinishedGoodStock>()
			   {
					new GarmentFinishedGoodStock(identity,"","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.SizeId(1),"",new Domain.Shared.ValueObjects.UomId(1),"",10,10,10)
			   });
			var result = await unitUnderTest.Get(identity.ToString());

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}
	}
}
