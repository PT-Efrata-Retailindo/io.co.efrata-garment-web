using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSewingIns
{
    public class PlaceGarmentSewingInCommandValidatorTest
    {
        private PlaceGarmentSewingInCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSewingInCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentSewingInCommand();

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
            var unitUnderTest = new PlaceGarmentSewingInCommand()
            {
                Article = "Article",
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                LoadingId = id,
                LoadingNo = "LoadingNo",
                Price = 1,
                RONo = "RONo",
                SewingFrom = "SewingFrom",
                SewingInDate = DateTimeOffset.Now,
                SewingInNo = "SewingInNo",
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
                Items = new List<GarmentSewingInItemValueObject>()
                {
                    new GarmentSewingInItemValueObject()
                    {
                        BasicPrice =1,
                        Color ="Color",
                        DesignColor ="DesignColor",
                        FinishingOutDetailId =id,
                        FinishingOutItemId =id,
                        IsSave =true,
                        LoadingItemId =id,
                        Price =1,
                        Id =id,
                        Product =new Product
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        RemainingQuantity= 2,
                        SewingInId =id,
                        SewingOutDetailId =id,
                        SewingOutItemId =id,
                        Size =new SizeValueObject()
                        {
                            Id =1,
                            Size ="Size"
                        },
                        Uom=new Uom()
                        {
                            Id=1,
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
