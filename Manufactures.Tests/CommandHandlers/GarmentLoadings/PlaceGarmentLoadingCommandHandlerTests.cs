using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentLoadings.CommandHandlers;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentLoadings.ValueObjects;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentLoadings
{
    public class PlaceGarmentLoadingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentLoadingRepository> _mockLoadingRepository;
        private readonly Mock<IGarmentLoadingItemRepository> _mockLoadingItemRepository;
        private readonly Mock<IGarmentSewingDOItemRepository> _mockSewingDOItemRepository;

        public PlaceGarmentLoadingCommandHandlerTests()
        {
            _mockLoadingRepository = CreateMock<IGarmentLoadingRepository>();
            _mockLoadingItemRepository = CreateMock<IGarmentLoadingItemRepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();

            _MockStorage.SetupStorage(_mockLoadingRepository);
            _MockStorage.SetupStorage(_mockLoadingItemRepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
        }

        private PlaceGarmentLoadingCommandHandler CreatePlaceGarmentLoadingCommandHandler()
        {
            return new PlaceGarmentLoadingCommandHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingDOItemGuid = Guid.NewGuid();
            Guid sewingDOGuid = Guid.NewGuid();
            PlaceGarmentLoadingCommandHandler unitUnderTest = CreatePlaceGarmentLoadingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentLoadingCommand placeGarmentLoadingCommand = new PlaceGarmentLoadingCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                LoadingDate = DateTimeOffset.Now,
                Article = "Article",
                SewingDOId = sewingDOGuid,
                UnitFrom= new UnitDepartment(1, "UnitCode", "UnitName"),
                Comodity= new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentLoadingItemValueObject>
                {
                    new GarmentLoadingItemValueObject
                    {
                        IsSave=true,
                        SewingDOItemId=sewingDOItemGuid,
                        Size=new SizeValueObject(1, "Size"),
                        Quantity=1,
                        RemainingQuantity=1,
                        SewingDORemainingQuantity=2,
                        Product= new Product(1, "ProdCode", "ProdName"),
                        Uom=new Uom(1, "Uom"),
                    }
                },

            };

            _mockLoadingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentLoadingReadModel>().AsQueryable());
            _mockSewingDOItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOItemReadModel>
                {
                    new GarmentSewingDOItemReadModel(sewingDOItemGuid)
                }.AsQueryable());

            _mockLoadingRepository
                .Setup(s => s.Update(It.IsAny<GarmentLoading>()))
                .Returns(Task.FromResult(It.IsAny<GarmentLoading>()));
            _mockLoadingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentLoadingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentLoadingItem>()));
            _mockSewingDOItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingDOItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingDOItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentLoadingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
