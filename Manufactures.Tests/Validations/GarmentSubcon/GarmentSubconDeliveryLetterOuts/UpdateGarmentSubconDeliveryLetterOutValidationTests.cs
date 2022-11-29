using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Xunit;
using static Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands.UpdateGarmentSubconDeliveryLetterOutCommand;

namespace Manufactures.Tests.Validations.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class UpdateGarmentSubconDeliveryLetterOutValidationTests
    {
        private UpdateGarmentSubconDeliveryLetterOutCommandValidator GetValidationRules()
        {
            return new UpdateGarmentSubconDeliveryLetterOutCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new UpdateGarmentSubconDeliveryLetterOutCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractType = "test",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                EPOItemId = 1,
                PONo = "test",
                Remark = "test",
                UENId = 1,
                TotalQty = 1,
                UENNo = "test",
                UsedQty = 1,
                EPONo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        ProductRemark="test",
                        FabricType="test",
                        UENItemId=1,
                        Uom=new Uom()
                        {
                            Id=1,
                            Unit="test"
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
