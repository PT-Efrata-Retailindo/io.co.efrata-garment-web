using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
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
    public class GarmentFinishingInControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockSewingOutItemRepository;

        public GarmentFinishingInControllerTests() : base()
        {
            _mockFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
        }

        private GarmentFinishingInController CreateGarmentFinishingInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentFinishingInController controller = (GarmentFinishingInController)Activator.CreateInstance(typeof(GarmentFinishingInController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentFinishingInController();

            _mockFinishingInRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentFinishingInReadModel>().AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingInReadModel>>()))
                .Returns(new List<GarmentFinishingIn>()
                {
                    new GarmentFinishingIn(Guid.NewGuid(), null,null , new UnitDepartmentId(1), null, null, "RONo",null,new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null, 0,null,null)
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>()
                {
                    new GarmentFinishingInItemReadModel(Guid.NewGuid())
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
            var unitUnderTest = CreateGarmentFinishingInController();

            _mockFinishingInRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingInReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingIn>()
                {
                    new GarmentFinishingIn(Guid.NewGuid(), null,null , new UnitDepartmentId(1), null, null, "RONo",null,new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1),null, null, 0,null, null)
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingInItem>()
                {
                    new GarmentFinishingInItem(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid(),Guid.NewGuid(), Guid.Empty,new SizeId(1), null, new ProductId(1), null, null, null, 1,1,new UomId(1),null, null,1,1)
                });

            //_mockSewingOutItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingOutItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingIn(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentFinishingInCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentFinishingInCommand>()));
        }

        /*[Fact]
        public async Task GetComplete_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingInController();
            var id = Guid.NewGuid();
            _mockFinishingInRepository
               .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns(new List<GarmentFinishingInReadModel>()
               {
                   new GarmentFinishingInReadModel(id)
               }
               .AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingInReadModel>>()))
                .Returns(new List<GarmentFinishingIn>()
                {
                    new GarmentFinishingIn(id,"finishingInNo","finishingInType" , new UnitDepartmentId(1),"unitFromCode", "unitFromName", "RONo","article",new UnitDepartmentId(1),"unitCode", "unitName", DateTimeOffset.Now, new GarmentComodityId(1),"comodityCode","comodityName", 0,"doNo", null)
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>()
                {
                    new GarmentFinishingInItemReadModel(id)
                }.AsQueryable());

            _mockFinishingInItemRepository
               .Setup(s => s.Find(It.IsAny<IQueryable<GarmentFinishingInItemReadModel>>()))
               .Returns(new List<GarmentFinishingInItem>()
               {
                    new GarmentFinishingInItem(id, id,id,id, id,new SizeId(1),"sizeName", new ProductId(1),"productCode", "productName","designColor", 1,1,new UomId(1),"uomUnit","color",1,1)
               });
            // Act

            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingIn(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null,null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentFinishingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentFinishingInCommand command = new UpdateDatesGarmentFinishingInCommand(ids, DateTimeOffset.Now, "CUTTING");
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
            var unitUnderTest = CreateGarmentFinishingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentFinishingInCommand command = new UpdateDatesGarmentFinishingInCommand(ids, DateTimeOffset.Now.AddDays(3), "CUTTING");

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentFinishingInCommand command2 = new UpdateDatesGarmentFinishingInCommand(ids, DateTimeOffset.MinValue, "CUTTING");

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }
    }
}
