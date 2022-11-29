using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentDeliveryReturns
{
   public class PlaceGarmentDeliveryReturnCommandValidatorTest
    {
        private PlaceGarmentDeliveryReturnCommandValidator GetValidationRules()
        {
            return new PlaceGarmentDeliveryReturnCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentDeliveryReturnCommand()
            {
                Items = new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject()
                    {
                        Product =new Product()
                    }
                }
            };
           
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }


        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentDeliveryReturnCommand()
            {
                Article = "Article",
                DRNo = "DRNo",
                IsUsed = true,
                PreparingId = id.ToString(),
                ReturnDate = DateTimeOffset.Now,
                ReturnType = "ReturnType",
                RONo = "RONo",
                Storage =new Storage()
                {
                    Id =1,
                    Code ="Code",
                    Name ="Name"
                },
                UENId =1,
                UnitDOId =1,
                UnitDONo = "UnitDONo",
                Unit =new UnitDepartment()
                {
                    Id =1,
                    Code = "Code",
                    Name = "Name"
                },
                Items =new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject()
                    {
                        IsSave =true,
                        DesignColor ="DesignColor",
                        DRId =id,
                        PreparingItemId =id.ToString(),
                        Product =new Product()
                        {
                            Id=1,
                            Code ="Code",
                            Name ="FABRIC"
                        },
                        Quantity =1,
                        QuantityUENItem =1,
                        RemainingQuantityPreparingItem =2,
                        RONo ="RONo",
                        UENItemId =1,
                        UnitDOItemId =1,
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit="Unit"
                        }
                    }
                }
            };

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}
