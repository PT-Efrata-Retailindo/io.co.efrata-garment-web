using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleCuttingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleCuttingOuts
{
    public class PlaceGarmentSampleCuttingOutCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleCuttingOutRepository> _mockCuttingOutRepository;
        private readonly Mock<IGarmentSampleCuttingOutItemRepository> _mockCuttingOutItemRepository;
        private readonly Mock<IGarmentSampleCuttingOutDetailRepository> _mockCuttingOutDetailRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentSampleSewingInRepository> _mockSewingDORepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingDOItemRepository;
       private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public PlaceGarmentSampleCuttingOutCommandHandlerTest()
        {
            _mockCuttingOutRepository = CreateMock<IGarmentSampleCuttingOutRepository>();
            _mockCuttingOutItemRepository = CreateMock<IGarmentSampleCuttingOutItemRepository>();
            _mockCuttingOutDetailRepository = CreateMock<IGarmentSampleCuttingOutDetailRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();
            _mockSewingDORepository = CreateMock<IGarmentSampleSewingInRepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockCuttingOutRepository);
            _MockStorage.SetupStorage(_mockCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockSewingDORepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
           _MockStorage.SetupStorage(_mockComodityPriceRepository);
        }

        private PlaceGarmentSampleCuttingOutCommandHandler CreatePlaceGarmentSampleCuttingOutCommandHandler()
        {
            return new PlaceGarmentSampleCuttingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid cuttingInDetailGuid = Guid.NewGuid();
            Guid cuttingInGuid = Guid.NewGuid();
            PlaceGarmentSampleCuttingOutCommandHandler unitUnderTest = CreatePlaceGarmentSampleCuttingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleCuttingOutCommand placeGarmentSampleCuttingOutCommand = new PlaceGarmentSampleCuttingOutCommand()
            {
                RONo = "RONo",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                CuttingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleCuttingOutItemValueObject>
                {
                    new GarmentSampleCuttingOutItemValueObject
                    {
                        Product=new Product(1, "ProductCode", "ProductName"),
                        CuttingInDetailId=cuttingInDetailGuid,
                        IsSave=true,
                        CuttingInId=cuttingInGuid,
                        Details = new List<GarmentSampleCuttingOutDetailValueObject>
                        {
                            new GarmentSampleCuttingOutDetailValueObject
                            {
                                CuttingOutUom = new Uom(2, "PCS"),
                                CuttingOutQuantity=1,
                                Size= new SizeValueObject(1,"Size"),
                                Color="kajsj"
                            }
                        }
                    }
                },

            };

            _mockCuttingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingOutReadModel>().AsQueryable());

            _mockSewingDORepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInReadModel>().AsQueryable());

            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleCuttingInDetailReadModel>
                {
                    new GarmentSampleCuttingInDetailReadModel(cuttingInDetailGuid)
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
            Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentSampleCuttingOutCommand.Unit.Id),
                placeGarmentSampleCuttingOutCommand.Unit.Code,
                placeGarmentSampleCuttingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSampleCuttingOutCommand.Comodity.Id),
                placeGarmentSampleCuttingOutCommand.Comodity.Code,
                placeGarmentSampleCuttingOutCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingOut>()));
            _mockCuttingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingOutItem>()));
            _mockCuttingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingOutDetail>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));

            _mockSewingDORepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingIn>()));
            _mockSewingDOItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentSampleCuttingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
