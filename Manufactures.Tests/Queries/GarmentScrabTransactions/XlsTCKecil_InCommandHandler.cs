
using Barebone.Tests;
using FluentAssertions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.TCKecil;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentScrabTransactions
{
    public class XlsTCKecil_InCommandHandler : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapTransactionRepository> _mockGarmentScrapTransactionRepository;
        private readonly Mock<IGarmentScrapTransactionItemRepository> _mockGarmentScrapTransactionItemRepository;

        public XlsTCKecil_InCommandHandler()
        {
            _mockGarmentScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
            _mockGarmentScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionRepository);
            _MockStorage.SetupStorage(_mockGarmentScrapTransactionItemRepository);
       }

        private GetXlsTCKecil_in_QueryHandler CreateGetXlsTCKecil_in_QueryHandler()
        {
            return new GetXlsTCKecil_in_QueryHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // arrage
            GetXlsTCKecil_in_QueryHandler unitundertest = CreateGetXlsTCKecil_in_QueryHandler();
            CancellationToken cancellationtoken = CancellationToken.None;

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
                    new GarmentScrapTransactionItem(guidScrapTransactionItem, guidScrapTransaction, guidScrapClassification, "AVAL TC KECIL", 20, 1, "23", null).GetReadModel(),
                    new GarmentScrapTransactionItem(guidScrapTransactionItem, guidScrapTransaction, guidScrapClassification, "AVAL TC KECIL", 20, 1, "23", null).GetReadModel(),
                    new GarmentScrapTransactionItem(guidScrapTransactionItem, guidScrapTransaction, guidScrapClassification, "AVAL TC KECIL", 20, 1, "23", null).GetReadModel(),

                }.AsQueryable());

            _mockGarmentScrapTransactionRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentScrapTransactionReadModel>
                {
                    new GarmentScrapTransaction(guidScrapTransaction, "1", "IN", DateTimeOffset.Now, guidScrapSource, "GUDANG AVAL", guidScrapDest, "GUDANG AVAL").GetReadModel()
                }.AsQueryable());

            GetXlsTCKecil_in_Query xlstckecil_in = new GetXlsTCKecil_in_Query(DateTime.UtcNow, DateTime.UtcNow, "token");

            // act
            var result = await unitundertest.Handle(xlstckecil_in, cancellationtoken);

            // assert
            result.Should().NotBeNull();
        }
    }
}
