using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
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

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentCuttingInControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
        private Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
        private Mock<IGarmentCuttingInDetailRepository> _mockGarmentCuttingInDetailRepository;
        private Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

        public GarmentCuttingInControllerTests() : base()
        {
            _mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockGarmentCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);
        }

        private GarmentCuttingInController CreateGarmentCuttingInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentCuttingInController controller = (GarmentCuttingInController)Activator.CreateInstance(typeof(GarmentCuttingInController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentCuttingInController();

            _mockGarmentCuttingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingInReadModel>().AsQueryable());

            Guid cuttingInGuid = Guid.NewGuid();
            _mockGarmentCuttingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingInReadModel>>()))
                .Returns(new List<GarmentCuttingIn>()
                {
                    new GarmentCuttingIn(cuttingInGuid, null, null, null,"RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0)
                });

            Guid cuttingInItemGuid = Guid.NewGuid();
            GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(cuttingInItemGuid, cuttingInGuid, Guid.NewGuid(), 1, null, Guid.Empty,null);
            _mockGarmentCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>()
                {
                    garmentCuttingInItem.GetReadModel()
                }.AsQueryable());

            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(Guid.NewGuid(), cuttingInItemGuid, Guid.NewGuid(), Guid.Empty, Guid.Empty,new ProductId(1), null, null, null, null, 1, new UomId(1), null, 1, new UomId(1), null, 1, 1,1,1, null);
            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>()
                {
                    garmentCuttingInDetail.GetReadModel()
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
            var unitUnderTest = CreateGarmentCuttingInController();

            _mockGarmentCuttingInRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingInReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingIn>()
                {
                    new GarmentCuttingIn(Guid.NewGuid(), null, null, null,"RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0)
                });

            _mockGarmentCuttingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingInItem>()
                {
                    new GarmentCuttingInItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, null,Guid.Empty,"")
                });

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingInDetailReadModel, bool>>>()))
                .Returns(new List<GarmentCuttingInDetail>()
                {
                    new GarmentCuttingInDetail(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, null, 0, new UomId(1), null, 0, new UomId(1), null, 0, 0,1,1,null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentCuttingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingIn(Guid.NewGuid(), null, null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentCuttingInCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentCuttingInCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentCuttingInCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentCuttingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingIn(Guid.NewGuid(), null, null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentCuttingInCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentCuttingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentCuttingIn(Guid.NewGuid(), null, null, null,"RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetLoaderByRO_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingInController();

            _mockGarmentCuttingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingInReadModel>().AsQueryable());

            _mockGarmentCuttingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingInReadModel>>()))
                .Returns(new List<GarmentCuttingIn>()
                {
                    new GarmentCuttingIn(Guid.NewGuid(), null, null, null,"RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0)
                });

            // Act
            var result = await unitUnderTest.GetLoaderByRO(It.IsAny<string>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        /*[Fact]
        public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentCuttingInController();

            _mockGarmentCuttingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingInReadModel>() { new GarmentCuttingInReadModel(id) }
                .AsQueryable());

            _mockGarmentCuttingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingInReadModel>>()))
                .Returns(new List<GarmentCuttingIn>()
                {
                    new GarmentCuttingIn(Guid.NewGuid(), null, null, null,"RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, 0)
                });

            _mockGarmentCuttingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInItemReadModel>()
                {
                    new GarmentCuttingInItemReadModel(Guid.NewGuid())
                }.AsQueryable());

            _mockGarmentCuttingInItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingInItemReadModel>>()))
                .Returns(new List<GarmentCuttingInItem>()
                {
                    new GarmentCuttingInItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 0, null,Guid.Empty,"")
                });

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>()
                {
                    new GarmentCuttingInDetailReadModel(Guid.NewGuid())
                }.AsQueryable());

            _mockGarmentCuttingInDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingInDetailReadModel>>()))
                .Returns(new List<GarmentCuttingInDetail>()
                {
                    new GarmentCuttingInDetail(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, null, 0, new UomId(1), null, 0, new UomId(1), null, 0, 0,1,1,null)
                });

            // Act
            var orderData = new
            {
                Article = "desc",
            };

            string oder = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1,25, oder,new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentCuttingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentCuttingInCommand command = new UpdateDatesGarmentCuttingInCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentCuttingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentCuttingInCommand command = new UpdateDatesGarmentCuttingInCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentCuttingInCommand command2 = new UpdateDatesGarmentCuttingInCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }

        
    }
}
