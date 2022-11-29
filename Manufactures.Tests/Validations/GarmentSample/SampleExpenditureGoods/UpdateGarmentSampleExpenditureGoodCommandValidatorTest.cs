using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.SampleExpenditureGoods
{
    public class UpdateGarmentSampleExpenditureGoodCommandValidatorTest
    {
        private UpdateGarmentSampleExpenditureGoodCommandValidator GetValidationRules()
        {
            return new UpdateGarmentSampleExpenditureGoodCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new UpdateGarmentSampleExpenditureGoodCommand();

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
            var unitUnderTest = new UpdateGarmentSampleExpenditureGoodCommand()
            {
                Article = "Article",
                Buyer = new Buyer()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Carton = 1,
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                ContractNo = "ContractNo",
                Description = "Description",
                ExpenditureDate = DateTimeOffset.Now,
                ExpenditureGoodNo = "ExpenditureGoodNo",
                ExpenditureType = "ExpenditureType",
                Invoice = "Invoice",
                IsReceived = true,
                RONo = "RONo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSampleExpenditureGoodItemValueObject>()
                {
                    new GarmentSampleExpenditureGoodItemValueObject()
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
