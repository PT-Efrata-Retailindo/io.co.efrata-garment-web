using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingIns.CommandHandlers;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns.ValueObjects;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSewingIns
{
    public class PlaceGarmentSewingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingInRepository> _mockSewingInRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentLoadingItemRepository> _mockLoadingItemRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockSewingOutItemRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockFinishingOutItemRepository;

        public PlaceGarmentSewingInCommandHandlerTests()
        {
            _mockSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
            _mockLoadingItemRepository = CreateMock<IGarmentLoadingItemRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();
            _mockFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();

            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockLoadingItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockFinishingOutItemRepository);
        }

        private PlaceGarmentSewingInCommandHandler CreatePlaceGarmentSewingInCommandHandler()
        {
            return new PlaceGarmentSewingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_CUTTING()
        {
            // Arrange
            Guid loadingItemGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            PlaceGarmentSewingInCommandHandler unitUnderTest = CreatePlaceGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSewingInCommand placeGarmentSewingInCommand = new PlaceGarmentSewingInCommand()
            {

                RONo = "RONo",
                Article = "Article",
                SewingFrom="CUTTING",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                LoadingId = Guid.NewGuid(),
                LoadingNo = "LoadingNo",
                SewingInDate = DateTimeOffset.Now,
                Comodity = new GarmentComodity(1, "ComodityCode", "ComodityName"),
                Items = new List<GarmentSewingInItemValueObject>
                {
                    new GarmentSewingInItemValueObject
                    {
                        LoadingItemId = loadingItemGuid,
                        Uom = new Uom(1, "UomUnit"),
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Size = new SizeValueObject(1,"SizeName"),
                        DesignColor = "DesignColor",
                        Color = "Color",
                        Quantity = 1,
                        RemainingQuantity = 1,
                        IsSave=true
                    }
                },

            };

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>().AsQueryable());

            _mockLoadingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentLoadingItemReadModel>
                {
                    new GarmentLoadingItemReadModel(loadingItemGuid)
                }.AsQueryable());

            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));
            _mockLoadingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentLoadingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentLoadingItem>()));


            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_SEWING()
        {
            // Arrange
            Guid loadingItemGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            PlaceGarmentSewingInCommandHandler unitUnderTest = CreatePlaceGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSewingInCommand placeGarmentSewingInCommand = new PlaceGarmentSewingInCommand()
            {

                RONo = "RONo",
                Article = "Article",
                SewingFrom = "SEWING",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                
                SewingInDate = DateTimeOffset.Now,
                Comodity = new GarmentComodity(1, "ComodityCode", "ComodityName"),
                Items = new List<GarmentSewingInItemValueObject>
                {
                    new GarmentSewingInItemValueObject
                    {
                        SewingOutItemId=sewingOutItemGuid,
                        SewingOutDetailId=Guid.NewGuid(),
                        Uom = new Uom(1, "UomUnit"),
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Size = new SizeValueObject(1,"SizeName"),
                        DesignColor = "DesignColor",
                        Color = "Color",
                        Quantity = 1,
                        RemainingQuantity = 1,
                        IsSave=true
                    }
                },

            };

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>().AsQueryable());
            _mockSewingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutItemReadModel>
                {
                    new GarmentSewingOutItemReadModel(sewingOutItemGuid)
                }.AsQueryable());


            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOutItem>()));


            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior_FINISHING()
        {
            // Arrange
            Guid loadingItemGuid = Guid.NewGuid();
            Guid finishingOutItemGuid = Guid.NewGuid();
            PlaceGarmentSewingInCommandHandler unitUnderTest = CreatePlaceGarmentSewingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSewingInCommand placeGarmentSewingInCommand = new PlaceGarmentSewingInCommand()
            {

                RONo = "RONo",
                Article = "Article",
                SewingFrom = "FINISHING",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),

                SewingInDate = DateTimeOffset.Now,
                Comodity = new GarmentComodity(1, "ComodityCode", "ComodityName"),
                Items = new List<GarmentSewingInItemValueObject>
                {
                    new GarmentSewingInItemValueObject
                    {
                        FinishingOutItemId=finishingOutItemGuid,
                        SewingOutDetailId=Guid.NewGuid(),
                        Uom = new Uom(1, "UomUnit"),
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Size = new SizeValueObject(1,"SizeName"),
                        DesignColor = "DesignColor",
                        Color = "Color",
                        Quantity = 1,
                        RemainingQuantity = 1,
                        IsSave=true
                    }
                },

            };

            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>().AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutItemReadModel>
                {
                    new GarmentFinishingOutItemReadModel(finishingOutItemGuid)
                }.AsQueryable());


            _mockSewingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingIn>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutItem>()));


            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSewingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}