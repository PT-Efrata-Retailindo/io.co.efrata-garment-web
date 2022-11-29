using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Commands;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentReturGoodReturns.Commands;
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
    public class GarmentExpenditureGoodReturnControllerTest : BaseControllerUnitTest
    {

        private readonly Mock<IGarmentExpenditureGoodReturnRepository> _mockGarmentExpenditureGoodReturnRepository;
        private readonly Mock<IGarmentExpenditureGoodReturnItemRepository> _mockGarmentExpenditureGoodReturnItemRepository;

        public GarmentExpenditureGoodReturnControllerTest() : base()
        {
            _mockGarmentExpenditureGoodReturnRepository = CreateMock<IGarmentExpenditureGoodReturnRepository>();
            _mockGarmentExpenditureGoodReturnItemRepository = CreateMock<IGarmentExpenditureGoodReturnItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentExpenditureGoodReturnRepository);
            _MockStorage.SetupStorage(_mockGarmentExpenditureGoodReturnItemRepository);
        }

        private GarmentExpenditureGoodReturnController CreateGarmentExpenditureGoodReturnController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentExpenditureGoodReturnController controller = (GarmentExpenditureGoodReturnController)Activator.CreateInstance(typeof(GarmentExpenditureGoodReturnController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentExpenditureGoodReturnController();
            var id = Guid.NewGuid();

            _mockGarmentExpenditureGoodReturnRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReturnReadModel>().AsQueryable());

            _mockGarmentExpenditureGoodReturnRepository
               .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReturnReadModel>>()))
               .Returns(new List<GarmentExpenditureGoodReturn>()
               {
                    new GarmentExpenditureGoodReturn(id,"returNo","returType","expenditureno","dono","urnno","bcno","bctype",new UnitDepartmentId(1),"unitCode","unitName","roNo","article",new GarmentComodityId(1),"comodityCode","comodityName",new BuyerId(1),"buyerCode","buyerName",DateTimeOffset.Now,"invoice","returDesc")
               });

            GarmentExpenditureGoodReturnItem goodReturnItem = new GarmentExpenditureGoodReturnItem(id, id, id, id, id, new SizeId(1), "sizeName", 1, new UomId(1), "uomUnit", "description", 1, 1);

            _mockGarmentExpenditureGoodReturnItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReturnItemReadModel>()
                {
                    goodReturnItem.GetReadModel()
                }.AsQueryable());

            // Act
            var result = await unitUnderTest.Get(1,25,"{}",new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetById()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodReturnController();
            var id = Guid.NewGuid();

            _mockGarmentExpenditureGoodReturnRepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodReturnReadModel,bool>>>()))
              .Returns(new List<GarmentExpenditureGoodReturn>()
              {
                    new GarmentExpenditureGoodReturn(id,"returNo","returType","expenditureno","dono","urnno","bcno","bctype",new UnitDepartmentId(1),"unitCode","unitName","roNo","article",new GarmentComodityId(1),"comodityCode","comodityName",new BuyerId(1),"buyerCode","buyerName",DateTimeOffset.Now,"invoice","returDesc")
              });

            _mockGarmentExpenditureGoodReturnItemRepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodReturnItemReadModel,bool>>>()))
              .Returns(new List<GarmentExpenditureGoodReturnItem>()
              {
                   new GarmentExpenditureGoodReturnItem(id,id,id,id,id,new SizeId(1),"sizename",1,new UomId(1),"uomUnit","description",1,1)
              });

            // Act
            var result = await unitUnderTest.Get(id.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));

        }


        [Fact]
        public async Task Post_Return_OK()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodReturnController();
            Guid id = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentExpenditureGoodReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new  GarmentExpenditureGoodReturn(id, "returNo", "returType", "expenditureno", "dono", "urnno", "bcno", "bctype", new UnitDepartmentId(1), "unitCode", "unitName", "roNo", "article", new GarmentComodityId(1), "comodityCode", "comodityName", new BuyerId(1), "buyerCode", "buyerName", DateTimeOffset.Now, "invoice", "returDesc"));

            // Act
            var command = new PlaceGarmentExpenditureGoodReturnCommand();
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodReturnController();
            Guid id = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentExpenditureGoodReturnCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var command = new PlaceGarmentExpenditureGoodReturnCommand();

            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(command));
        }

        [Fact]
        public async Task Delete_Return_OK()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodReturnController();
            Guid id = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentExpenditureGoodReturnCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentExpenditureGoodReturn(id, "returNo", "returType", "expenditureno", "dono", "urnno", "bcno", "bctype", new UnitDepartmentId(1), "unitCode", "unitName", "roNo", "article", new GarmentComodityId(1), "comodityCode", "comodityName", new BuyerId(1), "buyerCode", "buyerName", DateTimeOffset.Now, "invoice", "returDesc"));

            // Act
        
            var result = await unitUnderTest.Delete(id.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentExpenditureGoodReturnController();

            var id = Guid.NewGuid();

            _mockGarmentExpenditureGoodReturnRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentExpenditureGoodReturnReadModel>().AsQueryable());

            _mockGarmentExpenditureGoodReturnRepository
              .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReturnReadModel>>()))
              .Returns(new List<GarmentExpenditureGoodReturn>()
              {
                    new GarmentExpenditureGoodReturn(id,"returNo","returType","expenditureno","dono","urnno","bcno","bctype",new UnitDepartmentId(1),"unitCode","unitName","roNo","article",new GarmentComodityId(1),"comodityCode","comodityName",new BuyerId(1),"buyerCode","buyerName",DateTimeOffset.Now,"invoice","returDesc")
              });

            _mockGarmentExpenditureGoodReturnItemRepository
              .Setup(s => s.Find(It.IsAny<IQueryable<GarmentExpenditureGoodReturnItemReadModel>>()))
              .Returns(new List<GarmentExpenditureGoodReturnItem>()
              {
                   new GarmentExpenditureGoodReturnItem(id,id,id,id,id,new SizeId(1),"sizename",1,new UomId(1),"uomUnit","description",1,1)
              });

            GarmentExpenditureGoodReturnItem goodReturnItem = new GarmentExpenditureGoodReturnItem(id, id, id, id, id, new SizeId(1), "sizeName", 1, new UomId(1), "uomUnit", "description", 1, 1);

            _mockGarmentExpenditureGoodReturnItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReturnItemReadModel>()
                {
                    goodReturnItem.GetReadModel()
                }.AsQueryable());


            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));


        }
        }
}
