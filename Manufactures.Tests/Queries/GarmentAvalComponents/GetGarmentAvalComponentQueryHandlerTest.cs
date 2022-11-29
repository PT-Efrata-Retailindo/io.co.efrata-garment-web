using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalComponents.Queries.GetGarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Queries.GetGarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentAvalComponents
{
    public class GetGarmentAvalComponentQueryHandlerTest : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentAvalComponentRepository> _mockGarmentAvalComponentRepository;
        private readonly Mock<IGarmentAvalComponentItemRepository> _mockGarmentAvalComponentItemRepository;

        public GetGarmentAvalComponentQueryHandlerTest()
        {
            _mockGarmentAvalComponentRepository = CreateMock<IGarmentAvalComponentRepository>();
            _mockGarmentAvalComponentItemRepository = CreateMock<IGarmentAvalComponentItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalComponentItemRepository);
        }

        private GetGarmentAvalComponentsQueryHandler CreateGetGarmentAvalComponentsQueryHandler()
        {
            return new GetGarmentAvalComponentsQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetGarmentAvalComponentsQueryHandler unitUnderTest = CreateGetGarmentAvalComponentsQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            GetGarmentAvalComponentQuery query = new GetGarmentAvalComponentQuery(avalComponentGuid);

            _mockGarmentAvalComponentRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalComponentReadModel, bool>>>()))
                .Returns(new List<GarmentAvalComponent>()
                {
                    new GarmentAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false)
                });

            _mockGarmentAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentAvalComponentItem>()
                {
                    new GarmentAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
