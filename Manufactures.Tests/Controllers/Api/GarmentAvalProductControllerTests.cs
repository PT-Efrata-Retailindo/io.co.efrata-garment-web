using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
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
    public class GarmentAvalProductControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentAvalProductRepository> _mockGarmentAvalProductRepository;
        private Mock<IGarmentAvalProductItemRepository> _mockGarmentAvalProductItemRepository;
        private Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

        public GarmentAvalProductControllerTests() : base()
        {
            _mockGarmentAvalProductRepository = CreateMock<IGarmentAvalProductRepository>();
            _mockGarmentAvalProductItemRepository = CreateMock<IGarmentAvalProductItemRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalProductItemRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);
        }

        private GarmentAvalProductController CreateGarmentAvalProductController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentAvalProductController controller = (GarmentAvalProductController)Activator.CreateInstance(typeof(GarmentAvalProductController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentAvalProductController();

            _mockGarmentAvalProductRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentAvalProductReadModel>().AsQueryable());

            _mockGarmentAvalProductRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentAvalProductReadModel>>()))
                .Returns(new List<GarmentAvalProduct>()
                {
                    new GarmentAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null)
                });

            _mockGarmentAvalProductItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentAvalProductItemReadModel>>()))
                .Returns(new List<GarmentAvalProductItem>()
                {
                    new GarmentAvalProductItem(Guid.NewGuid(), Guid.NewGuid(), new GarmentPreparingId("1"), new GarmentPreparingItemId("1"), new Domain.GarmentAvalProducts.ValueObjects.ProductId(1), null, null, null, 0, new Domain.GarmentAvalProducts.ValueObjects.UomId(1), null,10,false)
                });

            _mockGarmentAvalProductItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalProductItemReadModel>()
                {
                    new GarmentAvalProductItemReadModel(Guid.NewGuid())
                }.AsQueryable());

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Get_with_keyword_and_order()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAvalProductController();

            _mockGarmentAvalProductRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentAvalProductReadModel>().AsQueryable());

            _mockGarmentAvalProductRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentAvalProductReadModel>>()))
                .Returns(new List<GarmentAvalProduct>()
                {
                    new GarmentAvalProduct(Guid.NewGuid(),"roNo", "article", DateTimeOffset.Now, new UnitDepartmentId(1),"unitCode", "unitName")
                });

            _mockGarmentAvalProductItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentAvalProductItemReadModel>>()))
                .Returns(new List<GarmentAvalProductItem>()
                {
                    new GarmentAvalProductItem(Guid.NewGuid(), Guid.NewGuid(), new GarmentPreparingId("1"), new GarmentPreparingItemId("1"), new Domain.GarmentAvalProducts.ValueObjects.ProductId(1),"productCode", "productName","designColor", 1, new Domain.GarmentAvalProducts.ValueObjects.UomId(1),"uomUnit",10,false)
                });

            _mockGarmentAvalProductItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalProductItemReadModel>()
                {
                    new GarmentAvalProductItemReadModel(Guid.NewGuid())
                }.AsQueryable());

            // Act
            var orderData = new
            {
                Article = "desc"
            };
            var order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1,25, order,new List<string>(), "article", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAvalProductController();

            _mockGarmentAvalProductRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalProductReadModel, bool>>>()))
                .Returns(new List<GarmentAvalProduct>()
                {
                    new GarmentAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null)
                });

            _mockGarmentAvalProductItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalProductItemReadModel, bool>>>()))
                .Returns(new List<GarmentAvalProductItem>()
                {
                    new GarmentAvalProductItem(Guid.NewGuid(), Guid.NewGuid(), new GarmentPreparingId("1"), new GarmentPreparingItemId("1"), new Domain.GarmentAvalProducts.ValueObjects.ProductId(1), null, null, null, 0, new Domain.GarmentAvalProducts.ValueObjects.UomId(1), null,10,false)
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
            var unitUnderTest = CreateGarmentAvalProductController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentAvalProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentAvalProductCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }


        [Fact]
        public async Task Post_Throws_InternalServerError()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAvalProductController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentAvalProductCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentAvalProductCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAvalProductController();

            _mockGarmentAvalProductRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalProductReadModel, bool>>>()))
                .Returns(new List<GarmentAvalProduct>()
                {
                    new GarmentAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null)
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentAvalProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

       


        [Fact]
        public async Task UpdateIsReceived_Return_OK()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAvalProductController();


            _MockMediator
              .Setup(s => s.Send(It.IsAny<UpdateIsReceivedGarmentAvalProductCommand>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(true);

            // Act
            var command =new  UpdateIsReceivedGarmentAvalProductCommand(new List<string>() { "ids" },true);
            var result = await unitUnderTest.UpdateIsReceived(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}