using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentExpenditureGoods.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.ValueObjects;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentExpenditureGoods
{
    public class PlaceGarmentExpenditureGoodCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockExpenditureGoodRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;
        private readonly Mock<IGarmentFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        private readonly Mock<IGarmentFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
		private readonly Mock<IGarmentExpenditureGoodInvoiceRelationRepository> _mockInvoiceRelationRepository;

        public PlaceGarmentExpenditureGoodCommandHandlerTests()
        {
            _mockExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
            _mockExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();
            _mockFinishedGoodStockRepository = CreateMock<IGarmentFinishedGoodStockRepository>();
            _mockFinishedGoodStockHistoryRepository = CreateMock<IGarmentFinishedGoodStockHistoryRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
			_mockInvoiceRelationRepository = CreateMock<IGarmentExpenditureGoodInvoiceRelationRepository>();


			_MockStorage.SetupStorage(_mockExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodItemRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockHistoryRepository);
			_MockStorage.SetupStorage(_mockInvoiceRelationRepository);
			_MockStorage.SetupStorage(_mockComodityPriceRepository);
        }
        private PlaceGarmentExpenditureGoodCommandHandler CreatePlaceGarmentExpenditureGoodCommandHandler()
        {
            return new PlaceGarmentExpenditureGoodCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid finStockGuid = Guid.NewGuid();
            PlaceGarmentExpenditureGoodCommandHandler unitUnderTest = CreatePlaceGarmentExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentExpenditureGoodCommand placeGarmentExpenditureGoodCommand = new PlaceGarmentExpenditureGoodCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Buyer = new Buyer(1, "buyerCode", "buyerName"),
                ExpenditureDate = DateTimeOffset.Now,
				Invoice ="Invoice",
				InvoiceId = 1,
                Items = new List<GarmentExpenditureGoodItemValueObject>
                {
                    new GarmentExpenditureGoodItemValueObject
                    {
                        Uom = new Uom(1, "UomUnit"),
                        FinishedGoodStockId= finStockGuid,
                        Description="Color",
                        Size=new SizeValueObject(1, "Size"),
                        isSave=true,
                        Quantity=1,
                        
                    }
                },

            };

            _mockExpenditureGoodRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReadModel>().AsQueryable());

            GarmentFinishedGoodStock garmentFinishedGoodStock = new GarmentFinishedGoodStock(finStockGuid,
                 "no", placeGarmentExpenditureGoodCommand.RONo, "article",new UnitDepartmentId( placeGarmentExpenditureGoodCommand.Unit.Id), placeGarmentExpenditureGoodCommand.Unit.Code, placeGarmentExpenditureGoodCommand.Unit.Name,
                 new GarmentComodityId(placeGarmentExpenditureGoodCommand.Comodity.Id), placeGarmentExpenditureGoodCommand.Comodity.Code, placeGarmentExpenditureGoodCommand.Comodity.Name,
                 new SizeId(1), null, new UomId(1), null, 1, 1, 1);


            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishedGoodStockReadModel>()
                {
                    garmentFinishedGoodStock.GetReadModel()
                }.AsQueryable());
            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentExpenditureGoodCommand.Unit.Id),
                placeGarmentExpenditureGoodCommand.Unit.Code,
                placeGarmentExpenditureGoodCommand.Unit.Name,
                new GarmentComodityId(placeGarmentExpenditureGoodCommand.Comodity.Id),
                placeGarmentExpenditureGoodCommand.Comodity.Code,
                placeGarmentExpenditureGoodCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockExpenditureGoodRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGood>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGood>()));
            _mockExpenditureGoodItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodItem>()));

            _mockFinishedGoodStockRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishedGoodStock>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishedGoodStock>()));

            _mockFinishedGoodStockHistoryRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishedGoodStockHistory>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishedGoodStockHistory>()));

			 _mockInvoiceRelationRepository
				 .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodInvoiceRelation>()))
				.Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodInvoiceRelation>()));

			_MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentExpenditureGoodCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
