using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleAvalProducts.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleAvalProducts.CommandHandlers
{
    public class PlaceGarmentSampleAvalProductCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleAvalProductRepository> _mockGarmentSampleAvalProductRepository;
        private readonly Mock<IGarmentSampleAvalProductItemRepository> _mockGarmentSampleAvalProductItemRepository;
        private readonly Mock<IGarmentSamplePreparingRepository> _mockGarmentSamplePreparingRepository;
        private readonly Mock<IGarmentSamplePreparingItemRepository> _mockGarmentSamplePreparingItemRepository;

        public PlaceGarmentSampleAvalProductCommandHandlerTests()
        {
            _mockGarmentSampleAvalProductRepository = CreateMock<IGarmentSampleAvalProductRepository>();
            _mockGarmentSampleAvalProductItemRepository = CreateMock<IGarmentSampleAvalProductItemRepository>();
            _mockGarmentSamplePreparingRepository = CreateMock<IGarmentSamplePreparingRepository>();
            _mockGarmentSamplePreparingItemRepository = CreateMock<IGarmentSamplePreparingItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalProductItemRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingRepository);
            _MockStorage.SetupStorage(_mockGarmentSamplePreparingItemRepository);
        }

        private PlaceGarmentSampleAvalProductCommandHandler CreatePlaceGarmentSampleAvalProductCommandHandler()
        {
            return new PlaceGarmentSampleAvalProductCommandHandler(_MockStorage.Object);
        }


        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            PlaceGarmentSampleAvalProductCommandHandler unitUnderTest = CreatePlaceGarmentSampleAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            PlaceGarmentSampleAvalProductCommand request = new PlaceGarmentSampleAvalProductCommand()
            {
                Article = "Article",
                AvalDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleAvalProductItemValueObject>()
                {
                    new GarmentSampleAvalProductItemValueObject()
                    {
                       APId =id,
                       BasicPrice =1,
                       DesignColor ="DesignColor",
                       Identity =id,
                       IsReceived =true,
                       SamplePreparingId =new GarmentSamplePreparingId(""),
                       PreparingQuantity =1,
                       SamplePreparingItemId =new GarmentSamplePreparingItemId(""),
                       Product =new Product(),
                       Quantity =1,
                       Uom =new Uom()
                    }
                },
                RONo = "RONo",
                Unit = new Domain.Shared.ValueObjects.UnitDepartment()
            };

            _mockGarmentSampleAvalProductItemRepository
              .Setup(s => s.Update(It.IsAny<GarmentSampleAvalProductItem>()))
              .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalProductItem>()));

            _mockGarmentSamplePreparingItemRepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSamplePreparingItemReadModel, bool>>>()))
              .Returns(new List<Domain.GarmentSample.SamplePreparings.GarmentSamplePreparingItem>()
              {
                    new Domain.GarmentSample.SamplePreparings.GarmentSamplePreparingItem(id,1,new Domain.GarmentSample.SamplePreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentSample.SamplePreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,id,"ro")
              });


            _mockGarmentSamplePreparingItemRepository
             .Setup(s => s.Update(It.IsAny<Domain.GarmentSample.SamplePreparings.GarmentSamplePreparingItem>()))
             .Returns(Task.FromResult(It.IsAny<Domain.GarmentSample.SamplePreparings.GarmentSamplePreparingItem>()));

            _mockGarmentSampleAvalProductRepository
             .Setup(s => s.Update(It.IsAny<GarmentSampleAvalProduct>()))
             .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalProduct>()));

            _MockStorage
               .Setup(x => x.Save())
               .Verifiable();

            var result = await unitUnderTest.Handle(request, cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
