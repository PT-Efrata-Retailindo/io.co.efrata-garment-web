using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport;
using Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport;
using Manufactures.Controllers.Api.GarmentSubcon;
using Manufactures.Domain.GarmentSubcon.SubconContracts;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconContracts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Repositories;
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

namespace Manufactures.Tests.Controllers.Api.GarmentSubcon
{
    public class GarmentSubconContractControllerTest : BaseControllerUnitTest
    {
        private readonly Mock<IGarmentSubconContractRepository> _mockGarmentSubconContractRepository;
        private readonly Mock<IGarmentSubconContractItemRepository> _mockGarmentSubconContractItemRepository;

        public GarmentSubconContractControllerTest() : base()
        {
            _mockGarmentSubconContractRepository = CreateMock<IGarmentSubconContractRepository>();
            _mockGarmentSubconContractItemRepository = CreateMock<IGarmentSubconContractItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSubconContractRepository);
            _MockStorage.SetupStorage(_mockGarmentSubconContractItemRepository);
        }

        private GarmentSubconContractController CreateGarmentSubconContractController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentSubconContractController controller = (GarmentSubconContractController)Activator.CreateInstance(typeof(GarmentSubconContractController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSubconContractController();

            _mockGarmentSubconContractRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconContractReadModel>().AsQueryable());

            Guid SubconContractGuid = Guid.NewGuid();
            _mockGarmentSubconContractRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconContractReadModel>>()))
                .Returns(new List<GarmentSubconContract>()
                {
                    new GarmentSubconContract(SubconContractGuid, "","","", new SupplierId(1),"","","","","",1,DateTimeOffset.Now,DateTimeOffset.Now,false,new BuyerId(1),"","","",new UomId(1),"","",DateTimeOffset.Now, 1)
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
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            _mockGarmentSubconContractRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
                .Returns(new List<GarmentSubconContract>()
                {
                    new GarmentSubconContract(SubconContractGuid, "","","", new SupplierId(1),"","","","","",1,DateTimeOffset.Now,DateTimeOffset.Now,false,new BuyerId(1),"","","",new UomId(1),"","",DateTimeOffset.Now, 1)
                });

            _mockGarmentSubconContractItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractItemReadModel, bool>>>()))
                .Returns(new List<GarmentSubconContractItem>()
                {
                    new GarmentSubconContractItem(Guid.NewGuid(),new Guid(),new Domain.Shared.ValueObjects.ProductId(1),"code","name",1,new Domain.Shared.ValueObjects.UomId(1),"unit", 1)
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
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            //_mockGarmentSubconContractRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconContract>());
            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconContract(SubconContractGuid, "", "a ", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "", "", new UomId(1), "", "", DateTimeOffset.Now, 1));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSubconContractCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throw_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            //_mockGarmentSubconContractRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconContract>());

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSubconContractCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            // Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSubconContractCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            //_mockGarmentSubconContractRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSubconContractReadModel, bool>>>()))
            //    .Returns(new List<GarmentSubconContract>());
            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSubconContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconContract(SubconContractGuid, "", "", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "", "", new UomId(1), "", "", DateTimeOffset.Now, 1));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSubconContractCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();
            Guid SubconContractGuid = Guid.NewGuid();
            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSubconContractCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconContract(SubconContractGuid, "", "", "", new SupplierId(1), "", "", "", "", "", 1, DateTimeOffset.Now, DateTimeOffset.Now, false, new BuyerId(1), "", "", "", new UomId(1), "", "", DateTimeOffset.Now, 1));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSubconContractController();

            _mockGarmentSubconContractRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSubconContractReadModel>().AsQueryable());

            Guid SubconContractGuid = Guid.NewGuid();
            _mockGarmentSubconContractRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconContractReadModel>>()))
                .Returns(new List<GarmentSubconContract>()
                {
                    new GarmentSubconContract(SubconContractGuid, "","","", new SupplierId(1),"","","","","",1,DateTimeOffset.Now,DateTimeOffset.Now,false,new BuyerId(1),"","","",new UomId(1),"","",DateTimeOffset.Now, 1)
                });

            Guid SubconDeliveryLetterOutItemGuid = Guid.NewGuid();
            GarmentSubconContractItem garmentSubconContractItem = new GarmentSubconContractItem(Guid.NewGuid(), new Guid(), new Domain.Shared.ValueObjects.ProductId(1), "code", "name", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", 1);

            _mockGarmentSubconContractItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconContractItemReadModel>() {
                    garmentSubconContractItem.GetReadModel()
                }.AsQueryable());

            _mockGarmentSubconContractItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconContractItemReadModel>>()))
                .Returns(new List<GarmentSubconContractItem>()
                {
                    new GarmentSubconContractItem(Guid.NewGuid(), new Guid(), new Domain.Shared.ValueObjects.ProductId(1), "code", "name", 1, new Domain.Shared.ValueObjects.UomId(1), "unit", 1)
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
        public async Task GetMonitoringRealizationSubconBehavior()
        {
            var unitUnderTest = CreateGarmentSubconContractController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GarmentRealizationSubconReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentRealizationSubconReportListViewModel());

            // Act

            var result = await unitUnderTest.GetReaizationReport("", 1, 25, "{}");


            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
            // Assert
            //Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLSSubconReportBehavior()
        {
            var unitUnderTest = CreateGarmentSubconContractController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentRealizationSubconReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act

            var result = await unitUnderTest.GetXlsReaizationReport("", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLSRealizationSubconReturn_InternalServerError()
        {
            var unitUnderTest = CreateGarmentSubconContractController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentRealizationSubconReportQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act

            var result = await unitUnderTest.GetXlsReaizationReport("", 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task GetMonitoringSubconContractBehavior()
        {
            var unitUnderTest = CreateGarmentSubconContractController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GarmentSubconContactReportQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSubconContactReportListViewModel());

            // Act

            var result = await unitUnderTest.GetSubconContractReport(It.IsAny<int>(),"", It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, 25, "{}");


            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
            // Assert
            //Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLSubconContractBehavior()
        {
            var unitUnderTest = CreateGarmentSubconContractController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconContractReporQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act

            var result = await unitUnderTest.GetXlsSubconContractReport(It.IsAny<int>(), "", It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLSSubconContractReturn_InternalServerError()
        {
            var unitUnderTest = CreateGarmentSubconContractController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsGarmentSubconContractReporQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act

            var result = await unitUnderTest.GetXlsSubconContractReport(It.IsAny<int>(), "", It.IsAny<DateTime>(), It.IsAny<DateTime>(), 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }


    }
}