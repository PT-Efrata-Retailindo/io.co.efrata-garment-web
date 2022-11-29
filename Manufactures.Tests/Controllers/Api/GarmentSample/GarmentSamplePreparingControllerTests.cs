using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SamplePreparings.Queries.GetMonitoringPrepareSample;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentSamplePreparingControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
        private Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;

        public GarmentSamplePreparingControllerTests() : base()
        {
            _mockGarmentSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);
        }

        private GarmentSamplePreparingController CreateGarmentSamplePreparingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSamplePreparingController controller = (GarmentSamplePreparingController)Activator.CreateInstance(typeof(GarmentSamplePreparingController), _MockServiceProvider.Object);
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
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSamplePreparingController();

            _mockGarmentSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>()
                {
                    new GarmentSamplePreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, false, new Domain.Shared.ValueObjects.BuyerId(1), null, null)
                });

            _mockGarmentSamplePreparingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingItemReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparingItem>()
                {
                    new GarmentSamplePreparingItem(Guid.NewGuid(), 0, new ProductId(1), null, null, null, 0, new UomId(1), null, null, 0, 0, Guid.NewGuid(),null)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSamplePreparingController();

            PlaceGarmentSamplePreparingCommand command = new PlaceGarmentSamplePreparingCommand();
            command.UENId = 1;

            _mockGarmentSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>());

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSamplePreparingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSamplePreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, false, new Domain.Shared.ValueObjects.BuyerId(1), null, null));

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Return_BadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSamplePreparingController();

            PlaceGarmentSamplePreparingCommand command = new PlaceGarmentSamplePreparingCommand();
            command.UENId = 1;

            _mockGarmentSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>() {
                    new GarmentSamplePreparing(id,1,"uenNo",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true, new Domain.Shared.ValueObjects.BuyerId(1), null, null)
                });

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Return_InternalServerError()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSamplePreparingController();

            PlaceGarmentSamplePreparingCommand command = new PlaceGarmentSamplePreparingCommand();
            command.UENId = 1;

            _mockGarmentSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>() {});

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSamplePreparingCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSamplePreparingController();

            _mockGarmentSamplePreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingReadModel, bool>>>()))
                .Returns(new List<GarmentSamplePreparing>()
                {
                    new GarmentSamplePreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, false,new Domain.Shared.ValueObjects.BuyerId(1), null,null)
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSamplePreparingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSamplePreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, false, new Domain.Shared.ValueObjects.BuyerId(1), null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetLoaderByRO_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSamplePreparingController();
            var garmentSamplePreparingReadModel = new GarmentSamplePreparingReadModel(id);

            _mockGarmentSamplePreparingRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentSamplePreparingReadModel>()
                {
                   garmentSamplePreparingReadModel
                }
                .AsQueryable());

            _mockGarmentSamplePreparingRepository
              .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
              .Returns(new List<GarmentSamplePreparingReadModel>()
              {
                  new GarmentSamplePreparingReadModel(id)
              }
              .AsQueryable());

            // Act
            var result = await unitUnderTest.GetLoaderByRO("", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
        [Fact]
        public async Task GetMonitoringBehavior()
        {
            var unitUnderTest = CreateGarmentSamplePreparingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetMonitoringSamplePrepareQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentMonitoringSamplePrepareViewModel());

            // Act
            var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetXLSPrepareBehavior()
        {
            var unitUnderTest = CreateGarmentSamplePreparingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSamplePrepareQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act

            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSamplePreparingController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSamplePreparingCommand command = new UpdateDatesGarmentSamplePreparingCommand(ids, DateTimeOffset.Now);
            _MockMediator
                .Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior_BadRequest()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSamplePreparingController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSamplePreparingCommand command = new UpdateDatesGarmentSamplePreparingCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentSamplePreparingCommand command2 = new UpdateDatesGarmentSamplePreparingCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }
    }
}
