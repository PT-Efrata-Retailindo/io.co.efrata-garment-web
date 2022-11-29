using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
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
    public class GarmentSewingInControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSewingInRepository> _mockGarmentSewingInRepository;
        private Mock<IGarmentSewingInItemRepository> _mockGarmentSewingInItemRepository;

        public GarmentSewingInControllerTests() : base()
        {
            _mockGarmentSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockGarmentSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSewingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingInItemRepository);
        }

        private GarmentSewingInController CreateGarmentSewingInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSewingInController controller = (GarmentSewingInController)Activator.CreateInstance(typeof(GarmentSewingInController), _MockServiceProvider.Object);
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

        /*[Fact]
        public async Task Get_with_keyword()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSewingInController();

            _mockGarmentSewingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    //new GarmentSewingInReadModel(id)
                }
                .AsQueryable());

            _mockGarmentSewingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingInReadModel>>()))
                .Returns(new List<GarmentSewingIn>()
                {
                    new GarmentSewingIn(id,"productCode", "sewingForm", id,"loadingNo", new UnitDepartmentId(1),"unitFromCode" ,"unitFromName", new UnitDepartmentId(1),"unitCode","unitName", "RONo","article", new GarmentComodityId(1),"comodityCode", "comodityName", DateTimeOffset.Now)
                });

            _mockGarmentSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingInItemReadModel>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(id,id,id, id, id,id,id, new ProductId(1),"productCode", "productName","designColor", new SizeId(1),"sizeName", 1, new UomId(1),"uomUnit","color", 1,1,1)
                });

            _mockGarmentSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>()
                {
                    new GarmentSewingInItemReadModel(id)
                }.AsQueryable());

            // Act
            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1,25,order,new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task Get_with_keyword_NoOrder()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSewingInController();

            _mockGarmentSewingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    //new GarmentSewingInReadModel(id)
                }
                .AsQueryable());

            _mockGarmentSewingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingInReadModel>>()))
                .Returns(new List<GarmentSewingIn>()
                {
                    new GarmentSewingIn(id,"sewingInNo", "sewingForm", id,"loadingNo", new UnitDepartmentId(1),"unitFromCode" ,"unitFromName", new UnitDepartmentId(1),"unitCode","unitName", "RONo","article", new GarmentComodityId(1),"comodityCode", "comodityName", DateTimeOffset.Now)
                });

            _mockGarmentSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingInItemReadModel>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(id,id,id, id, id,id,id, new ProductId(1),"productCode", "productName","designColor", new SizeId(1),"sizeName", 1, new UomId(1),"uomUnit","color", 1,1,1)
                });

            _mockGarmentSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>()
                {
                    new GarmentSewingInItemReadModel(id)
                }.AsQueryable());

            // Act
            var result = await unitUnderTest.Get(1, 25, "{}", new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task Get_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _mockGarmentSewingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSewingInReadModel>().AsQueryable());

            _mockGarmentSewingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingInReadModel>>()))
                .Returns(new List<GarmentSewingIn>()
                {
                    new GarmentSewingIn(Guid.NewGuid(),null, null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new GarmentComodityId(1), null, null, DateTimeOffset.Now)
                });

            _mockGarmentSewingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSewingInItemReadModel>()
               {
                    new GarmentSewingInItemReadModel(Guid.NewGuid())
               }.AsQueryable());

            _mockGarmentSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingInItemReadModel>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(Guid.NewGuid(),Guid.Empty,Guid.Empty, Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0,1,1)
                });

           

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task GetComplete_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _mockGarmentSewingInRepository
             .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
             .Returns(new List<GarmentSewingInReadModel>().AsQueryable());

            _mockGarmentSewingInRepository
               .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingInReadModel>>()))
               .Returns(new List<GarmentSewingIn>()
               {
                    new GarmentSewingIn(Guid.NewGuid(),null, null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new GarmentComodityId(1), null, null, DateTimeOffset.Now)
               });

            _mockGarmentSewingInItemRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSewingInItemReadModel>()
               {
                    new GarmentSewingInItemReadModel(Guid.NewGuid())
               }.AsQueryable());

            _mockGarmentSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingInItemReadModel>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(Guid.NewGuid(),Guid.Empty,Guid.Empty, Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0,1,1)
                });

            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            // Act
            var result = await unitUnderTest.GetComplete(1,25, order,new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _mockGarmentSewingInRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingInReadModel, bool>>>()))
                .Returns(new List<GarmentSewingIn>()
                {
                    new GarmentSewingIn(Guid.NewGuid(), null,null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new GarmentComodityId(1), null, null, DateTimeOffset.Now)
                });

            _mockGarmentSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(Guid.NewGuid(),Guid.Empty,Guid.Empty, Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0,1,1)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSewingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSewingIn(Guid.NewGuid(),null, null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new GarmentComodityId(1), null, null, DateTimeOffset.Now));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSewingInCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSewingInCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var command = new PlaceGarmentSewingInCommand()
            {
                SewingInNo = "SewingInNo",
                Price =1
            };
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(command));
        }*/

        /*[Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSewingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSewingIn(Guid.NewGuid(),null, null, Guid.NewGuid(), null, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new GarmentComodityId(1), null, null, DateTimeOffset.Now));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSewingInCommand command = new UpdateDatesGarmentSewingInCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentSewingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSewingInCommand command = new UpdateDatesGarmentSewingInCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentSewingInCommand command2 = new UpdateDatesGarmentSewingInCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }*/
    }
}