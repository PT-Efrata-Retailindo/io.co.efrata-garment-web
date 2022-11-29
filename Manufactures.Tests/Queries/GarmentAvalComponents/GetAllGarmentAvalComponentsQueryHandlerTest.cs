using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentAvalComponents
{
    public class GetAllGarmentAvalComponentsQueryHandlerTest : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentAvalComponentRepository> _mockGarmentAvalComponentRepository;
        private readonly Mock<IGarmentAvalComponentItemRepository> _mockGarmentAvalComponentItemRepository;

        public GetAllGarmentAvalComponentsQueryHandlerTest()
        {
            _mockGarmentAvalComponentRepository = CreateMock<IGarmentAvalComponentRepository>();
            _mockGarmentAvalComponentItemRepository = CreateMock<IGarmentAvalComponentItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalComponentItemRepository);
        }

        private GetAllGarmentAvalComponentsQueryHandler CreateGetAllGarmentAvalComponentsQueryHandler()
        {
            return new GetAllGarmentAvalComponentsQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetAllGarmentAvalComponentsQueryHandler unitUnderTest = CreateGetAllGarmentAvalComponentsQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            GetAllGarmentAvalComponentsQuery query = new GetAllGarmentAvalComponentsQuery(1, 25, "{}", null, "{}");

            _mockGarmentAvalComponentRepository
                .Setup(s => s.ReadList(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentAvalComponentReadModel>()
                {
                    new GarmentAvalComponentReadModel(avalComponentGuid)
                }.AsQueryable());

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentAvalComponentReadModel>>()))
                .Returns(new List<GarmentAvalComponent>()
                {
                    new GarmentAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false)
                });

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAvalComponentItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
