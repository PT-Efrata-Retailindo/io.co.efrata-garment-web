using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsDynamic;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleSewingOuts
{
    public class GetGarmentSampleSewingOutsDynamicQueryHandlerTest : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockGarmentSampleSewingOutRepository;

        public GetGarmentSampleSewingOutsDynamicQueryHandlerTest()
        {
            _mockGarmentSampleSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleSewingOutRepository);
        }

        GetGarmentSampleSewingOutsDynamicQueryHandler CreateGetGarmentSampleSewingOutsDynamicQueryHandler()
        {
            return new GetGarmentSampleSewingOutsDynamicQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetGarmentSampleSewingOutsDynamicQueryHandler unitUnderTest = CreateGetGarmentSampleSewingOutsDynamicQueryHandler();

            GetGarmentSampleSewingOutsDynamicQuery query = new GetGarmentSampleSewingOutsDynamicQuery(1, 1, "", "", "", "keyword", "filter");
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSampleSewingOutRepository
                .Setup(s => s.ReadDynamic(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<object>().AsQueryable());

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
