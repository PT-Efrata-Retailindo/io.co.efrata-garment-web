using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingOuts.Queries.GetGarmentSampleSewingOutsByRONo;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
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
    public class GetGarmentSampleSewingOutsByRONoQueryHandlerTest : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockGarmentSampleSewingOutRepository;

        public GetGarmentSampleSewingOutsByRONoQueryHandlerTest()
        {
            _mockGarmentSampleSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleSewingOutRepository);
        }

        GetGarmentSampleSewingOutsByRONoQueryHandler CreateGetGarmentSampleSewingOutsByRONoQueryHandler()
        {
            return new GetGarmentSampleSewingOutsByRONoQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetGarmentSampleSewingOutsByRONoQueryHandler unitUnderTest = CreateGetGarmentSampleSewingOutsByRONoQueryHandler();

            GetGarmentSampleSewingOutsByRONoQuery query = new GetGarmentSampleSewingOutsByRONoQuery("keyword", "filter");
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSampleSewingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSampleSewingOutReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
