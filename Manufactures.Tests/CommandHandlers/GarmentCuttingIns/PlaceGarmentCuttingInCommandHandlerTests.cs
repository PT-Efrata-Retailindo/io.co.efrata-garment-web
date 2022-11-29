using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentCuttingIns.CommandHandlers;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingIns.ValueObjects;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
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

namespace Manufactures.Tests.CommandHandlers.CuttingIn
{
    public class PlaceGarmentCuttingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockPreparingItemRepository;

        public PlaceGarmentCuttingInCommandHandlerTests()
        {
            _mockCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockPreparingItemRepository);
        }

        private PlaceGarmentCuttingInCommandHandler CreatePlaceGarmentCuttingInCommandHandler()
        {
            return new PlaceGarmentCuttingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid preparingItemGuid = Guid.NewGuid();
            PlaceGarmentCuttingInCommandHandler unitUnderTest = CreatePlaceGarmentCuttingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentCuttingInCommand placeGarmentCuttingInCommand = new PlaceGarmentCuttingInCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                CuttingInDate = DateTimeOffset.Now,
                Items = new List<GarmentCuttingInItemValueObject>
                {
                    new GarmentCuttingInItemValueObject
                    {
                        Details = new List<GarmentCuttingInDetailValueObject>
                        {
                            new GarmentCuttingInDetailValueObject
                            {
                                PreparingItemId = preparingItemGuid,
                                Product = new Product(1, "ProductCode", "ProductName"),
                                PreparingUom = new Uom(1, "UomUnit"),
                                CuttingInUom = new Uom(2, "PCS"),
                                IsSave = true,
                            }
                        }
                    }
                },

            };

            _mockCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInReadModel>().AsQueryable());
            _mockPreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentPreparingItemReadModel>
                {
                    new GarmentPreparingItemReadModel(preparingItemGuid)
                }.AsQueryable());

            _mockCuttingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingIn>()));
            _mockCuttingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingInItem>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingInDetail>()));
            _mockPreparingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentPreparingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentPreparingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentCuttingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
