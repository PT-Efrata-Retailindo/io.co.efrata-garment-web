using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentScrapTransactions.CommandHandler;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Manufactures.Domain.GarmentScrapSources.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentScrapTransactions
{
	public class PlaceGarmentScrapTransactionCommandHandlerTests : BaseCommandUnitTest
	{
		private readonly Mock<IGarmentScrapTransactionRepository> _mockScrapTransactionRepository;
		private readonly Mock<IGarmentScrapTransactionItemRepository> _mockScrapTransactionItemRepository;
		private readonly Mock<IGarmentScrapStockRepository> _mockScrapStockRepository;

		public PlaceGarmentScrapTransactionCommandHandlerTests()
		{
			_mockScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();
			_mockScrapTransactionItemRepository = CreateMock<IGarmentScrapTransactionItemRepository>();
			_mockScrapStockRepository = CreateMock<IGarmentScrapStockRepository>();

			_MockStorage.SetupStorage(_mockScrapTransactionRepository);
			_MockStorage.SetupStorage(_mockScrapTransactionItemRepository);
			_MockStorage.SetupStorage(_mockScrapStockRepository);
		}

		private PlaceGarmentScrapTransactionCommandHandler CreatePlaceGarmentScrapTransactionCommandHandler()
		{
			return new PlaceGarmentScrapTransactionCommandHandler(_MockStorage.Object);
		}

		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior()
		{
			// Arrange
			Guid scrapdestinationId = Guid.NewGuid();
			Guid scrapsourceid = Guid.NewGuid();
			Guid scrapclassificationid = Guid.NewGuid();
			Guid scrapIdentity = Guid.NewGuid();
			PlaceGarmentScrapTransactionCommandHandler unitUnderTest = CreatePlaceGarmentScrapTransactionCommandHandler();
			CancellationToken cancellationToken = CancellationToken.None;
			_mockScrapStockRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentScrapStockReadModel>
				{
					new GarmentScrapStock(new Guid(),scrapdestinationId,"destination",scrapclassificationid,"name",100,1,"KG").GetReadModel()
				}.AsQueryable());

			PlaceGarmentScrapTransactionCommand placeGarmentScrapTransactionCommand = new PlaceGarmentScrapTransactionCommand()
			{
				TransactionType="IN",
				TransactionDate= DateTimeOffset.Now,
				ScrapDestinationId= scrapdestinationId,
				ScrapDestinationName="destination",
				ScrapSourceId= scrapsourceid,
				ScrapSourceName="source",
				TransactionNo="",
				Items = new List<GarmentScrapTransactionItemValueObject>
				{
					new GarmentScrapTransactionItemValueObject
					{
						 ScrapClassificationId= scrapclassificationid,
						 ScrapClassificationName="name",
						 Quantity=100,
						 UomId=1,
						 UomUnit="KG",
						 Description="desc"
					}
				},

			};

			_mockScrapTransactionRepository
			   .Setup(s => s.Query)
			   .Returns(new List<GarmentScrapTransactionReadModel>().AsQueryable());
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
			var result = await unitUnderTest.Handle(placeGarmentScrapTransactionCommand, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior_OUT()
		{
			// Arrange
			Guid scrapdestinationId = Guid.NewGuid();
			Guid scrapsourceid = Guid.NewGuid();
			Guid scrapclassificationid = Guid.NewGuid();
			Guid scrapIdentity = Guid.NewGuid();
			PlaceGarmentScrapTransactionCommandHandler unitUnderTest = CreatePlaceGarmentScrapTransactionCommandHandler();
			CancellationToken cancellationToken = CancellationToken.None;
			_mockScrapStockRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentScrapStockReadModel>
				{
					new GarmentScrapStock(new Guid(),scrapdestinationId,"destination",scrapclassificationid,"name",100,1,"KG").GetReadModel()
				}.AsQueryable());

			PlaceGarmentScrapTransactionCommand placeGarmentScrapTransactionCommand = new PlaceGarmentScrapTransactionCommand()
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
						 Quantity=100,
						 UomId=1,
						 UomUnit="KG",
						 Description="desc"
					}
				},

			};

			_mockScrapTransactionRepository
			   .Setup(s => s.Query)
			   .Returns(new List<GarmentScrapTransactionReadModel>().AsQueryable());
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
			var result = await unitUnderTest.Handle(placeGarmentScrapTransactionCommand, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}
