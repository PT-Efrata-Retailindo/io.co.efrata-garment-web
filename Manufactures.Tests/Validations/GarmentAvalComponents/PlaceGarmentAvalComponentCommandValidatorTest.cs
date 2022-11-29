using Barebone.Tests;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentAvalComponents
{
    public class PlaceGarmentAvalComponentCommandValidatorTest :BaseValidatorUnitTest
    {
        private PlaceGarmentAvalComponentCommandValidator GetValidationRules()
        { 
            return new PlaceGarmentAvalComponentCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentAvalComponentCommand();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            // validator.ShouldHaveValidationErrorFor(r => r.Unit, null as UnitDepartment);
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date_SEWING()
        {
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentAvalComponentCommand();

            unitUnderTest.Date = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.SewingDate = DateTimeOffset.Now;
            unitUnderTest.AvalComponentType = "SEWING";

            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
            
        }

        [Fact]
        public void Place_HaveError_Date_CUTTING()
        {
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentAvalComponentCommand();

            unitUnderTest.Date = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.CuttingDate = DateTimeOffset.Now;
            unitUnderTest.AvalComponentType = "CUTTING";

            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();

        }

        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var validator = GetValidationRules();
            var a = new PlaceGarmentAvalComponentCommand();
            a.Unit = new UnitDepartment(1, "UnitCode", "UnitName");
            a.AvalComponentType = "AvalComponentType";
            a.RONo = "RONo";
            a.Price = 10;
            a.Article = "Article";
            a.Comodity = new GarmentComodity(1, "Code", "Name");
            a.Date = DateTimeOffset.Now.AddDays(-1);
            a.Items = new List<PlaceGarmentAvalComponentItemValueObject>
            {
                new PlaceGarmentAvalComponentItemValueObject
                {
                    IsSave = true,
                    Product = new Product(1, "Code","Name"),
                    Quantity = 10,
                    BasicPrice =1,
                    Color ="Color",
                    CuttingInDetailId =id,
                    DesignColor ="DesignColor",
                    IsDifferentSize =true,
                    Price =1,
                    SewingOutDetailId =id,
                    SewingOutItemId =id,
                    Size=new SizeValueObject()
                    {
                        Id =1,
                        Size="Size"
                    },
                    SourceQuantity = 10
                }
            };

            // Action
            var result = validator.TestValidate(a);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}
