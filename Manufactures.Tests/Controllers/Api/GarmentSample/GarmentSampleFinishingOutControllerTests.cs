using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleFinishingOuts.Queries;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
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
    public class GarmentSampleFinishingOutControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentSampleFinishingOutRepository> _mockGarmentSampleFinishingOutRepository;
        private readonly Mock<IGarmentSampleFinishingOutItemRepository> _mockGarmentSampleFinishingOutItemRepository;
        private readonly Mock<IGarmentSampleFinishingOutDetailRepository> _mockGarmentSampleFinishingOutDetailRepository;
        private readonly Mock<IGarmentSampleFinishingInItemRepository> _mockFinishingInItemRepository;

        public GarmentSampleFinishingOutControllerTests() : base()
        {
            _mockGarmentSampleFinishingOutRepository = CreateMock<IGarmentSampleFinishingOutRepository>();
            _mockGarmentSampleFinishingOutItemRepository = CreateMock<IGarmentSampleFinishingOutItemRepository>();
            _mockGarmentSampleFinishingOutDetailRepository = CreateMock<IGarmentSampleFinishingOutDetailRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentSampleFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleFinishingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleFinishingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleFinishingOutDetailRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
        }

        private GarmentSampleFinishingOutController CreateGarmentSampleFinishingOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleFinishingOutController controller = (GarmentSampleFinishingOutController)Activator.CreateInstance(typeof(GarmentSampleFinishingOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSampleFinishingOutController();

            _mockGarmentSampleFinishingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleFinishingOutReadModel>().AsQueryable());

            Guid finishingOutGuid = Guid.NewGuid();
            _mockGarmentSampleFinishingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleFinishingOutReadModel>>()))
                .Returns(new List<GarmentSampleFinishingOut>()
                {
                    new GarmentSampleFinishingOut(finishingOutGuid, null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            GarmentSampleFinishingOutItem GarmentSampleFinishingOutItem = new GarmentSampleFinishingOutItem(finishingOutItemGuid, finishingOutGuid, finishingInGuid, finishingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1, 1, 1);
            _mockGarmentSampleFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutItemReadModel>()
                {
                    GarmentSampleFinishingOutItem.GetReadModel()
                }.AsQueryable());

            GarmentSampleFinishingOutDetail GarmentSampleFinishingOutDetail = new GarmentSampleFinishingOutDetail(Guid.NewGuid(), finishingOutItemGuid, new SizeId(1), null, 1, new UomId(1), null);
            _mockGarmentSampleFinishingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutDetailReadModel>()
                {
                    GarmentSampleFinishingOutDetail.GetReadModel()
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
            var unitUnderTest = CreateGarmentSampleFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _mockGarmentSampleFinishingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOut>()
                {
                    new GarmentSampleFinishingOut(finishingOutGuid, null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            _mockGarmentSampleFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutItem>()
                {
                    new GarmentSampleFinishingOutItem(finishingOutItemGuid, finishingOutGuid, finishingInGuid, finishingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1,1,1)
                });

            _mockGarmentSampleFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutDetail>()
                {
                    new GarmentSampleFinishingOutDetail(Guid.NewGuid(), finishingOutItemGuid, new SizeId(1), null, 1, new UomId(1), null)
                });

            //_mockFinishingInItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSampleFinishingInItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _mockGarmentSampleFinishingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOut>()
                {
                    new GarmentSampleFinishingOut(finishingOutGuid, null,new UnitDepartmentId(1),null,null,"Finishing",DateTimeOffset.Now, "RONo", "art", new UnitDepartmentId(1), null, null,new GarmentComodityId(1),null,null,true)
                });

            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingInGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            _mockGarmentSampleFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutItem>()
                {
                    new GarmentSampleFinishingOutItem(finishingOutItemGuid, finishingOutGuid, finishingInGuid, finishingInItemGuid, new ProductId(1), null, null, "design", new SizeId(1), "size", 1, new UomId(1), null, "color", 1,1,1)
                });

            _mockGarmentSampleFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutDetail>()
                {
                    new GarmentSampleFinishingOutDetail(Guid.NewGuid(), finishingOutItemGuid, new SizeId(1), "size", 1, new UomId(1), null)
                });
            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString(), "buyerCode");

            // Assert
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleFinishingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleFinishingOut(finishingOutGuid, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleFinishingOutCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleFinishingOutCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());


            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSampleFinishingOutCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSampleFinishingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleFinishingOut(finishingOutGuid, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSampleFinishingOutCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleFinishingOutController();
            Guid finishingOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleFinishingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleFinishingOut(finishingOutGuid, null, new UnitDepartmentId(1), null, null, "Finishing", DateTimeOffset.Now, "RONo", null, new UnitDepartmentId(1), null, null, new GarmentComodityId(1), null, null, true));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
        

        //[Fact]
        //public async Task GetColor_Return_Exception()
        //{
        //    var unitUnderTest = CreateGarmentSampleFinishingOutController();
        //    var id = Guid.NewGuid();
        //    _mockGarmentSampleFinishingOutRepository
        //       .Setup(s => s.ReadColor(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //       .Returns(new List<GarmentSampleFinishingOutReadModel>() { new GarmentSampleFinishingOutReadModel(id) }.AsQueryable());

        //    _mockGarmentSampleFinishingOutRepository
        //       .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleFinishingOutReadModel>>()))
        //       .Returns(new List<GarmentSampleFinishingOut>()
        //       {
        //            new GarmentSampleFinishingOut(id,"finishingOutNo",new UnitDepartmentId(1),"unitToCode","unitToName","Finishing",DateTimeOffset.Now, "RONo","article", new UnitDepartmentId(1),"unitCode", "unitName",new GarmentComodityId(1),"comodityCode","comodityName",true)
        //       });

        //    _mockGarmentSampleFinishingOutItemRepository
        //        .Setup(s => s.Query)
        //        .Returns(new List<GarmentSampleFinishingOutItemReadModel>()
        //        {
        //           new GarmentSampleFinishingOutItemReadModel(id)
        //        }.AsQueryable());

        //    // Act
        //    var orderData = new
        //    {
        //        Name = "desc",
        //    };

        //    string order = JsonConvert.SerializeObject(orderData);
        //    var result = await unitUnderTest.GetColor(1, 25, order, new List<string>(), "", "{}");

        //    // Assert
        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));

        //}

        [Fact]
        public async Task GetComplete_Success()
        {
            var unitUnderTest = CreateGarmentSampleFinishingOutController();

            var id = Guid.NewGuid();
            _mockGarmentSampleFinishingOutRepository
                 .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                 .Returns(new List<GarmentSampleFinishingOutReadModel>().AsQueryable());

            
            _mockGarmentSampleFinishingOutRepository
                .Setup(s => s.ReadExecute(It.IsAny<IQueryable<GarmentSampleFinishingOutReadModel>>()))
                .Returns(new List<object>()
                .AsQueryable());

            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
        
        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleFinishingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleFinishingOutCommand command = new UpdateDatesGarmentSampleFinishingOutCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentSampleFinishingOutController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentSampleFinishingOutCommand command = new UpdateDatesGarmentSampleFinishingOutCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentSampleFinishingOutCommand command2 = new UpdateDatesGarmentSampleFinishingOutCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }

        [Fact]
        public async Task GetMonitoringBehavior()
        {
            var unitUnderTest = CreateGarmentSampleFinishingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetSampleFinishingMonitoringQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleFinishingMonitoringListViewModel());

            // Act
            var result = await unitUnderTest.GetMonitoring(1, DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentSampleFinishingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleFinishingQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
            ;
        }

        [Fact]
        public async Task GetXLS_Throws_Exception()
        {
            var unitUnderTest = CreateGarmentSampleFinishingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsSampleFinishingQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Assert
            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }
    }
}
