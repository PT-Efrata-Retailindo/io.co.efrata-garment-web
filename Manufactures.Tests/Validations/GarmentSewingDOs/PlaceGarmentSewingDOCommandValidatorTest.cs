using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSewingDOs.Commands;
using Manufactures.Domain.GarmentSewingDOs.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSewingDOs
{
    public class PlaceGarmentSewingDOCommandValidatorTest
    {
        private PlaceGarmentSewingDOCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSewingDOCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentSewingDOCommand();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSewingDOCommand()
            {
                Article = "Article",
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                CuttingOutId = id,
                RONo = "RONo",
                SewingDODate = DateTimeOffset.Now,
                SewingDONo = "SewingDONo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UnitFrom = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSewingDOItemValueObject>()
                {
                    new GarmentSewingDOItemValueObject()
                    {
                        BasicPrice=1,
                        Color ="Color",
                        CuttingOutDetailId =id,
                        CuttingOutItemId =id,
                        DesignColor ="DesignColor",
                        Price =1,
                        Product =new Product()
                        {
                             Id = 1,
                             Code = "Code",
                             Name = "Name"
                        },
                        Quantity =1,
                        RemainingQuantity =2,
                        SewingDOId =id,
                        Size =new SizeValueObject()
                        {
                             Id = 1,
                             Size ="Size"
                        },
                        Uom =new Uom()
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
