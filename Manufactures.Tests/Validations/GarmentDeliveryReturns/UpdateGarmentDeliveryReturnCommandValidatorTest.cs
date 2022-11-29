using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentDeliveryReturns.Commands;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentDeliveryReturns
{
    public class UpdateGarmentDeliveryReturnCommandValidatorTest
    {
        private UpdateGarmentDeliveryReturnCommandValidator GetValidationRules()
        {
            return new UpdateGarmentDeliveryReturnCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new UpdateGarmentDeliveryReturnCommand()
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
            var unitUnderTest = new UpdateGarmentDeliveryReturnCommand()
            {
                Article = "Article",
                DRNo = "DRNo",
                IsUsed = true,
                PreparingId = id.ToString(),
                ReturnDate = DateTimeOffset.Now,
                ReturnType = "ReturnType",
                RONo = "RONo",
                Storage = new Storage()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UENId = 1,
                UnitDOId = 1,
                UnitDONo = "UnitDONo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentDeliveryReturnItemValueObject>()
                {
                    new GarmentDeliveryReturnItemValueObject(id,id,1,1,id.ToString(),new Product(),"DesignColor","roNo",1,new Uom(),id,1,1,true),
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
