using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalProducts.CommandHandlers;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentAvalProducts.CommandHandlers
{
   public class RemoveGarmentAvalProductCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAvalProductRepository> _mockGarmentAvalProductRepository;
        private readonly Mock<IGarmentAvalProductItemRepository> _mockGarmentAvalProductItemRepository;
        private readonly Mock<IGarmentPreparingRepository> _mockGarmentPreparingRepository;
        private readonly Mock<IGarmentPreparingItemRepository> _mockGarmentPreparingItemRepository;

        public RemoveGarmentAvalProductCommandHandlerTest()
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

        private RemoveGarmentAvalProductCommandHandler CreateRemoveGarmentAvalProductCommandHandler()
        {
            return new RemoveGarmentAvalProductCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            RemoveGarmentAvalProductCommandHandler unitUnderTest =  CreateRemoveGarmentAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            RemoveGarmentAvalProductCommand request = new RemoveGarmentAvalProductCommand();
            request.SetId(id);

            _mockGarmentAvalProductRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalProductReadModel, bool>>>()))
            .Returns(new List<GarmentAvalProduct>()
            {
                  new GarmentAvalProduct(id,"roNo","article",DateTimeOffset.Now,new Domain.Shared.ValueObjects.UnitDepartmentId(1),"unitCode","unitName")
            });


            _mockGarmentAvalProductItemRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalProductItemReadModel, bool>>>()))
            .Returns(new List<GarmentAvalProductItem>()
            {
                  new GarmentAvalProductItem(id,id,new GarmentPreparingId("value"),new GarmentPreparingItemId("value"),new ProductId(1),"productCode","productName","designColor",1,new UomId(1),"uomUnit",1,false)
            });

            _mockGarmentAvalProductItemRepository
              .Setup(s => s.Update(It.IsAny<GarmentAvalProductItem>()))
              .Returns(Task.FromResult(It.IsAny<GarmentAvalProductItem>()));

            _mockGarmentPreparingItemRepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentPreparingItemReadModel, bool>>>()))
              .Returns(new List<Domain.GarmentPreparings.GarmentPreparingItem>()
              {
                    new Domain.GarmentPreparings.GarmentPreparingItem(id,1,new Domain.GarmentPreparings.ValueObjects.ProductId(1),"productCode","productName","designColor",1,new Domain.GarmentPreparings.ValueObjects.UomId(1),"uomUnit","fabricType",1,1,id,null,"fasilitas")
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

        [Fact]
        public async Task Handle_Throws_ErrorValidation()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            RemoveGarmentAvalProductCommandHandler unitUnderTest = CreateRemoveGarmentAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;

            RemoveGarmentAvalProductCommand request = new RemoveGarmentAvalProductCommand();
            request.SetId(id);

            _mockGarmentAvalProductRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalProductReadModel, bool>>>()))
            .Returns(new List<GarmentAvalProduct>()
            {
                 
            });


            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() => unitUnderTest.Handle(request, cancellationToken));

        }
    }
    }
