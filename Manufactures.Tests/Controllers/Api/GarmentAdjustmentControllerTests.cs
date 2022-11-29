using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
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
    public class GarmentAdjustmentControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentAdjustmentRepository> _mockAdjustmentRepository;
        private readonly Mock<IGarmentAdjustmentItemRepository> _mockAdjustmentItemRepository;

        public GarmentAdjustmentControllerTests() : base()
        {
            _mockAdjustmentRepository = CreateMock<IGarmentAdjustmentRepository>();
            _mockAdjustmentItemRepository = CreateMock<IGarmentAdjustmentItemRepository>();

            _MockStorage.SetupStorage(_mockAdjustmentRepository);
            _MockStorage.SetupStorage(_mockAdjustmentItemRepository);
        }

        private GarmentAdjustmentController CreateGarmentAdjustmentController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentAdjustmentController controller = (GarmentAdjustmentController)Activator.CreateInstance(typeof(GarmentAdjustmentController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentAdjustmentController();

            _mockAdjustmentRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentAdjustmentReadModel>().AsQueryable());

            _mockAdjustmentRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentAdjustmentReadModel>>()))
                .Returns(new List<GarmentAdjustment>()
                {
                    new GarmentAdjustment(Guid.NewGuid(), null ,null, null,null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null, null)
                });

            _mockAdjustmentItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentItemReadModel>()
                {
                    new GarmentAdjustmentItemReadModel(Guid.NewGuid())
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
            var unitUnderTest = CreateGarmentAdjustmentController();

            _mockAdjustmentRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAdjustmentReadModel, bool>>>()))
                .Returns(new List<GarmentAdjustment>()
                {
                    new GarmentAdjustment(Guid.NewGuid(), null ,null, null,null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null, null)
                });

            _mockAdjustmentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAdjustmentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAdjustmentItem>()
                {
                    new GarmentAdjustmentItem(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(),Guid.Empty,Guid.Empty,new SizeId(1), null, new ProductId(1), null, null, null, 1,1,new UomId(1),null, null,1)
                });

            //_mockSewingDOItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingDOItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAdjustmentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentAdjustmentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentAdjustment(Guid.NewGuid(), null, null, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentAdjustmentCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAdjustmentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentAdjustmentCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentAdjustmentCommand>()));
          
        }

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockAdjustmentRepository
            .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(new List<GarmentAdjustmentReadModel>() {
                new GarmentAdjustmentReadModel(id)    
                }
            .AsQueryable());

            _mockAdjustmentRepository
             .Setup(s => s.Find(It.IsAny<IQueryable<GarmentAdjustmentReadModel>>()))
             .Returns(new List<GarmentAdjustment>()
             {
                    new GarmentAdjustment(id, null ,null, null,null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null, null)
             });

            _mockAdjustmentItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentAdjustmentItemReadModel>()
               {
                  new GarmentAdjustmentItemReadModel(id)
               }.AsQueryable());

            _mockAdjustmentItemRepository
            .Setup(s => s.Find(It.IsAny<IQueryable<GarmentAdjustmentItemReadModel>>()))
            .Returns(new List<GarmentAdjustmentItem>()
            {
                    new GarmentAdjustmentItem(id,id,id,id,id,id,new SizeId(1),"sizeName",new ProductId(1),"productCode","productName","designCOlor",1,1,new UomId(1),"uomUnit","color",1)
            });


            // Act
            var orderData = new
            {
                AdjustmentNo = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var unitUnderTest = CreateGarmentAdjustmentController();
            var result = await unitUnderTest.GetComplete(1,25, order, new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentAdjustmentController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentAdjustmentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentAdjustment(Guid.NewGuid(), null, null, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
