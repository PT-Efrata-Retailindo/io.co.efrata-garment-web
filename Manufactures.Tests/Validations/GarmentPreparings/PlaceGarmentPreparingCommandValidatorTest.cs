using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentLoadings.Commands;
using Manufactures.Domain.GarmentPreparings.Commands;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentPreparings
{
    public class PlaceGarmentPreparingCommandValidatorTest
    {
        private PlaceGarmentPreparingCommandValidator GetValidationRules()
        {
            return new PlaceGarmentPreparingCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentPreparingCommand();

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
            var unitUnderTest = new PlaceGarmentPreparingCommand();
            unitUnderTest.ProcessDate = DateTimeOffset.Now.AddDays(-7);
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
            var unitUnderTest = new PlaceGarmentPreparingCommand()
            {
                Article = "Article",
                IsCuttingIn = true,
                ProcessDate = DateTimeOffset.Now,
                ExpenditureDate= DateTimeOffset.Now,
                RONo = "RONo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UENId = 1,
                UENNo = "UENNo",
                Items = new List<GarmentPreparingItemValueObject>()
                {
                    new GarmentPreparingItemValueObject()
                    {
                        BasicPrice =1,
                        DesignColor ="DesignColor",
                        FabricType ="FabricType",
                        GarmentPreparingId =id,
                        Product =new Product()
                        {
                            Id =1,
                            Code ="Code",
                            Name ="Name"
                        },
                        Quantity =1,
                        RemainingQuantity=2,
                        UENItemId =1,
                        Uom = new Uom()
                        {
                            Id=1,
                            Unit ="Unit"
                        }
                    }
                }
            };
            // Act
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }

    }
}
