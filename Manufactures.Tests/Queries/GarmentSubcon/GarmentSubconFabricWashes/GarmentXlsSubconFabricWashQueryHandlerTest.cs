using System;
using System.Collections.Generic;
using System.Text;
using Manufactures.Domain.Shared.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using FluentAssertions;
using Xunit;
using Barebone.Tests;
using Moq;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.Queries;
using System.Threading.Tasks;
using System.Threading;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentServiceSubconFabricWashes
{
    public class GarmentXlsSubconShrinkagePanelQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconFabricWashRepository> _mockgarmentSubconFabricWashRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashItemRepository> _mockgarmentSubconFabricWashItemRepository;
        private readonly Mock<IGarmentServiceSubconFabricWashDetailRepository> _mockgarmentSubconFabricWashDetailRepository;

        private Mock<IServiceProvider> serviceProviderMock;

        public GarmentXlsSubconShrinkagePanelQueryHandlerTest()
        {

            _mockgarmentSubconFabricWashRepository = CreateMock<IGarmentServiceSubconFabricWashRepository>();
            _mockgarmentSubconFabricWashItemRepository = CreateMock<IGarmentServiceSubconFabricWashItemRepository>();
            _mockgarmentSubconFabricWashDetailRepository = CreateMock<IGarmentServiceSubconFabricWashDetailRepository>();

            _MockStorage.SetupStorage(_mockgarmentSubconFabricWashRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconFabricWashItemRepository);
            _MockStorage.SetupStorage(_mockgarmentSubconFabricWashDetailRepository);

            serviceProviderMock = new Mock<IServiceProvider>();

        }

        private GetXlsServiceSubconFabricWashQueryHandler CreateGetXlsServiceSubconFabricWashQueryHandler()
        {
            return new GetXlsServiceSubconFabricWashQueryHandler(_MockStorage.Object, serviceProviderMock.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            GetXlsServiceSubconFabricWashQueryHandler unitUnderTest = CreateGetXlsServiceSubconFabricWashQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconFabricWash = Guid.NewGuid();
            Guid guidSubconFabricWashItem = Guid.NewGuid();
            Guid guidSubconFabricWashDetail = Guid.NewGuid();


            GetXlsServiceSubconFabricWashQuery getMonitoring = new GetXlsServiceSubconFabricWashQuery(1,25,"{}",DateTime.Now, DateTime.Now.AddDays(2), "token");




            _mockgarmentSubconFabricWashRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashReadModel>
                {
                    new GarmentServiceSubconFabricWash(guidSubconFabricWash, "serviceSubconFabricWashNo", DateTimeOffset.Now, "remark", true, 5, "uomUnit").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconFabricWashItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashItemReadModel>
                {
                    new GarmentServiceSubconFabricWashItem(guidSubconFabricWashItem, guidSubconFabricWash, "unitExpenditureNo", DateTimeOffset.Now, new UnitSenderId(1), "unitSenderCode", "unitSenderName", new UnitRequestId(1), "unitRequestCode", "unitRequestName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconFabricWashDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashDetailReadModel>
                {
                    new GarmentServiceSubconFabricWashDetail(guidSubconFabricWashDetail, guidSubconFabricWashItem, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uom" ).GetReadModel()
                }.AsQueryable());

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }

        [Fact]
        public async Task Handle_StateUnderTest_Expected()
        {
            GetXlsServiceSubconFabricWashQueryHandler unitUnderTest = CreateGetXlsServiceSubconFabricWashQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            Guid guidSubconFabricWash = Guid.NewGuid();
            Guid guidSubconFabricWashItem = Guid.NewGuid();
            Guid guidSubconFabricWashDetail = Guid.NewGuid();


            GetXlsServiceSubconFabricWashQuery getMonitoring = new GetXlsServiceSubconFabricWashQuery(1, 25, "{}", DateTime.Now.AddDays(2), DateTime.Now,  "token");




            _mockgarmentSubconFabricWashRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashReadModel>
                {
                    new GarmentServiceSubconFabricWash(guidSubconFabricWash, "serviceSubconFabricWashNo", DateTimeOffset.Now, "remark", true, 5, "uomUnit").GetReadModel()

                }.AsQueryable());

            _mockgarmentSubconFabricWashItemRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashItemReadModel>
                {
                    new GarmentServiceSubconFabricWashItem(guidSubconFabricWashItem, guidSubconFabricWash, "unitExpenditureNo", DateTimeOffset.Now, new UnitSenderId(1), "unitSenderCode", "unitSenderName", new UnitRequestId(1), "unitRequestCode", "unitRequestName").GetReadModel()
                }.AsQueryable());

            _mockgarmentSubconFabricWashDetailRepository
                 .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconFabricWashDetailReadModel>
                {
                    new GarmentServiceSubconFabricWashDetail(guidSubconFabricWashDetail, guidSubconFabricWashItem, new ProductId(1), "productCode", "productName", "productRemark", "designColor", 2, new UomId(1), "uom" ).GetReadModel()
                }.AsQueryable());

            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
