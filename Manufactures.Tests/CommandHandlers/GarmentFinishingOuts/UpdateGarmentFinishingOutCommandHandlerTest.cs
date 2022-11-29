using Barebone.Tests;
using Manufactures.Application.GarmentFinishingOuts.CommandHandlers;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentFinishingOuts.ValueObjects;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns;
using FluentAssertions;
using System.Linq;

namespace Manufactures.Tests.CommandHandlers.GarmentFinishingOuts
{
    public class UpdateGarmentFinishingOutCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentFinishingOutRepository> _mockFinishingOutRepository;
        private readonly Mock<IGarmentFinishingOutItemRepository> _mockFinishingOutItemRepository;
        private readonly Mock<IGarmentFinishingOutDetailRepository> _mockFinishingOutDetailRepository;
        private readonly Mock<IGarmentFinishingInItemRepository> _mockFinishingInItemRepository;

        public UpdateGarmentFinishingOutCommandHandlerTest()
        {
            _mockFinishingOutRepository = CreateMock<IGarmentFinishingOutRepository>();
            _mockFinishingOutItemRepository = CreateMock<IGarmentFinishingOutItemRepository>();
            _mockFinishingOutDetailRepository = CreateMock<IGarmentFinishingOutDetailRepository>();
            _mockFinishingInItemRepository = CreateMock<IGarmentFinishingInItemRepository>();

            _MockStorage.SetupStorage(_mockFinishingOutRepository);
            _MockStorage.SetupStorage(_mockFinishingOutItemRepository);
            _MockStorage.SetupStorage(_mockFinishingOutDetailRepository);
            _MockStorage.SetupStorage(_mockFinishingInItemRepository);
        }
        private UpdateGarmentFinishingOutCommandHandler CreateUpdateGarmentFinishingOutCommandHandler()
        {
            return new UpdateGarmentFinishingOutCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid finishingInItemGuid = Guid.NewGuid();
            Guid finishingOutGuid = Guid.NewGuid();
            Guid finishingInId = Guid.NewGuid();
            UpdateGarmentFinishingOutCommandHandler unitUnderTest = CreateUpdateGarmentFinishingOutCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentFinishingOutCommand UpdateGarmentFinishingOutCommand = new UpdateGarmentFinishingOutCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                UnitTo = new UnitDepartment(2, "UnitCode2", "UnitName2"),
                Article = "Article",
                IsDifferentSize = true,
                FinishingTo = "FINISHING",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                FinishingOutDate = DateTimeOffset.Now,
                Items = new List<GarmentFinishingOutItemValueObject>
                {
                    new GarmentFinishingOutItemValueObject
                    {
                        Product = new Product(1, "ProductCode", "ProductName"),
                        Uom = new Uom(1, "UomUnit"),
                        FinishingInId= finishingInId,
                        FinishingInItemId=finishingInItemGuid,
                        Color="Color",
                        Size=new SizeValueObject(1, "Size"),
                        IsSave=true,
                        Quantity=1,
                        DesignColor= "ColorD",
                        Details = new List<GarmentFinishingOutDetailValueObject>
                        {
                            new GarmentFinishingOutDetailValueObject
                            {
                                Size=new SizeValueObject(1, "Size"),
                                Uom = new Uom(1, "UomUnit"),
                                Quantity=1
                            }
                        }
                    }
                },

            };
            UpdateGarmentFinishingOutCommand.SetIdentity(finishingOutGuid);

            _mockFinishingOutRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingOutReadModel>()
                {
                    new GarmentFinishingOutReadModel(finishingOutGuid)
                }.AsQueryable());
            _mockFinishingOutItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutItemReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutItem>()
                {
                    new GarmentFinishingOutItem(Guid.Empty, finishingOutGuid, Guid.Empty,finishingInItemGuid,new ProductId(1),null,null,null,new SizeId(1), null, 1, new UomId(1), null,null, 1,1,1)
                });
            _mockFinishingOutDetailRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentFinishingOutDetailReadModel, bool>>>()))
                .Returns(new List<GarmentFinishingOutDetail>()
                {
                    new GarmentFinishingOutDetail(Guid.Empty, Guid.Empty,new SizeId(1), null, 1, new UomId(1),null )
                });

            _mockFinishingInItemRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInItemReadModel>
                {
                    new GarmentFinishingInItemReadModel(finishingInItemGuid)
                }.AsQueryable());

            _mockFinishingOutRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOut>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOut>()));
            _mockFinishingOutItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutItem>()));
            _mockFinishingOutDetailRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingOutDetail>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingOutDetail>()));
            _mockFinishingInItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentFinishingInItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentFinishingInItem>()));

            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentFinishingOutCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}