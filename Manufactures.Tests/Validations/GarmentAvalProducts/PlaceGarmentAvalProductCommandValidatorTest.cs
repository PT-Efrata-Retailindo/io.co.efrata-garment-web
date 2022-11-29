using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentAvalProducts.Commands;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Product = Manufactures.Domain.GarmentAvalProducts.ValueObjects.Product;

namespace Manufactures.Tests.Validations.GarmentAvalProducts
{
   public class PlaceGarmentAvalProductCommandValidatorTest
    {
        private PlaceGarmentAvalProductCommandValidator GetValidationRules()
        {
            return new PlaceGarmentAvalProductCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentAvalProductCommand();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentAvalProductCommand();

            unitUnderTest.AvalDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.PreparingDate = DateTimeOffset.Now;

            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();

        }

        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentAvalProductCommand()
            {
                Article = "Article",
                AvalDate =DateTimeOffset.Now,
                PreparingDate= DateTimeOffset.Now,
                RONo = "RONo",
                Unit =new UnitDepartment()
                {
                    Id =1,
                    Code ="Code",
                    Name ="name"
                },
                Items =new List<GarmentAvalProductItemValueObject>()
                {
                  
                    new GarmentAvalProductItemValueObject()
                    {
                        APId =Guid.NewGuid(),
                        BasicPrice =1,
                        DesignColor ="DesignColor",
                        Identity =Guid.NewGuid(),
                        IsReceived =true,
                        PreparingId =new GarmentPreparingId(id.ToString()),
                        PreparingItemId =new GarmentPreparingItemId(id.ToString()),
                        PreparingQuantity =2,
                        Product =new Product()
                        {
                            Id =1,
                            Code ="Code",
                            Name ="Name"
                        },
                        Quantity =1,
                        Uom =new Domain.GarmentAvalProducts.ValueObjects.Uom()
                        {
                            Id =1,
                            Unit ="Unit"
                        },
                        
                    }
                }
            };

            var validator = GetValidationRules();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();

        }

        }
}
