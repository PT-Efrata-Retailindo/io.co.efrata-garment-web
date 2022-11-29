using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSewingOuts.CommandHandlers;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentSewingOuts.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSewingOuts
{
    public class UpdateGarmentSewingOutCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSewingOutRepository> _mockSewingOutRepository;
        private readonly Mock<IGarmentSewingOutItemRepository> _mockSewingOutItemRepository;
        private readonly Mock<IGarmentSewingOutDetailRepository> _mockSewingOutDetailRepository;
        private readonly Mock<IGarmentSewingInItemRepository> _mockSewingInItemRepository;
        private readonly Mock<IGarmentCuttingInRepository> _mockCuttingInRepository;
        private readonly Mock<IGarmentCuttingInItemRepository> _mockCuttingInItemRepository;
        private readonly Mock<IGarmentCuttingInDetailRepository> _mockCuttingInDetailRepository;
        private readonly Mock<IGarmentComodityPriceRepository> _mockComodityPriceRepository;

        public UpdateGarmentSewingOutCommandHandlerTests()
        {
            _mockSewingOutRepository = CreateMock<IGarmentSewingOutRepository>();
            _mockSewingOutItemRepository = CreateMock<IGarmentSewingOutItemRepository>();
            _mockSewingOutDetailRepository = CreateMock<IGarmentSewingOutDetailRepository>();
            _mockSewingInItemRepository = CreateMock<IGarmentSewingInItemRepository>();
            _mockCuttingInRepository = CreateMock<IGarmentCuttingInRepository>();
            _mockCuttingInItemRepository = CreateMock<IGarmentCuttingInItemRepository>();
            _mockCuttingInDetailRepository = CreateMock<IGarmentCuttingInDetailRepository>();
            _mockComodityPriceRepository = CreateMock<IGarmentComodityPriceRepository>();

            _MockStorage.SetupStorage(_mockSewingOutRepository);
            _MockStorage.SetupStorage(_mockSewingOutItemRepository);
            _MockStorage.SetupStorage(_mockSewingOutDetailRepository);
            _MockStorage.SetupStorage(_mockSewingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInRepository);
            _MockStorage.SetupStorage(_mockCuttingInItemRepository);
            _MockStorage.SetupStorage(_mockCuttingInDetailRepository);
			_MockStorage.SetupStorage(_mockComodityPriceRepository);
		}
        private UpdateGarmentSewingOutCommandHandler CreateUpdateGarmentSewingOutCommandHandler()
        {
            return new UpdateGarmentSewingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid sewingInItemGuid = Guid.NewGuid();
            Guid sewingOutGuid = Guid.NewGuid();
            Guid sewingInId = Guid.NewGuid();
            UpdateGarmentSewingOutCommandHandler unitUnderTest = CreateUpdateGarmentSewingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSewingOutCommand UpdateGarmentSewingOutCommand = new UpdateGarmentSewingOutCommand()
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
                Items = new List<GarmentSewingOutItemValueObject>
                {
                    new GarmentSewingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        SewingInId= sewingInId,
                        SewingInItemId=sewingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentSewingOutDetailValueObject>
                        {
                            new GarmentSewingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };
            UpdateGarmentSewingOutCommand.SetIdentity(sewingOutGuid);

            _mockSewingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingOutReadModel>()
                {
                    new GarmentSewingOutReadModel(sewingOutGuid)
                }.AsQueryable());
            _mockSewingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentSewingOutItem>()
                {
                    new GarmentSewingOutItem(Guid.Empty, sewingOutGuid, Guid.Empty,sewingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            _mockSewingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSewingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentSewingOutDetail>()
                {
                    new GarmentSewingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
                });

            GarmentComodityPrice garmentComodity = new GarmentComodityPrice(
                Guid.NewGuid(),
                true,
                DateTimeOffset.Now,
                new UnitDepartmentId(UpdateGarmentSewingOutCommand.Unit.Id),
                UpdateGarmentSewingOutCommand.Unit.Code,
                UpdateGarmentSewingOutCommand.Unit.Name,
                new GarmentComodityId(UpdateGarmentSewingOutCommand.Comodity.Id),
                UpdateGarmentSewingOutCommand.Comodity.Code,
                UpdateGarmentSewingOutCommand.Comodity.Name,
                1000
                );
            _mockComodityPriceRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentComodityPriceReadModel>
                {
                    garmentComodity.GetReadModel()
                }.AsQueryable());

            _mockSewingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSewingInItemReadModel>
                {
                    new GarmentSewingInItemReadModel(sewingInItemGuid)
                }.AsQueryable());

            _mockSewingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOut>()));
            _mockSewingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOutItem>()));
            _mockSewingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingOutDetail>()));
            _mockSewingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSewingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSewingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentSewingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
