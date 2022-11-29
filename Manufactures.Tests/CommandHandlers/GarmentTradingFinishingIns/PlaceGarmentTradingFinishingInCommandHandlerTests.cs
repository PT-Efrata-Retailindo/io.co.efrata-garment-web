using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentFinishingIns.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
//using Manufactures.Domain.GarmentSubconCuttingOuts;
//using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
//using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentTradingFinishingIns.Commands;
using Manufactures.Domain.GarmentTradingFinishingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentTradingFinishingIns
{
    public class PlaceGarmentTradingFinishingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;
        //private readonly Mock<IGarmentSubconCuttingRepository> _mockSubconCuttingRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockIComodityPriceRepository;

        public PlaceGarmentTradingFinishingInCommandHandlerTests()
        {
            _mockFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();
            //_mockSubconCuttingRepository = CreateMock<IGarmentSubconCuttingRepository>();
            _mockIComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            //_MockStorage.SetupStorage(_mockSubconCuttingRepository);
            _MockStorage.SetupStorage(_mockIComodityPriceRepository);
        }

        private PlaceGarmentTradingFinishingInCommandHandler CreatePlaceGarmentTradingFinishingInCommandHandler()
        {
            return new PlaceGarmentTradingFinishingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_Place_Success()
        {
            // Arrange
            Guid subconCuttingId = Guid.NewGuid();
            Guid sewingOutGuid = Guid.NewGuid();
            PlaceGarmentTradingFinishingInCommandHandler unitUnderTest = CreatePlaceGarmentTradingFinishingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentTradingFinishingInCommand placeGarmentTradingFinishingInCommand = new PlaceGarmentTradingFinishingInCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                FinishingInDate = DateTimeOffset.Now,
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentTradingFinishingInItemValueObject>
                {
                    new GarmentTradingFinishingInItemValueObject
                    {
                        IsSave = true,
                        SubconCuttingId = subconCuttingId,
                        Size = new SizeValueObject(1, "Size"),
                        Quantity = 1,
                        RemainingQuantity = 1,
                        Product = new Product(1, "ProdCode", "ProdName"),
                        Uom = new Uom(1, "Uom"),
                    },
                },
            };

            _mockFinishingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInReadModel>().AsQueryable());
            //_mockSubconCuttingRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentSubconCuttingReadModel>
            //    {
            //        new GarmentSubconCuttingReadModel(subconCuttingId)
            //    }.AsQueryable());
            _mockIComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    new GarmentComodityPriceReadModel(Guid.NewGuid())
                }.AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingIn>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));
            //_mockSubconCuttingRepository
            //    .Setup(s => s.Update(It.IsAny<GarmentSubconCutting>()))
            //    .Returns(Task.FromResult(It.IsAny<GarmentSubconCutting>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentTradingFinishingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}