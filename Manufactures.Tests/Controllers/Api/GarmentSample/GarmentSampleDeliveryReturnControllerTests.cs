using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Commands;
using System.Threading;

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentSampleDeliveryReturnControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSampleDeliveryReturnRepository> _mockGarmentSampleDeliveryReturnRepository;
        private Mock<IGarmentSampleDeliveryReturnItemRepository> _mockGarmentSampleDeliveryReturnItemRepository;

        public GarmentSampleDeliveryReturnControllerTests() : base()
        {
            _mockGarmentSampleDeliveryReturnRepository = CreateMock<IGarmentSampleDeliveryReturnRepository>();
            _mockGarmentSampleDeliveryReturnItemRepository = CreateMock<IGarmentSampleDeliveryReturnItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleDeliveryReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleDeliveryReturnItemRepository);
        }

        private GarmentSampleDeliveryReturnController CreateGarmentSampleDeliveryReturnController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleDeliveryReturnController controller = (GarmentSampleDeliveryReturnController)Activator.CreateInstance(typeof(GarmentSampleDeliveryReturnController), _MockServiceProvider.Object);
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
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSampleDeliveryReturnController();

            _mockGarmentSampleDeliveryReturnRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleDeliveryReturnReadModel>().AsQueryable());

            _mockGarmentSampleDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleDeliveryReturnReadModel>>()))
                .Returns(new List<GarmentSampleDeliveryReturn>()
                {
                    new GarmentSampleDeliveryReturn(id,"drNo","roNo","article",1,"unitDONo",1,"preparingId",DateTimeOffset.Now,"returnType",new UnitDepartmentId(1),"unitCode","unitName",new StorageId(1),"storageName","storageCode",true)
                });

            _mockGarmentSampleDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleDeliveryReturnItemReadModel>>()))
                .Returns(new List<GarmentSampleDeliveryReturnItem>()
                {
                    new GarmentSampleDeliveryReturnItem(Guid.NewGuid(), Guid.NewGuid(), 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null,"","","","","")
                });

            _mockGarmentSampleDeliveryReturnItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleDeliveryReturnItemReadModel>()
                {
                    new GarmentSampleDeliveryReturnItemReadModel(Guid.NewGuid())
                }.AsQueryable());

            // Act
            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1, 25, order, new List<string>(), "article", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleDeliveryReturnController();

            _mockGarmentSampleDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
                .Returns(new List<GarmentSampleDeliveryReturn>()
                {
                    new GarmentSampleDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false)
                });

            _mockGarmentSampleDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleDeliveryReturnItem>()
                {
                    new GarmentSampleDeliveryReturnItem(Guid.NewGuid(), Guid.NewGuid(), 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null,"","","","","")
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
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSampleDeliveryReturnController();

            _mockGarmentSampleDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
                .Returns(new List<GarmentSampleDeliveryReturn>()
                {

                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

            PlaceGarmentSampleDeliveryReturnCommand command = new PlaceGarmentSampleDeliveryReturnCommand();
            command.Items = new List<GarmentSampleDeliveryReturnItemValueObject>()
            {
                new GarmentSampleDeliveryReturnItemValueObject()
                {
                    Product =new Product()
                    {
                        Name ="NO FABRIC"
                    }
                }
            };

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
            var unitUnderTest = CreateGarmentSampleDeliveryReturnController();

            _mockGarmentSampleDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
                .Returns(new List<GarmentSampleDeliveryReturn>() {
                new GarmentSampleDeliveryReturn(id,"drNo","roNo","article",1,"unitDONo",1,"preparingId",DateTimeOffset.Now,"returnType",new UnitDepartmentId(1),"unitCode","unitName",new StorageId(1),"storageName","storageCode",true)
                });



            PlaceGarmentSampleDeliveryReturnCommand command = new PlaceGarmentSampleDeliveryReturnCommand();
            command.Items = new List<GarmentSampleDeliveryReturnItemValueObject>()
            {
                new GarmentSampleDeliveryReturnItemValueObject()
                {
                    Product =new Product()
                    {
                        Name ="FABRIC"
                    }
                }
            };

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Return_InternalServerError()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleDeliveryReturnController();

            _mockGarmentSampleDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnReadModel, bool>>>()))
                .Returns(new List<GarmentSampleDeliveryReturn>());

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            PlaceGarmentSampleDeliveryReturnCommand command = new PlaceGarmentSampleDeliveryReturnCommand();
            command.Items = new List<GarmentSampleDeliveryReturnItemValueObject>();

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_Success_When_Item_Not_Save()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSampleDeliveryReturnController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSampleDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

            _mockGarmentSampleDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleDeliveryReturnItem>()
                {
                    new GarmentSampleDeliveryReturnItem(id, id, 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null,"","","","","")
                });

            UpdateGarmentSampleDeliveryReturnCommand command = new UpdateGarmentSampleDeliveryReturnCommand();
            command.Items = new List<GarmentSampleDeliveryReturnItemValueObject>()
            {
                new GarmentSampleDeliveryReturnItemValueObject()
                {
                    IsSave =false,
                    Product =new Product()
                    {
                        Name ="NOFABRIC"
                    }
                }
            };

            // Act
            var result = await unitUnderTest.Put(id.ToString(), command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_Success_When_Item_Save()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSampleDeliveryReturnController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSampleDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

            _mockGarmentSampleDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleDeliveryReturnItem>()
                {
                    new GarmentSampleDeliveryReturnItem(id, id, 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null,"","","","","")
                });

            UpdateGarmentSampleDeliveryReturnCommand command = new UpdateGarmentSampleDeliveryReturnCommand();
            command.Items = new List<GarmentSampleDeliveryReturnItemValueObject>()
            {
                new GarmentSampleDeliveryReturnItemValueObject()
                {
                    IsSave =true,
                    Product =new Product()
                    {
                        Name ="NOFABRIC"
                    }
                }
            };

            // Act
            var result = await unitUnderTest.Put(id.ToString(), command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleDeliveryReturnController();

            _mockGarmentSampleDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleDeliveryReturnItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleDeliveryReturnItem>()
                {
                    new GarmentSampleDeliveryReturnItem(Guid.NewGuid(), Guid.NewGuid(), 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null,"","","","","")
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

    }
}
