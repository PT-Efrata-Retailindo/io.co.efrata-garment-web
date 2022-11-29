using Barebone.Tests;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOCuttingSewingReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOComponentServiceReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOSewingReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOGarmentWashReport;
using System.IO;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentSubconDeliveryLetterOutControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSubconDeliveryLetterOutRepository> _mockGarmentSubconDeliveryLetterOutRepository;
        private Mock<IGarmentSubconDeliveryLetterOutItemRepository> _mockGarmentSubconDeliveryLetterOutItemRepository;
        private Mock<IGarmentSubconContractRepository> _mockGarmentSubconContractRepository;
        private readonly Mock<IGarmentSubconCuttingOutRepository> _mockSubconCuttingOutRepository;
        private readonly Mock<IGarmentSubconCuttingOutItemRepository> _mockSubconCuttingOutItemRepository;
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockGarmentServiceSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconSewingItemRepository> _mockGarmentServiceSubconSewingItemRepository;
        private readonly Mock<IGarmentServiceSubconSewingDetailRepository> _mockGarmentServiceSubconSewingDetailRepository;

        public GarmentSubconDeliveryLetterOutControllerTests() : base()
        {
            _mockGarmentSubconDeliveryLetterOutRepository = CreateMock<IGarmentSubconDeliveryLetterOutRepository>();
            _mockGarmentSubconDeliveryLetterOutItemRepository = CreateMock<IGarmentSubconDeliveryLetterOutItemRepository>();
            _mockGarmentSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();
            _mockSubconCuttingOutRepository = CreateMock<IGarmentSubconCuttingOutRepository>();
            _mockSubconCuttingOutItemRepository = CreateMock<IGarmentSubconCuttingOutItemRepository>();
            _mockGarmentServiceSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockGarmentServiceSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();
            _mockGarmentServiceSubconSewingDetailRepository = CreateMock<IGarmentServiceSubconSewingDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentSubconDeliveryLetterOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSubconDeliveryLetterOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSubconContractRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutRepository);
            _MockStorage.SetupStorage(_mockSubconCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSubconSewingRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSubconSewingItemRepository);
            _MockStorage.SetupStorage(_mockGarmentServiceSubconSewingDetailRepository);


        }

        private GarmentSubconDeliveryLetterOutController CreateGarmentSubconDeliveryLetterOutController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconDeliveryLetterOutController controller = (GarmentSubconDeliveryLetterOutController)Activator.CreateInstance(typeof(GarmentSubconDeliveryLetterOutController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            //_mockGarmentSubconDeliveryLetterOutRepository
            //    .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            //    .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());

            //Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            //_mockGarmentSubconDeliveryLetterOutRepository
            //    .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconDeliveryLetterOutReadModel>>()))
            //    .Returns(new List<GarmentSubconDeliveryLetterOut>()
            //    {
            //        new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid, null,null,new Guid(),"","",DateTimeOffset.Now,1,"","",1,"", false,"","")
            //    });

            ////Guid SubconDeliveryLetterOutItemGuid = Guid.NewGuid();
            ////GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem = new GarmentSubconDeliveryLetterOutItem(Guid.Empty, new Guid(), 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", 
            ////    "color", 1, new Domain.Shared.ValueObjects.UomId(1), "uomUnit", new Domain.Shared.ValueObjects.UomId(1), "UomOutUnit", "fabType", new Guid(), "roNo", "poSerialNumber", "subconNo");
            ////_mockGarmentSubconDeliveryLetterOutItemRepository
            ////    .Setup(s => s.Query)
            ////    .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>()
            ////    {
            ////        garmentSubconDeliveryLetterOutItem.GetReadModel()
            ////    }.AsQueryable());
            //Guid SubconDeliveryLetterOutItemGuid = Guid.NewGuid();
            //_mockGarmentSubconDeliveryLetterOutItemRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
            //    {
            //        new GarmentSubconDeliveryLetterOutItem(Guid.NewGuid(),new Guid(),1,new Domain.Shared.ValueObjects.ProductId(1),"code","name","remark","color",1,new Domain.Shared.ValueObjects.UomId(1),"unit",new Domain.Shared.ValueObjects.UomId(1),"unit","fabType",new Guid(),"","","")
            //    });

            _mockGarmentSubconDeliveryLetterOutRepository
               .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
               .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());

            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            _mockGarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconDeliveryLetterOutReadModel>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOut>()
                {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid, null,"","",DateTimeOffset.Now,1,"","",1,"", false,"","",1,"",1,"")
                });

            Guid SubconDeliveryLetterOutItemGuid = Guid.NewGuid();

            GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem = new GarmentSubconDeliveryLetterOutItem(Guid.NewGuid(), SubconDeliveryLetterOutGuid, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "",1,"");

            _mockGarmentSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>() {
                    garmentSubconDeliveryLetterOutItem.GetReadModel()
                }.AsQueryable());

            _mockGarmentSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconDeliveryLetterOutItemReadModel>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.NewGuid(),SubconDeliveryLetterOutGuid,1,new Domain.Shared.ValueObjects.ProductId(1),"code","name","remark","color",1,new Domain.Shared.ValueObjects.UomId(1),"unit",new Domain.Shared.ValueObjects.UomId(1),"unit","fabType",new Guid(),"","","",1,"")
                });

            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _mockGarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOut>()
                {
                    new GarmentSubconDeliveryLetterOut(Guid.NewGuid(), null,null,"",DateTimeOffset.Now,1,"","",1,"", false,"","",It.IsAny<int>(),"",It.IsAny<int>(),"")
                });

            _mockGarmentSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.NewGuid(),new Guid(),1,new Domain.Shared.ValueObjects.ProductId(1),"code","name","remark","color",1,new Domain.Shared.ValueObjects.UomId(1),"unit",new Domain.Shared.ValueObjects.UomId(1),"unit","fabType",new Guid(),"","","", It.IsAny<int>(),"")
                });


            // Act
            var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();
            PlaceGarmentSubconDeliveryLetterOutCommand command = new PlaceGarmentSubconDeliveryLetterOutCommand();
            command.UENId = 1;
            command.ContractType = "SUBCON BAHAN BAKU";
            //_mockGarmentSubconDeliveryLetterOutRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconDeliveryLetterOut>());
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconDeliveryLetterOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconDeliveryLetterOut(Guid.NewGuid(), null, null, "", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "", It.IsAny<int>(), "", It.IsAny<int>(), ""));

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconDeliveryLetterOutCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconDeliveryLetterOutCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();
            Guid subconDeliveryLetterOutGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSubconDeliveryLetterOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid, null, null, "", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "", It.IsAny<int>(), "", It.IsAny<int>(), ""));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSubconDeliveryLetterOutCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();
            var subconDeliveryLetterOutGuid = Guid.NewGuid();

            _mockGarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOut>()
                {
                    new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid, null, null, "", DateTimeOffset.Now, 1, "", "", 1, "", false,"","",It.IsAny<int>(),"",It.IsAny<int>(),"")
                });


            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconDeliveryLetterOutCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconDeliveryLetterOut(subconDeliveryLetterOutGuid, null, null, "SUBCON BAHAN BAKU", DateTimeOffset.Now, 1, "", "", 1, "", false, "", "", It.IsAny<int>(), "", It.IsAny<int>(), ""));

            // Act
            var result = await unitUnderTest.Delete(subconDeliveryLetterOutGuid.ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();
            
            _mockGarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutReadModel>().AsQueryable());

            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            _mockGarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconDeliveryLetterOutReadModel>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOut>()
                {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid, null,null,"",DateTimeOffset.Now,1,"","",1,"", false,"","",It.IsAny<int>(),"",It.IsAny<int>(),"")
                });

            Guid SubconDeliveryLetterOutItemGuid = Guid.NewGuid();
            GarmentSubconDeliveryLetterOutItem garmentSubconDeliveryLetterOutItem = new GarmentSubconDeliveryLetterOutItem(Guid.NewGuid(), SubconDeliveryLetterOutGuid, 1, new Domain.Shared.ValueObjects.ProductId(1), "code", "name", "remark", "color", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "", It.IsAny<int>(), "");

            _mockGarmentSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconDeliveryLetterOutItemReadModel>() {
                    garmentSubconDeliveryLetterOutItem.GetReadModel()
                }.AsQueryable());
            
            _mockGarmentSubconDeliveryLetterOutItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconDeliveryLetterOutItemReadModel>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                {
                    new GarmentSubconDeliveryLetterOutItem(Guid.NewGuid(),SubconDeliveryLetterOutGuid,1,new Domain.Shared.ValueObjects.ProductId(1),"code","name","remark","color",1,new Domain.Shared.ValueObjects.UomId(1),"unit",new Domain.Shared.ValueObjects.UomId(1),"unit","fabType",new Guid(),"","","", It.IsAny<int>(),"")
                });
            var orderData = new
            {
                Id = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            // Act
            var result = await unitUnderTest.GetComplete(1, 25, order, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();
            Guid SubconDeliveryLetterOutGuid = Guid.NewGuid();
            Guid SubconContractGuid = Guid.NewGuid();
            Guid SubconCuttingGuid = Guid.NewGuid();
            _mockGarmentSubconDeliveryLetterOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutReadModel, bool>>>()))
                .Returns(new List<GarmentSubconDeliveryLetterOut>()
                {
                    new GarmentSubconDeliveryLetterOut(SubconDeliveryLetterOutGuid, null,"","",DateTimeOffset.Now,1,"","",1,"", false,"","SUBCON CUTTING SEWING",1,"",1,"")
                });

            Guid SubconDeliveryLetterOutItemGuid = Guid.NewGuid();

            _mockGarmentSubconDeliveryLetterOutItemRepository
                 .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconDeliveryLetterOutItemReadModel, bool>>>()))
                 .Returns(new List<GarmentSubconDeliveryLetterOutItem>()
                 {
                    new GarmentSubconDeliveryLetterOutItem(SubconDeliveryLetterOutItemGuid,SubconDeliveryLetterOutGuid,1,new Domain.Shared.ValueObjects.ProductId(1),"code","name","remark","color",1,new Domain.Shared.ValueObjects.UomId(1),"unit",new Domain.Shared.ValueObjects.UomId(1),"unit","fabType",SubconCuttingGuid,"","","", 1,"")
                 });

            //_mockGarmentSubconContractRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconContract>()
            //    {
            //        new GarmentSubconContract(SubconContractGuid, "","","", new SupplierId(1),"","","","","",1,DateTimeOffset.Now,DateTimeOffset.Now,false,new BuyerId(1),"","","",new UomId(1),"","",DateTimeOffset.Now,1)

            //    });


            _mockSubconCuttingOutRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOut>()
                {
                    new GarmentSubconCuttingOut(SubconCuttingGuid, null, null,new UnitDepartmentId(1),null,null,DateTimeOffset.Now, "RONo", null, new GarmentComodityId(1) , null, null, 1,1,null,false)
                });

            _mockSubconCuttingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentCuttingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconCuttingOutItem>()
                {
                    new GarmentSubconCuttingOutItem(Guid.NewGuid(), SubconCuttingGuid, Guid.NewGuid(), SubconCuttingGuid, new ProductId(1), null, null, null, 0)
                });

            _mockGarmentServiceSubconSewingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewing>()
                {
                    new GarmentServiceSubconSewing(SubconDeliveryLetterOutItemGuid, "",DateTimeOffset.Now,true, new BuyerId(1),"","",1,"")
                });

            _mockGarmentServiceSubconSewingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentServiceSubconSewingItemReadModel, bool>>>()))
                .Returns(new List<GarmentServiceSubconSewingItem>()
                {
                    new GarmentServiceSubconSewingItem(SubconCuttingGuid, SubconDeliveryLetterOutItemGuid,"","",new GarmentComodityId(1),"","",new BuyerId(1),"","",new UnitDepartmentId(1),"","")
                });

            //_mockSewingInItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSewingInItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.GetPdf(Guid.NewGuid().ToString());

            // Assert
            Assert.NotNull(result.GetType().GetProperty("FileStream"));
        }

        /* Garment-Cutting-Sewing */
        [Fact]
        public async Task GetXLSBehavior()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLOCuttingSewingReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLOCuttingSewingReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetXLS_Return_InternalServerError()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLOCuttingSewingReportQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLOCuttingSewingReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }

        /* garment-component */
        [Fact]
        public async Task GetXLSBehavior_RawMaterial()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLORawMaterialReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLORawMaterialReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetXLS_RawMaterial_Return_InternalServerError()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLORawMaterialReportQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLORawMaterialReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }

        /* garment-component */
        [Fact]
        public async Task GetXLSBehavior_Component()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLOComponentServiceReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLOComponentServiceReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetXLS_Component_Return_InternalServerError()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLOComponentServiceReportQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLOComponentServiceReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }

        /* garment-sewing */
        [Fact]
        public async Task GetXLSBehavior_Sewing()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLOSewingReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLOSewingReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetXLS_Sewing_Return_InternalServerError()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLOSewingReportQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLOSewingReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }

        /* garment-wash */
        [Fact]
        public async Task GetXLSBehavior_GarmentWash()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLOGarmentWashReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLOGarmentWashReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));

        }

        [Fact]
        public async Task GetXLS_GarmentWash_Return_InternalServerError()
        {
            var unitUnderTest = CreateGarmentSubconDeliveryLetterOutController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconDLOGarmentWashReportQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.GetXlsSubconDLOGarmentWashReport(DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));

        }
    }
}
