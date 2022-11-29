using Barebone.Tests;
using Manufactures.Application.GarmentFinishingOuts.CommandHandlers;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns;
using FluentAssertions;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;

namespace Manufactures.Tests.CommandHandlers.GarmentFinishingOuts
{
    public class RemoveGarmentFinishingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingOutRepository> _mockFinishingOutRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockFinishingOutItemRepository;
        private readonly Mock<IGarmentFinishingOutDetailRepository> _mockFinishingOutDetailRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        private readonly Mock<IGarmentFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
        private readonly Mock<IGarmentSewingInRepository> _mockSewingInRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;

        public RemoveGarmentFinishingOutCommandHandlerTests()
        {
            _mockFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
            _mockFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();
            _mockFinishingOutDetailRepository = CreateMock<IGarmentFinishingOutDetailRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
            _mockFinishedGoodStockRepository = CreateMock<IGarmentFinishedGoodStockRepository>();
            _mockFinishedGoodStockHistoryRepository = CreateMock<IGarmentFinishedGoodStockHistoryRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
            _mockSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingOutRepository);
            _MockStorage.SetupStorage(_mockFinishingOutItemRepository);
            _MockStorage.SetupStorage(_mockFinishingOutDetailRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockRepository);
            _MockStorage.SetupStorage(_mockFinishedGoodStockHistoryRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
        }
        private RemoveGarmentFinishingOutCommandHandler CreateRemoveGarmentFinishingOutCommandHandler()
        {
            return new RemoveGarmentFinishingOutCommandHandler(_MockStorage.Object);
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_GUDANGJADI()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingOutGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            Guid finishingOutDetailGuid = Guid.NewGuid();
            RemoveGarmentFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentFinishingOutCommand RemoveGarmentFinishingOutCommand = new RemoveGarmentFinishingOutCommand(finishingOutGuid);

            GarmentFinishingOut garmentFinishingOut = new GarmentFinishingOut(
                finishingOutGuid,
                "no", new UnitDepartmentId(1),"uCode","Uname","GUDANG JADI", DateTimeOffset.Now, "ro", "article",
                 new UnitDepartmentId(1), "uCode", "Uname", new GarmentComodityId(1),"cCode", "cName",false );
        
            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>()
                {
                    garmentFinishingOut.GetReadModel()
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutItem>()
                {
                    new GarmentFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutDetailReadModel, bool>>>()))
            //    .Returns(new List<GarmentFinishingOutDetail>()
            //    {
            //        new GarmentFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
            //    });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            GarmentFinishedGoodStock garmentFinishedGoodStock = new GarmentFinishedGoodStock(Guid.NewGuid(),
                "no", "ro", "article", garmentFinishingOut.UnitId,garmentFinishingOut.UnitCode,garmentFinishingOut.UnitName,
                garmentFinishingOut.ComodityId, garmentFinishingOut.ComodityCode, garmentFinishingOut.ComodityName,
                new SizeId(1), null, new UomId(1), null, 1,1,1);

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishedGoodStockReadModel>
                {
                    garmentFinishedGoodStock.GetReadModel()
                }.AsQueryable());

            GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(Guid.NewGuid(), garmentFinishedGoodStock.Identity,
               finishingOutItemGuid,Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, null, "ro", "article", garmentFinishingOut.UnitId, garmentFinishingOut.UnitCode, garmentFinishingOut.UnitName,
               garmentFinishingOut.ComodityId, garmentFinishingOut.ComodityCode, garmentFinishingOut.ComodityName,
               new SizeId(1), null, new UomId(1), null, 1, 1, 1);

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
                garmentFinishingOut.UnitId,
                garmentFinishingOut.UnitCode,
                garmentFinishingOut.UnitName,
                garmentFinishingOut.ComodityId,
                garmentFinishingOut.ComodityCode,
                garmentFinishingOut.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutItem>()));
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentFinishingOutDetail>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));

            _mockFinishedGoodStockRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishedGoodStock>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishedGoodStock>()));

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

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_FINISHING()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingOutGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            Guid finishingOutDetailGuid = Guid.NewGuid();
            RemoveGarmentFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentFinishingOutCommand RemoveGarmentFinishingOutCommand = new RemoveGarmentFinishingOutCommand(finishingOutGuid);

            GarmentFinishingOut garmentFinishingOut = new GarmentFinishingOut(
                finishingOutGuid,
                "no", new UnitDepartmentId(1), "uCode", "Uname", "FINISHING", DateTimeOffset.Now, "ro", "article",
                 new UnitDepartmentId(1), "uCode", "Uname", new GarmentComodityId(1), "cCode", "cName", false);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>()
                {
                    garmentFinishingOut.GetReadModel()
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutItem>()
                {
                    new GarmentFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutDetailReadModel, bool>>>()))
            //    .Returns(new List<GarmentFinishingOutDetail>()
            //    {
            //        new GarmentFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
            //    });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                garmentFinishingOut.UnitId,
                garmentFinishingOut.UnitCode,
                garmentFinishingOut.UnitName,
                garmentFinishingOut.ComodityId,
                garmentFinishingOut.ComodityCode,
                garmentFinishingOut.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutItem>()));
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentFinishingOutDetail>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_DifferentSize()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingOutGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            Guid finishingOutDetailGuid = Guid.NewGuid();
            RemoveGarmentFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentFinishingOutCommand RemoveGarmentFinishingOutCommand = new RemoveGarmentFinishingOutCommand(finishingOutGuid);

            GarmentFinishingOut garmentFinishingOut = new GarmentFinishingOut(
                finishingOutGuid,
                "no", new UnitDepartmentId(1), "uCode", "Uname", "FINISHING", DateTimeOffset.Now, "ro", "article",
                 new UnitDepartmentId(1), "uCode", "Uname", new GarmentComodityId(1), "cCode", "cName", true);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>()
                {
                    garmentFinishingOut.GetReadModel()
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutItem>()
                {
                    new GarmentFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            _mockFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutDetail>()
                {
                    new GarmentFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                garmentFinishingOut.UnitId,
                garmentFinishingOut.UnitCode,
                garmentFinishingOut.UnitName,
                garmentFinishingOut.ComodityId,
                garmentFinishingOut.ComodityCode,
                garmentFinishingOut.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutItem>()));
            _mockFinishingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_Sewing_DifferentSize()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingOutGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            Guid finishingOutDetailGuid = Guid.NewGuid();
            RemoveGarmentFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentFinishingOutCommand RemoveGarmentFinishingOutCommand = new RemoveGarmentFinishingOutCommand(finishingOutGuid);

            GarmentFinishingOut garmentFinishingOut = new GarmentFinishingOut(
                finishingOutGuid,
                "no", new UnitDepartmentId(1), "uCode", "Uname", "SEWING", DateTimeOffset.Now, "ro", "article",
                 new UnitDepartmentId(1), "uCode", "Uname", new GarmentComodityId(1), "cCode", "cName", true);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>()
                {
                    garmentFinishingOut.GetReadModel()
                }.AsQueryable());
            var garmentFinishingOutItem = new GarmentFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty, finishingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1, 1, 1);
            _mockFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutItemReadModel>()
                {
                    garmentFinishingOutItem.GetReadModel()
                }.AsQueryable());

            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutItem>()
                {
                    new GarmentFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            _mockFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutDetail>()
                {
                    new GarmentFinishingOutDetail(finishingOutDetailGuid, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                garmentFinishingOut.UnitId,
                garmentFinishingOut.UnitCode,
                garmentFinishingOut.UnitName,
                garmentFinishingOut.ComodityId,
                garmentFinishingOut.ComodityCode,
                garmentFinishingOut.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            Guid SewingInGuid = Guid.NewGuid();
            GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                SewingInGuid, null, "FINiSHING", Guid.Empty, null, new UnitDepartmentId(1), null, null,
                new UnitDepartmentId(1), null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now);

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    garmentSewingIn.GetReadModel()
                }.AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingInItem>()
                {
                    new GarmentSewingInItem(Guid.Empty,SewingInGuid,Guid.Empty,Guid.Empty,Guid.Empty,finishingOutItemGuid,finishingOutDetailGuid, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0,1,1)
                });


            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutItem>()));
            _mockFinishingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));

            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
