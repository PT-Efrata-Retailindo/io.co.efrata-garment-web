using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalProducts.CommandHandlers;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;

namespace Manufactures.Tests.CommandHandlers.GarmentAvalProducts.CommandHandlers
{
    public class PlaceGarmentAvalProductCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAvalProductRepository> _mockGarmentAvalProductRepository;
        private readonly Mock<IGarmentAvalProductItemRepository> _mockGarmentAvalProductItemRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

        public PlaceGarmentAvalProductCommandHandlerTest()
        {
            _mockGarmentAvalProductRepository = CreateMock<IGarmentAvalProductRepository>();
            _mockGarmentAvalProductItemRepository = CreateMock<IGarmentAvalProductItemRepository>();
            _mockGarmentPreparingRepository = CreateMock<IGarmentPreparingRepository>();
            _mockGarmentPreparingItemRepository = CreateMock<IGarmentPreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalProductItemRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentPreparingItemRepository);
        }

        private PlaceGarmentAvalProductCommandHandler CreatePlaceGarmentCuttingInCommandHandler()
        {
            return new PlaceGarmentAvalProductCommandHandler(_MockStorage.Object);
        }


        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            PlaceGarmentAvalProductCommandHandler unitUnderTest = CreatePlaceGarmentCuttingInCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;


            // Act
            PlaceGarmentAvalProductCommand request = new PlaceGarmentAvalProductCommand()
            {
                Article = "Article",
                AvalDate =DateTimeOffset.Now,
                Items =new List<GarmentAvalProductItemValueObject>()
                {
                    new GarmentAvalProductItemValueObject()
                    {
                       APId =id,
                       BasicPrice =1,
                       DesignColor ="DesignColor",
                       Identity =id,
                       IsReceived =true,
                       PreparingId =new GarmentPreparingId(""),
                       PreparingQuantity =1,
                       PreparingItemId =new GarmentPreparingItemId(""),
                       Product =new Product(),
                       Quantity =1,
                       Uom =new Uom()
                    }
                },
                RONo = "RONo",
                Unit =new Domain.Shared.ValueObjects.UnitDepartment()
            };

            _mockGarmentAvalProductItemRepository
              .Setup(s => s.Update(It.IsAny<GarmentAvalProductItem>()))
              .Returns(Task.FromResult(It.IsAny<GarmentAvalProductItem>()));

            _mockGarmentPreparingItemRepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingItemReadModel,bool>>>()))
              .Returns(new List<Domain.GarmentPreparings.GarmentPreparingItem>()
              {
                    new Domain.GarmentPreparings.GarmentPreparingItem(id,1,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentPreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,id,"ro","fasilitas")
              });


            _mockGarmentPreparingItemRepository
             .Setup(s => s.Update(It.IsAny<Domain.GarmentPreparings.GarmentPreparingItem>()))
             .Returns(Task.FromResult(It.IsAny<Domain.GarmentPreparings.GarmentPreparingItem>()));

            _mockGarmentAvalProductRepository
             .Setup(s => s.Update(It.IsAny<GarmentAvalProduct>()))
             .Returns(Task.FromResult(It.IsAny<GarmentAvalProduct>()));

            _MockStorage
               .Setup(x => x.Save())
               .Verifiable();

            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();

        }

        }
}
