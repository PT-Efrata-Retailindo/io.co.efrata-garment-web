using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ValueObjects;
using Manufactures.Domain.GarmentReturGoodReturns.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentExpenditureGoodReturns
{
    public class PlaceGarmentReturGoodCommandValidatorTest
    {
        private PlaceGarmentReturGoodCommandValidator GetValidationRules()
        {
            return new PlaceGarmentReturGoodCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentExpenditureGoodReturnCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentExpenditureGoodReturnCommand();
            unitUnderTest.ReturDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.ExpenditureDate = DateTimeOffset.Now;

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentExpenditureGoodReturnCommand()
            {
                Article = "Article",
                Buyer = new Buyer()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Invoice = "Invoice",
                Price = 1,
                ReturDate = DateTimeOffset.Now,
                ExpenditureDate= DateTimeOffset.Now,
                ReturDesc = "ReturDesc",
                ReturGoodNo = "ReturGoodNo",
                ReturType = "ReturType",
                RONo = "RONo",
                ExpenditureNo ="ExNo",
                DONo = "DONo123",
                URNNo ="UrnNo123",
                BCNo ="BCNo123",
                BCType ="3.0",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentExpenditureGoodReturnItemValueObject>()
                {
                    new GarmentExpenditureGoodReturnItemValueObject()
                    {
                        BasicPrice=1,
                        Description ="Description",
                        ExpenditureGoodId =id,
                        ExpenditureGoodItemId =id,
                        FinishedGoodStockId =id,
                        isSave =true,
                        Price =1,
                        Quantity =1,
                        ReturId =id,
                        Size =new SizeValueObject()
                        {
                            Id =1,
                            Size ="Size"
                        },
                        Id =id,
                        StockQuantity=1,
                        Uom=new Uom()
                        {
                            Id =1,
                            Unit ="Unit"
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
