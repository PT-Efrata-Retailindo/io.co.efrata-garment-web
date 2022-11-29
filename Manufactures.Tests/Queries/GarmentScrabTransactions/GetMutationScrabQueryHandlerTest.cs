using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap;
using Manufactures.Domain.GarmentScrapClassifications;
using Manufactures.Domain.GarmentScrapClassifications.ReadModels;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentScrabTransactions
{
    public class GetMutationScrabQueryHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentScrapTransactionRepository> _mockgarmentScrapTransactionRepository;
        private readonly Mock<IGarmentScrapTransactionItemRepository> _mockgarmentScrapTransactionItemRepository;
        private readonly Mock<IGarmentScrapClassificationRepository> _mockgarmentScrapClassificationRepository;
        //protected readonly Mock<IHttpClientService> _mockhttpService;
        private Mock<IServiceProvider> serviceProviderMock;


        public GetMutationScrabQueryHandlerTest()
        {
            _mockgarmentScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
            _mockgarmentScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();

            _MockStorage.SetupStorage(_mockgarmentScrapTransactionRepository);
            _MockStorage.SetupStorage(_mockgarmentScrapTransactionItemRepository);

            _mockgarmentScrapClassificationRepository = CreateMock<IGarmentScrapClassificationRepository>();

            _MockStorage.SetupStorage(_mockgarmentScrapClassificationRepository);
        }

        private GetMutationScrapQueryHandler CreateGetMutationScrapQueryHandler()
        {
            return new GetMutationScrapQueryHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            GetMutationScrapQueryHandler unitUnderTest = CreateGetMutationScrapQueryHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            Guid guidScrapTransaction = Guid.NewGuid();
            Guid guidScrapTransactionItem = Guid.NewGuid();
            Guid guidScrapClassification = Guid.NewGuid();
            Guid guidScrapSource = Guid.NewGuid();
            Guid guidScrapDest = Guid.NewGuid();

            GetMutationScrapQuery getMonitoring = new GetMutationScrapQuery(DateTime.Now, DateTime.Now.AddDays(2), "token");

            _mockgarmentScrapTransactionItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentScrapTransactionItemReadModel>
                {
                    new GarmentScrapTransactionItem(guidScrapTransactionItem, guidScrapTransaction, guidScrapClassification, "class01", 20, 1, "", "").GetReadModel()
                    //new (guidLoadingItem,guidLoading,new Guid(),new SizeId(1),"",new ProductId(1),"","","",0,0,0, new UomId(1),"","",10).GetReadModel()
                }.AsQueryable());

            _mockgarmentScrapTransactionRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentScrapTransactionReadModel>
                {
                    new GarmentScrapTransaction(guidScrapTransaction, "", "IN", DateTimeOffset.Now, guidScrapSource, "", guidScrapDest, "" ).GetReadModel()
                }.AsQueryable());

            _mockgarmentScrapClassificationRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentScrapClassificationReadModel>
                {
                    new GarmentScrapClassification(guidScrapClassification, "AVP01", "class01", "").GetReadModel()
                }.AsQueryable());

            // Act
            var result = await unitUnderTest.Handle(getMonitoring, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
