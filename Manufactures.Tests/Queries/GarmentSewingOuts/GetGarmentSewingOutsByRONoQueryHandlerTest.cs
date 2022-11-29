using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingOuts.Queries.GetGarmentSewingOutsByRONo;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSewingOuts
{
    public class GetGarmentSewingOutsByRONoQueryHandlerTest : BaseQueryUnitTest
    {
        private readonly Mock<IGarmentSewingOutRepository> _mockGarmentSewingOutRepository;

        public GetGarmentSewingOutsByRONoQueryHandlerTest()
        {
            _mockGarmentSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();

            _MockStorage.SetupStorage(_mockGarmentSewingOutRepository);
        }

        GetGarmentSewingOutsByRONoQueryHandler CreateGetGarmentSewingOutsByRONoQueryHandler()
        {
            return new GetGarmentSewingOutsByRONoQueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetGarmentSewingOutsByRONoQueryHandler unitUnderTest = CreateGetGarmentSewingOutsByRONoQueryHandler();

            GetGarmentSewingOutsByRONoQuery query = new GetGarmentSewingOutsByRONoQuery("keyword", "filter");
            CancellationToken cancellationToken = CancellationToken.None;

            _mockGarmentSewingOutRepository
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new List<GarmentSewingOutReadModel>().AsQueryable());

            // Act
            var result = await unitUnderTest.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
