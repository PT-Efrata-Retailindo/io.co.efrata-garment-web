using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.GarmentMonitoringSampleFlows.Queries;
using Manufactures.Controllers.Api;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Manufactures.Domain.GarmentSample.MonitoringSampleStockFlow;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
	public class GarmentMonitoringSampleFlowControllerTests: BaseControllerUnitTest
	{
		private Mock<IGarmentMonitoringSampleStockFlowRepository> _mockGarmentSampleFlowRepository;
	 
		public GarmentMonitoringSampleFlowControllerTests() : base()
		{
			_mockGarmentSampleFlowRepository = CreateMock<IGarmentMonitoringSampleStockFlowRepository>();
			 
			_MockStorage.SetupStorage(_mockGarmentSampleFlowRepository);

		}

		private GarmentMonitoringSampleFlowController CreateGarmentMonitoringSampleFlowController()
		{
			var user = new Mock<ClaimsPrincipal>();
			var claims = new Claim[]
			{
				new Claim("username", "unittestusername")
			};
			user.Setup(u => u.Claims).Returns(claims);
			GarmentMonitoringSampleFlowController controller = (GarmentMonitoringSampleFlowController)Activator.CreateInstance(typeof(GarmentMonitoringSampleFlowController), _MockServiceProvider.Object);
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
			var unitUnderTest = CreateGarmentMonitoringSampleFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringSampleFlowQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringSampleFlowListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoring(1, DateTime.Now,null, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}
		[Fact]
		public async Task GetXLSBehavior()
		{
			var unitUnderTest = CreateGarmentMonitoringSampleFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsMonitoringSampleFlowQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, null, 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
			

		}

		[Fact]
		public async Task GetXLS_Throws_InternalServerError()
		{
			var unitUnderTest = CreateGarmentMonitoringSampleFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsMonitoringSampleFlowQuery>(), It.IsAny<CancellationToken>()))
				.Throws(new Exception());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, null, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.InternalServerError);

		}
		[Fact]
		public async Task GetMonitoringStockBehavior()
		{
			var unitUnderTest = CreateGarmentMonitoringSampleFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringSampleFlowQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringSampleFlowListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, null, 1, 25, "{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}
		[Fact]
		public async Task GetXLSStockBehavior()
		{
			var unitUnderTest = CreateGarmentMonitoringSampleFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsMonitoringSampleFlowQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now,"", 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

		}

		[Fact]
		public async Task GetXLSStock_InternalServerError()
		{
			var unitUnderTest = CreateGarmentMonitoringSampleFlowController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsMonitoringSampleFlowQuery>(), It.IsAny<CancellationToken>()))
				.Throws(new Exception());

			// Act
			var result = await unitUnderTest.GetXls(1, DateTime.Now, "", 1, 25, "{}");
			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.InternalServerError);

		}
	}
}
