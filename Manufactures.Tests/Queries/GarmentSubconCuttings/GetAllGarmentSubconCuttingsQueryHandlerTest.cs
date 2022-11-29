using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubconCuttings.Queries.GetAllGarmentSubconCuttings;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubconCuttings
{
    public class GetAllGarmentSubconCuttingsQueryHandlerTest : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentSubconCuttingRepository> _mockGarmentSubconCuttingRepository;

        public GetAllGarmentSubconCuttingsQueryHandlerTest()
        {
            _mockGarmentSubconCuttingRepository = CreateMock<IGarmentSubconCuttingRepository>();

            _MockStorage.SetupStorage(_mockGarmentSubconCuttingRepository);
        }

        private GetAllGarmentSubconCuttingsQueryHandler CreateUnitUnderTest()
        {
            return new GetAllGarmentSubconCuttingsQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_Get_All_Success()
        {
            // Arrange
            GetAllGarmentSubconCuttingsQueryHandler unitUnderTest = CreateUnitUnderTest();
            GetAllGarmentSubconCuttingsQuery query = new GetAllGarmentSubconCuttingsQuery(1, 25, "{}", null, "{}");
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSubconCuttingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSubconCuttingReadModel>().AsQueryable());

            _mockGarmentSubconCuttingRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSubconCuttingReadModel>>()))
                .Returns(new List<GarmentSubconCutting>
                {
                    new GarmentSubconCutting(Guid.NewGuid(), "RONo", new SizeId(1), "SizeName", 100, new ProductId(1), "ProductCode", "ProductName", new GarmentComodityId(1), "ComodityCode", "ComodityName", "DesignColor", "Remark", 100)
                });

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assrt
            result.Should().NotBeNull();
        }
    }
}
