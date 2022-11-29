using Barebone.Tests;
using FluentAssertions;
using Manufactures.Application.GarmentSample.SampleAvalProducts.CommandHandlers;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.CommandHandlers.GarmentSample.SampleAvalProducts.CommandHandlers
{
    public class UpdateGarmentSampleAvalProductCommandHandlerTests : BaseCommandUnitTest
    {
        private readonly Mock<IGarmentSampleAvalProductRepository> _mockGarmentSampleAvalProductRepository;
        private readonly Mock<IGarmentSampleAvalProductItemRepository> _mockGarmentSampleAvalProductItemRepository;
        public UpdateGarmentSampleAvalProductCommandHandlerTests()
        {
            _mockGarmentSampleAvalProductRepository = CreateMock<IGarmentSampleAvalProductRepository>();
            _mockGarmentSampleAvalProductItemRepository = CreateMock<IGarmentSampleAvalProductItemRepository>();

            _MockStorage.SetupStorage(_mockGarmentSampleAvalProductRepository);
            _MockStorage.SetupStorage(_mockGarmentSampleAvalProductItemRepository);
        }

        private UpdateGarmentSampleAvalProductCommandHandler CreateUpdateGarmentSampleAvalProductCommandHandler()
        {
            return new UpdateGarmentSampleAvalProductCommandHandler(_MockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            UpdateGarmentSampleAvalProductCommandHandler unitUnderTest = CreateUpdateGarmentSampleAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSampleAvalProductCommand request = new UpdateGarmentSampleAvalProductCommand()
            {
                Article = "Article",
                AvalDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleAvalProductItemValueObject>()
                   {
                       new GarmentSampleAvalProductItemValueObject(id,id,new GarmentSamplePreparingId(id.ToString()),new GarmentSamplePreparingItemId(id.ToString()),new Product(),"designColor",1,new Uom())
                       {
                           Identity =id,
                           APId =id,
                           SamplePreparingItemId =new GarmentSamplePreparingItemId(id.ToString()),
                           SamplePreparingId =new GarmentSamplePreparingId(id.ToString()),
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

            _mockGarmentSampleAvalProductRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalProductReadModel, bool>>>()))
            .Returns(new List<GarmentSampleAvalProduct>()
            {
                  new GarmentSampleAvalProduct(id,"roNo","article",DateTimeOffset.Now,new Domain.Shared.ValueObjects.UnitDepartmentId(1),"unitCode","unitName")
            });

            _mockGarmentSampleAvalProductItemRepository
              .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalProductItemReadModel, bool>>>()))
              .Returns(new List<GarmentSampleAvalProductItem>()
              {
                      new GarmentSampleAvalProductItem(id,id,new GarmentSamplePreparingId(id.ToString()),new GarmentSamplePreparingItemId(id.ToString()),new ProductId(1),"productCode","productName","designColor",1,new UomId(1),"uomUnit",1,false)
              });


            _mockGarmentSampleAvalProductItemRepository
            .Setup(s => s.Update(It.IsAny<GarmentSampleAvalProductItem>()))
            .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalProductItem>()));

            _mockGarmentSampleAvalProductRepository
             .Setup(s => s.Update(It.IsAny<GarmentSampleAvalProduct>()))
             .Returns(Task.FromResult(It.IsAny<GarmentSampleAvalProduct>()));

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
            UpdateGarmentSampleAvalProductCommandHandler unitUnderTest = CreateUpdateGarmentSampleAvalProductCommandHandler();
            CancellationToken cancellationToken = CancellationToken.None;
            UpdateGarmentSampleAvalProductCommand request = new UpdateGarmentSampleAvalProductCommand()
            {
                Article = "Article",
                AvalDate = DateTimeOffset.Now,
                Items = new List<GarmentSampleAvalProductItemValueObject>()
                   {
                       new GarmentSampleAvalProductItemValueObject(id,id,new GarmentSamplePreparingId(id.ToString()),new GarmentSamplePreparingItemId(id.ToString()),new Product(),"designColor",1,new Uom())
                       {
                           Identity =id,
                           APId =id,
                           SamplePreparingItemId =new GarmentSamplePreparingItemId(id.ToString()),
                           SamplePreparingId =new GarmentSamplePreparingId(id.ToString()),
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

            _mockGarmentSampleAvalProductRepository
            .Setup(s => s.Find(It.IsAny<Expression<Func<GarmentSampleAvalProductReadModel, bool>>>()))
            .Throws(new ValidationException());


            await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(() => unitUnderTest.Handle(request, cancellationToken));
        }
    }
}
