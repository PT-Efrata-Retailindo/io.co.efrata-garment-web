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
    public class Sapuan_OutCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapTransactionRepository> _mockGarmentScrapTransactionRepository;
        private readonly Mock<IGarmentScrapTransactionItemRepository> _mockGarmentScrapTransactionItemRepository;

        public Sapuan_OutCommandHandlerTest()
        {
            _mockGarmentScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
            _mockGarmentScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionRepository);
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionItemRepository);
        }

        private Sapuan_Out_QueryHandler CreateSapuan_Out_QueryHandler()
        {
            return new Sapuan_Out_QueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrage
            Sapuan_Out_QueryHandler unitUnderTest = CreateSapuan_Out_QueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

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

            Sapuan_Out_Query sapuan_out = new Sapuan_Out_Query(DateTime.UtcNow, DateTime.UtcNow, "token");

            // Act
            var result = await unitUnderTest.Handle(sapuan_out, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
