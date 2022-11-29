using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
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
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api.GarmentSample
{
    public class GarmentSampleFinishedGoodStockControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentSampleFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        public GarmentSampleFinishedGoodStockControllerTests() : base()
        {
            _mockFinishedGoodStockRepository = CreateMock<IGarmentSampleFinishedGoodStockRepository>();

            _MockStorage.SetupStorage(_mockFinishedGoodStockRepository);

        }

        private GarmentSampleFinishedGoodStockController CreateGarmentSampleFinishedGoodStockController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSampleFinishedGoodStockController controller = (GarmentSampleFinishedGoodStockController)Activator.CreateInstance(typeof(GarmentSampleFinishedGoodStockController), _MockServiceProvider.Object);
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

        //[Fact]
        //public async Task Get_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentSampleFinishedGoodStockController();

        //    _mockFinishedGoodStockRepository
        //        .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(new List<GarmentSampleFinishedGoodStockReadModel>().AsQueryable());

        //    _mockFinishedGoodStockRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleFinishedGoodStockReadModel>>()))
        //        .Returns(new List<GarmentSampleFinishedGoodStock>()
        //        {
        //            new GarmentSampleFinishedGoodStock(Guid.NewGuid(),"","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.SizeId(1),"",new Domain.Shared.ValueObjects.UomId(1),"",10,10,10)
        //        });

        //    // Act
        //    var result = await unitUnderTest.Get();

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}


        [Fact]
        public async Task GetList_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSampleFinishedGoodStockController();
            _mockFinishedGoodStockRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleFinishedGoodStockReadModel>().AsQueryable());

            _mockFinishedGoodStockRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleFinishedGoodStockReadModel>>()))
                .Returns(new List<GarmentSampleFinishedGoodStock>()
                {
                    new GarmentSampleFinishedGoodStock(id,"","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.SizeId(1),"",new Domain.Shared.ValueObjects.UomId(1),"",10,10,10)
                });
            var orderData = new
            {
                UnitName = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetList(1, 25, order, new List<string>(), "", "{}");
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentSampleFinishedGoodStockController();

        //    _mockFinishedGoodStockRepository
        //        .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(new List<GarmentSampleFinishedGoodStockReadModel>().AsQueryable());

        //    _mockFinishedGoodStockRepository
        //        .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleFinishedGoodStockReadModel>>()))
        //        .Returns(new List<GarmentSampleFinishedGoodStock>()
        //        {
        //            new GarmentSampleFinishedGoodStock(Guid.NewGuid(),"","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.SizeId(1),"",new Domain.Shared.ValueObjects.UomId(1),"",10,10,10)
        //        });

        //    // Act

        //    var orderData = new
        //    {
        //        UnitName = "desc",
        //    };

        //    string order = JsonConvert.SerializeObject(orderData);
        //    var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}

        //[Fact]
        //public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        //{
        //    var unitUnderTest = CreateGarmentSampleFinishedGoodStockController();
        //    Guid identity = Guid.NewGuid();


        //    _mockFinishedGoodStockRepository
        //       .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishedGoodStockReadModel, bool>>>()))
        //       .Returns(new List<GarmentSampleFinishedGoodStock>()
        //       {
        //            new GarmentSampleFinishedGoodStock(identity,"","","",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"","",new Domain.Shared.ValueObjects.GarmentComodityId(1),"","",new Domain.Shared.ValueObjects.SizeId(1),"",new Domain.Shared.ValueObjects.UomId(1),"",10,10,10)
        //       });
        //    var result = await unitUnderTest.Get(identity.ToString());

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //}
    }
}
