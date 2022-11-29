using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample
{
    public class GetAllSampleCuttingOutQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleCuttingOutRepository> _mockGarmentSampleCuttingOutRepository;
        private readonly Mock<IGarmentSampleCuttingOutItemRepository> _mockGarmentSampleCuttingOutItemRepository;
        private readonly Mock<IGarmentSampleCuttingOutDetailRepository> _mockGarmentSampleCuttingOutDetailRepository;

        public GetAllSampleCuttingOutQueryHandlerTest()
        {
            _mockGarmentSampleCuttingOutRepository = CreateMock<IGarmentSampleCuttingOutRepository>();
            _mockGarmentSampleCuttingOutItemRepository = CreateMock<IGarmentSampleCuttingOutItemRepository>();
            _mockGarmentSampleCuttingOutDetailRepository = CreateMock<IGarmentSampleCuttingOutDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleCuttingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleCuttingOutDetailRepository);
        }

        private GetAllSampleCuttingOutQueryHandler CreateGetAllCuttingOutQueryHandler()
        {
            return new GetAllSampleCuttingOutQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_GetAllCuttingOut_Success()
        {
            GetAllSampleCuttingOutQueryHandler unitUnderTest = CreateGetAllCuttingOutQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidCuttingOut = Guid.NewGuid();
            Guid guidCuttingOutItem = Guid.NewGuid();
            Guid guidCuttingOutDetail = Guid.NewGuid();

            GarmentSampleCuttingOutReadModel garmentCuttingOut = new GarmentSampleCuttingOut(guidCuttingOut, "cutOutNo", "cuttingOutType", new UnitDepartmentId(1), "unitFromCode", "unitFromName", DateTimeOffset.Now, "rONo", "article", new UnitDepartmentId(1), "unitCode", "unitName", new GarmentComodityId(1), "comodityCode", "comodityName", false).GetReadModel();
            GarmentSampleCuttingOutItemReadModel garmentCuttingOutItem = new GarmentSampleCuttingOutItem(guidCuttingOutItem, Guid.NewGuid(), Guid.NewGuid(), guidCuttingOut, new ProductId(1), "productCode", "productName", "designColor", 10).GetReadModel();
            garmentCuttingOutItem.GarmentSampleCuttingOutDetail.Add(new GarmentSampleCuttingOutDetail(guidCuttingOutDetail, guidCuttingOutItem, new SizeId(1), "sizeName", "color", 10, 10, new UomId(1), "cuttingOutUomUnit", 10, 10).GetReadModel());
            garmentCuttingOut.GarmentSampleCuttingOutItem.Add(garmentCuttingOutItem);

            _mockGarmentSampleCuttingOutRepository.Setup(s => s.Query).Returns(new List<GarmentSampleCuttingOutReadModel>
                {
                    garmentCuttingOut
                }.AsQueryable());
            _mockGarmentSampleCuttingOutItemRepository.Setup(s => s.Query).Returns(new List<GarmentSampleCuttingOutItemReadModel>
                {
                    garmentCuttingOutItem
                }.AsQueryable());
            _mockGarmentSampleCuttingOutDetailRepository.Setup(s => s.Query).Returns(new List<GarmentSampleCuttingOutDetailReadModel>
                {
                    new GarmentSampleCuttingOutDetail(guidCuttingOutDetail, guidCuttingOutItem, new SizeId(1), "sizeName", "color", 10, 10, new UomId(1), "cuttingOutUomUnit", 10, 10).GetReadModel()
                }.AsQueryable());

            GetAllSampleCuttingOutQuery query = new GetAllSampleCuttingOutQuery(1, 25, "{}", "rONo", "{}");

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
