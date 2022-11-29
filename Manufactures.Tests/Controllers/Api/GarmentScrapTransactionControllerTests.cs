using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Commands;
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
	public class GarmentScrapTransactionControllerTests : BaseControllerUnitTest
	{
		private readonly Mock<IGarmentScrapTransactionRepository> _mockGarmentScrapTransactionRepository;
		private readonly Mock<IGarmentScrapTransactionItemRepository> _mockGarmentScrapTransactionItemRepository;


		public GarmentScrapTransactionControllerTests() : base()
		{
			_mockGarmentScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
			_mockGarmentScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();
			_MockStorage.SetupStorage(_mockGarmentScrapTransactionRepository);
			_MockStorage.SetupStorage(_mockGarmentScrapTransactionItemRepository);
		}

		private GarmentScrapTransactionController CreateGarmentScrapTransactionController()
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);
			GarmentScrapTransactionController controller = (GarmentScrapTransactionController)Activator.CreateInstance(typeof(GarmentScrapTransactionController), _MockServiceProvider.Object);
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
			var unitUnderTest = CreateGarmentScrapTransactionController();

			_mockGarmentScrapTransactionRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentScrapTransactionReadModel>().AsQueryable());


			_mockGarmentScrapTransactionRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentScrapTransactionReadModel>>()))
				.Returns(new List<GarmentScrapTransaction>()
				{
					new GarmentScrapTransaction(new Guid(),"","IN",DateTimeOffset.Now,new Guid(),"",new Guid(),"")
				});


			// Act
			var result = await unitUnderTest.Get();

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}
		[Fact]
		public async Task Get_StateUnderTest_ExpectedBehavior_Out()
		{
			// Arrange
			var unitUnderTest = CreateGarmentScrapTransactionController();

			_mockGarmentScrapTransactionRepository
				.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
				.Returns(new List<GarmentScrapTransactionReadModel>().AsQueryable());


			_mockGarmentScrapTransactionRepository
				.Setup(s => s.Find(It.IsAny<IQueryable<GarmentScrapTransactionReadModel>>()))
				.Returns(new List<GarmentScrapTransaction>()
				{
					new GarmentScrapTransaction(new Guid(),"","OUT",DateTimeOffset.Now,new Guid(),"",new Guid(),"")
				});


			// Act
			var result = await unitUnderTest.GetOut();

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}
		[Fact]
		public async Task GetSingle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var unitUnderTest = CreateGarmentScrapTransactionController();
			Guid identity = Guid.NewGuid();
			_mockGarmentScrapTransactionRepository
			   .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapTransactionReadModel, bool>>>()))
			   .Returns(new List<GarmentScrapTransaction>()
			   {
					new  GarmentScrapTransaction(identity,"","",DateTimeOffset.Now,new Guid(),"",new Guid(),"")
			   });

			_mockGarmentScrapTransactionItemRepository
				.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapTransactionItemReadModel, bool>>>()))
				.Returns(new List<GarmentScrapTransactionItem>()
				{
					new GarmentScrapTransactionItem(Guid.NewGuid(),identity, Guid.NewGuid(),"",0,1,"","")
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
			var unitUnderTest = CreateGarmentScrapTransactionController();
			Guid identity = Guid.NewGuid();
			_MockMediator
				.Setup(s => s.Send(It.IsAny<PlaceGarmentScrapTransactionCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentScrapTransaction(identity, "", "", DateTimeOffset.Now, new Guid(), "", new Guid(), ""));

			// Act
			var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentScrapTransactionCommand>());

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}

		[Fact]
		public async Task Put_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var unitUnderTest = CreateGarmentScrapTransactionController();
			Guid identity = Guid.NewGuid();
			_MockMediator
				.Setup(s => s.Send(It.IsAny<UpdateGarmentScrapTransactionCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentScrapTransaction(identity, "", "", DateTimeOffset.Now, new Guid(), "", new Guid(), ""));

			// Act
			var result = await unitUnderTest.Put(identity.ToString(), new UpdateGarmentScrapTransactionCommand());

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}

		[Fact]
		public async Task Delete_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			var unitUnderTest = CreateGarmentScrapTransactionController();
			Guid identity = Guid.NewGuid();
			_MockMediator
				.Setup(s => s.Send(It.IsAny<RemoveGarmentScrapTransactionCommand>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentScrapTransaction(identity, "", "", DateTimeOffset.Now, new Guid(), "", new Guid(), ""));

			// Act
			var result = await unitUnderTest.Delete(identity.ToString());

			// Assert
			Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
		}
	}
}
