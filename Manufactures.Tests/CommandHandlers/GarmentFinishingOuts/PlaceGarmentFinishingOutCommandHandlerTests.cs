using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentFinishingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentFinishingOuts
{
    public class PlaceGarmentFinishingOutCommandHandlerTests : BaseCommandUnitTest
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

        public PlaceGarmentFinishingOutCommandHandlerTests()
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
            //_MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
        }
        private PlaceGarmentFinishingOutCommandHandler CreatePlaceGarmentFinishingOutCommandHandler()
        {
            return new PlaceGarmentFinishingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_GudangJadi()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentFinishingOutCommand placeGarmentFinishingOutCommand = new PlaceGarmentFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                FinishingTo = "GUDANG JADI",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentFinishingOutItemValueObject>
                {
                    new GarmentFinishingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        FinishingInId= new Guid(),
                        FinishingInItemId=finishingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentFinishingOutDetailValueObject>
                        {
                            new GarmentFinishingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>().AsQueryable());
            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishedGoodStockReadModel>().AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentFinishingOutCommand.Unit.Id),
                placeGarmentFinishingOutCommand.Unit.Code,
                placeGarmentFinishingOutCommand.Unit.Name,
                new GarmentComodityId( placeGarmentFinishingOutCommand.Comodity.Id),
                placeGarmentFinishingOutCommand.Comodity.Code,
                placeGarmentFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_IsnotDifferentSize_GudangJadi()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentFinishingOutCommand placeGarmentFinishingOutCommand = new PlaceGarmentFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = false,
                FinishingTo = "GUDANG JADI",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentFinishingOutItemValueObject>
                {
                    new GarmentFinishingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        FinishingInId= new Guid(),
                        FinishingInItemId=finishingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        
                    }
                },

            };

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>().AsQueryable());
            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishedGoodStockReadModel>().AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentFinishingOutCommand.Unit.Id),
                placeGarmentFinishingOutCommand.Unit.Code,
                placeGarmentFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentFinishingOutCommand.Comodity.Id),
                placeGarmentFinishingOutCommand.Comodity.Code,
                placeGarmentFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_Finishing_DiffSize()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentFinishingOutCommand placeGarmentFinishingOutCommand = new PlaceGarmentFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                FinishingTo = "FINISHING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentFinishingOutItemValueObject>
                {
                    new GarmentFinishingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        FinishingInId= new Guid(),
                        FinishingInItemId=finishingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentFinishingOutDetailValueObject>
                        {
                            new GarmentFinishingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>().AsQueryable());
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
                new UnitDepartmentId(placeGarmentFinishingOutCommand.Unit.Id),
                placeGarmentFinishingOutCommand.Unit.Code,
                placeGarmentFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentFinishingOutCommand.Comodity.Id),
                placeGarmentFinishingOutCommand.Comodity.Code,
                placeGarmentFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_Sewing_DiffSize()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentFinishingOutCommand placeGarmentFinishingOutCommand = new PlaceGarmentFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                FinishingTo = "SEWING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentFinishingOutItemValueObject>
                {
                    new GarmentFinishingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        FinishingInId= new Guid(),
                        FinishingInItemId=finishingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentFinishingOutDetailValueObject>
                        {
                            new GarmentFinishingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };
            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>().AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>().AsQueryable());
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
                new UnitDepartmentId(placeGarmentFinishingOutCommand.Unit.Id),
                placeGarmentFinishingOutCommand.Unit.Code,
                placeGarmentFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentFinishingOutCommand.Comodity.Id),
                placeGarmentFinishingOutCommand.Comodity.Code,
                placeGarmentFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_Sewing()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentFinishingOutCommand placeGarmentFinishingOutCommand = new PlaceGarmentFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = false,
                FinishingTo = "SEWING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentFinishingOutItemValueObject>
                {
                    new GarmentFinishingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        FinishingInId= new Guid(),
                        FinishingInItemId=finishingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD"
                    }
                },

            };
            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>().AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>().AsQueryable());
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
                new UnitDepartmentId(placeGarmentFinishingOutCommand.Unit.Id),
                placeGarmentFinishingOutCommand.Unit.Code,
                placeGarmentFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentFinishingOutCommand.Comodity.Id),
                placeGarmentFinishingOutCommand.Comodity.Code,
                placeGarmentFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}