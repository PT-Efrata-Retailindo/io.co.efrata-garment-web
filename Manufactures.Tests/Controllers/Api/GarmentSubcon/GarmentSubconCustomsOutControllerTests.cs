using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Commands;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
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

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentSubconCustomsOutControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSubconCustomsOutRepository> _mockGarmentSubconCustomsOutRepository;
        private Mock<IGarmentSubconCustomsOutItemRepository> _mockGarmentSubconCustomsOutItemRepository;

        public GarmentSubconCustomsOutControllerTests() : base()
        {
            _mockGarmentSubconCustomsOutRepository = CreateMock<IGarmentSubconCustomsOutRepository>();
            _mockGarmentSubconCustomsOutItemRepository = CreateMock<IGarmentSubconCustomsOutItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSubconCustomsOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSubconCustomsOutItemRepository);

        }

        private GarmentSubconCustomsOutController CreateGarmentSubconCustomsOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconCustomsOutController controller = (GarmentSubconCustomsOutController)Activator.CreateInstance(typeof(GarmentSubconCustomsOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSubconCustomsOutController();

            _mockGarmentSubconCustomsOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconCustomsOutReadModel>().AsQueryable());

            Guid SubconCustomsOutGuid = Guid.NewGuid();
            _mockGarmentSubconCustomsOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconCustomsOutReadModel>>()))
                .Returns(new List<GarmentSubconCustomsOut>()
                {
                    new GarmentSubconCustomsOut(SubconCustomsOutGuid,"",DateTimeOffset.Now,"","",Guid.NewGuid(),"",new SupplierId(1),"","","", "")
                });

            Guid SubconCustomsOutItemGuid = Guid.NewGuid();
            //GarmentSubconCustomsOutItem garmentSubconCustomsOutItem = new GarmentSubconCustomsOutItem(Guid.Empty,new Guid(), 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType");
            //_mockGarmentSubconCustomsOutItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSubconCustomsOutItemReadModel>()
            //    {
            //        garmentSubconCustomsOutItem.GetReadModel()
            //    }.AsQueryable());

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsOutController();

            _mockGarmentSubconCustomsOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsOutReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCustomsOut>()
                {
                    new GarmentSubconCustomsOut(Guid.NewGuid(),"",DateTimeOffset.Now,"","",Guid.NewGuid(),"",new SupplierId(1),"","","","")
                });

            _mockGarmentSubconCustomsOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCustomsOutItem>()
                {
                    new GarmentSubconCustomsOutItem(Guid.NewGuid(),Guid.NewGuid(),"",Guid.NewGuid(),1)
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
            var unitUnderTest = CreateGarmentSubconCustomsOutController();
            PlaceGarmentSubconCustomsOutCommand command = new PlaceGarmentSubconCustomsOutCommand();
            //_mockGarmentSubconCustomsOutRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsOutReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconCustomsOut>());
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconCustomsOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconCustomsOut(Guid.NewGuid(), "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "", new SupplierId(1), "", "", "", ""));

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconCustomsOutCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconCustomsOutCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsOutController();
            Guid subconCustomsOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSubconCustomsOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconCustomsOut(subconCustomsOutGuid, "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "", new SupplierId(1), "", "", "",""));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSubconCustomsOutCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsOutController();
            var subconCustomsOutGuid = Guid.NewGuid();

            _mockGarmentSubconCustomsOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconCustomsOutReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCustomsOut>()
                {
                    new GarmentSubconCustomsOut(subconCustomsOutGuid, "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "", new SupplierId(1), "", "", "","")
                });


            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconCustomsOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconCustomsOut(subconCustomsOutGuid, "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "", new SupplierId(1), "", "", "",""));

            // Act
            var result = await unitUnderTest.Delete(subconCustomsOutGuid.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCustomsOutController();

            _mockGarmentSubconCustomsOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconCustomsOutReadModel>().AsQueryable());

            Guid SubconCustomsOutGuid = Guid.NewGuid();
            _mockGarmentSubconCustomsOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconCustomsOutReadModel>>()))
                .Returns(new List<GarmentSubconCustomsOut>()
                {
                    new GarmentSubconCustomsOut(SubconCustomsOutGuid, "", DateTimeOffset.Now, "", "", Guid.NewGuid(), "", new SupplierId(1), "", "", "","")
                });

            Guid SubconCustomsOutItemGuid = Guid.NewGuid();
            GarmentSubconCustomsOutItem garmentSubconCustomsOutItem = new GarmentSubconCustomsOutItem(Guid.NewGuid(), Guid.NewGuid(), "", Guid.NewGuid(), 1);

            _mockGarmentSubconCustomsOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCustomsOutItemReadModel>() {
                    garmentSubconCustomsOutItem.GetReadModel()
                }.AsQueryable());

            _mockGarmentSubconCustomsOutItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconCustomsOutItemReadModel>>()))
                .Returns(new List<GarmentSubconCustomsOutItem>()
                {
                    new GarmentSubconCustomsOutItem(Guid.NewGuid(),Guid.NewGuid(),"",Guid.NewGuid(),1)
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