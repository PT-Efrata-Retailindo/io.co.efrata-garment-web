using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentFinishingIns.CommandHandlers;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingIns.ValueObjects;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentFinishingIns
{
    public class PlaceGarmentFinishingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockSewingOutItemRepository;

        public PlaceGarmentFinishingInCommandHandlerTests()
        {
            _mockFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
        }

        private PlaceGarmentFinishingInCommandHandler CreatePlaceGarmentFinishingInCommandHandler()
        {
            return new PlaceGarmentFinishingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingOutItemGuid = Guid.NewGuid();
            Guid sewingOutGuid = Guid.NewGuid();
            PlaceGarmentFinishingInCommandHandler unitUnderTest = CreatePlaceGarmentFinishingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentFinishingInCommand placeGarmentFinishingInCommand = new PlaceGarmentFinishingInCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                FinishingInDate = DateTimeOffset.Now,
                Article = "Article",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentFinishingInItemValueObject>
                {
                    new GarmentFinishingInItemValueObject
                    {
                        SewingOutItemId=sewingOutItemGuid,
                        Size=new SizeValueObject(1, "Size"),
                        Quantity=1,
                        RemainingQuantity=1,
                        Product= new Product(1, "ProdCode", "ProdName"),
                        Uom=new Uom(1, "Uom"),
                    }
                },

            };

            _mockFinishingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInReadModel>().AsQueryable());
            _mockSewingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutItemReadModel>
                {
                    new GarmentSewingOutItemReadModel(sewingOutItemGuid)
                }.AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingIn>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentFinishingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}