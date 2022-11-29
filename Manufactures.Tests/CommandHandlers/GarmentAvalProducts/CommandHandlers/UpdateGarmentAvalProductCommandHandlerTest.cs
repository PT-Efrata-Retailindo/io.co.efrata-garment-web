using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentAvalProducts.CommandHandlers;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentAvalProducts.CommandHandlers
{
    public class UpdateGarmentAvalProductCommandHandlerTest : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentAvalProductRepository> _mockGarmentAvalProductRepository;
        private readonly Mock<IGarmentAvalProductItemRepository> _mockGarmentAvalProductItemRepository;
        public UpdateGarmentAvalProductCommandHandlerTest()
        {
            _mockGarmentAvalProductRepository = CreateMock<IGarmentAvalProductRepository>();
            _mockGarmentAvalProductItemRepository = CreateMock<IGarmentAvalProductItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentAvalProductItemRepository);
        }

        private UpdateGarmentAvalProductCommandHandler CreateUpdateGarmentAvalProductCommandHandler()
        {
            return new UpdateGarmentAvalProductCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateGarmentAvalProductCommandHandler unitUnderTest = CreateUpdateGarmentAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentAvalProductCommand request = new UpdateGarmentAvalProductCommand()
            {
                Article = "Article",
                AvalDate = DateTimeOffset.Now,
                Items = new List<GarmentAvalProductItemValueObject>()
                   {
                       new GarmentAvalProductItemValueObject(id,id,new GarmentPreparingId(id.ToString()),new GarmentPreparingItemId(id.ToString()),new Product(),"designColor",1,new Uom())
                       {
                           Identity =id,
                           APId =id,
                           PreparingItemId =new GarmentPreparingItemId(id.ToString()),
                           PreparingId =new GarmentPreparingId(id.ToString()),
                           Product =new Product()
                           {
                               Id =1,
                               Code ="Code",
                               Name ="Name"
                           },
                           DesignColor ="DesignColor",
                           Quantity =1,
                           Uom =new Uom()
                           {
                               Id =1,
                               Unit ="Unit"
                           }
                       }
                   },
                RONo= "RONo",
                Unit =new Domain.Shared.ValueObjects.UnitDepartment(1,"code","name"),
                

            };
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
                      new GarmentAvalProductItem(id,id,new GarmentPreparingId(id.ToString()),new GarmentPreparingItemId(id.ToString()),new ProductId(1),"productCode","productName","designColor",1,new UomId(1),"uomUnit",1,false)
              });
    

            _mockGarmentAvalProductItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentAvalProductItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentAvalProductItem>()));

            _mockGarmentAvalProductRepository
             .Setup(s => s.Update(It.IsAny<GarmentAvalProduct>()))
             .Returns(Task.FromResult(It.IsAny<GarmentAvalProduct>()));

            _MockStorage
              .Setup(x => x.Save())
              .Verifiable();

            var result = await unitUnderTest.Handle(request, cancellationToken);
            result.Should().NotBeNull();

            // Assert

        }


        [Fact]
        public async Task Handle_Throws_ErrorValidation()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateGarmentAvalProductCommandHandler unitUnderTest = CreateUpdateGarmentAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentAvalProductCommand request = new UpdateGarmentAvalProductCommand()
            {
                Article = "Article",
                AvalDate = DateTimeOffset.Now,
                Items = new List<GarmentAvalProductItemValueObject>()
                   {
                       new GarmentAvalProductItemValueObject(id,id,new GarmentPreparingId(id.ToString()),new GarmentPreparingItemId(id.ToString()),new Product(),"designColor",1,new Uom())
                       {
                           Identity =id,
                           APId =id,
                           PreparingItemId =new GarmentPreparingItemId(id.ToString()),
                           PreparingId =new GarmentPreparingId(id.ToString()),
                           Product =new Product()
                           {
                               Id =1,
                               Code ="Code",
                               Name ="Name"
                           },
                           DesignColor ="DesignColor",
                           Quantity =1,
                           Uom =new Uom()
                           {
                               Id =1,
                               Unit ="Unit"
                           }
                       }
                   },
                RONo = "RONo",
                Unit = new Domain.Shared.ValueObjects.UnitDepartment(1, "code", "name"),


            };
            request.SetId(id);

            _mockGarmentAvalProductRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentAvalProductReadModel, bool>>>()))
            .Throws(new ValidationException());


            await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => unitUnderTest.Handle(request, cancellationToken));


        }
    }
}
