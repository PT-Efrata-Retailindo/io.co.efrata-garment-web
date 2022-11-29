using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleRequest.Queries.GetMonitoringReceiptSample;
using Manufactures.Application.AzureUtility;
using Manufactures.Controllers.Api.GarmentSample;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
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
    public class GarmentSampleRequestControllerTests : BaseControllerUnitTest
    {
        private Mock<IGarmentSampleRequestRepository> _mockGarmentSampleRequestRepository;
        private Mock<IGarmentSampleRequestProductRepository> _mockGarmentSampleRequestProductRepository;
        private Mock<IGarmentSampleRequestSpecificationRepository> _mockGarmentSampleRequestSpecificationRepository;
        private Mock<IAzureImage> _AzureImage;
        private Mock<IAzureDocument> _AzureDocument;

        public GarmentSampleRequestControllerTests() : base()
        {
            _mockGarmentSampleRequestRepository = CreateMock<IGarmentSampleRequestRepository>();
            _mockGarmentSampleRequestProductRepository = CreateMock<IGarmentSampleRequestProductRepository>();
            _mockGarmentSampleRequestSpecificationRepository = CreateMock<IGarmentSampleRequestSpecificationRepository>();
            _AzureImage = CreateMock<IAzureImage>();
            _AzureDocument = CreateMock<IAzureDocument>();

            _MockStorage.SetupStorage(_mockGarmentSampleRequestRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestProductRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleRequestSpecificationRepository);

            _MockServiceProvider.Setup(x => x.GetService(typeof(IAzureImage))).Returns(_AzureImage.Object);
            _MockServiceProvider.Setup(x => x.GetService(typeof(IAzureDocument))).Returns(_AzureDocument.Object);

        }

        private GarmentSampleRequestController CreateGarmentSampleRequestController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            GarmentSampleRequestController controller = (GarmentSampleRequestController)Activator.CreateInstance(typeof(GarmentSampleRequestController), _MockServiceProvider.Object);
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
            var unitUnderTest = CreateGarmentSampleRequestController();

            _mockGarmentSampleRequestRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleRequestReadModel>().AsQueryable());

            Guid SampleRequestGuid = Guid.NewGuid();
            _mockGarmentSampleRequestRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleRequestReadModel>>()))
                .Returns(new List<GarmentSampleRequest>()
                {
                    new GarmentSampleRequest(SampleRequestGuid, null, null, null, null, DateTimeOffset.Now, new BuyerId(1), null,
                    null,new GarmentComodityId(1),null,null,null,null, DateTimeOffset.Now,null,null,null,false,false,DateTimeOffset.Now,
                    null,false, DateTimeOffset.Now,null,null,false, DateTimeOffset.Now, null,null,null,null,null,null, new SectionId(1),null,null)
                });

            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        /* [Fact]
         public async Task GetSingle_StateUnderTest_ExpectedBehavior()
         {
             // Arrange
             var unitUnderTest = CreateGarmentSampleRequestController();

             _mockGarmentSampleRequestRepository
                 .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestReadModel, bool>>>()))
                 .Returns(new List<GarmentSampleRequest>()
                 {
                     new GarmentSampleRequest(Guid.NewGuid(), null, null, null, null, DateTimeOffset.Now, new BuyerId(1), null, null,
                     new GarmentComodityId(1),null,null,null,null, DateTimeOffset.Now,null,null,null,false,false, DateTimeOffset.Now,
                     null,false, DateTimeOffset.Now,null,false,null,"[]","[]",null,null,null, new SectionId(1),null)
                 });

             _mockGarmentSampleRequestProductRepository
                 .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestProductReadModel, bool>>>()))
                 .Returns(new List<GarmentSampleRequestProduct>()
                 {
                     new GarmentSampleRequestProduct(Guid.NewGuid(), Guid.NewGuid(),null,null,new SizeId(1),null,null,1,1)
                 });

             _mockGarmentSampleRequestSpecificationRepository
                 .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestSpecificationReadModel, bool>>>()))
                 .Returns(new List<GarmentSampleRequestSpecification>()
                 {
                     new GarmentSampleRequestSpecification(Guid.NewGuid(), Guid.NewGuid(),null,null,1,null,new UomId(1),null,1)
                 });

             _AzureImage
                 .Setup(s => s.DownloadMultipleImages(It.IsAny<string>(), It.IsAny<List<string>>()))
                 .ReturnsAsync(null);

             _AzureDocument
                 .Setup(s => s.DownloadMultipleFiles(It.IsAny<string>(), It.IsAny<List<string>>()))
                 .ReturnsAsync(null);

             // Act
             var result = await unitUnderTest.Get(Guid.NewGuid().ToString());

             // Assert
             Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
         }*/

        [Fact]
        public async Task Post_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleRequestController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleRequestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleRequest(Guid.NewGuid(), null, null, null, null, DateTimeOffset.Now, new BuyerId(1), null,
                null, new GarmentComodityId(1), null, null, null, null, DateTimeOffset.Now, null, null, null, false, false, DateTimeOffset.Now,
                null, false, DateTimeOffset.Now, null, null, false, DateTimeOffset.Now, null, null, null, null, null, null, new SectionId(1), null, null));

            // Act
            var result = await unitUnderTest.Post(It.IsAny<PlaceGarmentSampleRequestCommand>());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Post_Throws_Exception()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleRequestController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<PlaceGarmentSampleRequestCommand>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception());

            // Act and Assert
            await Assert.ThrowsAsync<Exception>(() => unitUnderTest.Post(It.IsAny<PlaceGarmentSampleRequestCommand>()));
        }

        [Fact]
        public async Task Put_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleRequestController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<UpdateGarmentSampleRequestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleRequest(Guid.NewGuid(), null, null, null, null, DateTimeOffset.Now, new BuyerId(1), null,
                null, new GarmentComodityId(1), null, null, null, null, DateTimeOffset.Now, null, null, null, false, false, DateTimeOffset.Now,
                null, false, DateTimeOffset.Now, null, null, false, DateTimeOffset.Now, null, null, null, null, null, null, new SectionId(1), null, null));

            // Act
            var result = await unitUnderTest.Put(Guid.NewGuid().ToString(), new UpdateGarmentSampleRequestCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {

            // Arrange
            var unitUnderTest = CreateGarmentSampleRequestController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<RemoveGarmentSampleRequestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleRequest(Guid.NewGuid(), null, null, null, null, DateTimeOffset.Now, new BuyerId(1), null, null,
                new GarmentComodityId(1), null, null, null, null, DateTimeOffset.Now, null, null, null, false, false, DateTimeOffset.Now,
                null, false, DateTimeOffset.Now, null, null, false, DateTimeOffset.Now, null, null, null, null, null, null, new SectionId(1), null, null));

            // Act
            var result = await unitUnderTest.Delete(Guid.NewGuid().ToString());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }



        [Fact]
        public async Task GetComplete_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid SampleRequestProductGuid = Guid.NewGuid();
            Guid SampleRequestGuid = Guid.NewGuid();
            Guid subconCuttingSpecificationGuid = Guid.NewGuid();
            var unitUnderTest = CreateGarmentSampleRequestController();

            _mockGarmentSampleRequestRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleRequestReadModel>() { new GarmentSampleRequestReadModel(SampleRequestGuid) }
                .AsQueryable());

            _mockGarmentSampleRequestRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleRequestReadModel>>()))
                .Returns(new List<GarmentSampleRequest>()
                {
                    new GarmentSampleRequest(SampleRequestGuid, null, null, null, null, DateTimeOffset.Now, new BuyerId(1), null, null,
                    new GarmentComodityId(1),null,null,null,null, DateTimeOffset.Now,null,null,null,false,false,DateTimeOffset.Now,
                    null,false, DateTimeOffset.Now,null,null, false, DateTimeOffset.Now,null,null,null,null,null,null, new SectionId(1),null, null)
                });

            _mockGarmentSampleRequestProductRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestProductReadModel>()
                {
                    new GarmentSampleRequestProductReadModel(SampleRequestProductGuid)
                }.AsQueryable());

            _mockGarmentSampleRequestProductRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleRequestProductReadModel>>()))
                .Returns(new List<GarmentSampleRequestProduct>()
                {
                    new GarmentSampleRequestProduct(SampleRequestProductGuid, SampleRequestGuid,null,null,null,new SizeId(1),null,null,1,1)
                });

            _mockGarmentSampleRequestSpecificationRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleRequestSpecificationReadModel>()
                {
                    new GarmentSampleRequestSpecificationReadModel(subconCuttingSpecificationGuid)
                }.AsQueryable());

            _mockGarmentSampleRequestSpecificationRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleRequestSpecificationReadModel>>()))
                .Returns(new List<GarmentSampleRequestSpecification>()
                {
                    new GarmentSampleRequestSpecification(subconCuttingSpecificationGuid, SampleRequestProductGuid,null,null,1,null,new UomId(1),null,1)
                });

            // Act
            var orderData = new
            {
                SampleRequestNo = "desc",
            };

            string oder = JsonConvert.SerializeObject(orderData);
            var result = await unitUnderTest.GetComplete(1, 25, oder, new List<string>(), "", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task PostData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleRequestController();
            Guid sampleGuid = Guid.NewGuid();
            List<string> ids = new List<string>();
            ids.Add(sampleGuid.ToString());

            PostGarmentSampleRequestCommand command = new PostGarmentSampleRequestCommand(ids, true);
            _MockMediator
               .Setup(s => s.Send(command, It.IsAny<CancellationToken>()))
               .ReturnsAsync(1);

            // Act
            var result = await unitUnderTest.postData(command);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task ReceivedData_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = CreateGarmentSampleRequestController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<ReceivedGarmentSampleRequestCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentSampleRequest(Guid.NewGuid(), null, null, null, null, DateTimeOffset.Now, new BuyerId(1), null,
                null, new GarmentComodityId(1), null, null, null, null, DateTimeOffset.Now, null, null, null, false, false,
                DateTimeOffset.Now, null, false, DateTimeOffset.Now, null, null, false, DateTimeOffset.Now, null, null, null, null, null, null, new SectionId(1), null, null));

            // Act
            var result = await unitUnderTest.receivedData(Guid.NewGuid().ToString(), new ReceivedGarmentSampleRequestCommand());

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        //[Fact]
        //public async Task GetSingle_PDF_StateUnderTest_ExpectedBehavior()
        //{
        //    // Arrange
        //    var unitUnderTest = CreateGarmentSampleRequestController();
        //    Guid sampleRequestGuid = Guid.NewGuid();
        //    _mockGarmentSampleRequestRepository
        //        .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestReadModel, bool>>>()))
        //        .Returns(new List<GarmentSampleRequest>()
        //        {
        //            new GarmentSampleRequest(Guid.NewGuid(), "","","", "", DateTimeOffset.Now, new BuyerId(1), "", "", new GarmentComodityId(1), "","","","", DateTimeOffset.Now, "","","",
        //            false, false, DateTimeOffset.Now, "",false, DateTimeOffset.Now,null,null,false, DateTimeOffset.Now,null,null,null,null,null,null, new SectionId(1),null)
        //        });

        //    Guid sampleRequestProductGuid = Guid.NewGuid();
        //    Guid sampleRequestSpecificationGuid = Guid.NewGuid();
        //    _mockGarmentSampleRequestProductRepository
        //        .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestProductReadModel, bool>>>()))
        //        .Returns(new List<GarmentSampleRequestProduct>()
        //        {
        //            new GarmentSampleRequestProduct(sampleRequestProductGuid, sampleRequestGuid, "", "", new SizeId(1), "","",1,1)
        //        });

        //    _mockGarmentSampleRequestSpecificationRepository
        //        .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleRequestSpecificationReadModel, bool>>>()))
        //        .Returns(new List<GarmentSampleRequestSpecification>()
        //        {
        //            new GarmentSampleRequestSpecification(sampleRequestSpecificationGuid, sampleRequestGuid, "", "", 1, "", new UomId(1), "",1)
        //        });

        //    // Act
        //    var result = await unitUnderTest.GetPdf(sampleRequestGuid.ToString());

        //    // Assert
        //    Assert.NotNull(result.GetType().GetProperty("FileStream"));
        //}
        [Fact]
        public async Task GetMonitoringBehavior()
        {
            var unitUnderTest = CreateGarmentSampleRequestController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetMonitoringReceiptSampleQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GarmentMonitoringReceiptSampleViewModel());

            // Act
            var result = await unitUnderTest.GetMonitoring( DateTime.Now, DateTime.Now, 1, 25, "{}");

            // Assert
            GetStatusCode(result).Should().Equals((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetXLSPrepareBehavior()
        {
            var unitUnderTest = CreateGarmentSampleRequestController();

            _MockMediator
                .Setup(s => s.Send(It.IsAny<GetXlsReceiptSampleQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream());

            // Act

            var result = await unitUnderTest.GetXls(1, DateTime.Now, DateTime.Now, "", 1, 25, "{}");

            // Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result.GetType().GetProperty("ContentType").GetValue(result, null));
        }
    }
}
