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

namespace Manufactures.Tests.CommandHandlers.GarmentExpenditureGoods
{
    public class RemoveGarmentExpenditureGoodCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockExpenditureGoodRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;
        private readonly Mock<IGarmentFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        private readonly Mock<IGarmentFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
		private readonly Mock<IGarmentExpenditureGoodInvoiceRelationRepository> _mockInvoiceRelationRepository;
		public RemoveGarmentExpenditureGoodCommandHandlerTest()
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
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
			_MockStorage.SetupStorage(_mockInvoiceRelationRepository);
		}

        private RemoveGarmentExpenditureGoodCommandHandler CreateRemoveGarmentExpenditureGoodCommandHandler()
        {
            return new RemoveGarmentExpenditureGoodCommandHandler(_MockStorage.Object);
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
            RemoveGarmentExpenditureGoodCommandHandler unitUnderTest = CreateRemoveGarmentExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentExpenditureGoodCommand RemoveGarmentFinishingOutCommand = new RemoveGarmentExpenditureGoodCommand(exGoodGuid);

            GarmentExpenditureGood expenditureGood = new GarmentExpenditureGood(
                exGoodGuid, "no", "export", new UnitDepartmentId(1), "uCode", "Uname", "roNo", "art", new GarmentComodityId(1),
                "cCode", "cName", new BuyerId(1), "nam", "bCode", DateTimeOffset.Now, "inv","con",0, null,false,0);

            GarmentFinishedGoodStock garmentFinishedGoodStock = new GarmentFinishedGoodStock(finStockGuid,
                 "no", "ro", "article", expenditureGood.UnitId, expenditureGood.UnitCode, expenditureGood.UnitName,
                 expenditureGood.ComodityId, expenditureGood.ComodityCode, expenditureGood.ComodityName,
                 new SizeId(1), null, new UomId(1), null, 1, 1, 1);

            GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(Guid.NewGuid(), garmentFinishedGoodStock.Identity,
               Guid.Empty, Guid.Empty, exGoodGuid, exGoodItemGuid, Guid.Empty, Guid.Empty, returId, returItemId, null, "ro", "article", expenditureGood.UnitId, expenditureGood.UnitCode, expenditureGood.UnitName,
               expenditureGood.ComodityId, expenditureGood.ComodityCode, expenditureGood.ComodityName,
               new SizeId(1), null, new UomId(1), null, 1, 1, 1);

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishedGoodStockReadModel>
                {
                    garmentFinishedGoodStock.GetReadModel()
                }.AsQueryable());
            _mockExpenditureGoodRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentExpenditureGoodReadModel>
                {
                    expenditureGood.GetReadModel()
                }.AsQueryable());

            _mockExpenditureGoodItemRepository
               .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodItemReadModel, bool>>>()))
               .Returns(new List<GarmentExpenditureGoodItem>()
               {
                    new GarmentExpenditureGoodItem(exGoodItemGuid, exGoodGuid, finStockGuid,new SizeId(1), null, 1,0, new UomId(1), null,null, 1,1)
               });

            //_mockExpenditureGoodItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentExpenditureGoodItemReadModel>
            //    {
            //        new GarmentExpenditureGoodItemReadModel(exGoodItemGuid)
            //    }.AsQueryable());

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

			GarmentExpenditureGoodInvoiceRelation invoiceRelation = new GarmentExpenditureGoodInvoiceRelation(Guid.NewGuid(), expenditureGood.Identity,expenditureGood.ExpenditureGoodNo,"unit","ro",10,expenditureGood.PackingListId,1,expenditureGood.Invoice);

			_mockInvoiceRelationRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodInvoiceRelationReadModel>
				{
					invoiceRelation.GetReadModel()
				}.AsQueryable());

			_mockInvoiceRelationRepository
				 .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodInvoiceRelation>()))
				 .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodInvoiceRelation>()));

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
