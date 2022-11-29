using Barebone.Tests;
using Manufactures.Application.GarmentExpenditureGoods.CommandHandlers;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentExpenditureGoods.ValueObjects;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using System.Linq;
using System.Linq.Expressions;
using Manufactures.Domain.GarmentExpenditureGoods;
using FluentAssertions;

namespace Manufactures.Tests.CommandHandlers.GarmentExpenditureNotes
{
    public class UpdateGarmentExpenditureGoodCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentExpenditureGoodRepository> _mockExpenditureGoodRepository;
        private readonly Mock<IGarmentExpenditureGoodItemRepository> _mockExpenditureGoodItemRepository;
		private readonly Mock<IGarmentExpenditureGoodInvoiceRelationRepository> _mockInvoiceRelationRepository;
		public UpdateGarmentExpenditureGoodCommandHandlerTests()
        {
            _mockExpenditureGoodRepository = CreateMock<IGarmentExpenditureGoodRepository>();
            _mockExpenditureGoodItemRepository = CreateMock<IGarmentExpenditureGoodItemRepository>();
			_mockInvoiceRelationRepository = CreateMock<IGarmentExpenditureGoodInvoiceRelationRepository>();

			_MockStorage.SetupStorage(_mockExpenditureGoodRepository);
            _MockStorage.SetupStorage(_mockExpenditureGoodItemRepository);
			_MockStorage.SetupStorage(_mockInvoiceRelationRepository);
		}

        private UpdateGarmentExpenditureGoodCommandHandler CreateUpdateGarmentExpenditureGoodCommandHandler()
        {
            return new UpdateGarmentExpenditureGoodCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid ExpenditureGoodGuid = Guid.NewGuid();
            Guid ExpenditureGoodItemGuid = Guid.NewGuid();
            UpdateGarmentExpenditureGoodCommandHandler unitUnderTest = CreateUpdateGarmentExpenditureGoodCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentExpenditureGoodCommand UpdateGarmentExpenditureGoodCommand = new UpdateGarmentExpenditureGoodCommand()
            {
                RONo = "RONo",
                Unit = new UnitDepartment(1, "UnitCode", "UnitName"),
                Article = "Article",
                Comodity = new GarmentComodity(1, "ComoCode", "ComoName"),
                Buyer = new Buyer(1, "buyerCode", "buyerName"),
                ExpenditureDate = DateTimeOffset.Now,
                Items = new List<GarmentExpenditureGoodItemValueObject>
                {
                    new GarmentExpenditureGoodItemValueObject
                    {
                        Uom = new Uom(1, "UomUnit"),
                        FinishedGoodStockId= new Guid(),
                        Size=new SizeValueObject(1, "Size"),
                        isSave=true,
                        Quantity=1,
                    }
                },

            };
            UpdateGarmentExpenditureGoodCommand.SetIdentity(ExpenditureGoodGuid);

            _mockExpenditureGoodRepository
                .Setup(s => s.Query)
                .Returns(new List<GarmentExpenditureGoodReadModel>()
                {
                    new GarmentExpenditureGoodReadModel(ExpenditureGoodGuid)
                }.AsQueryable());
            _mockExpenditureGoodItemRepository
                .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentExpenditureGoodItemReadModel, bool>>>()))
                .Returns(new List<GarmentExpenditureGoodItem>()
                {
                    new GarmentExpenditureGoodItem(ExpenditureGoodItemGuid, ExpenditureGoodGuid, Guid.Empty,new SizeId(1), null, 1,0, new UomId(1), null,null, 1,1)
                });
            
            _mockExpenditureGoodRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGood>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGood>()));
            _mockExpenditureGoodItemRepository
                .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodItem>()))
                .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodItem>()));
			GarmentExpenditureGoodInvoiceRelation invoiceRelation = new GarmentExpenditureGoodInvoiceRelation(Guid.NewGuid(), UpdateGarmentExpenditureGoodCommand.Identity, UpdateGarmentExpenditureGoodCommand.ExpenditureGoodNo, "unit", "ro", 10, UpdateGarmentExpenditureGoodCommand.PackingListId, 1, UpdateGarmentExpenditureGoodCommand.Invoice);

			_mockInvoiceRelationRepository
				.Setup(s => s.Query)
				.Returns(new List<GarmentExpenditureGoodInvoiceRelationReadModel>
				{
					invoiceRelation.GetReadModel()
				}.AsQueryable());

			_mockInvoiceRelationRepository
				 .Setup(s => s.Update(It.IsAny<GarmentExpenditureGoodInvoiceRelation>()))
				 .Returns(Task.FromResult(It.IsAny<GarmentExpenditureGoodInvoiceRelation>()));

			_MockStorage
                .Setup(x => x.Save())
                .Verifiable();

            // Act
            var result = await unitUnderTest.Handle(UpdateGarmentExpenditureGoodCommand, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}

