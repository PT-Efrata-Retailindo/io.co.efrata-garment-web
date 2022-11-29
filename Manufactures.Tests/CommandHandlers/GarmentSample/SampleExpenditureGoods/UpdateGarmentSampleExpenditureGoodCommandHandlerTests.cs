using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleExpenditureGoods.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ValueObjects;
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

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleExpenditureGoods
{
    public class UpdateGarmentSampleExpenditureGoodCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleExpenditureGoodRepository> _mockExpenditureGoodRepository;
        private readonly Mock<IGarmentSampleExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;

        public UpdateGarmentSampleExpenditureGoodCommandHandlerTests()
        {
            _mockExpenditureGoodRepository = CreateMock<IGarmentSampleExpenditureGoodRepository>();
            _mockExpenditureGoodItemRepository = CreateMock<IGarmentSampleExpenditureGoodItemRepository>();

            _MockStorage.SetupStorage(_mockExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodItemRepository);
        }

        private UpdateGarmentSampleExpenditureGoodCommandHandler CreateUpdateGarmentSampleExpenditureGoodCommandHandler()
        {
            return new UpdateGarmentSampleExpenditureGoodCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            UpdateGarmentSampleExpenditureGoodCommandHandler unitUnderTest = CreateUpdateGarmentSampleExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSampleExpenditureGoodCommand updateGarmentSampleExpenditureGoodCommand = new UpdateGarmentSampleExpenditureGoodCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Buyer = new Buyer(1, "buyerCode", "buyerName"),
                ExpenditureDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleExpenditureGoodItemValueObject>
                {
                    new GarmentSampleExpenditureGoodItemValueObject
                    {
                        Uom = new Uom(1, "UomUnit"),
                        FinishedGoodStockId= new Guid(),
                        Size=new SizeValueObject(1, "Size"),
                        isSave=true,
                        Quantity=1,
                    }
                },

            };
            updateGarmentSampleExpenditureGoodCommand.SetIdentity(ExpenditureGoodGuid);

            _mockExpenditureGoodRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentSampleExpenditureGoodReadModel>()
                {
                    new GarmentSampleExpenditureGoodReadModel(ExpenditureGoodGuid)
                }.AsQueryable());
            _mockExpenditureGoodItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleExpenditureGoodItemReadModel, bool>>>()))
                .Returns(new List<GarmentSampleExpenditureGoodItem>()
                {
                    new GarmentSampleExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, Guid.Empty,new SizeId(1), null, 1,0, new UomId(1), null,null, 1,1)
                });

            _mockExpenditureGoodRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleExpenditureGood>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleExpenditureGood>()));
            _mockExpenditureGoodItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentSampleExpenditureGoodItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentSampleExpenditureGoodItem>()));


            _MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(updateGarmentSampleExpenditureGoodCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
