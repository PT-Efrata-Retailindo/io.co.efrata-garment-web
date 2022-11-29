using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.TCKecil;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentScrabTransactions
{
    public class TCKecil_InCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapTransactionRepository> _mockGarmentScrapTransactionRepository;
        private readonly Mock<IGarmentScrapTransactionItemRepository> _mockGarmentScrapTransactionItemRepository;

        public TCKecil_InCommandHandlerTest()
        {
            _mockGarmentScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
            _mockGarmentScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionRepository);
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionItemRepository);
        }

        private TCKecil_In_QueryHandler CreateTCKecil_In_QueryHandler()
        {
            return new TCKecil_In_QueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrage
            TCKecil_In_QueryHandler unitUnderTest = CreateTCKecil_In_QueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidScrapTransaction = Guid.NewGuid();
            Guid guidScrapTransactionItem = Guid.NewGuid();
            Guid guidScrapClassification = Guid.NewGuid();
            Guid guidScrapSource = Guid.NewGuid();
            Guid guidScrapDest = Guid.NewGuid();

            TCKecil_In_Query tckecil_in = new TCKecil_In_Query(DateTime.UtcNow, DateTime.UtcNow, "token");

            _mockGarmentScrapTransactionItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentScrapTransactionItemReadModel>
                {
                    new GarmentScrapTransactionItem(guidScrapTransactionItem, guidScrapTransaction, guidScrapClassification, "AVAL TC KECIL", 20, 1, "23", null).GetReadModel(),
                }.AsQueryable());

            _mockGarmentScrapTransactionRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentScrapTransactionReadModel>
                {
                    new GarmentScrapTransaction(guidScrapTransaction, "1", "IN", DateTimeOffset.Now, guidScrapSource, "GUDANG AVAL", guidScrapDest, "GUDANG AVAL").GetReadModel()
                }.AsQueryable());

            // Act
            var result = await unitUnderTest.Handle(tckecil_in, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
