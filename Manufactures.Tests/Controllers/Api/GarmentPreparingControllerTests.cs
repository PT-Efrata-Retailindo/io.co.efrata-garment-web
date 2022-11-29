using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents;
using Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare;
using Manufactures.Application.GarmentPreparings.Queries.GetPrepareTraceable;
using Manufactures.Application.GarmentPreparings.Queries.GetWIP;
using Manufactures.Controllers.Api;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
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
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Controllers.Api
{
    public class GarmentPreparingControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;
		
		public GarmentPreparingControllerTests() : base()
        {
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);

		}

        private GarmentPreparingController CreateGarmentPreparingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            GarmentPreparingController controller = (GarmentPreparingController)Activator.CreateInstance(typeof(GarmentPreparingController), _MockServiceProvider.Object);
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

        /*[Fact]
        public async Task Get_with_Keyword_Return_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentPreparingReadModel>()
                {
                    new GarmentPreparingReadModel(id)
                }
                .AsQueryable());

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingReadModel>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(id, 0,"uenNo", new UnitDepartmentId(1),"unitCode", "unitName", DateTimeOffset.Now,"roNo" ,"article", false, new Domain.Shared.ValueObjects.BuyerId(1), null, null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingItemReadModel>>()))
                .Returns(new List<GarmentPreparingItem>()
                {
                    new GarmentPreparingItem(id, 0, new ProductId(1),"productCode", "productName","designColor", 1, new UomId(1),"uomUnit", "FABRIC", 1, 1,id,null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>()
                {
                    new GarmentPreparingItemReadModel(id)
                }.AsQueryable());

            // Act
            
            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1,25,order,new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task Get_with_Keyword_and_NoOrder_Return_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentPreparingReadModel>()
                {
                    new GarmentPreparingReadModel(id)
                }
                .AsQueryable());

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingReadModel>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(id, 0,"uenNo", new UnitDepartmentId(1),"unitCode", "unitName", DateTimeOffset.Now,"roNo" ,"article", false, new Domain.Shared.ValueObjects.BuyerId(1), null, null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingItemReadModel>>()))
                .Returns(new List<GarmentPreparingItem>()
                {
                    new GarmentPreparingItem(id, 0, new ProductId(1),"productCode", "productName","designColor", 1, new UomId(1),"uomUnit", "FABRIC", 1, 1,id,null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>()
                {
                    new GarmentPreparingItemReadModel(id)
                }.AsQueryable());

            // Act

            var result = await unitUnderTest.Get(1, 25, "{}", new List<string>(), "productCode", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task Get_with_Empty_Keyword_Return_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentPreparingReadModel>()
                {
                    new GarmentPreparingReadModel(id)
                }
                .AsQueryable());

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingReadModel>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(id, 0,"uenNo", new UnitDepartmentId(1),"unitCode", "unitName", DateTimeOffset.Now,"roNo" ,"article", false, new Domain.Shared.ValueObjects.BuyerId(1), null, null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingItemReadModel>>()))
                .Returns(new List<GarmentPreparingItem>()
                {
                    new GarmentPreparingItem(id, 0, new ProductId(1),"productCode", "productName","designColor", 1, new UomId(1),"uomUnit", "FABRIC", 1, 1,id,null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>()
                {
                    new GarmentPreparingItemReadModel(id)
                }.AsQueryable());

            // Act

            var orderData = new
            {
                Article = "desc",
            };

            string order = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.Get(1, 25, order, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        /*[Fact]
        public async Task Get_with_Empty_Keyword_and_NotOrder_Return_Success()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentPreparingReadModel>()
                {
                    new GarmentPreparingReadModel(id)
                }
                .AsQueryable());

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingReadModel>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(id, 0,"uenNo", new UnitDepartmentId(1),"unitCode", "unitName", DateTimeOffset.Now,"roNo" ,"article", false, new Domain.Shared.ValueObjects.BuyerId(1), null, null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentPreparingItemReadModel>>()))
                .Returns(new List<GarmentPreparingItem>()
                {
                    new GarmentPreparingItem(id, 0, new ProductId(1),"productCode", "productName","designColor", 1, new UomId(1),"uomUnit", "FABRIC", 1, 1,id,null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>()
                {
                    new GarmentPreparingItemReadModel(id)
                }.AsQueryable());

            // Act
            var result = await unitUnderTest.Get(1, 25, "{}", new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }*/

        [Fact]
        public async Task GetSingle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, false, new Domain.Shared.ValueObjects.BuyerId(1), null, null)
                });

            _mockGarmentPreparingItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingItemReadModel, bool>>>()))
                .Returns(new List<GarmentPreparingItem>()
                {
                    new GarmentPreparingItem(Guid.NewGuid(), 0, new ProductId(1), null, null, null, 0, new UomId(1), null, null, 0, 0, Guid.NewGuid(),null,"fasilitas")
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
            var unitUnderTest = CreateGarmentPreparingController();

            PlaceGarmentPreparingCommand command = new PlaceGarmentPreparingCommand();
            command.UENId = 1;

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>());

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentPreparingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, false, new Domain.Shared.ValueObjects.BuyerId(1), null, null));

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Return_BadRequest()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentPreparingController();

            PlaceGarmentPreparingCommand command = new PlaceGarmentPreparingCommand();
            command.UENId = 1;

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>() { 
                    new GarmentPreparing(id,1,"uenNo",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,"roNo","article",true, new Domain.Shared.ValueObjects.BuyerId(1), null, null)
                });

          

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Return_InternalServerError()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentPreparingController();

            PlaceGarmentPreparingCommand command = new PlaceGarmentPreparingCommand();
            command.UENId = 1;

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>() {
                  
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentPreparingCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act
            var result = await unitUnderTest.Post(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentPreparingController();

            _mockGarmentPreparingRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingReadModel, bool>>>()))
                .Returns(new List<GarmentPreparing>()
                {
                    new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, null, null, false,new Domain.Shared.ValueObjects.BuyerId(1), null,null)
                });

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentPreparingCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentPreparing(Guid.NewGuid(), 0, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, "RONo", null, false, new Domain.Shared.ValueObjects.BuyerId(1), null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetLoaderByRO_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var id = Guid.NewGuid();
            var unitUnderTest = CreateGarmentPreparingController();
            var garmentPreparingReadModel = new GarmentPreparingReadModel(id);

            _mockGarmentPreparingRepository
                .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
                .Returns(new List<GarmentPreparingReadModel>()
                {
                   garmentPreparingReadModel
                }
                .AsQueryable());

            _mockGarmentPreparingRepository
              .Setup(s => s.Read(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>()))
              .Returns(new List<GarmentPreparingReadModel>()
              {
                  new GarmentPreparingReadModel(id)
              }
              .AsQueryable());

            // Act
            var result = await unitUnderTest.GetLoaderByRO("", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

		[Fact]
		public async Task GetMonitoringBehavior()
		{
			var unitUnderTest = CreateGarmentPreparingController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetMonitoringPrepareQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new GarmentMonitoringPrepareListViewModel());

			// Act
			var result = await unitUnderTest.GetMonitoring(1,DateTime.Now,DateTime.Now,1,25,"{}");

			// Assert
			GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetXLSPrepareBehavior()
		{
			var unitUnderTest = CreateGarmentPreparingController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsPrepareQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act

			var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now,"", 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
		}

        [Fact]
        public async Task GetXLSPrepareReturn_InternalServerError()
        {
            var unitUnderTest = CreateGarmentPreparingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsPrepareQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act

            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
		public async Task GetXLSBookkeepingPrepareBehavior()
		{
			var unitUnderTest = CreateGarmentPreparingController();

			_MockMediator
				.Setup(s => s.Send(It.IsAny<GetXlsPrepareQuery>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(new MemoryStream());

			// Act

			var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "bookkeping", 1, 25, "{}");

			// Assert
			Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
		}

        [Fact]
        public async Task Put_Dates_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentPreparingController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentPreparingCommand command = new UpdateDatesGarmentPreparingCommand(ids, DateTimeOffset.Now);
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
            var unitUnderTest = CreateGarmentPreparingController();
            Guid sewingOutGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sewingOutGuid.ToString());

            UpdateDatesGarmentPreparingCommand command = new UpdateDatesGarmentPreparingCommand(ids, DateTimeOffset.Now.AddDays(3));

            // Act
            var result = await unitUnderTest.UpdateDates(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result));

            UpdateDatesGarmentPreparingCommand command2 = new UpdateDatesGarmentPreparingCommand(ids, DateTimeOffset.MinValue);

            // Act
            var result1 = await unitUnderTest.UpdateDates(command2);

            // Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(result1));
        }

        [Fact]
        public async Task GetMonitoringWIPBehavior()
        {
            var unitUnderTest = CreateGarmentPreparingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetWIPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentWIPListViewModel());

            // Act
            var result = await unitUnderTest.GetWIP(DateTime.Now, 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetXLSWIPBehavior()
        {
            var unitUnderTest = CreateGarmentPreparingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsWIPQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act

            var result = await unitUnderTest.GetXlsWIP(DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }

        [Fact]
        public async Task GetXLSWIPReturn_InternalServerError()
        {
            var unitUnderTest = CreateGarmentPreparingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsWIPQuery>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act

            var result = await unitUnderTest.GetXlsWIP(DateTime.Now, 1, 25, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }

        [Fact]
        public async Task GetDataTraceableBehavior()
        {
            var unitUnderTest = CreateGarmentPreparingController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetPrepareTraceableQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetPrepareTraceableListViewModel());

            // Act

            var result = await unitUnderTest.GetpreparingbyRONO("ro1,ro2");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

    }
}