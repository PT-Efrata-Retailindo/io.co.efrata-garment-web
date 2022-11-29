using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentExpenditureGoodReturns.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Commands;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentExpenditureGoodReturns
{
    public class RemoveGarmentExpenditureGoodReturnCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodReturnRepository> _mockExpenditureGoodReturnRepository;
        private readonly Mock<IGarmentExpenditureGoodReturnItemRepository> _mockExpenditureGoodReturnItemRepository;
        private readonly Mock<IGarmentFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        private readonly Mock<IGarmentFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;

        public RemoveGarmentExpenditureGoodReturnCommandHandlerTests()
        {
            _mockExpenditureGoodReturnRepository = CreateMock<IGarmentExpenditureGoodReturnRepository>();
            _mockExpenditureGoodReturnItemRepository = CreateMock<IGarmentExpenditureGoodReturnItemRepository>();
            _mockFinishedGoodStockRepository = CreateMock<IGarmentFinishedGoodStockRepository>();
            _mockFinishedGoodStockHistoryRepository = CreateMock<IGarmentFinishedGoodStockHistoryRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
            _mockExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();

            _MockStorage.SetupStorage(_mockExpenditureGoodReturnRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodReturnItemRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockHistoryRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodItemRepository);
        }

        private RemoveGarmentExpenditureGoodReturnCommandHandler CreateRemoveGarmentExpenditureGoodReturnCommandHandler()
        {
            return new RemoveGarmentExpenditureGoodReturnCommandHandler(_MockStorage.Object);
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
            RemoveGarmentExpenditureGoodReturnCommandHandler unitUnderTest = CreateRemoveGarmentExpenditureGoodReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentExpenditureGoodReturnCommand RemoveGarmentFinishingOutCommand = new RemoveGarmentExpenditureGoodReturnCommand(returId);

            GarmentExpenditureGoodReturn expenditureGoodReturn = new GarmentExpenditureGoodReturn(
                returId, "no","export", "expenditureno", "dono", "urnno", "bcno", "bctype", new UnitDepartmentId(1), "uCode", "Uname", "roNo","art", new GarmentComodityId(1),
                "cCode", "cName", new BuyerId(1),"nam","bCode", DateTimeOffset.Now,"inv",null);

            GarmentFinishedGoodStock garmentFinishedGoodStock = new GarmentFinishedGoodStock(finStockGuid,
                 "no", "ro", "article", expenditureGoodReturn.UnitId, expenditureGoodReturn.UnitCode, expenditureGoodReturn.UnitName,
                 expenditureGoodReturn.ComodityId, expenditureGoodReturn.ComodityCode, expenditureGoodReturn.ComodityName,
                 new SizeId(1), null, new UomId(1), null, 1, 1, 1);

            GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(Guid.NewGuid(), garmentFinishedGoodStock.Identity,
               Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, returId, returItemId, null, "ro", "article", expenditureGoodReturn.UnitId, expenditureGoodReturn.UnitCode, expenditureGoodReturn.UnitName,
               expenditureGoodReturn.ComodityId, expenditureGoodReturn.ComodityCode, expenditureGoodReturn.ComodityName,
               new SizeId(1), null, new UomId(1), null, 1, 1, 1);

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishedGoodStockReadModel>
                {
                    garmentFinishedGoodStock.GetReadModel()
                }.AsQueryable());
            _mockExpenditureGoodReturnRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentExpenditureGoodReturnReadModel>
                {
                    expenditureGoodReturn.GetReadModel()
                }.AsQueryable());

            _mockExpenditureGoodReturnItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodReturnItemReadModel, bool>>>()))
               .Returns(new List<GarmentExpenditureGoodReturnItem>()
               {
                    new GarmentExpenditureGoodReturnItem(returItemId,returId,  Guid.Empty,exGoodItemGuid,finStockGuid,new SizeId(1), null, 1, new UomId(1), null,null, 1,1)
               });

            _mockExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>
                {
                    new GarmentExpenditureGoodItemReadModel(exGoodItemGuid)
                }.AsQueryable());

            _mockFinishedGoodStockHistoryRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentFinishedGoodStockHistoryReadModel>
                {
                    garmentFinishedGoodStockHistory.GetReadModel()
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(expenditureGoodReturn.UnitId.Value),
                expenditureGoodReturn.UnitCode,
                expenditureGoodReturn.UnitName,
                new GarmentComodityId(expenditureGoodReturn.ComodityId.Value),
                expenditureGoodReturn.ComodityCode,
                expenditureGoodReturn.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockExpenditureGoodReturnRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodReturn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodReturn>()));
            _mockExpenditureGoodReturnItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodReturnItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodReturnItem>()));

            _mockFinishedGoodStockRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishedGoodStock>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishedGoodStock>()));

            _mockExpenditureGoodItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodItem>()));

            _mockFinishedGoodStockHistoryRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishedGoodStockHistory>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishedGoodStockHistory>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
