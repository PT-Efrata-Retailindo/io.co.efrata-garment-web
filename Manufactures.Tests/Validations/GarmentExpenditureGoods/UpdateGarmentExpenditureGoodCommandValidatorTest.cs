using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentExpenditureGoods
{
   public class UpdateGarmentExpenditureGoodCommandValidatorTest
    {
        private UpdateGarmentExpenditureGoodCommandValidator GetValidationRules()
        {
            return new UpdateGarmentExpenditureGoodCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new UpdateGarmentExpenditureGoodCommand();

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
            var unitUnderTest = new UpdateGarmentExpenditureGoodCommand()
            {
                Article = "Article",
                Buyer =new Buyer()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Carton=1,
                Comodity =new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                ContractNo = "ContractNo",
                Description = "Description",
                ExpenditureDate =DateTimeOffset.Now,
                ExpenditureGoodNo = "ExpenditureGoodNo",
                ExpenditureType = "ExpenditureType",
                Invoice = "Invoice",
                IsReceived=true,
                RONo = "RONo",
                Unit =new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items =new List<GarmentExpenditureGoodItemValueObject>()
                {
                    new GarmentExpenditureGoodItemValueObject()
                    {
                        BasicPrice=1,
                        Description ="Description",
                        ExpenditureGoodId =id,
                        FinishedGoodStockId =id,
                        Id =id,
                        isSave =true,
                        Price =1,
                        Quantity =1,
                        ReturQuantity =1,
                        Size =new SizeValueObject()
                        {
                            Id = 1,
                        },
                        StockQuantity =2,
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
