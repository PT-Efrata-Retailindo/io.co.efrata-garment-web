using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentMonitoringProductionFlows.Queries;
using Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries;
using Manufactures.Controllers.Api;
using Manufactures.Domain.MonitoringProductionFlow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
	public class GarmentMonitoringProductionFlowControllerTests: BaseControllerUnitTest
	{
		private Mock<IGarmentMonitoringProductionFlowRepository> _mockGarmentProductionFlowRepository;
	 
		public GarmentMonitoringProductionFlowControllerTests() : base()
		{
			_mockGarmentProductionFlowRepository = CreateMock<IGarmentMonitoringProductionFlowRepository>();
			 
			_MockStorage.SetupStorage(_mockGarmentProductionFlowRepository);

		}

		private GarmentMonitoringProductionFlowController CreateGarmentMonitoringProductionFlowController()
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);
			GarmentMonitoringProductionFlowController controller = (GarmentMonitoringProductionFlowController)Activator.CreateInstance(typeof(GarmentMonitoringProductionFlowController), _MockServiceProvider.Object);
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
		public async Task GetMonitoringBehavior()
		{
			var unitUnderTest = CreateGarmentMonitoringProductionFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringProductionFlowQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringProductionFlowListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoring(1, DateTime.Now,null, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}
		[Fact]
		public async Task GetXLSBehavior()
		{
			var unitUnderTest = CreateGarmentMonitoringProductionFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsMonitoringProductionFlowQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, null, 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
			

		}

		[Fact]
		public async Task GetXLS_Throws_InternalServerError()
		{
			var unitUnderTest = CreateGarmentMonitoringProductionFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsMonitoringProductionFlowQuery>(), It.IsAny<CancellationToken>()))
				.Throws(new Exception());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, null, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.InternalServerError);

		}
		[Fact]
		public async Task GetMonitoringStockBehavior()
		{
			var unitUnderTest = CreateGarmentMonitoringProductionFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringProductionStockFlowQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringProductionStockFlowListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoringProductionStockFlow(1, DateTime.Now,DateTime.Now, null, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}
		[Fact]
		public async Task GetXLSStockBehavior()
		{
			var unitUnderTest = CreateGarmentMonitoringProductionFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsMonitoringProductionStockFlowQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act
			var result = await unitUnderTest.GetXlsMonitoringProductionStockFlow("bookkeeping",1, DateTime.Now,DateTime.Now,"", 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

		}

		[Fact]
		public async Task GetXLSStock_InternalServerError()
		{
			var unitUnderTest = CreateGarmentMonitoringProductionFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsMonitoringProductionStockFlowQuery>(), It.IsAny<CancellationToken>()))
				.Throws(new Exception());

			// Act
			var result = await unitUnderTest.GetXlsMonitoringProductionStockFlow("bookkeeping", 1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.InternalServerError);

		}
	}
}
