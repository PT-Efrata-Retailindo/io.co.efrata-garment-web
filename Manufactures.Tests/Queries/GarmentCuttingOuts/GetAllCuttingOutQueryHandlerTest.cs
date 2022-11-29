using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentCuttingOuts.Queries;
using Manufactures.Application.GarmentCuttingOuts.Queries.GetAllCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentCuttingOuts
{
    public class GetAllCuttingOutQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentCuttingOutRepository> _mockGarmentCuttingOutRepository;
        private readonly Mock<IGarmentCuttingOutItemRepository> _mockGarmentCuttingOutItemRepository;
        private readonly Mock<IGarmentCuttingOutDetailRepository> _mockGarmentCuttingOutDetailRepository;

        public GetAllCuttingOutQueryHandlerTest()
        {
            _mockGarmentCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockGarmentCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockGarmentCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();

            _MockStorage.SetupStorage(_mockGarmentCuttingOutRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockGarmentCuttingOutDetailRepository);
        }

        private GetAllCuttingOutQueryHandler CreateGetAllCuttingOutQueryHandler()
        {
            return new GetAllCuttingOutQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_GetAllCuttingOut_Success()
        {
            GetAllCuttingOutQueryHandler unitUnderTest = CreateGetAllCuttingOutQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidCuttingOut = Guid.NewGuid();
            Guid guidCuttingOutItem = Guid.NewGuid();
            Guid guidCuttingOutDetail = Guid.NewGuid();

            GarmentCuttingOutReadModel garmentCuttingOut = new GarmentCuttingOut(guidCuttingOut, "cutOutNo", "cuttingOutType", new UnitDepartmentId(1), "unitFromCode", "unitFromName", DateTimeOffset.Now, "rONo", "article", new UnitDepartmentId(1), "unitCode", "unitName", new GarmentComodityId(1), "comodityCode", "comodityName", false).GetReadModel();
            GarmentCuttingOutItemReadModel garmentCuttingOutItem = new GarmentCuttingOutItem(guidCuttingOutItem, Guid.NewGuid(), Guid.NewGuid(), guidCuttingOut, new ProductId(1), "productCode", "productName", "designColor", 10).GetReadModel();
            garmentCuttingOutItem.GarmentCuttingOutDetail.Add(new GarmentCuttingOutDetail(guidCuttingOutDetail, guidCuttingOutItem, new SizeId(1), "sizeName", "color", 10, 10, new UomId(1), "cuttingOutUomUnit", 10, 10).GetReadModel());
            garmentCuttingOut.GarmentCuttingOutItem.Add(garmentCuttingOutItem);

            _mockGarmentCuttingOutRepository.Setup(s => s.Query).Returns(new List<GarmentCuttingOutReadModel>
                {
                    garmentCuttingOut
                }.AsQueryable());
            _mockGarmentCuttingOutItemRepository.Setup(s => s.Query).Returns(new List<GarmentCuttingOutItemReadModel>
                {
                    garmentCuttingOutItem
                }.AsQueryable());
            _mockGarmentCuttingOutDetailRepository.Setup(s => s.Query).Returns(new List<GarmentCuttingOutDetailReadModel>
                {
                    new GarmentCuttingOutDetail(guidCuttingOutDetail, guidCuttingOutItem, new SizeId(1), "sizeName", "color", 10, 10, new UomId(1), "cuttingOutUomUnit", 10, 10).GetReadModel()
                }.AsQueryable());

            GetAllCuttingOutQuery query = new GetAllCuttingOutQuery(1, 25, "{}", "rONo", "{}");

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }
    }
}
