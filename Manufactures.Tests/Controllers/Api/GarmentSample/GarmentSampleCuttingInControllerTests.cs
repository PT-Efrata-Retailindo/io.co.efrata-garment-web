using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
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
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentSampleCuttingInControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSampleCuttingInRepository> _mockGarmentSampleCuttingInRepository;
        private Mock<IGarmentSampleCuttingInItemRepository> _mockGarmentSampleCuttingInItemRepository;
        private Mock<IGarmentSampleCuttingInDetailRepository> _mockGarmentSampleCuttingInDetailRepository;
        private Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;

        public GarmentSampleCuttingInControllerTests() : base()
        {
            _mockGarmentSampleCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockGarmentSampleCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockGarmentSampleCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);
        }

        private GarmentSampleCuttingInController CreateGarmentSampleCuttingInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleCuttingInController controller = (GarmentSampleCuttingInController)Activator.CreateInstance(typeof(GarmentSampleCuttingInController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSampleCuttingInController();

            _mockGarmentSampleCuttingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleCuttingInReadModel>().AsQueryable());

            Guid cuttingInGuid = Guid.NewGuid();
            _mockGarmentSampleCuttingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleCuttingInReadModel>>()))
                .Returns(new List<GarmentSampleCuttingIn>()
                {
                    new GarmentSampleCuttingIn(cuttingInGuid, null, null, null,"RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0)
                });

            Guid cuttingInItemGuid = Guid.NewGuid();
            GarmentSampleCuttingInItem garmentSampleCuttingInItem = new GarmentSampleCuttingInItem(cuttingInItemGuid, cuttingInGuid, Guid.NewGuid(), 1, null, Guid.Empty, null);
            _mockGarmentSampleCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInItemReadModel>()
                {
                    garmentSampleCuttingInItem.GetReadModel()
                }.AsQueryable());

            GarmentSampleCuttingInDetail garmentSampleCuttingInDetail = new GarmentSampleCuttingInDetail(Guid.NewGuid(), cuttingInItemGuid, Guid.NewGuid(), Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 1, new UomId(1), null, 1, new UomId(1), null, 1, 1, 1, 1, null);
            _mockGarmentSampleCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInDetailReadModel>()
                {
                    garmentSampleCuttingInDetail.GetReadModel()
                }.AsQueryable());

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingInController();

            _mockGarmentSampleCuttingInRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingIn>()
                {
                    new GarmentSampleCuttingIn(Guid.NewGuid(), null, null, null,"RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0)
                });

            _mockGarmentSampleCuttingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingInItem>()
                {
                    new GarmentSampleCuttingInItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, null,Guid.Empty,"")
                });

            _mockGarmentSampleCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingInDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingInDetail>()
                {
                    new GarmentSampleCuttingInDetail(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, null, 0, new UomId(1), null, 0, new UomId(1), null, 0, 0,1,1,null)
                });

            _mockGarmentSamplePreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSamplePreparingItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleCuttingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleCuttingIn(Guid.NewGuid(), null, null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleCuttingInCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleCuttingInCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSampleCuttingInCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSampleCuttingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleCuttingIn(Guid.NewGuid(), null, null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSampleCuttingInCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleCuttingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleCuttingIn(Guid.NewGuid(), null, null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetLoaderByRO_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingInController();

            _mockGarmentSampleCuttingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleCuttingInReadModel>().AsQueryable());

            _mockGarmentSampleCuttingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleCuttingInReadModel>>()))
                .Returns(new List<GarmentSampleCuttingIn>()
                {
                    new GarmentSampleCuttingIn(Guid.NewGuid(), null, null, null,"RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0)
                });

            // Act
            var result = await unitUnderTest.GetLoaderByRO(It.IsAny<string>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SampleCuttingInGuid = Guid.NewGuid();
            Guid SampleCuttingInItemGuid = Guid.NewGuid();
            Guid SampleCuttingInDetailGuid = Guid.NewGuid();
            Guid SamplePreparingGuid = Guid.NewGuid();
            Guid SamplePreparingItemGuid = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSampleCuttingInController();

            _mockGarmentSampleCuttingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleCuttingInReadModel>() { new GarmentSampleCuttingInReadModel(SampleCuttingInGuid) }
                .AsQueryable());

            _mockGarmentSampleCuttingInRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentSampleCuttingInReadModel>>()))
                .Returns(new List<GarmentSampleCuttingInReadModel>() { new GarmentSampleCuttingInReadModel(SampleCuttingInGuid) }
                .AsQueryable());

            // Act
            var orderData = new
            {
                cutInNo = "desc",
            };

            string oder = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, oder, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleCuttingInCommand command = new UpdateDatesGarmentSampleCuttingInCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentSampleCuttingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleCuttingInCommand command = new UpdateDatesGarmentSampleCuttingInCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentSampleCuttingInCommand command2 = new UpdateDatesGarmentSampleCuttingInCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }
    }
}
