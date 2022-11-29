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
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleFinishingOuts
{
    public class PlaceGarmentSampleFinishingOutCommandHandlerTests : BaseCommandUnitTest
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

        public PlaceGarmentSampleFinishingOutCommandHandlerTests()
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
            //_MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
        }
        private PlaceGarmentSampleFinishingOutCommandHandler CreatePlaceGarmentSampleFinishingOutCommandHandler()
        {
            return new PlaceGarmentSampleFinishingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_GudangJadi()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentSampleFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleFinishingOutCommand placeGarmentSampleFinishingOutCommand = new PlaceGarmentSampleFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                FinishingTo = "GUDANG JADI",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleFinishingOutItemValueObject>
                {
                    new GarmentSampleFinishingOutItemValueObject
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
                        Details = new List<GarmentSampleFinishingOutDetailValueObject>
                        {
                            new GarmentSampleFinishingOutDetailValueObject
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
                .Returns(new List<GarmentSampleFinishingOutReadModel>().AsQueryable());
            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInItemReadModel>
                {
                    new GarmentSampleFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishedGoodStockReadModel>().AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentSampleFinishingOutCommand.Unit.Id),
                placeGarmentSampleFinishingOutCommand.Unit.Code,
                placeGarmentSampleFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSampleFinishingOutCommand.Comodity.Id),
                placeGarmentSampleFinishingOutCommand.Comodity.Code,
                placeGarmentSampleFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_IsnotDifferentSize_GudangJadi()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentSampleFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleFinishingOutCommand placeGarmentSampleFinishingOutCommand = new PlaceGarmentSampleFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = false,
                FinishingTo = "GUDANG JADI",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleFinishingOutItemValueObject>
                {
                    new GarmentSampleFinishingOutItemValueObject
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
                .Returns(new List<GarmentSampleFinishingOutReadModel>().AsQueryable());
            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingInItemReadModel>
                {
                    new GarmentSampleFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            _mockFinishedGoodStockRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishedGoodStockReadModel>().AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentSampleFinishingOutCommand.Unit.Id),
                placeGarmentSampleFinishingOutCommand.Unit.Code,
                placeGarmentSampleFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSampleFinishingOutCommand.Comodity.Id),
                placeGarmentSampleFinishingOutCommand.Comodity.Code,
                placeGarmentSampleFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_Finishing_DiffSize()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentSampleFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleFinishingOutCommand placeGarmentSampleFinishingOutCommand = new PlaceGarmentSampleFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                FinishingTo = "FINISHING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleFinishingOutItemValueObject>
                {
                    new GarmentSampleFinishingOutItemValueObject
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
                        Details = new List<GarmentSampleFinishingOutDetailValueObject>
                        {
                            new GarmentSampleFinishingOutDetailValueObject
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
                .Returns(new List<GarmentSampleFinishingOutReadModel>().AsQueryable());
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
                new UnitDepartmentId(placeGarmentSampleFinishingOutCommand.Unit.Id),
                placeGarmentSampleFinishingOutCommand.Unit.Code,
                placeGarmentSampleFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSampleFinishingOutCommand.Comodity.Id),
                placeGarmentSampleFinishingOutCommand.Comodity.Code,
                placeGarmentSampleFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_Sewing_DiffSize()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentSampleFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleFinishingOutCommand placeGarmentSampleFinishingOutCommand = new PlaceGarmentSampleFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                FinishingTo = "SEWING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleFinishingOutItemValueObject>
                {
                    new GarmentSampleFinishingOutItemValueObject
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
                        Details = new List<GarmentSampleFinishingOutDetailValueObject>
                        {
                            new GarmentSampleFinishingOutDetailValueObject
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
                .Returns(new List<GarmentSampleSewingInReadModel>().AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutReadModel>().AsQueryable());
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
                new UnitDepartmentId(placeGarmentSampleFinishingOutCommand.Unit.Id),
                placeGarmentSampleFinishingOutCommand.Unit.Code,
                placeGarmentSampleFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSampleFinishingOutCommand.Comodity.Id),
                placeGarmentSampleFinishingOutCommand.Comodity.Code,
                placeGarmentSampleFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_Sewing()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            PlaceGarmentSampleFinishingOutCommandHandler unitUnderTest = CreatePlaceGarmentSampleFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleFinishingOutCommand placeGarmentSampleFinishingOutCommand = new PlaceGarmentSampleFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(1, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = false,
                FinishingTo = "SEWING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleFinishingOutItemValueObject>
                {
                    new GarmentSampleFinishingOutItemValueObject
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
                .Returns(new List<GarmentSampleSewingInReadModel>().AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleFinishingOutReadModel>().AsQueryable());
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
                new UnitDepartmentId(placeGarmentSampleFinishingOutCommand.Unit.Id),
                placeGarmentSampleFinishingOutCommand.Unit.Code,
                placeGarmentSampleFinishingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSampleFinishingOutCommand.Comodity.Id),
                placeGarmentSampleFinishingOutCommand.Comodity.Code,
                placeGarmentSampleFinishingOutCommand.Comodity.Name,
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
            var result = await unitUnderTest.Handle(placeGarmentSampleFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}