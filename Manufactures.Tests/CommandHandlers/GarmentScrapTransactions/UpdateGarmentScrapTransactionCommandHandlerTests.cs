using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapTransactions.CommandHandler;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Manufactures.Domain.GarmentScrapSources.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentScrapTransactions
{

	public class UpdateGarmentScrapTransactionCommandHandlerTests : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentScrapTransactionRepository> _mockScrapTransactionRepository;
		private readonly Mock<IGarmentScrapTransactionItemRepository> _mockScrapTransactionItemRepository;
		private readonly Mock<IGarmentScrapStockRepository> _mockScrapStockRepository;

		public UpdateGarmentScrapTransactionCommandHandlerTests()
		{
			_mockScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
			_mockScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();
			_mockScrapStockRepository = CreateMock<IGarmentScrapStockRepository>();

			_MockStorage.SetupStorage(_mockScrapTransactionRepository);
			_MockStorage.SetupStorage(_mockScrapTransactionItemRepository);
			_MockStorage.SetupStorage(_mockScrapStockRepository);
		}

		private UpdateGarmentScrapTransactionCommandHandler CreateUpdateGarmentScrapTransactionCommandHandler()
		{
			return new UpdateGarmentScrapTransactionCommandHandler(_MockStorage.Object);
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			Guid scrapdestinationId = Guid.NewGuid();
			Guid scrapsourceid = Guid.NewGuid();
			Guid scrapclassificationid = Guid.NewGuid();
			Guid scrapIdentity = Guid.NewGuid();
			UpdateGarmentScrapTransactionCommandHandler unitUnderTest = CreateUpdateGarmentScrapTransactionCommandHandler();
			CancellationToken cancellationToken = CancellationToken.None;
			_mockScrapStockRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentScrapStockReadModel>
				{
					new GarmentScrapStock(new Guid(),scrapdestinationId,"destination",scrapclassificationid,"name",100,1,"KG").GetReadModel()
				}.AsQueryable());

			_mockScrapTransactionRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentScrapTransactionReadModel>
				{
					new GarmentScrapTransaction(scrapIdentity,"","IN",DateTimeOffset.Now,scrapsourceid,"",scrapdestinationId,"").GetReadModel()
				}.AsQueryable());

			_mockScrapTransactionItemRepository
			  .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapTransactionItemReadModel, bool>>>()))
			  .Returns(new List<GarmentScrapTransactionItem>()
			  {
					new GarmentScrapTransactionItem(new Guid(),scrapIdentity,scrapclassificationid,"",100,1,"KG","")
			  });


			UpdateGarmentScrapTransactionCommand updateGarmentScrapTransactionCommand = new UpdateGarmentScrapTransactionCommand()
			{
				TransactionType = "IN",
				TransactionDate = DateTimeOffset.Now,
				ScrapDestinationId = scrapdestinationId,
				ScrapDestinationName = "destination",
				ScrapSourceId = scrapsourceid,
				ScrapSourceName = "source",
				TransactionNo = "",
				Items = new List<GarmentScrapTransactionItemValueObject>
				{
					new GarmentScrapTransactionItemValueObject
					{
						 ScrapClassificationId= scrapclassificationid,
						 ScrapClassificationName="name",
						 Quantity=1000,
						 UomId=1,
						 UomUnit="KG",
						 Description="desc"
					}
				},

			};
			updateGarmentScrapTransactionCommand.SetIdentity(scrapIdentity);

			_mockScrapTransactionRepository
			  .Setup(s => s.Update(It.IsAny<GarmentScrapTransaction>()))
			  .Returns(Task.FromResult(It.IsAny<GarmentScrapTransaction>()));

			_mockScrapTransactionItemRepository
				.Setup(s => s.Update(It.IsAny<GarmentScrapTransactionItem>()))
				.Returns(Task.FromResult(It.IsAny<GarmentScrapTransactionItem>()));

			_mockScrapStockRepository
			  .Setup(s => s.Update(It.IsAny<GarmentScrapStock>()))
			  .Returns(Task.FromResult(It.IsAny<GarmentScrapStock>()));

			_MockStorage
				.Setup(x => x.Save())
				.Verifiable();

			// Act
			var result = await unitUnderTest.Handle(updateGarmentScrapTransactionCommand, cancellationToken);

			// Assert
			result.Should().NotBeNull();

		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehaviorOUT()
		{
			Guid scrapdestinationId = Guid.NewGuid();
			Guid scrapsourceid = Guid.NewGuid();
			Guid scrapclassificationid = Guid.NewGuid();
			Guid scrapIdentity = Guid.NewGuid();
			UpdateGarmentScrapTransactionCommandHandler unitUnderTest = CreateUpdateGarmentScrapTransactionCommandHandler();
			CancellationToken cancellationToken = CancellationToken.None;
			_mockScrapStockRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentScrapStockReadModel>
				{
					new GarmentScrapStock(new Guid(),scrapdestinationId,"destination",scrapclassificationid,"name",100,1,"KG").GetReadModel()
				}.AsQueryable());

			_mockScrapTransactionRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentScrapTransactionReadModel>
				{
					new GarmentScrapTransaction(scrapIdentity,"","OUT",DateTimeOffset.Now,scrapsourceid,"",scrapdestinationId,"").GetReadModel()
				}.AsQueryable());

			_mockScrapTransactionItemRepository
			  .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapTransactionItemReadModel, bool>>>()))
			  .Returns(new List<GarmentScrapTransactionItem>()
			  {
					new GarmentScrapTransactionItem(new Guid(),scrapIdentity,scrapclassificationid,"",100,1,"KG","")
			  });


			UpdateGarmentScrapTransactionCommand updateGarmentScrapTransactionCommand = new UpdateGarmentScrapTransactionCommand()
			{
				TransactionType = "OUT",
				TransactionDate = DateTimeOffset.Now,
				ScrapDestinationId = scrapdestinationId,
				ScrapDestinationName = "destination",
				ScrapSourceId = scrapsourceid,
				ScrapSourceName = "source",
				TransactionNo = "",
				Items = new List<GarmentScrapTransactionItemValueObject>
				{
					new GarmentScrapTransactionItemValueObject
					{
						 ScrapClassificationId= scrapclassificationid,
						 ScrapClassificationName="name",
						 Quantity=1000,
						 UomId=1,
						 UomUnit="KG",
						 Description="desc"
					}
				},

			};
			updateGarmentScrapTransactionCommand.SetIdentity(scrapIdentity);

			_mockScrapTransactionRepository
			  .Setup(s => s.Update(It.IsAny<GarmentScrapTransaction>()))
			  .Returns(Task.FromResult(It.IsAny<GarmentScrapTransaction>()));

			_mockScrapTransactionItemRepository
				.Setup(s => s.Update(It.IsAny<GarmentScrapTransactionItem>()))
				.Returns(Task.FromResult(It.IsAny<GarmentScrapTransactionItem>()));

			_mockScrapStockRepository
			  .Setup(s => s.Update(It.IsAny<GarmentScrapStock>()))
			  .Returns(Task.FromResult(It.IsAny<GarmentScrapStock>()));

			_MockStorage
				.Setup(x => x.Save())
				.Verifiable();

			// Act
			var result = await unitUnderTest.Handle(updateGarmentScrapTransactionCommand, cancellationToken);

			// Assert
			result.Should().NotBeNull();

		}
	}
}
