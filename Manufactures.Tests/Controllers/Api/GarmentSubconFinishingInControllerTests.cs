using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSubconFinishingIns.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentSubconFinishingInControllerTests : BaseControllerUnitTest
    {

        private Mock<IGarmentFinishingInRepository> _mockGarmentFinishingInRepository;
        private Mock<IGarmentFinishingInItemRepository> _mockGarmentFinishingItemRepository;        

        public GarmentSubconFinishingInControllerTests() : base()
        {
            _mockGarmentFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
            _mockGarmentFinishingItemRepository = CreateMock<IGarmentFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentFinishingInRepository);
            _MockStorage.SetupStorage(_mockGarmentFinishingItemRepository);
        }

        private GarmentSubconFinishingInController CreateGarmentSubconFinishingInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconFinishingInController controller = new GarmentSubconFinishingInController(_MockServiceProvider.Object);
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
        public async Task Post_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingIn(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSubconFinishingInCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            //Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconFinishingInCommand>()));
        }

        [Fact]
        public async Task Delete_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconFinishingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconFinishingInCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentFinishingIn(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, "RONo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconFinishingInController();

            Guid finInGuid = Guid.NewGuid();
            _mockGarmentFinishingInRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingInReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingIn>()
                {
                    new GarmentFinishingIn(finInGuid, "finInNo", null, new UnitDepartmentId(1), null, null, "RONo", "art", new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, 0, null, null)
                });

            Guid finInItemGuid = Guid.NewGuid();
            _mockGarmentFinishingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingInItem>()
                {
                    new GarmentFinishingInItem(finInItemGuid, finInGuid, Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),new SizeId(1), "sizename", new ProductId(1),"productCode", "productName", "design", 1, 1, new UomId(1), "PCS", "Black", 0, 0)
                });

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString(), "buyerCode");

            // Assert
            //Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }
    }
}
