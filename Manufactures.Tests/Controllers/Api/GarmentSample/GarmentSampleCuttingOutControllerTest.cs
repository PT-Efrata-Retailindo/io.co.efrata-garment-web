using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries;
using Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.Monitoring;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
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
    public class GarmentSampleCuttingOutControllerTest : BaseControllerUnitTest
    {
        private Mock<IGarmentSampleCuttingOutRepository> _mockGarmentSampleCuttingOutRepository;
        private Mock<IGarmentSampleCuttingOutItemRepository> _mockGarmentSampleCuttingOutItemRepository;
        private Mock<IGarmentSampleCuttingOutDetailRepository> _mockGarmentSampleCuttingOutDetailRepository;
        private Mock<IGarmentSampleSewingInRepository> _mockGarmentSampleSewingInRepository;
        private Mock<IGarmentSampleSewingInItemRepository> _mockGarmentSampleSewingInItemRepository;

        public GarmentSampleCuttingOutControllerTest() : base()
        {
            _mockGarmentSampleCuttingOutRepository = CreateMock<IGarmentSampleCuttingOutRepository>();
            _mockGarmentSampleCuttingOutItemRepository = CreateMock<IGarmentSampleCuttingOutItemRepository>();
            _mockGarmentSampleCuttingOutDetailRepository = CreateMock<IGarmentSampleCuttingOutDetailRepository>();
            _mockGarmentSampleSewingInRepository = CreateMock<IGarmentSampleSewingInRepository>();
            _mockGarmentSampleSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleCuttingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleSewingInRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleSewingInItemRepository);
        }

        private GarmentSampleCuttingOutController CreateGarmentSampleCuttingOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleCuttingOutController controller = (GarmentSampleCuttingOutController)Activator.CreateInstance(typeof(GarmentSampleCuttingOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllSampleCuttingOutQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SampleCuttingOutListViewModel
                {
                    data = new List<GarmentSampleCuttingOutListDto>()
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
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllSampleCuttingOutQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SampleCuttingOutListViewModel
                {
                    data = new List<GarmentSampleCuttingOutListDto>()
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
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllSampleCuttingOutQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SampleCuttingOutListViewModel
                {
                    data = new List<GarmentSampleCuttingOutListDto>()
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
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetAllSampleCuttingOutQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SampleCuttingOutListViewModel
                {
                    data = new List<GarmentSampleCuttingOutListDto>()
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
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _mockGarmentSampleCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingOut>()
                {
                    new GarmentSampleCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null,false)
                });

            Guid cuttingOutItemGuid = Guid.NewGuid();
            _mockGarmentSampleCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingOutItem>()
                {
                    new GarmentSampleCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null, 1)
                });

            _mockGarmentSampleCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingOutDetail>()
                {
                    new GarmentSampleCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), null, null, 1, 1, new UomId(1), null, 1, 1)
                });

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _mockGarmentSampleCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingOut>()
                {
                    new GarmentSampleCuttingOut(cuttingOutGuid,"cutOutNo", null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", "art", new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null,false)
                });

            Guid cuttingOutItemGuid = Guid.NewGuid();
            _mockGarmentSampleCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingOutItem>()
                {
                    new GarmentSampleCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1),"productCode", "productName", "design", 1)
                });

            _mockGarmentSampleCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleCuttingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleCuttingOutDetail>()
                {
                    new GarmentSampleCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), "size", "color", 1, 1, new UomId(1), "uom", 1, 1)
                });

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString(), "buyerCode");

            // Assert
            //Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, false));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleCuttingOutCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSampleCuttingOutCommand>()));
        }

        //[Fact]
        //public async Task Put_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentSampleCuttingOutController();

        //    Guid cuttingOutGuid = Guid.NewGuid();
        //    _MockMediator
        //        .Setup(s => s.Send(It.IsAny<UpdateGarmentSampleCuttingOutCommand>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(new GarmentSampleCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, false));

        //    // Act
        //    var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSampleCuttingOutCommand());

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        [Fact]
        public async Task Delete_Return_BadRequest()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();

            _mockGarmentSampleSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInReadModel>()
                {
                    new GarmentSampleSewingIn(Guid.NewGuid(), null, "CUTTING", cuttingOutGuid, null, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleSewingInItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingInItemReadModel, bool>>>()))
               .Returns(new List<GarmentSampleSewingInItem>()
               {
                    new GarmentSampleSewingInItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1),"producCode","productName","designColor",new SizeId(1),"sizeName",5,new UomId(1),"uomUnit","color",1,1,1)
               });


            // Act
            var result = await unitUnderTest.Delete(cuttingOutGuid.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();

            _mockGarmentSampleSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInReadModel>()
                {
                    new GarmentSampleSewingIn(Guid.NewGuid(), null, "CUTTING", cuttingOutGuid,null, new UnitDepartmentId(1), null, null, new UnitDepartmentId(1), null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now).GetReadModel()
                }.AsQueryable());

            _mockGarmentSampleSewingInItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingInItemReadModel, bool>>>()))
               .Returns(new List<GarmentSampleSewingInItem>()
               {
                    new GarmentSampleSewingInItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),Guid.Empty,Guid.Empty, new ProductId(1),"producCode","productName","designColor",new SizeId(1),"sizeName",1,new UomId(1),"uomUnit","color",1,1,1)
               });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, false));

            // Act
            var result = await unitUnderTest.Delete(cuttingOutGuid.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        
        [Fact]
        public async Task GetComplete_ExpectedBehavior()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSampleCuttingOutController();
            _mockGarmentSampleCuttingOutRepository
                 .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(new List<GarmentSampleCuttingOutReadModel>() { new GarmentSampleCuttingOutReadModel(id) }.AsQueryable());

            _mockGarmentSampleCuttingOutRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentSampleCuttingOutReadModel>>()))
                .Returns(new List<object>()
                .AsQueryable());

            // Act
            var orderData = new
            {
                CuttingOutType = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetMonitoringBehavior()
        {
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetSampleCuttingMonitoringQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleCuttingMonitoringViewModel());

            // Act
            var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetXLSBehavior_Throws_Exception()
        {
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleCuttingQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());
            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.InternalServerError);

        }

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentSampleCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleCuttingQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleCuttingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleCuttingOutCommand command = new UpdateDatesGarmentSampleCuttingOutCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentSampleCuttingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleCuttingOutCommand command = new UpdateDatesGarmentSampleCuttingOutCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentSampleCuttingOutCommand command2 = new UpdateDatesGarmentSampleCuttingOutCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }
    }
}
