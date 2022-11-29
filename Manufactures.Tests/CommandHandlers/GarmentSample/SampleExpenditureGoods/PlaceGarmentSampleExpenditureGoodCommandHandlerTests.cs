using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleExpenditureGoods.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentSample.SampleStocks;
using Manufactures.Domain.GarmentSample.SampleStocks.ReadModels;
using Manufactures.Domain.GarmentSample.SampleStocks.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleExpenditureGoods
{
    public class PlaceGarmentSampleExpenditureGoodCommandHandlerTests: BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleExpenditureGoodRepository> _mockExpenditureGoodRepository;
        private readonly Mock<IGarmentSampleExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;
        private readonly Mock<IGarmentSampleFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        private readonly Mock<IGarmentSampleFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
        private readonly Mock<IGarmentSampleStockRepository> _mockStockRepository;
        private readonly Mock<IGarmentSampleStockHistoryRepository> _mockStockHistoryRepository;

        public PlaceGarmentSampleExpenditureGoodCommandHandlerTests()
        {
            _mockExpenditureGoodRepository = CreateMock<IGarmentSampleExpenditureGoodRepository>();
            _mockExpenditureGoodItemRepository = CreateMock<IGarmentSampleExpenditureGoodItemRepository>();
            _mockFinishedGoodStockRepository = CreateMock<IGarmentSampleFinishedGoodStockRepository>();
            _mockFinishedGoodStockHistoryRepository = CreateMock<IGarmentSampleFinishedGoodStockHistoryRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
            _mockStockRepository = CreateMock<IGarmentSampleStockRepository>();
            _mockStockHistoryRepository = CreateMock<IGarmentSampleStockHistoryRepository>();

            _MockStorage.SetupStorage(_mockExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodItemRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockHistoryRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
            _MockStorage.SetupStorage(_mockStockRepository);
            _MockStorage.SetupStorage(_mockStockHistoryRepository);
        }
        private PlaceGarmentSampleExpenditureGoodCommandHandler CreatePlaceGarmentSampleExpenditureGoodCommandHandler()
        {
            return new PlaceGarmentSampleExpenditureGoodCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid finStockGuid = Guid.NewGuid();
            PlaceGarmentSampleExpenditureGoodCommandHandler unitUnderTest = CreatePlaceGarmentSampleExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleExpenditureGoodCommand placeGarmentSampleExpenditureGoodCommand = new PlaceGarmentSampleExpenditureGoodCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                ExpenditureType="EXPORT",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Buyer = new Buyer(1, "buyerCode", "buyerName"),
                ExpenditureDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleExpenditureGoodItemValueObject>
                {
                    new GarmentSampleExpenditureGoodItemValueObject
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
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>().AsQueryable());

            GarmentSampleFinishedGoodStock garmentFinishedGoodStock = new GarmentSampleFinishedGoodStock(finStockGuid,
                 "no", placeGarmentSampleExpenditureGoodCommand.RONo, "article",new UnitDepartmentId( placeGarmentSampleExpenditureGoodCommand.Unit.Id), placeGarmentSampleExpenditureGoodCommand.Unit.Code, placeGarmentSampleExpenditureGoodCommand.Unit.Name,
                 new GarmentComodityId(placeGarmentSampleExpenditureGoodCommand.Comodity.Id), placeGarmentSampleExpenditureGoodCommand.Comodity.Code, placeGarmentSampleExpenditureGoodCommand.Comodity.Name,
                 new SizeId(1), null, new UomId(1), null, 1, 1, 1);


            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishedGoodStockReadModel>()
                {
                    garmentFinishedGoodStock.GetReadModel()
                }.AsQueryable());
            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentSampleExpenditureGoodCommand.Unit.Id),
                placeGarmentSampleExpenditureGoodCommand.Unit.Code,
                placeGarmentSampleExpenditureGoodCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSampleExpenditureGoodCommand.Comodity.Id),
                placeGarmentSampleExpenditureGoodCommand.Comodity.Code,
                placeGarmentSampleExpenditureGoodCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockExpenditureGoodRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleExpenditureGood>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleExpenditureGood>()));
            _mockExpenditureGoodItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleExpenditureGoodItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleExpenditureGoodItem>()));

            _mockFinishedGoodStockRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishedGoodStock>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishedGoodStock>()));

            _mockFinishedGoodStockHistoryRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishedGoodStockHistory>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishedGoodStockHistory>()));


            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSampleExpenditureGoodCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_Arsip()
        {
            // Arrange
            Guid finStockGuid = Guid.NewGuid();
            PlaceGarmentSampleExpenditureGoodCommandHandler unitUnderTest = CreatePlaceGarmentSampleExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleExpenditureGoodCommand placeGarmentSampleExpenditureGoodCommand = new PlaceGarmentSampleExpenditureGoodCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                ExpenditureType = "ARSIP SAMPLE",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Buyer = new Buyer(1, "buyerCode", "buyerName"),
                ExpenditureDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleExpenditureGoodItemValueObject>
                {
                    new GarmentSampleExpenditureGoodItemValueObject
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
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>().AsQueryable());

            GarmentSampleFinishedGoodStock garmentFinishedGoodStock = new GarmentSampleFinishedGoodStock(finStockGuid,
                 "no", placeGarmentSampleExpenditureGoodCommand.RONo, "article", new UnitDepartmentId(placeGarmentSampleExpenditureGoodCommand.Unit.Id), placeGarmentSampleExpenditureGoodCommand.Unit.Code, placeGarmentSampleExpenditureGoodCommand.Unit.Name,
                 new GarmentComodityId(placeGarmentSampleExpenditureGoodCommand.Comodity.Id), placeGarmentSampleExpenditureGoodCommand.Comodity.Code, placeGarmentSampleExpenditureGoodCommand.Comodity.Name,
                 new SizeId(1), null, new UomId(1), null, 1, 1, 1);


            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishedGoodStockReadModel>()
                {
                    garmentFinishedGoodStock.GetReadModel()
                }.AsQueryable());

            GarmentSampleStock garmentStock = new GarmentSampleStock(finStockGuid, "no",
                 "ARSIP SAMPLE", "roNo", "art", garmentFinishedGoodStock.ComodityId, garmentFinishedGoodStock.ComodityCode,
                 garmentFinishedGoodStock.ComodityName, new SizeId(1), null, new UomId(1), null, 1, "Color");

            GarmentSampleStockHistory garmentStockHistory = new GarmentSampleStockHistory(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "no", "ARSIP SAMPLE", "roNo", "art",
               garmentFinishedGoodStock.ComodityId, garmentFinishedGoodStock.ComodityCode, garmentFinishedGoodStock.ComodityName,
               new SizeId(1), null, new UomId(1), null, 1, "Color");
            _mockStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleStockReadModel>
                {
                    garmentStock.GetReadModel()
                }.AsQueryable());
            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentSampleExpenditureGoodCommand.Unit.Id),
                placeGarmentSampleExpenditureGoodCommand.Unit.Code,
                placeGarmentSampleExpenditureGoodCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSampleExpenditureGoodCommand.Comodity.Id),
                placeGarmentSampleExpenditureGoodCommand.Comodity.Code,
                placeGarmentSampleExpenditureGoodCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockExpenditureGoodRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleExpenditureGood>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleExpenditureGood>()));
            _mockExpenditureGoodItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleExpenditureGoodItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleExpenditureGoodItem>()));

            _mockFinishedGoodStockRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishedGoodStock>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishedGoodStock>()));

            _mockFinishedGoodStockHistoryRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishedGoodStockHistory>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishedGoodStockHistory>()));
            _mockStockRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleStock>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleStock>()));

            _mockStockHistoryRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleStockHistory>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleStockHistory>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSampleExpenditureGoodCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}