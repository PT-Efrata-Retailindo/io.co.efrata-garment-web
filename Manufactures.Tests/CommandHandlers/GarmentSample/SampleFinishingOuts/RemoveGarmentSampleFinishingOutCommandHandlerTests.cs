using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleFinishingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleFinishingOuts
{
    public class RemoveGarmentSampleFinishingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleFinishingOutRepository> _mockFinishingOutRepository;
        private readonly Mock<IGarmentSampleFinishingOutItemRepository> _mockFinishingOutItemRepository;
        private readonly Mock<IGarmentSampleFinishingOutDetailRepository> _mockFinishingOutDetailRepository;
        private readonly Mock<IGarmentSampleFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentSampleFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
        private readonly Mock<IGarmentSampleFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
        private readonly Mock<IGarmentSampleSewingInRepository> _mockSewingInRepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingInItemRepository;

        public RemoveGarmentSampleFinishingOutCommandHandlerTests()
        {
            _mockFinishingOutRepository = CreateMock<IGarmentSampleFinishingOutRepository>();
            _mockFinishingOutItemRepository = CreateMock<IGarmentSampleFinishingOutItemRepository>();
            _mockFinishingOutDetailRepository = CreateMock<IGarmentSampleFinishingOutDetailRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentSampleFinishingInItemRepository>();
            _mockFinishedGoodStockRepository = CreateMock<IGarmentSampleFinishedGoodStockRepository>();
            _mockFinishedGoodStockHistoryRepository = CreateMock<IGarmentSampleFinishedGoodStockHistoryRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
            _mockSewingInRepository = CreateMock<IGarmentSampleSewingInRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();

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
        private RemoveGarmentSampleFinishingOutCommandHandler CreateRemoveGarmentSampleFinishingOutCommandHandler()
        {
            return new RemoveGarmentSampleFinishingOutCommandHandler(_MockStorage.Object);
        }
        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_GUDANGJADI()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingOutGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            Guid finishingOutDetailGuid = Guid.NewGuid();
            RemoveGarmentSampleFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSampleFinishingOutCommand removeGarmentSampleFinishingOutCommand = new RemoveGarmentSampleFinishingOutCommand(finishingOutGuid);

            GarmentSampleFinishingOut garmentSampleFinishingOut = new GarmentSampleFinishingOut(
                finishingOutGuid,
                "no", new UnitDepartmentId(1), "uCode", "Uname", "GUDANG JADI", DateTimeOffset.Now, "ro", "article",
                 new UnitDepartmentId(1), "uCode", "Uname", new GarmentComodityId(1), "cCode", "cName", false);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutReadModel>()
                {
                    garmentSampleFinishingOut.GetReadModel()
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutItem>()
                {
                    new GarmentSampleFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutDetailReadModel, bool>>>()))
            //    .Returns(new List<GarmentSampleFinishingOutDetail>()
            //    {
            //        new GarmentSampleFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
            //    });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInItemReadModel>
                {
                    new GarmentSampleFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            GarmentSampleFinishedGoodStock garmentSampleFinishedGoodStock = new GarmentSampleFinishedGoodStock(Guid.NewGuid(),
                "no", "ro", "article", garmentSampleFinishingOut.UnitId, garmentSampleFinishingOut.UnitCode, garmentSampleFinishingOut.UnitName,
                garmentSampleFinishingOut.ComodityId, garmentSampleFinishingOut.ComodityCode, garmentSampleFinishingOut.ComodityName,
                new SizeId(1), null, new UomId(1), null, 1, 1, 1);

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishedGoodStockReadModel>
                {
                    garmentSampleFinishedGoodStock.GetReadModel()
                }.AsQueryable());

            GarmentSampleFinishedGoodStockHistory GarmentSampleFinishedGoodStockHistory = new GarmentSampleFinishedGoodStockHistory(Guid.NewGuid(), garmentSampleFinishingOut.Identity,
               finishingOutItemGuid, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, null, "ro", "article", 
               garmentSampleFinishingOut.UnitId, garmentSampleFinishingOut.UnitCode, garmentSampleFinishingOut.UnitName,
               garmentSampleFinishingOut.ComodityId, garmentSampleFinishingOut.ComodityCode, garmentSampleFinishingOut.ComodityName,
               new SizeId(1), null, new UomId(1), null, 1, 1, 1);

            _mockFinishedGoodStockHistoryRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishedGoodStockHistoryReadModel>
                {
                    GarmentSampleFinishedGoodStockHistory.GetReadModel()
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                garmentSampleFinishingOut.UnitId,
                garmentSampleFinishingOut.UnitCode,
                garmentSampleFinishingOut.UnitName,
                garmentSampleFinishingOut.ComodityId,
                garmentSampleFinishingOut.ComodityCode,
                garmentSampleFinishingOut.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutItem>()));
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutDetail>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));

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
            var result = await unitUnderTest.Handle(removeGarmentSampleFinishingOutCommand, cancellationToken);

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
            RemoveGarmentSampleFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSampleFinishingOutCommand RemoveGarmentSampleFinishingOutCommand = new RemoveGarmentSampleFinishingOutCommand(finishingOutGuid);

            GarmentSampleFinishingOut GarmentSampleFinishingOut = new GarmentSampleFinishingOut(
                finishingOutGuid,
                "no", new UnitDepartmentId(1), "uCode", "Uname", "FINISHING", DateTimeOffset.Now, "ro", "article",
                 new UnitDepartmentId(1), "uCode", "Uname", new GarmentComodityId(1), "cCode", "cName", false);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutReadModel>()
                {
                    GarmentSampleFinishingOut.GetReadModel()
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutItem>()
                {
                    new GarmentSampleFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutDetailReadModel, bool>>>()))
            //    .Returns(new List<GarmentSampleFinishingOutDetail>()
            //    {
            //        new GarmentSampleFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
            //    });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInItemReadModel>
                {
                    new GarmentSampleFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                GarmentSampleFinishingOut.UnitId,
                GarmentSampleFinishingOut.UnitCode,
                GarmentSampleFinishingOut.UnitName,
                GarmentSampleFinishingOut.ComodityId,
                GarmentSampleFinishingOut.ComodityCode,
                GarmentSampleFinishingOut.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutItem>()));
            //_mockFinishingOutDetailRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutDetail>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSampleFinishingOutCommand, cancellationToken);

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
            RemoveGarmentSampleFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSampleFinishingOutCommand RemoveGarmentSampleFinishingOutCommand = new RemoveGarmentSampleFinishingOutCommand(finishingOutGuid);

            GarmentSampleFinishingOut GarmentSampleFinishingOut = new GarmentSampleFinishingOut(
                finishingOutGuid,
                "no", new UnitDepartmentId(1), "uCode", "Uname", "FINISHING", DateTimeOffset.Now, "ro", "article",
                 new UnitDepartmentId(1), "uCode", "Uname", new GarmentComodityId(1), "cCode", "cName", true);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutReadModel>()
                {
                    GarmentSampleFinishingOut.GetReadModel()
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutItem>()
                {
                    new GarmentSampleFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            _mockFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutDetail>()
                {
                    new GarmentSampleFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInItemReadModel>
                {
                    new GarmentSampleFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                GarmentSampleFinishingOut.UnitId,
                GarmentSampleFinishingOut.UnitCode,
                GarmentSampleFinishingOut.UnitName,
                GarmentSampleFinishingOut.ComodityId,
                GarmentSampleFinishingOut.ComodityCode,
                GarmentSampleFinishingOut.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutItem>()));
            _mockFinishingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSampleFinishingOutCommand, cancellationToken);

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
            RemoveGarmentSampleFinishingOutCommandHandler unitUnderTest = CreateRemoveGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            RemoveGarmentSampleFinishingOutCommand RemoveGarmentSampleFinishingOutCommand = new RemoveGarmentSampleFinishingOutCommand(finishingOutGuid);

            GarmentSampleFinishingOut GarmentSampleFinishingOut = new GarmentSampleFinishingOut(
                finishingOutGuid,
                "no", new UnitDepartmentId(1), "uCode", "Uname", "SEWING", DateTimeOffset.Now, "ro", "article",
                 new UnitDepartmentId(1), "uCode", "Uname", new GarmentComodityId(1), "cCode", "cName", true);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutReadModel>()
                {
                    GarmentSampleFinishingOut.GetReadModel()
                }.AsQueryable());
            var GarmentSampleFinishingOutItem = new GarmentSampleFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty, finishingInItemGuid, new ProductId(1), null, null, null, new SizeId(1), null, 1, new UomId(1), null, null, 1, 1, 1);
            _mockFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutItemReadModel>()
                {
                    GarmentSampleFinishingOutItem.GetReadModel()
                }.AsQueryable());

            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutItem>()
                {
                    new GarmentSampleFinishingOutItem(finishingOutItemGuid, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            _mockFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSampleFinishingOutDetail>()
                {
                    new GarmentSampleFinishingOutDetail(finishingOutDetailGuid, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInItemReadModel>
                {
                    new GarmentSampleFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                GarmentSampleFinishingOut.UnitId,
                GarmentSampleFinishingOut.UnitCode,
                GarmentSampleFinishingOut.UnitName,
                GarmentSampleFinishingOut.ComodityId,
                GarmentSampleFinishingOut.ComodityCode,
                GarmentSampleFinishingOut.ComodityName,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            Guid SewingInGuid = Guid.NewGuid();
            GarmentSampleSewingIn GarmentSampleSewingIn = new GarmentSampleSewingIn(
                SewingInGuid, null, "FINiSHING", Guid.Empty, null, new UnitDepartmentId(1), null, null,
                new UnitDepartmentId(1), null, null, null, null, new GarmentComodityId(1), null, null, DateTimeOffset.Now);

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInReadModel>()
                {
                    GarmentSampleSewingIn.GetReadModel()
                }.AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleSewingInItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleSewingInItem>()
                {
                    new GarmentSampleSewingInItem(Guid.Empty,SewingInGuid,Guid.Empty,Guid.Empty,Guid.Empty,finishingOutItemGuid,finishingOutDetailGuid, new ProductId(1), null, null, null, new SizeId(1), null, 0, new UomId(1), null, null, 0,1,1)
                });


            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutItem>()));
            _mockFinishingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));

            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(RemoveGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}