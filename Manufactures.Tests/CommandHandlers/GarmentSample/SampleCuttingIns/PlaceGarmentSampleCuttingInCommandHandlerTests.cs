using Barebone.Tests;
using Manufactures.Application.GarmentSample.SampleCuttingIns.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ValueObjects;
using System.Linq;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleCuttingIns
{
    public class PlaceGarmentSampleCuttingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockSampleCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockSampleCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockSampleCuttingInDetailRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockSamplePreparingItemRepository;

        public PlaceGarmentSampleCuttingInCommandHandlerTests()
        {
            _mockSampleCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockSampleCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockSampleCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockSampleCuttingInRepository);
            _MockStorage.SetupStorage(_mockSampleCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockSampleCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockSamplePreparingItemRepository);
        }

        private PlaceGarmentSampleCuttingInCommandHandler CreatePlaceGarmentSampleCuttingInCommandHandler()
        {
            return new PlaceGarmentSampleCuttingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid preparingItemGuid = Guid.NewGuid();
            PlaceGarmentSampleCuttingInCommandHandler unitUnderTest = CreatePlaceGarmentSampleCuttingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleCuttingInCommand placeGarmentSampleCuttingInCommand = new PlaceGarmentSampleCuttingInCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                CuttingInDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleCuttingInItemValueObject>
                {
                    new GarmentSampleCuttingInItemValueObject
                    {
                        Details = new List<GarmentSampleCuttingInDetailValueObject>
                        {
                            new GarmentSampleCuttingInDetailValueObject
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

            _mockSampleCuttingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInReadModel>().AsQueryable());
            _mockSamplePreparingItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSamplePreparingItemReadModel>
                {
                    new GarmentSamplePreparingItemReadModel(preparingItemGuid)
                }.AsQueryable());

            _mockSampleCuttingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingIn>()));
            _mockSampleCuttingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInItem>()));
            _mockSampleCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));
            _mockSamplePreparingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSamplePreparingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSamplePreparingItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSampleCuttingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
