using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentFinishingIns
{
    public class PlaceGarmentFinishingInCommandValidatorTest
    {
        private PlaceGarmentFinishingInCommandValidator GetValidationRules()
        {
            return new PlaceGarmentFinishingInCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentFinishingInCommand();

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
            var unitUnderTest = new PlaceGarmentFinishingInCommand();
            unitUnderTest.FinishingInDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.SewingOutDate = DateTimeOffset.Now;

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
            var unitUnderTest = new PlaceGarmentFinishingInCommand()
            {
                DONo = "DONo",
                Article = "Article",
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                DOId = 1,
                FinishingInDate = DateTimeOffset.Now,
                SewingOutDate= DateTimeOffset.Now,
                FinishingInNo = "FinishingInNo",
                FinishingInType = "FinishingInType",
                Price = 1,
                RONo = "RONo",
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
                Items = new List<GarmentFinishingInItemValueObject>()
                {
                    new GarmentFinishingInItemValueObject()
                    {
                        BasicPrice=1,
                        Color ="Color",
                        DesignColor ="DesignColor",
                        FinishingInId =id,
                        Id =id,
                        Price=1,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        RemainingQuantity =2,
                        SewingOutDetailId=id,
                        SewingOutItemId =id,
                        Size =new SizeValueObject()
                        {
                            Id=1,
                            Size ="Size"
                        },
                        Uom=new Uom()
                        {
                            Id =1,
                            Unit="unit"
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
