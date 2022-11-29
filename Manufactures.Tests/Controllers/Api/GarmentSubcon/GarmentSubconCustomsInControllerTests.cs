using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Commands;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
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

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentSubconCustomsInControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSubconCustomsInRepository> _mockGarmentSubconCustomsInRepository;
        private Mock<IGarmentSubconCustomsInItemRepository> _mockGarmentSubconCustomsInItemRepository;

        public GarmentSubconCustomsInControllerTests() : base()
        {
            _mockGarmentSubconCustomsInRepository = CreateMock<IGarmentSubconCustomsInRepository>();
            _mockGarmentSubconCustomsInItemRepository = CreateMock<IGarmentSubconCustomsInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSubconCustomsInRepository);
            _MockStorage.SetupStorage(_mockGarmentSubconCustomsInItemRepository);

        }

        private GarmentSubconCustomsInController CreateGarmentSubconCustomsInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconCustomsInController controller = (GarmentSubconCustomsInController)Activator.CreateInstance(typeof(GarmentSubconCustomsInController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSubconCustomsInController();

            _mockGarmentSubconCustomsInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconCustomsInReadModel>().AsQueryable());

            Guid subconCustomsInGuid = Guid.NewGuid();
            _mockGarmentSubconCustomsInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconCustomsInReadModel>>()))
                .Returns(new List<GarmentSubconCustomsIn>()
                {
                    new GarmentSubconCustomsIn(subconCustomsInGuid, "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "no", new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", false, "")
                });

            Guid subconCustomsInItemGuid = Guid.NewGuid();
            GarmentSubconCustomsInItem garmentSubconCustomsInItem = new GarmentSubconCustomsInItem(subconCustomsInItemGuid, subconCustomsInGuid, new Domain.Shared.ValueObjects.SupplierId(1), "", "", 1, "", 1);

            _mockGarmentSubconCustomsInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCustomsInItemReadModel>()
                {
                    garmentSubconCustomsInItem.GetReadModel()
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
            var unitUnderTest = CreateGarmentSubconCustomsInController();

            _mockGarmentSubconCustomsInRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsInReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCustomsIn>()
                {
                    new GarmentSubconCustomsIn(Guid.NewGuid(), "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "no", new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", false, "")
                });

            _mockGarmentSubconCustomsInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCustomsInItem>()
                {
                    new GarmentSubconCustomsInItem(Guid.NewGuid(), Guid.NewGuid(), new Domain.Shared.ValueObjects.SupplierId(1), "", "", 1, "", 1)
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
            var unitUnderTest = CreateGarmentSubconCustomsInController();
            PlaceGarmentSubconCustomsInCommand command = new PlaceGarmentSubconCustomsInCommand();
            //_mockGarmentSubconCustomsOutRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsOutReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconCustomsOut>());
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconCustomsInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconCustomsIn(Guid.NewGuid(), "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "no", new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", false, ""));

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconCustomsInCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconCustomsInCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsInController();
            Guid subconCustomsInGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSubconCustomsInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconCustomsIn(subconCustomsInGuid, "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "no", new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", false, ""));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSubconCustomsInCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsInController();
            Guid subconCustomsInGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconCustomsInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconCustomsIn(subconCustomsInGuid, "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "", new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", false, ""));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsInController();

            _mockGarmentSubconCustomsInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconCustomsInReadModel>().AsQueryable());

            Guid SubconCustomsInGuid = Guid.NewGuid();
            _mockGarmentSubconCustomsInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconCustomsInReadModel>>()))
                .Returns(new List<GarmentSubconCustomsIn>()
                {
                    new GarmentSubconCustomsIn(SubconCustomsInGuid, "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "no", new Domain.Shared.ValueObjects.SupplierId(1), "", "", "", false, "")
                });

            Guid SubconCustomsInItemGuid = Guid.NewGuid();
            GarmentSubconCustomsInItem garmentSubconCustomsInItem = new GarmentSubconCustomsInItem(Guid.NewGuid(), Guid.NewGuid(), new Domain.Shared.ValueObjects.SupplierId(1), "", "", 1, "", 1);

            _mockGarmentSubconCustomsInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCustomsInItemReadModel>() {
                    garmentSubconCustomsInItem.GetReadModel()
                }.AsQueryable());

            _mockGarmentSubconCustomsInItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconCustomsInItemReadModel>>()))
                .Returns(new List<GarmentSubconCustomsInItem>()
                {
                    new GarmentSubconCustomsInItem(Guid.NewGuid(), Guid.NewGuid(), new Domain.Shared.ValueObjects.SupplierId(1), "", "", 1, "", 1)
                });
            var orderData = new
            {
                Id = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            // Act
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
