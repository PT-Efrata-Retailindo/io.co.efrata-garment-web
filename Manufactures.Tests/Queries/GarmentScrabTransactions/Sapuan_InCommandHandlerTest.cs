using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.SampahSapuan;
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
    public class Sapuan_InCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapTransactionRepository> _mockGarmentScrapTransactionRepository;
        private readonly Mock<IGarmentScrapTransactionItemRepository> _mockGarmentScrapTransactionItemRepository;

        public Sapuan_InCommandHandlerTest()
        {
            _mockGarmentScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
            _mockGarmentScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionRepository);
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionItemRepository);
        }

        private Sapuan_In_QueryHandler CreateSapuan_In_QueryHandler()
        {
            return new Sapuan_In_QueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrage
            Sapuan_In_QueryHandler unitUnderTest = CreateSapuan_In_QueryHandler();
            CancellationToken cancellicationToken = CancellationToken.None;

            Guid guidScrapTransaction = Guid.NewGuid();
            Guid guidScrapTransactionItem = Guid.NewGuid();
            Guid guidScrapClassification = Guid.NewGuid();
            Guid guidScrapSource = Guid.NewGuid();
            Guid guidScrapDest = Guid.NewGuid();

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

            Sapuan_In_Query sapuan_in = new Sapuan_In_Query(DateTime.UtcNow, DateTime.UtcNow, "token");

            // Act
            var result = await unitUnderTest.Handle(sapuan_in, cancellicationToken);

            // Asssert
            result.Should().NotBeNull();
        }
    }
}
