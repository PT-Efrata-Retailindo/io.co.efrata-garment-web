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
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleExpenditureGoods
{
    public class RemoveGarmentSampleExpenditureGoodCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleExpenditureGoodRepository> _mockExpenditureGoodRepository;
        private readonly Mock<IGarmentSampleExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;
        private readonly Mock<IGarmentSampleFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        private readonly Mock<IGarmentSampleFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentSampleStockRepository> _mockStockRepository;
        private readonly Mock<IGarmentSampleStockHistoryRepository> _mockStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public RemoveGarmentSampleExpenditureGoodCommandHandlerTest()
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

        private RemoveGarmentSampleExpenditureGoodCommandHandler CreateRemoveGarmentSampleExpenditureGoodCommandHandler()
        {
            return new RemoveGarmentSampleExpenditureGoodCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid finStockGuid = Guid.NewGuid();
            Guid exGoodGuid = Guid.NewGuid();
            Guid exGoodItemGuid = Guid.NewGuid();
            Guid returId = Guid.NewGuid();
            Guid returItemId = Guid.NewGuid();
            RemoveGarmentSampleExpenditureGoodCommandHandler unitUnderTest = CreateRemoveGarmentSampleExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSampleExpenditureGoodCommand RemoveGarmentSampleFinishingOutCommand = new RemoveGarmentSampleExpenditureGoodCommand(exGoodGuid);

            GarmentSampleExpenditureGood expenditureGood = new GarmentSampleExpenditureGood(
                exGoodGuid, "no", "ARSIP SAMPLE", new UnitDepartmentId(1), "uCode", "Uname", "roNo", "art", 
                new GarmentComodityId(1), "cCode", "cName", new BuyerId(1), "nam", "bCode", DateTimeOffset.Now, "inv", "con", 0, null, false, 0);

            GarmentSampleFinishedGoodStock garmentFinishedGoodStock = new GarmentSampleFinishedGoodStock(finStockGuid,
                 "no", "roNo", "article", expenditureGood.UnitId, expenditureGood.UnitCode, expenditureGood.UnitName,
                 expenditureGood.ComodityId, expenditureGood.ComodityCode, expenditureGood.ComodityName,
                 new SizeId(1), null, new UomId(1), null, 1, 1, 1);

            GarmentSampleFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentSampleFinishedGoodStockHistory(Guid.NewGuid(), garmentFinishedGoodStock.Identity,
               Guid.Empty, Guid.Empty, exGoodGuid, exGoodItemGuid, Guid.Empty, Guid.Empty, returId, returItemId, null, "ro", "article", expenditureGood.UnitId, expenditureGood.UnitCode, expenditureGood.UnitName,
               expenditureGood.ComodityId, expenditureGood.ComodityCode, expenditureGood.ComodityName,
               new SizeId(1), null, new UomId(1), null, 1, 1, 1);

            GarmentSampleStock garmentStock = new GarmentSampleStock(finStockGuid,"no",
                 "ARSIP SAMPLE", "roNo", "art", expenditureGood.ComodityId, expenditureGood.ComodityCode, 
                 expenditureGood.ComodityName, new SizeId(1), null, new UomId(1), null, 1, "desc");

            GarmentSampleStockHistory garmentStockHistory = new GarmentSampleStockHistory(Guid.NewGuid(), exGoodGuid, exGoodItemGuid, "no", "ARSIP SAMPLE", "roNo", "art",
               expenditureGood.ComodityId, expenditureGood.ComodityCode, expenditureGood.ComodityName,
               new SizeId(1), null, new UomId(1), null, 1, "desc");
            _mockStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleStockReadModel>
                {
                    garmentStock.GetReadModel()
                }.AsQueryable());

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishedGoodStockReadModel>
                {
                    garmentFinishedGoodStock.GetReadModel()
                }.AsQueryable());
            _mockExpenditureGoodRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleExpenditureGoodReadModel>
                {
                    expenditureGood.GetReadModel()
                }.AsQueryable());

            _mockExpenditureGoodItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleExpenditureGoodItemReadModel, bool>>>()))
               .Returns(new List<GarmentSampleExpenditureGoodItem>()
               {
                    new GarmentSampleExpenditureGoodItem(exGoodItemGuid, exGoodGuid, finStockGuid,new SizeId(1), null, 1,0, new UomId(1), null,"desc", 1,1)
               });

            //_mockExpenditureGoodItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSampleExpenditureGoodItemReadModel>
            //    {
            //        new GarmentSampleExpenditureGoodItemReadModel(exGoodItemGuid)
            //    }.AsQueryable());

            _mockFinishedGoodStockHistoryRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleFinishedGoodStockHistoryReadModel>
                {
                    garmentFinishedGoodStockHistory.GetReadModel()
                }.AsQueryable());

            _mockStockHistoryRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleStockHistoryReadModel>
                {
                    garmentStockHistory.GetReadModel()
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(expenditureGood.UnitId.Value),
                expenditureGood.UnitCode,
                expenditureGood.UnitName,
                new GarmentComodityId(expenditureGood.ComodityId.Value),
                expenditureGood.ComodityCode,
                expenditureGood.ComodityName,
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
            var result = await unitUnderTest.Handle(RemoveGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}