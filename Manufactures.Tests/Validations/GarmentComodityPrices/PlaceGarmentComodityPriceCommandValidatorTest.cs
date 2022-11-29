using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentComodityPrices.Commands;
using Manufactures.Domain.GarmentComodityPrices.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentComodityPrices
{
    public class PlaceGarmentComodityPriceCommandValidatorTest
    {
        private PlaceGarmentComodityPriceCommandValidator GetValidationRules()
        {
            return new PlaceGarmentComodityPriceCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentComodityPriceCommand();

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
            var unitUnderTest = new PlaceGarmentComodityPriceCommand()
            {
                Date = DateTimeOffset.Now,
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentComodityPriceItemValueObject>()
                {
                   new GarmentComodityPriceItemValueObject()
                   {
                       Comodity =new GarmentComodity()
                       {
                           Id =1,
                           Code ="Code",
                           Name ="Name"
                       },
                       IsValid =true,
                       NewPrice =1,
                       Price =1,
                       Unit =new UnitDepartment()
                       {
                           Id =1,
                           Code ="Code",
                           Name="Name"
                       },
                       Date =DateTimeOffset.Now,
                       Id =id
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
