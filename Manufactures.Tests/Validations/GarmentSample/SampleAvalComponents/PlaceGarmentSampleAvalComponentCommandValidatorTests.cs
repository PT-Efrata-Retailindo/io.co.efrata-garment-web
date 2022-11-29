using Barebone.Tests;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.SampleAvalComponents
{
    public class PlaceGarmentSampleAvalComponentCommandValidatorTests : BaseValidatorUnitTest
    {
        private PlaceGarmentSampleAvalComponentCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSampleAvalComponentCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentSampleAvalComponentCommand();

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
            var unitUnderTest = new PlaceGarmentSampleAvalComponentCommand();

            unitUnderTest.Date = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.SewingDate = DateTimeOffset.Now;
            unitUnderTest.SampleAvalComponentType = "SEWING";

            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();

        }

        [Fact]
        public void Place_HaveError_Date_CUTTING()
        {
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentSampleAvalComponentCommand();

            unitUnderTest.Date = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.CuttingDate = DateTimeOffset.Now;
            unitUnderTest.SampleAvalComponentType = "CUTTING";

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
            var a = new PlaceGarmentSampleAvalComponentCommand();
            a.Unit = new UnitDepartment(1, "UnitCode", "UnitName");
            a.SampleAvalComponentType = "AvalComponentType";
            a.RONo = "RONo";
            a.Price = 10;
            a.Article = "Article";
            a.Comodity = new GarmentComodity(1, "Code", "Name");
            a.Date = DateTimeOffset.Now.AddDays(-1);
            a.Items = new List<PlaceGarmentSampleAvalComponentItemValueObject>
            {
                new PlaceGarmentSampleAvalComponentItemValueObject
                {
                    IsSave = true,
                    Product = new Product(1, "Code","Name"),
                    Quantity = 10,
                    BasicPrice =1,
                    Color ="Color",
                    SampleCuttingInDetailId =id,
                    DesignColor ="DesignColor",
                    IsDifferentSize =true,
                    Price =1,
                    SampleSewingOutDetailId =id,
                    SampleSewingOutItemId =id,
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
