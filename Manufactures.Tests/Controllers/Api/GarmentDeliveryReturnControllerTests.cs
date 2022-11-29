using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
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
    public class GarmentDeliveryReturnControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentDeliveryReturnRepository> _mockGarmentDeliveryReturnRepository;
        private Mock<IGarmentDeliveryReturnItemRepository> _mockGarmentDeliveryReturnItemRepository;

        public GarmentDeliveryReturnControllerTests() : base()
        {
            _mockGarmentDeliveryReturnRepository = CreateMock<IGarmentDeliveryReturnRepository>();
            _mockGarmentDeliveryReturnItemRepository = CreateMock<IGarmentDeliveryReturnItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentDeliveryReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentDeliveryReturnItemRepository);
        }

        private GarmentDeliveryReturnController CreateGarmentDeliveryReturnController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentDeliveryReturnController controller = (GarmentDeliveryReturnController)Activator.CreateInstance(typeof(GarmentDeliveryReturnController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentDeliveryReturnController();

            _mockGarmentDeliveryReturnRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentDeliveryReturnReadModel>().AsQueryable());

            _mockGarmentDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentDeliveryReturnReadModel>>()))
                .Returns(new List<GarmentDeliveryReturn>()
                {
                    new GarmentDeliveryReturn(id,"drNo","roNo","article",1,"unitDONo",1,"preparingId",DateTimeOffset.Now,"returnType",new UnitDepartmentId(1),"unitCode","unitName",new StorageId(1),"storageName","storageCode",true)
                });

            _mockGarmentDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentDeliveryReturnItemReadModel>>()))
                .Returns(new List<GarmentDeliveryReturnItem>()
                {
                    new GarmentDeliveryReturnItem(Guid.NewGuid(), Guid.NewGuid(), 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null)
                });

            _mockGarmentDeliveryReturnItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentDeliveryReturnItemReadModel>()
                {
                    new GarmentDeliveryReturnItemReadModel(Guid.NewGuid())
                }.AsQueryable());

            // Act
            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1,25,order,new List<string>(), "article", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentDeliveryReturnController();

            _mockGarmentDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
                .Returns(new List<GarmentDeliveryReturn>()
                {
                    new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false)
                });

            _mockGarmentDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnItemReadModel, bool>>>()))
                .Returns(new List<GarmentDeliveryReturnItem>()
                {
                    new GarmentDeliveryReturnItem(Guid.NewGuid(), Guid.NewGuid(), 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null)
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
            var unitUnderTest = CreateGarmentDeliveryReturnController();

            _mockGarmentDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
                .Returns(new List<GarmentDeliveryReturn>() {
              
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

            PlaceGarmentDeliveryReturnCommand command = new PlaceGarmentDeliveryReturnCommand();
            command.Items = new List<GarmentDeliveryReturnItemValueObject>()
            {
                new GarmentDeliveryReturnItemValueObject()
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
            var unitUnderTest = CreateGarmentDeliveryReturnController();

            _mockGarmentDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
                .Returns(new List<GarmentDeliveryReturn>() {
                new GarmentDeliveryReturn(id,"drNo","roNo","article",1,"unitDONo",1,"preparingId",DateTimeOffset.Now,"returnType",new UnitDepartmentId(1),"unitCode","unitName",new StorageId(1),"storageName","storageCode",true)
                });

          

            PlaceGarmentDeliveryReturnCommand command = new PlaceGarmentDeliveryReturnCommand();
            command.Items = new List<GarmentDeliveryReturnItemValueObject>()
            {
                new GarmentDeliveryReturnItemValueObject()
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
            var unitUnderTest = CreateGarmentDeliveryReturnController();

            _mockGarmentDeliveryReturnRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnReadModel, bool>>>()))
                .Returns(new List<GarmentDeliveryReturn>());

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            PlaceGarmentDeliveryReturnCommand command = new PlaceGarmentDeliveryReturnCommand();
            command.Items = new List<GarmentDeliveryReturnItemValueObject>();

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
            var unitUnderTest = CreateGarmentDeliveryReturnController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

            _mockGarmentDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnItemReadModel, bool>>>()))
                .Returns(new List<GarmentDeliveryReturnItem>()
                {
                    new GarmentDeliveryReturnItem(id, id, 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null)
                });

            UpdateGarmentDeliveryReturnCommand command = new UpdateGarmentDeliveryReturnCommand();
            command.Items = new List<GarmentDeliveryReturnItemValueObject>()
            {
                new GarmentDeliveryReturnItemValueObject()
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
            var unitUnderTest = CreateGarmentDeliveryReturnController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

            _mockGarmentDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnItemReadModel, bool>>>()))
                .Returns(new List<GarmentDeliveryReturnItem>()
                {
                    new GarmentDeliveryReturnItem(id, id, 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null)
                });

            UpdateGarmentDeliveryReturnCommand command = new UpdateGarmentDeliveryReturnCommand();
            command.Items = new List<GarmentDeliveryReturnItemValueObject>()
            {
                new GarmentDeliveryReturnItemValueObject()
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
            var unitUnderTest = CreateGarmentDeliveryReturnController();

            _mockGarmentDeliveryReturnItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentDeliveryReturnItemReadModel, bool>>>()))
                .Returns(new List<GarmentDeliveryReturnItem>()
                {
                    new GarmentDeliveryReturnItem(Guid.NewGuid(), Guid.NewGuid(), 0, 0, null, new ProductId(1), null, null, null, "RONo", 0, new UomId(1), null)
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentDeliveryReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentDeliveryReturn(Guid.NewGuid(), null, "RONo", null, 0, null, 0, null, DateTimeOffset.Now, null, new UnitDepartmentId(1), null, null, new StorageId(1), null, null, false));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}