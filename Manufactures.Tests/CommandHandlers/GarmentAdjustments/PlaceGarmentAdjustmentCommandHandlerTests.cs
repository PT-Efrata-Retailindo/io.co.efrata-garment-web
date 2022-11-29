using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAdjustments.CommandHandlers;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentAdjustments.ValueObjects;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentAdjustments
{
    public class PlaceGarmentAdjustmentCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAdjustmentRepository> _mockAdjustmentRepository;
        private readonly Mock<IGarmentAdjustmentItemRepository> _mockAdjustmentItemRepository;
        private readonly Mock<IGarmentSewingDOItemRepository> _mockSewingDOItemRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;
		private readonly Mock<IGarmentFinishedGoodStockRepository> _mockFinishedGoodStockRepository;
		private readonly Mock<IGarmentFinishedGoodStockHistoryRepository> _mockFinishedGoodStockHistoryRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public PlaceGarmentAdjustmentCommandHandlerTests()
        {
            _mockAdjustmentRepository = CreateMock<IGarmentAdjustmentRepository>();
            _mockAdjustmentItemRepository = CreateMock<IGarmentAdjustmentItemRepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
			_mockFinishedGoodStockRepository = CreateMock<IGarmentFinishedGoodStockRepository>();
			_mockFinishedGoodStockHistoryRepository = CreateMock<IGarmentFinishedGoodStockHistoryRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockAdjustmentRepository);
            _MockStorage.SetupStorage(_mockAdjustmentItemRepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
			_MockStorage.SetupStorage(_mockFinishedGoodStockRepository);
			_MockStorage.SetupStorage(_mockFinishedGoodStockHistoryRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
        }

        private PlaceGarmentAdjustmentCommandHandler CreatePlaceGarmentAdjustmentCommandHandler()
        {
            return new PlaceGarmentAdjustmentCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_LOADING()
        {
            // Arrange
            Guid sewingDOItemGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            PlaceGarmentAdjustmentCommandHandler unitUnderTest = CreatePlaceGarmentAdjustmentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentAdjustmentCommand placeGarmentAdjustmentCommand = new PlaceGarmentAdjustmentCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                AdjustmentType="LOADING",
                AdjustmentDate = DateTimeOffset.Now,
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentAdjustmentItemValueObject>
                {
                    new GarmentAdjustmentItemValueObject
                    {
                        IsSave=true,
                        SewingDOItemId=sewingDOItemGuid,
                        Size=new SizeValueObject(1, "Size"),
                        Quantity=1,
                        RemainingQuantity=2,
                        Product= new Product(1, "ProdCode", "ProdName"),
                        Uom=new Uom(1, "Uom"),
                    }
                },

            };

            _mockAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>().AsQueryable());
            _mockSewingDOItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOItemReadModel>
                {
                    new GarmentSewingDOItemReadModel(sewingDOItemGuid)
                }.AsQueryable());

            _mockAdjustmentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustment>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustment>()));
            _mockAdjustmentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustmentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustmentItem>()));
            _mockSewingDOItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingDOItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingDOItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentAdjustmentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_SEWING()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            PlaceGarmentAdjustmentCommandHandler unitUnderTest = CreatePlaceGarmentAdjustmentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentAdjustmentCommand placeGarmentAdjustmentCommand = new PlaceGarmentAdjustmentCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                AdjustmentType = "SEWING",
                AdjustmentDate = DateTimeOffset.Now,
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentAdjustmentItemValueObject>
                {
                    new GarmentAdjustmentItemValueObject
                    {
                        IsSave=true,
                        SewingInItemId=sewingInItemGuid,
                        Size=new SizeValueObject(1, "Size"),
                        Quantity=1,
                        RemainingQuantity=2,
                        Product= new Product(1, "ProdCode", "ProdName"),
                        Uom=new Uom(1, "Uom"),
                    }
                },

            };

            _mockAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>().AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>
                {
                    new GarmentSewingInItemReadModel(sewingInItemGuid)
                }.AsQueryable());

            _mockAdjustmentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustment>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustment>()));
            _mockAdjustmentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustmentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustmentItem>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentAdjustmentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_FINISHING()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            PlaceGarmentAdjustmentCommandHandler unitUnderTest = CreatePlaceGarmentAdjustmentCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentAdjustmentCommand placeGarmentAdjustmentCommand = new PlaceGarmentAdjustmentCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                AdjustmentType = "FINISHING",
                AdjustmentDate = DateTimeOffset.Now,
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentAdjustmentItemValueObject>
                {
                    new GarmentAdjustmentItemValueObject
                    {
                        IsSave=true,
                        FinishingInItemId=finishingInItemGuid,
                        Size=new SizeValueObject(1, "Size"),
                        Quantity=1,
                        RemainingQuantity=2,
                        Product= new Product(1, "ProdCode", "ProdName"),
                        Uom=new Uom(1, "Uom"),
                    }
                },

            };

            _mockAdjustmentRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentAdjustmentReadModel>().AsQueryable());
            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            _mockAdjustmentRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustment>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustment>()));
            _mockAdjustmentItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentAdjustmentItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentAdjustmentItem>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentAdjustmentCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
		[Fact]
		public async Task Handle_StateUnderTest_ExpectedBehavior_BARANGJADI()
		{
			// Arrange
			Guid finishedGoodStockId = Guid.NewGuid();
			Guid sewingDOGuid = Guid.NewGuid();
			PlaceGarmentAdjustmentCommandHandler unitUnderTest = CreatePlaceGarmentAdjustmentCommandHandler();
			CancellationToken cancellationToken = CancellationToken.None;
			PlaceGarmentAdjustmentCommand placeGarmentAdjustmentCommand = new PlaceGarmentAdjustmentCommand()
			{
				RONo = "RONo",
				Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
				AdjustmentType = "BARANG JADI",
				AdjustmentDate = DateTimeOffset.Now,
				Article = "Article",
				Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
				Items = new List<GarmentAdjustmentItemValueObject>
				{
					new GarmentAdjustmentItemValueObject
					{
						IsSave=true,
						FinishingInItemId=Guid.Empty,
						FinishedGoodStockId=finishedGoodStockId,
						Size=new SizeValueObject(1, "Size"),
						Quantity=1,
						RemainingQuantity=2,
						Product= new Product(1, "ProdCode", "ProdName"),
						Uom=new Uom(1, "Uom"),
						Color="www"
					}
				},

			};
			_mockFinishedGoodStockRepository
			   .Setup(s => s.Query)
			   .Returns(new List<GarmentFinishedGoodStockReadModel>
			   {
					new GarmentFinishedGoodStock(finishedGoodStockId,"","RONo","article",new UnitDepartmentId(1),"code","name",new GarmentComodityId(1),"","",new SizeId(1),"", new UomId(1),"",10,100,100).GetReadModel()
			   }.AsQueryable());

			_mockAdjustmentRepository
			   .Setup(s => s.Query)
			   .Returns(new List<GarmentAdjustmentReadModel>().AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentAdjustmentCommand.Unit.Id),
                placeGarmentAdjustmentCommand.Unit.Code,
                placeGarmentAdjustmentCommand.Unit.Name,
                new GarmentComodityId(placeGarmentAdjustmentCommand.Comodity.Id),
                placeGarmentAdjustmentCommand.Comodity.Code,
                placeGarmentAdjustmentCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockAdjustmentRepository
				.Setup(s => s.Update(It.IsAny<GarmentAdjustment>()))
				.Returns(Task.FromResult(It.IsAny<GarmentAdjustment>()));
			_mockAdjustmentItemRepository
				.Setup(s => s.Update(It.IsAny<GarmentAdjustmentItem>()))
				.Returns(Task.FromResult(It.IsAny<GarmentAdjustmentItem>()));
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
			var result = await unitUnderTest.Handle(placeGarmentAdjustmentCommand, cancellationToken);

			// Assert
			result.Should().NotBeNull();
		}
	}
}