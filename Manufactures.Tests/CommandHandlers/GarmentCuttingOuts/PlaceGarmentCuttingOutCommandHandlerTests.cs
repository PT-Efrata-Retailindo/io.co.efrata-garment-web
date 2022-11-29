using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentCuttingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentCuttingOuts
{
    public class PlaceGarmentCuttingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentCuttingOutRepository> _mockCuttingOutRepository;
        private readonly Mock<IGarmentCuttingOutItemRepository> _mockCuttingOutItemRepository;
        private readonly Mock<IGarmentCuttingOutDetailRepository> _mockCuttingOutDetailRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentSewingDORepository> _mockSewingDORepository;
        private readonly Mock<IGarmentSewingDOItemRepository> _mockSewingDOItemRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public PlaceGarmentCuttingOutCommandHandlerTests()
        {
            _mockCuttingOutRepository = CreateMock<IGarmentCuttingOutRepository>();
            _mockCuttingOutItemRepository = CreateMock<IGarmentCuttingOutItemRepository>();
            _mockCuttingOutDetailRepository = CreateMock<IGarmentCuttingOutDetailRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockSewingDORepository = CreateMock<IGarmentSewingDORepository>();
            _mockSewingDOItemRepository = CreateMock<IGarmentSewingDOItemRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockCuttingOutRepository);
            _MockStorage.SetupStorage(_mockCuttingOutItemRepository);
            _MockStorage.SetupStorage(_mockCuttingOutDetailRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockSewingDORepository);
            _MockStorage.SetupStorage(_mockSewingDOItemRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
        }

        private PlaceGarmentCuttingOutCommandHandler CreatePlaceGarmentCuttingOutCommandHandler()
        {
            return new PlaceGarmentCuttingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid cuttingInDetailGuid = Guid.NewGuid();
            Guid cuttingInGuid = Guid.NewGuid();
            PlaceGarmentCuttingOutCommandHandler unitUnderTest = CreatePlaceGarmentCuttingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentCuttingOutCommand placeGarmentCuttingOutCommand = new PlaceGarmentCuttingOutCommand()
            {
                RONo = "RONo",
                UnitFrom = new UnitDepartment(1, "UnitCode", "UnitName"),
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                CuttingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentCuttingOutItemValueObject>
                {
                    new GarmentCuttingOutItemValueObject
                    {
                        Product=new Product(1, "ProductCode", "ProductName"),
                        CuttingInDetailId=cuttingInDetailGuid,
                        IsSave=true,
                        CuttingInId=cuttingInGuid,
                        Details = new List<GarmentCuttingOutDetailValueObject>
                        {
                            new GarmentCuttingOutDetailValueObject
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
                .Returns(new List<GarmentCuttingOutReadModel>().AsQueryable());

            _mockSewingDORepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingDOReadModel>().AsQueryable());

            _mockCuttingInDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentCuttingInDetailReadModel>
                {
                    new GarmentCuttingInDetailReadModel(cuttingInDetailGuid)
                }.AsQueryable());

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentCuttingOutCommand.Unit.Id),
                placeGarmentCuttingOutCommand.Unit.Code,
                placeGarmentCuttingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentCuttingOutCommand.Comodity.Id),
                placeGarmentCuttingOutCommand.Comodity.Code,
                placeGarmentCuttingOutCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockCuttingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingOut>()));
            _mockCuttingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingOutItem>()));
            _mockCuttingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingOutDetail>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentCuttingInDetail>()));

            _mockSewingDORepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingDO>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingDO>()));
            _mockSewingDOItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingDOItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingDOItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentCuttingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
