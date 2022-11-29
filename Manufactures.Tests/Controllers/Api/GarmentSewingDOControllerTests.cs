using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Commands;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingDOs.ValueObjects;
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
    public class GarmentSewingDOControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSewingDORepository> _mockGarmentSewingDORepository;
        private Mock<IGarmentSewingDOItemRepository> _mockGarmentSewingDOItemRepository;

        public GarmentSewingDOControllerTests() : base()
        {
            _mockGarmentSewingDORepository = CreateMock<IGarmentSewingDORepository>();
            _mockGarmentSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSewingDORepository);
            _MockStorage.SetupStorage(_mockGarmentSewingDOItemRepository);
        }

        private GarmentSewingDOController CreateGarmentSewingDOController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSewingDOController controller = (GarmentSewingDOController)Activator.CreateInstance(typeof(GarmentSewingDOController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSewingDOController();
            var id = Guid.NewGuid();
            _mockGarmentSewingDORepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSewingDOReadModel>().AsQueryable());

            _mockGarmentSewingDORepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingDOReadModel>>()))
                .Returns(new List<GarmentSewingDO>()
                {
                    new GarmentSewingDO(id,"sewingDONo",id,new UnitDepartmentId(1),"unitFromCode","unitFromName",new UnitDepartmentId(1),"unitCode","unitName","roNo","article",new GarmentComodityId(1),"comodityCode","comodityName",DateTimeOffset.Now)
                });

            _mockGarmentSewingDOItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOItemReadModel>()
                {
                    new GarmentSewingDOItemReadModel(id)
                }.AsQueryable());


            // Act
            var orderData = new
            {
                article = "desc",
            };
            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1,25, order,new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetBy_id()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingDOController();
            var id = Guid.NewGuid();

            _mockGarmentSewingDORepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOReadModel, bool>>>()))
               .Returns(new List<GarmentSewingDO>()
               {
                    new GarmentSewingDO(id,"sewingDONo",id,new UnitDepartmentId(1),"unitFromCode","unitFromName",new UnitDepartmentId(1),"unitCode","unitName","roNo","article",new GarmentComodityId(1),"comodityCode","comodityName",DateTimeOffset.Now)
               });


            _mockGarmentSewingDOItemRepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOItemReadModel,bool>>>()))
              .Returns(new List<GarmentSewingDOItem>()
              {
                    new GarmentSewingDOItem(id,id,id,id,new ProductId(1),"productCode","productName","designColor",new SizeId(1),"sizeName",1,new UomId(1),"uomUnit","color",1,1,1)
              });

            var result = await unitUnderTest.Get(id.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetByCutOutId_return_OK()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingDOController();
            var id = Guid.NewGuid();

            _mockGarmentSewingDORepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOReadModel,bool>>>()))
              .Returns(new List<GarmentSewingDO>()
              {
                    new GarmentSewingDO(id,"sewingDONo",id,new UnitDepartmentId(1),"unitFromCode","unitFromName",new UnitDepartmentId(1),"unitCode","unitName","roNo","article",new GarmentComodityId(1),"comodityCode","comodityName",DateTimeOffset.Now)
              });

            _mockGarmentSewingDOItemRepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingDOItemReadModel, bool>>>()))
              .Returns(new List<GarmentSewingDOItem>()
              {
                    new GarmentSewingDOItem(id,id,id,id,new ProductId(1),"productCode","productName","designColor",new SizeId(1),"sizeName",1,new UomId(1),"uomUnit","color",1,1,1)
              });


            var result = await unitUnderTest.GetByCutOutId(id.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
            
        }

        /*[Fact]
        public async Task GetComplete_return_OK()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSewingDOController();
            var id = Guid.NewGuid();
            _mockGarmentSewingDORepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSewingDOReadModel>().AsQueryable());


            _mockGarmentSewingDORepository
               .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingDOReadModel>>()))
               .Returns(new List<GarmentSewingDO>()
               {
                    new GarmentSewingDO(id,"sewingDONo",id,new UnitDepartmentId(1),"unitFromCode","unitFromName",new UnitDepartmentId(1),"unitCode","unitName","roNo","article",new GarmentComodityId(1),"comodityCode","comodityName",DateTimeOffset.Now)
               });


            _mockGarmentSewingDOItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOItemReadModel>()
                {
                    new GarmentSewingDOItemReadModel(id)
                }.AsQueryable());

            _mockGarmentSewingDOItemRepository
             .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSewingDOItemReadModel>>()))
             .Returns(new List<GarmentSewingDOItem>()
             {
                    new GarmentSewingDOItem(id,id,id,id,new ProductId(1),"productCode","productName","designColor",new SizeId(1),"sizeName",1,new UomId(1),"uomUnit","color",1,1,1)
             });


            // Act
            var orderData = new
            {
                article = "desc",
            };
            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1,25,order,new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));

        }*/

        }
}