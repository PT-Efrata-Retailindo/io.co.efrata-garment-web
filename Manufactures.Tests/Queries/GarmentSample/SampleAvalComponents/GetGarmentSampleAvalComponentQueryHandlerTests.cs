using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetGarmentSampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleAvalComponents
{
    public class GetGarmentSampleAvalComponentQueryHandlerTests : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentSampleAvalComponentRepository> _mockGarmentSampleAvalComponentRepository;
        private readonly Mock<IGarmentSampleAvalComponentItemRepository> _mockGarmentSampleAvalComponentItemRepository;

        public GetGarmentSampleAvalComponentQueryHandlerTests()
        {
            _mockGarmentSampleAvalComponentRepository = CreateMock<IGarmentSampleAvalComponentRepository>();
            _mockGarmentSampleAvalComponentItemRepository = CreateMock<IGarmentSampleAvalComponentItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalComponentItemRepository);
        }

        private GetGarmentSampleAvalComponentsQueryHandler CreateGetGarmentSampleAvalComponentsQueryHandler()
        {
            return new GetGarmentSampleAvalComponentsQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetGarmentSampleAvalComponentsQueryHandler unitUnderTest = CreateGetGarmentSampleAvalComponentsQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            GetGarmentSampleAvalComponentQuery query = new GetGarmentSampleAvalComponentQuery(avalComponentGuid);

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalComponentReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalComponent>()
                {
                    new GarmentSampleAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false)
                });

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalComponentItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleAvalComponentItem>()
                {
                    new GarmentSampleAvalComponentItem(Guid.Empty, avalComponentGuid, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, null, null, 0, 0, new SizeId(1), null, 0,1)
                });

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
