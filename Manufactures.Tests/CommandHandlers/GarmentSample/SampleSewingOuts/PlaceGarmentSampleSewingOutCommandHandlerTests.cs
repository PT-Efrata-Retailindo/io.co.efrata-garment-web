using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleSewingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleSewingOuts
{
    public class PlaceGarmentSampleSewingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleSewingOutRepository> _mockSewingOutRepository;
        private readonly Mock<IGarmentSampleSewingOutItemRepository> _mockSewingOutItemRepository;
        private readonly Mock<IGarmentSampleSewingOutDetailRepository> _mockSewingOutDetailRepository;
        private readonly Mock<IGarmentSampleSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;
        private readonly Mock<IGarmentSampleCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentSampleCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentSampleCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentSampleFinishingInRepository> _mockFinishingInRepository;
        private readonly Mock<IGarmentSampleFinishingInItemRepository> _mockFinishingInItemRepository;

        public PlaceGarmentSampleSewingOutCommandHandlerTests()
        {
            _mockSewingOutRepository = CreateMock<IGarmentSampleSewingOutRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSampleSewingOutItemRepository>();
            _mockSewingOutDetailRepository = CreateMock<IGarmentSampleSewingOutDetailRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSampleSewingInItemRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();
            _mockCuttingInRepository = CreateMock<IGarmentSampleCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentSampleCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentSampleCuttingInDetailRepository>();

            _mockFinishingInRepository = CreateMock<IGarmentSampleFinishingInRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentSampleFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
            _MockStorage.SetupStorage(_mockSewingOutRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockComodityPriceRepository);
            _MockStorage.SetupStorage(_mockFinishingInRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
        }
        private PlaceGarmentSampleSewingOutCommandHandler CreatePlaceGarmentSampleSewingOutCommandHandler()
        {
            return new PlaceGarmentSampleSewingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {

            Guid sewinginItemId = Guid.NewGuid();
            PlaceGarmentSampleSewingOutCommandHandler unitUnderTest = CreatePlaceGarmentSampleSewingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentSampleSewingOutCommand placeGarmentSewingOutCommand = new PlaceGarmentSampleSewingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(2, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                SewingTo = "CUTTING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                SewingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleSewingOutItemValueObject>
                {
                    new GarmentSampleSewingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        SewingInId= new Guid(),
                        SewingInItemId=sewinginItemId,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentSampleSewingOutDetailValueObject>
                        {
                            new GarmentSampleSewingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };
            PlaceGarmentSampleSewingOutCommand placeGarmentSewingOutCommands = new PlaceGarmentSampleSewingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(2, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                SewingTo = "FINISHING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                SewingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleSewingOutItemValueObject>
                {
                    new GarmentSampleSewingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        SewingInId= new Guid(),
                        SewingInItemId=sewinginItemId,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentSampleSewingOutDetailValueObject>
                        {
                            new GarmentSampleSewingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };
            _mockCuttingInRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleCuttingInReadModel>().AsQueryable());

            _mockCuttingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingIn>()));
            _mockCuttingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInItem>()));
            _mockCuttingInDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleCuttingInDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleCuttingInDetail>()));

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingOutReadModel>().AsQueryable());

            _mockSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleSewingInItemReadModel>
                {
                    new GarmentSampleSewingInItemReadModel(sewinginItemId)
                }.AsQueryable());

            _mockSewingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOut>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutItem>()));
            _mockSewingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingOutDetail>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleSewingInItem>()));

            _mockFinishingInRepository
               .Setup(s => s.Query)
               .Returns(new List<GarmentSampleFinishingInReadModel>().AsQueryable());

            _mockFinishingInRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingIn>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingIn>()));

            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleFinishingInItem>()));

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(placeGarmentSewingOutCommand.Unit.Id),
                placeGarmentSewingOutCommand.Unit.Code,
                placeGarmentSewingOutCommand.Unit.Name,
                new GarmentComodityId(placeGarmentSewingOutCommand.Comodity.Id),
                placeGarmentSewingOutCommand.Comodity.Code,
                placeGarmentSewingOutCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

           var result = await unitUnderTest.Handle(placeGarmentSewingOutCommand, cancellationToken);
            var result2 = await unitUnderTest.Handle(placeGarmentSewingOutCommands, cancellationToken);

            result.Should().NotBeNull();
        }
    }
}
