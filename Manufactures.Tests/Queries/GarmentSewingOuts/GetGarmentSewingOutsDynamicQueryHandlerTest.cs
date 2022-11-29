using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsByRONo;
using Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsDynamic;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSewingOuts
{
    public class GetGarmentSewingOutsDynamicQueryHandlerTest : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentSewingOutRepository> _mockGarmentSewingOutRepository;

        public GetGarmentSewingOutsDynamicQueryHandlerTest()
        {
            _mockGarmentSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();

            _MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
        }

        GetGarmentSewingOutsDynamicQueryHandler CreateGetGarmentSewingOutsDynamicQueryHandler()
        {
            return new GetGarmentSewingOutsDynamicQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetGarmentSewingOutsDynamicQueryHandler unitUnderTest = CreateGetGarmentSewingOutsDynamicQueryHandler();

            GetGarmentSewingOutsDynamicQuery query = new GetGarmentSewingOutsDynamicQuery(1, 1, "", "", "", "keyword", "filter");
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSewingOutRepository
                .Setup(s => s.ReadDynamic(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<object>().AsQueryable());

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
