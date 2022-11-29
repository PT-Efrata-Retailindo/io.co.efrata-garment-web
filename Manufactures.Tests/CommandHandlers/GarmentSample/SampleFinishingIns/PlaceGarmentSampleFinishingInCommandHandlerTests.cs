using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleFinishingIns.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ValueObjects;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleFinishingIns
{
    public class PlaceGarmentSampleFinishingInCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentSampleFinishingInItemRepository> _mockFinishingInItemRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockSewingOutItemRepository;

        public PlaceGarmentSampleFinishingInCommandHandlerTests()
        {
            _mockFinishingInRepository = CreateMock<IGarmentSampleFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentSampleFinishingInItemRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
        }

        private PlaceGarmentSampleFinishingInCommandHandler CreatePlaceGarmentSampleFinishingInCommandHandler()
        {
            return new PlaceGarmentSampleFinishingInCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingOutItemGuid = Guid.NewGuid();
            Guid sewingOutGuid = Guid.NewGuid();
            PlaceGarmentSampleFinishingInCommandHandler unitUnderTest = CreatePlaceGarmentSampleFinishingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleFinishingInCommand placeGarmentSampleFinishingInCommand = new PlaceGarmentSampleFinishingInCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                FinishingInDate = DateTimeOffset.Now,
                Article = "Article",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Items = new List<GarmentSampleFinishingInItemValueObject>
                {
                    new GarmentSampleFinishingInItemValueObject
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
                .Returns(new List<GarmentSampleFinishingInReadModel>().AsQueryable());
            _mockSewingOutItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutItemReadModel>
                {
                    new GarmentSampleSewingOutItemReadModel(sewingOutItemGuid)
                }.AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingIn>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSampleFinishingInCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
