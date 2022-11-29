using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Threading;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentSampleAvalProductControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSampleAvalProductRepository> _mockGarmentSampleAvalProductRepository;
        private Mock<IGarmentSampleAvalProductItemRepository> _mockGarmentSampleAvalProductItemRepository;
        private Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
        private Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;

        public GarmentSampleAvalProductControllerTests() : base()
        {
            _mockGarmentSampleAvalProductRepository = CreateMock<IGarmentSampleAvalProductRepository>();
            _mockGarmentSampleAvalProductItemRepository = CreateMock<IGarmentSampleAvalProductItemRepository>();
            _mockGarmentSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalProductItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);
        }

        private GarmentSampleAvalProductController CreateGarmentSampleAvalProductController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleAvalProductController controller = (GarmentSampleAvalProductController)Activator.CreateInstance(typeof(GarmentSampleAvalProductController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSampleAvalProductController();

            _mockGarmentSampleAvalProductRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleAvalProductReadModel>().AsQueryable());

            _mockGarmentSampleAvalProductRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleAvalProductReadModel>>()))
                .Returns(new List<GarmentSampleAvalProduct>()
                {
                    new GarmentSampleAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null)
                });

            _mockGarmentSampleAvalProductItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleAvalProductItemReadModel>>()))
                .Returns(new List<GarmentSampleAvalProductItem>()
                {
                    new GarmentSampleAvalProductItem(Guid.NewGuid(), Guid.NewGuid(), new GarmentSamplePreparingId("1"), new GarmentSamplePreparingItemId("1"), new Domain.GarmentSample.SampleAvalProducts.ValueObjects.ProductId(1), null, null, null, 0, new Domain.GarmentSample.SampleAvalProducts.ValueObjects.UomId(1), null,10,false)
                });

            _mockGarmentSampleAvalProductItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleAvalProductItemReadModel>()
                {
                    new GarmentSampleAvalProductItemReadModel(Guid.NewGuid())
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
            var unitUnderTest = CreateGarmentSampleAvalProductController();

            _mockGarmentSampleAvalProductRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleAvalProductReadModel>().AsQueryable());

            _mockGarmentSampleAvalProductRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleAvalProductReadModel>>()))
                .Returns(new List<GarmentSampleAvalProduct>()
                {
                    new GarmentSampleAvalProduct(Guid.NewGuid(),"roNo", "article", DateTimeOffset.Now, new UnitDepartmentId(1),"unitCode", "unitName")
                });

            _mockGarmentSampleAvalProductItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleAvalProductItemReadModel>>()))
                .Returns(new List<GarmentSampleAvalProductItem>()
                {
                    new GarmentSampleAvalProductItem(Guid.NewGuid(), Guid.NewGuid(), new GarmentSamplePreparingId("1"), new GarmentSamplePreparingItemId("1"), new Domain.GarmentSample.SampleAvalProducts.ValueObjects.ProductId(1),"productCode", "productName","designColor", 1, new Domain.GarmentSample.SampleAvalProducts.ValueObjects.UomId(1),"uomUnit",10,false)
                });

            _mockGarmentSampleAvalProductItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleAvalProductItemReadModel>()
                {
                    new GarmentSampleAvalProductItemReadModel(Guid.NewGuid())
                }.AsQueryable());

            // Act
            var orderData = new
            {
                Article = "desc"
            };
            var order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1, 25, order, new List<string>(), "article", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleAvalProductController();

            _mockGarmentSampleAvalProductRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalProductReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalProduct>()
                {
                    new GarmentSampleAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null)
                });

            _mockGarmentSampleAvalProductItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalProductItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalProductItem>()
                {
                    new GarmentSampleAvalProductItem(Guid.NewGuid(), Guid.NewGuid(), new GarmentSamplePreparingId("1"), new GarmentSamplePreparingItemId("1"), new Domain.GarmentSample.SampleAvalProducts.ValueObjects.ProductId(1), null, null, null, 0, new Domain.GarmentSample.SampleAvalProducts.ValueObjects.UomId(1), null,10,false)
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
            var unitUnderTest = CreateGarmentSampleAvalProductController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleAvalProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleAvalProductCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }


        [Fact]
        public async Task Post_Throws_InternalServerError()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleAvalProductController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleAvalProductCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleAvalProductCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleAvalProductController();

            _mockGarmentSampleAvalProductRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalProductReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalProduct>()
                {
                    new GarmentSampleAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null)
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleAvalProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleAvalProduct(Guid.NewGuid(), null, null, DateTimeOffset.Now, new UnitDepartmentId(1), null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task UpdateIsReceived_Return_OK()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleAvalProductController();


            _MockMediator
              .Setup(s => s.Send(It.IsAny<UpdateIsReceivedGarmentSampleAvalProductCommand>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(true);

            // Act
            var command = new UpdateIsReceivedGarmentSampleAvalProductCommand(new List<string>() { "ids" }, true);
            var result = await unitUnderTest.UpdateIsReceived(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
