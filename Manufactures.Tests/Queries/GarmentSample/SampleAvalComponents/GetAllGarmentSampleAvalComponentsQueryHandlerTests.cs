using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetAllGarmentSampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleAvalComponents
{
    public class GetAllGarmentSampleAvalComponentsQueryHandlerTests : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentSampleAvalComponentRepository> _mockGarmentSampleAvalComponentRepository;
        private readonly Mock<IGarmentSampleAvalComponentItemRepository> _mockGarmentSampleAvalComponentItemRepository;

        public GetAllGarmentSampleAvalComponentsQueryHandlerTests()
        {
            _mockGarmentSampleAvalComponentRepository = CreateMock<IGarmentSampleAvalComponentRepository>();
            _mockGarmentSampleAvalComponentItemRepository = CreateMock<IGarmentSampleAvalComponentItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleAvalComponentRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalComponentItemRepository);
        }

        private GetAllGarmentSampleAvalComponentsQueryHandler CreateGetAllGarmentSampleAvalComponentsQueryHandler()
        {
            return new GetAllGarmentSampleAvalComponentsQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetAllGarmentSampleAvalComponentsQueryHandler unitUnderTest = CreateGetAllGarmentSampleAvalComponentsQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid avalComponentGuid = Guid.NewGuid();

            GetAllGarmentSampleAvalComponentsQuery query = new GetAllGarmentSampleAvalComponentsQuery(1, 25, "{}", null, "{}");

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.ReadList(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleAvalComponentReadModel>()
                {
                    new GarmentSampleAvalComponentReadModel(avalComponentGuid)
                }.AsQueryable());

            _mockGarmentSampleAvalComponentRepository
                .Setup(s => s.Find(It.IsAny<IQueryable<GarmentSampleAvalComponentReadModel>>()))
                .Returns(new List<GarmentSampleAvalComponent>()
                {
                    new GarmentSampleAvalComponent(Guid.Empty, null, new UnitDepartmentId(1), null, null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now, false)
                });

            _mockGarmentSampleAvalComponentItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleAvalComponentItemReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
