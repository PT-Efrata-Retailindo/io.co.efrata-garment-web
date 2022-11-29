using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentExpenditureGoodReturns.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ValueObjects;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentReturGoodReturns.Commands;
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
    public class PlaceGarmentExpenditureGoodReturnCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodReturnRepository> _mockExpenditureGoodReturnRepository;
        private readonly Mock<IGarmentExpenditureGoodReturnItemRepository> _mockExpenditureGoodReturnItemRepository;
        private readonly Mock<IGarmentFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        private readonly Mock<IGarmentFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockExpenditureGoodRepository;

        public PlaceGarmentExpenditureGoodReturnCommandHandlerTests()
        {
            _mockExpenditureGoodReturnRepository = CreateMock<IGarmentExpenditureGoodReturnRepository>();
            _mockExpenditureGoodReturnItemRepository = CreateMock<IGarmentExpenditureGoodReturnItemRepository>();
            _mockFinishedGoodStockRepository = CreateMock<IGarmentFinishedGoodStockRepository>();
            _mockFinishedGoodStockHistoryRepository = CreateMock<IGarmentFinishedGoodStockHistoryRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
            _mockExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();
            _mockExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();

            _MockStorage.SetupStorage(_mockExpenditureGoodReturnRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodReturnItemRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockHistoryRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodItemRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodRepository);
        }
        private PlaceGarmentExpenditureGoodReturnCommandHandler CreatePlaceGarmentExpenditureGoodReturnCommandHandler()
        {
            return new PlaceGarmentExpenditureGoodReturnCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid finStockGuid = Guid.NewGuid();
            Guid exGoodGuid = Guid.NewGuid();
            Guid exGoodItemGuid = Guid.NewGuid();
            PlaceGarmentExpenditureGoodReturnCommandHandler unitUnderTest = CreatePlaceGarmentExpenditureGoodReturnCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentExpenditureGoodReturnCommand placeGarmentExpenditureGoodReturnCommand = new PlaceGarmentExpenditureGoodReturnCommand()
            {
                ExpenditureNo = "ExpenditureNo",
                DONo = "DONo",
                URNNo = "URNNo",
                BCNo = "BCNo",
                BCType = "BCType",
                Invoice = "Invoice",
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Buyer = new Buyer(1, "buyerCode", "buyerName"),
                ReturDate = DateTimeOffset.Now,
                Items = new List<GarmentExpenditureGoodReturnItemValueObject>
                {
                    new GarmentExpenditureGoodReturnItemValueObject
                    {
                        Uom = new Uom(1, "UomUnit"),
                        FinishedGoodStockId= finStockGuid,
                        ExpenditureGoodId=exGoodGuid,
                        ExpenditureGoodItemId=exGoodItemGuid,
                        Description="Color",
                        Size=new SizeValueObject(1, "Size"),
                        isSave=true,
                        Quantity=1,

                    }
                },

            };

            _mockExpenditureGoodReturnRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReturnReadModel>().AsQueryable());
            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishedGoodStockReadModel>()
                {
                    new GarmentFinishedGoodStockReadModel(finStockGuid)
                }.AsQueryable());

            GarmentExpenditureGood garmentExpenditureGood = new GarmentExpenditureGood(exGoodGuid, placeGarmentExpenditureGoodReturnCommand.ExpenditureNo, null,
                new UnitDepartmentId(placeGarmentExpenditureGoodReturnCommand.Unit.Id), null,null, placeGarmentExpenditureGoodReturnCommand.RONo,null,new GarmentComodityId(1),
                null,null,new BuyerId(1),null,null,DateTimeOffset.Now,null,null,0,null,false,0);

            GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(
                exGoodItemGuid, exGoodGuid, finStockGuid, new SizeId(1), null, 1, 0, new UomId(1), null, "Color", 1, 1);

            _mockExpenditureGoodRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReadModel>
                {
                    garmentExpenditureGood.GetReadModel()
                }.AsQueryable());

            _mockExpenditureGoodItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodItemReadModel>
                {
                    garmentExpenditureGoodItem.GetReadModel()
                }.AsQueryable());



            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentExpenditureGoodReturnCommand.Unit.Id),
                placeGarmentExpenditureGoodReturnCommand.Unit.Code,
                placeGarmentExpenditureGoodReturnCommand.Unit.Name,
                new GarmentComodityId(placeGarmentExpenditureGoodReturnCommand.Comodity.Id),
                placeGarmentExpenditureGoodReturnCommand.Comodity.Code,
                placeGarmentExpenditureGoodReturnCommand.Comodity.Name,
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

            _mockFinishedGoodStockHistoryRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishedGoodStockHistory>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishedGoodStockHistory>()));

            _mockExpenditureGoodItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodItem>()));

            //_mockExpenditureGoodRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentExpenditureGood>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGood>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentExpenditureGoodReturnCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
