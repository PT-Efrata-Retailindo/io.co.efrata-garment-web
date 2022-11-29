using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSubcon.GarmentServiceSubconSewings
{
    public class PlaceGarmentServiceSubconSewingCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentServiceSubconSewingRepository> _mockServiceSubconSewingRepository;
        private readonly Mock<IGarmentServiceSubconSewingItemRepository> _mockServiceSubconSewingItemRepository;
        private readonly Mock<IGarmentServiceSubconSewingDetailRepository> _mockServiceSubconSewingDetailRepository;
        private readonly Mock<IGarmentSewingInRepository> _mockSewingInRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;

        public PlaceGarmentServiceSubconSewingCommandHandlerTests()
        {
            _mockServiceSubconSewingRepository = CreateMock<IGarmentServiceSubconSewingRepository>();
            _mockServiceSubconSewingItemRepository = CreateMock<IGarmentServiceSubconSewingItemRepository>();
            _mockServiceSubconSewingDetailRepository = CreateMock<IGarmentServiceSubconSewingDetailRepository>();
            _mockSewingInRepository = CreateMock<IGarmentSewingInRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();

            _MockStorage.SetupStorage(_mockServiceSubconSewingRepository);
            _MockStorage.SetupStorage(_mockServiceSubconSewingItemRepository);
            _MockStorage.SetupStorage(_mockServiceSubconSewingDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
        }

        private PlaceGarmentServiceSubconSewingCommandHandler CreatePlaceGarmentServiceSubconSewingCommandHandler()
        {
            return new PlaceGarmentServiceSubconSewingCommandHandler(_MockStorage.Object);
        }

        /*[Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid SewingInGuid = Guid.NewGuid();
            Guid sewingOutItemGuid = Guid.NewGuid();
            PlaceGarmentServiceSubconSewingCommandHandler unitUnderTest = CreatePlaceGarmentServiceSubconSewingCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            PlaceGarmentServiceSubconSewingCommand placeGarmentServiceSubconSewingCommand = new PlaceGarmentServiceSubconSewingCommand()
            {
                Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                Items = new List<GarmentServiceSubconSewingItemValueObject>
                {
                    new GarmentServiceSubconSewingItemValueObject
                    {
                        RONo = "RONo",
                        Article = "Article",
                        Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                        Buyer = new Buyer(1, "BuyerCode", "BuyerName"),
                        Details= new List<GarmentServiceSubconSewingDetailValueObject>
                        {
                            new GarmentServiceSubconSewingDetailValueObject
                            {
                                Product = new Product(1, "ProductCode", "ProductName"),
                                Uom = new Uom(1, "UomUnit"),
                                SewingInId= new Guid(),
                                SewingInItemId=sewingInItemGuid,
                                IsSave=true,
                                Quantity=1,
                                DesignColor= "ColorD",
                                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                            }
                        }
                        
                    }
                },

            };
            GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                SewingInGuid, null, "SEWING", Guid.Empty, null, new UnitDepartmentId(1), null, null,
                new UnitDepartmentId(1), null, null, "RONo", null, new GarmentComodityId(1), null, null, DateTimeOffset.Now);
            GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(sewingInItemGuid, SewingInGuid, sewingOutItemGuid, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, new ProductId(1), null, null, "ColorD", new SizeId(1), null, 10, new UomId(1), null, null, 0, 1, 1);
            GarmentServiceSubconSewingDetail garmentServiceSubconSewingDetail = new GarmentServiceSubconSewingDetail(new Guid(), new Guid(), SewingInGuid, sewingInItemGuid, new ProductId(1), null, null, "ColorD", 1, new UomId(1), null, new UnitDepartmentId(1), null, null);
            _mockSewingInRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInReadModel>()
                {
                    garmentSewingIn.GetReadModel()
                }.AsQueryable());
            _mockSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>()
                {
                    garmentSewingInItem.GetReadModel()
                }.AsQueryable());

            _mockServiceSubconSewingRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingReadModel>().AsQueryable());
            //_mockServiceSubconSewingItemRepository
            //    .Setup(s => s.Query)
            //    .Returns(new List<GarmentServiceSubconSewingItemReadModel>().AsQueryable());
            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentServiceSubconSewingDetailReadModel>() {
                    garmentServiceSubconSewingDetail.GetReadModel()
                }.AsQueryable());


            _mockServiceSubconSewingRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewing>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewing>()));
            _mockServiceSubconSewingItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingItem>()));
            _mockServiceSubconSewingDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentServiceSubconSewingDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentServiceSubconSewingDetail>()));

            _mockGarmentPreparingRepository
                .Setup(s => s.RoChecking(It.IsAny<IEnumerable<string>>(), string.Empty))
                .Returns(true);

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(placeGarmentServiceSubconSewingCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }*/
    }
}
