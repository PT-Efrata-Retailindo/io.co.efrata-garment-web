using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.MonitoringSewing;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class GarmentSampleSewingOutControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockGarmentSewingOutRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockGarmentSewingOutItemRepository;
        private readonly Mock<IGarmentSampleSewingOutDetailRepository> _mockGarmentSewingOutDetailRepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingInItemRepository;

        public GarmentSampleSewingOutControllerTests() : base()
        {
            _mockGarmentSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
            _mockGarmentSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
            _mockGarmentSewingOutDetailRepository = CreateMock<IGarmentSampleSewingOutDetailRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSewingOutDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
        }

        private GarmentSampleSewingOutController CreateGarmentSewingOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleSewingOutController controller = (GarmentSampleSewingOutController)Activator.CreateInstance(typeof(GarmentSampleSewingOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSewingOutController();

            _mockGarmentSewingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleSewingOutReadModel>().AsQueryable());

            Guid sewingOutGuid = Guid.NewGuid();
            _mockGarmentSewingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleSewingOutReadModel>>()))
                .Returns(new List<GarmentSampleSewingOut>()
                {
                    new GarmentSampleSewingOut(sewingOutGuid, null,new BuyerId(1),null,null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            GarmentSampleSewingOutItem garmentSewingOutItem = new GarmentSampleSewingOutItem(sewingOutItemGuid, sewingOutGuid, sewingInGuid, sewingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1, 1, 1);
            _mockGarmentSewingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutItemReadModel>()
                {
                    garmentSewingOutItem.GetReadModel()
                }.AsQueryable());

            GarmentSampleSewingOutDetail garmentSewingOutDetail = new GarmentSampleSewingOutDetail(Guid.NewGuid(), sewingOutItemGuid, new SizeId(1), null, 1, new UomId(1), null);
            _mockGarmentSewingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutDetailReadModel>()
                {
                    garmentSewingOutDetail.GetReadModel()
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
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _mockGarmentSewingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOut>()
                {
                    new GarmentSampleSewingOut(sewingOutGuid, null,new BuyerId(1),null,null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            _mockGarmentSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOutItem>()
                {
                    new GarmentSampleSewingOutItem(sewingOutItemGuid, sewingOutGuid, sewingInGuid, sewingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1,1,1)
                });

            _mockGarmentSewingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOutDetail>()
                {
                    new GarmentSampleSewingOutDetail(Guid.NewGuid(), sewingOutItemGuid, new SizeId(1), null, 1, new UomId(1), null)
                });

            //_mockSewingInItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingInItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _mockGarmentSewingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOut>()
                {
                    new GarmentSampleSewingOut(sewingOutGuid, null,new BuyerId(1),null,null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", "art", new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingInGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            _mockGarmentSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOutItem>()
                {
                    new GarmentSampleSewingOutItem(sewingOutItemGuid, sewingOutGuid, sewingInGuid, sewingInItemGuid, new ProductId(1), null, null, "design", new SizeId(1), "size", 1, new UomId(1), null, "color", 1,1,1)
                });

            _mockGarmentSewingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingOutDetail>()
                {
                    new GarmentSampleSewingOutDetail(Guid.NewGuid(), sewingOutItemGuid, new SizeId(1), "size", 1, new UomId(1), null)
                });

            //_mockSewingInItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingInItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString(), "buyerCode");

            // Assert
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleSewingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleSewingOut(sewingOutGuid, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleSewingOutCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleSewingOutCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSampleSewingOutCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSampleSewingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleSewingOut(sewingOutGuid, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSampleSewingOutCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleSewingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleSewingOut(sewingOutGuid, null, new BuyerId(1), null, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }



        [Fact]
        public async Task GetMonitoringBehavior()
        {
            var unitUnderTest = CreateGarmentSewingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetMonitoringSampleSewingQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentMonitoringSampleSewingListViewModel());

            // Act
            var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid id = Guid.NewGuid();
            _mockGarmentSewingOutRepository
              .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new List<GarmentSampleSewingOutReadModel>().AsQueryable());


            _mockGarmentSewingOutRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentSampleSewingOutReadModel>>()))
                .Returns(new List<object>()
                .AsQueryable());

            // Act
            var orderData = new
            {
                article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetRoByQuery_Return_Success()
        {
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid id = Guid.NewGuid();

            _mockGarmentSewingOutRepository
              .Setup(s => s.ReadComplete(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
              .Returns(new List<GarmentSampleSewingOutReadModel>().AsQueryable());


            _mockGarmentSewingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleSewingOutReadModel>>()))
                .Returns(new List<GarmentSampleSewingOut>()
                {
                    new GarmentSampleSewingOut(id, null,new BuyerId(1),null,null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            // Act
            var orderData = new
            {
                article = "desc"
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetByRo(1, 25, order, new List<string>(), "", "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }


        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentSewingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleSewingQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }
        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleSewingOutCommand command = new UpdateDatesGarmentSampleSewingOutCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentSewingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleSewingOutCommand command = new UpdateDatesGarmentSampleSewingOutCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentSampleSewingOutCommand command2 = new UpdateDatesGarmentSampleSewingOutCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }

    }
}
