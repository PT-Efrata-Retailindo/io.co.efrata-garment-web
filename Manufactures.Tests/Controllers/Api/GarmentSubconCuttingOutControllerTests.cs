using Barebone.Tests;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Commands;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
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
    public class GarmentSubconCuttingOutControllerTests : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;
        private readonly Mock<IGarmentSubconCuttingOutItemRepository> _mockSubconCuttingOutItemRepository;
        private readonly Mock<IGarmentSubconCuttingOutDetailRepository> _mockSubconCuttingOutDetailRepository;
        private readonly Mock<IGarmentCuttingInRepository> _mockGarmentCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockGarmentCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;

        public GarmentSubconCuttingOutControllerTests() : base()
        {
            _mockSubconCuttingOutRepository = CreateMock<IGarmentSubconCuttingOutRepository>();
            _mockSubconCuttingOutItemRepository = CreateMock<IGarmentSubconCuttingOutItemRepository>();
            _mockSubconCuttingOutDetailRepository = CreateMock<IGarmentSubconCuttingOutDetailRepository>();
            _mockGarmentCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockGarmentCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();

            _MockStorage.SetupStorage(_mockSubconCuttingOutRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
        }

        private GarmentSubconCuttingOutController CreateGarmentSubconCuttingOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconCuttingOutController controller = (GarmentSubconCuttingOutController)Activator.CreateInstance(typeof(GarmentSubconCuttingOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSubconCuttingOutController();

            _mockSubconCuttingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingOutReadModel>().AsQueryable());

            Guid cuttingOutGuid = Guid.NewGuid();
            _mockSubconCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOut>()
                {
                    new GarmentSubconCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null,  new GarmentComodityId(1), null, null,1,1,null,false)
                });

            Guid cuttingOutItemGuid = Guid.NewGuid();
            GarmentSubconCuttingOutItem garmentCuttingOutItem = new GarmentSubconCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null, 1);
            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>()
                {
                    garmentCuttingOutItem.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutItemReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOutItem>()
                {
                    new GarmentSubconCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null,1)
                });

            GarmentSubconCuttingOutDetail garmentCuttingOutDetail = new GarmentSubconCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), null, null, 1, 1, new UomId(1), null, 1,  1,null);
            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>()
                {
                    garmentCuttingOutDetail.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutDetailReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOutDetail>()
                {
                    new GarmentSubconCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), null, null, 1, 1, new UomId(1), null, 1, 1,null)
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Get_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCuttingOutController();
            var id = Guid.NewGuid();
            _mockSubconCuttingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingOutReadModel>().AsQueryable());

            _mockSubconCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOut>()
                {
                    new GarmentSubconCuttingOut(id,"productCode", "cutOutType", new UnitDepartmentId(1),"unitFromCode", "unitFromName", DateTimeOffset.Now, "RONo","article",  new GarmentComodityId(1),"comodityCode", "comodityName",1,1,"remark",false)
                });

            GarmentSubconCuttingOutItem garmentCuttingOutItem = new GarmentSubconCuttingOutItem(id, id,id, id, new ProductId(1),"productCode", "productName","designColor", 1);
            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>()
                {
                    garmentCuttingOutItem.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutItemReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOutItem>()
                {
                    new GarmentSubconCuttingOutItem(id, id, id, id, new ProductId(1),"productCode", "productName","designColor",1)
                });

            GarmentSubconCuttingOutDetail garmentCuttingOutDetail = new GarmentSubconCuttingOutDetail(id, id, new SizeId(1),"sizeName", "color", 1, 1, new UomId(1),"cuttingOutUomUnit", 1, 1,"remark");
            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>()
                {
                    garmentCuttingOutDetail.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutDetailReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOutDetail>()
                {
                    new GarmentSubconCuttingOutDetail(id, id, new SizeId(1),"sizeName", "color", 1, 1, new UomId(1),"cuttingOutUomUnit", 1, 1,"remark")
                });

            // Act
            var orderData = new
            {
                article = "desc",
            };
            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1,25, order,new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }


        [Fact]
        public async Task Get_with_NoOrder_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCuttingOutController();
            var id = Guid.NewGuid();
            _mockSubconCuttingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingOutReadModel>().AsQueryable());

            _mockSubconCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOut>()
                {
                    new GarmentSubconCuttingOut(id,"cutOutNo", "cutOutType", new UnitDepartmentId(1),"unitFromCode", "unitFromName", DateTimeOffset.Now, "RONo","article",  new GarmentComodityId(1),"comodityCode", "comodityName",1,1,"remark",false)
                });

            GarmentSubconCuttingOutItem garmentCuttingOutItem = new GarmentSubconCuttingOutItem(id, id, id, id, new ProductId(1), "productCode", "productName", "designColor", 1);
            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>()
                {
                    garmentCuttingOutItem.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutItemReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOutItem>()
                {
                    new GarmentSubconCuttingOutItem(id, id, id, id, new ProductId(1),"productCode", "productName","designColor",1)
                });

            GarmentSubconCuttingOutDetail garmentCuttingOutDetail = new GarmentSubconCuttingOutDetail(id, id, new SizeId(1), "sizeName", "color", 1, 1, new UomId(1), "cuttingOutUomUnit", 1, 1, "remark");
            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>()
                {
                    garmentCuttingOutDetail.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutDetailReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOutDetail>()
                {
                    new GarmentSubconCuttingOutDetail(id, id, new SizeId(1),"sizeName", "color", 1, 1, new UomId(1),"cuttingOutUomUnit", 1, 1,"remark")
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
            var unitUnderTest = CreateGarmentSubconCuttingOutController();

            _mockSubconCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOut>()
                {
                    new GarmentSubconCuttingOut(Guid.NewGuid(), null, null,new UnitDepartmentId(1),null,null,DateTimeOffset.Now, "RONo", null, new GarmentComodityId(1) , null, null, 1,1,null,false)
                });

            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOutItem>()
                {
                    new GarmentSubconCuttingOutItem(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, null, 0)
                });

            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOutDetail>()
                {
                    new GarmentSubconCuttingOutDetail(Guid.NewGuid(),Guid.NewGuid(), new SizeId(1), null, null, 0, 0, new UomId(1), null, 0, 0, null)
                });
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCuttingOutController();

            Guid cuttingOutGuid = Guid.NewGuid();
            _mockSubconCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOut>()
                {
                    new GarmentSubconCuttingOut(cuttingOutGuid, null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", "art", new GarmentComodityId(1),  null, null, 1,1,null,false)
                });

            Guid cuttingOutItemGuid = Guid.NewGuid();
            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOutItem>()
                {
                    new GarmentSubconCuttingOutItem(cuttingOutItemGuid, cuttingOutGuid, Guid.NewGuid(), Guid.NewGuid(), new ProductId(1), null, null, "design", 1)
                });

            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOutDetail>()
                {
                    new GarmentSubconCuttingOutDetail(Guid.NewGuid(), cuttingOutItemGuid, new SizeId(1), "size", "color", 1, 1, new UomId(1), "uom", 1, 1, "aa" )
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
            var unitUnderTest = CreateGarmentSubconCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconCuttingOut(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new GarmentComodityId(1), null, null, 1, 1, null, false));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSubconCuttingOutCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconCuttingOutCommand>()));
           
        }

        [Fact]
        public async Task GetComplete_Return_Success()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCuttingOutController();
            _mockSubconCuttingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingOutReadModel>().AsQueryable());

            Guid id = Guid.NewGuid();
            _mockSubconCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOut>()
                {
                    new GarmentSubconCuttingOut(id,"cutOutNo", "cutOutType", new UnitDepartmentId(1),"unitFromCode","unitFromName", DateTimeOffset.Now, "RONo","article",  new GarmentComodityId(1),"comodityCode", "comodityName",1,1,"poSerialNumber",false)
                });

            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutItemReadModel>()
                {
                    new GarmentCuttingOutItemReadModel(id)
                }.AsQueryable());

            _mockSubconCuttingOutItemRepository
               .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutItemReadModel>>()))
               .Returns(new List<GarmentSubconCuttingOutItem>()
               {
                    new GarmentSubconCuttingOutItem(id, id, id, id, new ProductId(1),"productCode", "productName", "design", 1)
               });

            GarmentSubconCuttingOutDetail garmentCuttingOutDetail = new GarmentSubconCuttingOutDetail(id, id, new SizeId(1),"sizeName", "color", 1, 1, new UomId(1),"cuttingOutUomUnit", 1, 1, "remark");
            _mockSubconCuttingOutDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingOutDetailReadModel>()
                {
                    garmentCuttingOutDetail.GetReadModel()
                }.AsQueryable());

            _mockSubconCuttingOutDetailRepository
               .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutDetailReadModel>>()))
               .Returns(new List<GarmentSubconCuttingOutDetail>()
               {
                    new GarmentSubconCuttingOutDetail(id,id,new SizeId(1),"sizeName","color",1,1,new UomId(1),"cuttingOutUomUnit",1,1,"remark")
               });


            // Act
            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1,25, order,new List<string>(),"","{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCuttingOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconCuttingOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconCuttingOut(Guid.NewGuid(), null, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, new GarmentComodityId(1), null, null, 1, 1, null, false));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetByRONo_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconCuttingOutController();

            _mockSubconCuttingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentCuttingOutReadModel>().AsQueryable());

            Guid id = Guid.NewGuid();
            _mockSubconCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentCuttingOutReadModel>>()))
                .Returns(new List<GarmentSubconCuttingOut>()
                {
                    new GarmentSubconCuttingOut(id,"cutOutNo", "cutOutType", new UnitDepartmentId(1),"unitFromCode","unitFromName", DateTimeOffset.Now, "RONo","article",  new GarmentComodityId(1),"comodityCode", "comodityName",1,1,"poSerialNumber",false)
                });
            // Act
            var result = await unitUnderTest.GetLoaderByRO(It.IsAny<string>(), It.IsAny<string>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
