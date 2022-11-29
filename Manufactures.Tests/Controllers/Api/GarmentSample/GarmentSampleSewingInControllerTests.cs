using Barebone.Tests;
using Manufactures.Application.GarmentSample.SampleSewingIns.Queries;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
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

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentSampleSewingInControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSampleSewingInRepository> _mockGarmentSewingInRepository;
        private Mock<IGarmentSampleSewingInItemRepository> _mockGarmentSewingInItemRepository;

        public GarmentSampleSewingInControllerTests() : base()
        {
            _mockGarmentSewingInRepository = CreateMock<IGarmentSampleSewingInRepository>();
            _mockGarmentSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSewingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingInItemRepository);
        }

        private GarmentSampleSewingInController CreateGarmentSewingInController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleSewingInController controller = (GarmentSampleSewingInController)Activator.CreateInstance(typeof(GarmentSampleSewingInController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSewingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllSampleSewingInQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SampleSewingInListViewModel
                {
                    data = new List<GarmentSampleSewingInListDto>()
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Get_with_order()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllSampleSewingInQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SampleSewingInListViewModel
                {
                    data = new List<GarmentSampleSewingInListDto>()
                });

            // Act
            var orderData = new
            {
                article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1, 1, order, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Get_with_Keyword_and_Order()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllSampleSewingInQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SampleSewingInListViewModel
                {
                    data = new List<GarmentSampleSewingInListDto>()
                });

            // Act
            var orderData = new
            {
                article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1, 25, order, new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Get_with_Keyword_No_Order()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllSampleSewingInQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SampleSewingInListViewModel
                {
                    data = new List<GarmentSampleSewingInListDto>()
                });

            // Act
            var result = await unitUnderTest.Get(1, 25, "{}", new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();

            Guid SewingInGuid = Guid.NewGuid();
            _mockGarmentSewingInRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingInReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingIn>()
                {
                    new GarmentSampleSewingIn(SewingInGuid, null, null, Guid.NewGuid(),null, new UnitDepartmentId(1), null, null,  new UnitDepartmentId(1), null, null, null,null, new GarmentComodityId(1), null, null,DateTimeOffset.Now)
                });

            Guid SewingInItemGuid = Guid.NewGuid();
            _mockGarmentSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingInItem>()
                {
                    new GarmentSampleSewingInItem(SewingInItemGuid, SewingInGuid, Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, new SizeId(1),null, 1, new UomId(1),null,null,1,0,0)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_ExpectedBehavior()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSewingInController();
            _mockGarmentSewingInRepository
                 .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(new List<GarmentSampleSewingInReadModel>() { new GarmentSampleSewingInReadModel(id) }.AsQueryable());

            _mockGarmentSewingInRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleSewingInReadModel>>()))
                .Returns(new List<GarmentSampleSewingIn>()
                {
                    new GarmentSampleSewingIn(id, null, null, Guid.NewGuid(),null, new UnitDepartmentId(1), null, null,  new UnitDepartmentId(1), null, null, null,null, new GarmentComodityId(1), null, null,DateTimeOffset.Now)
                }
                );
            _mockGarmentSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInItemReadModel>()
                {
                    new GarmentSampleSewingInItemReadModel(id)
                }.AsQueryable());
            _mockGarmentSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleSewingInItemReadModel>>()))
                .Returns(new List<GarmentSampleSewingInItem>()
                {
                    new GarmentSampleSewingInItem(id, id, Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1), null, null, null, new SizeId(1),null, 1, new UomId(1),null,null,1,0,0)
                }
                );

            // Act
            var orderData = new
            {
                SewingInNo = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingInController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleSewingInCommand command = new UpdateDatesGarmentSampleSewingInCommand(ids, DateTimeOffset.Now);
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

            UpdateDatesGarmentSampleSewingInCommand command = new UpdateDatesGarmentSampleSewingInCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentSampleSewingInCommand command2 = new UpdateDatesGarmentSampleSewingInCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }
    }
}
