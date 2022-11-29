using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSewingOuts
{
    public class PlaceGarmentSewingOutCommandValidatorTest
    {
        private PlaceGarmentSewingOutCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSewingOutCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange
           
            var unitUnderTest = new PlaceGarmentSewingOutCommand();

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
            var unitUnderTest = new PlaceGarmentSewingOutCommand();
            unitUnderTest.SewingOutDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.SewingInDate = DateTimeOffset.Now;

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
            var unitUnderTest = new PlaceGarmentSewingOutCommand()
            {
                IsUsed = true,
                IsSave = true,
                Article = "Article",
                Price = 1,
                RONo = "RONo",
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Buyer = new Buyer()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                IsDifferentSize = true,
                SewingOutDate = DateTimeOffset.Now,
                SewingInDate= DateTimeOffset.Now,
                SewingOutNo = "SewingOutNo",
                SewingTo = "SewingTo",
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                UnitTo = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSewingOutItemValueObject>()
                {
                    new GarmentSewingOutItemValueObject()
                    {
                        BasicPrice=1,
                        Color ="Color",
                        DesignColor ="DesignColor",
                        Id =id,
                        IsDifferentSize =true,
                        IsSave =true,
                        Price =1,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        SewingInId =id,
                        RemainingQuantity =2,
                        SewingInItemId =id,
                        SewingInQuantity =1,
                        SewingOutId =id,
                        Size =new SizeValueObject()
                        {
                            Id =1,
                            Size ="Size"
                        },
                        TotalQuantity =1,
                        Uom =new Uom()
                        {
                            Id =1,
                            Unit ="Unit"
                        },
                        Details =new List<GarmentSewingOutDetailValueObject>()
                        {
                            new GarmentSewingOutDetailValueObject()
                            {
                                Id =id,
                                Quantity =1,
                                SewingOutItemId =id,
                                Size =new SizeValueObject()
                                {
                                    Id =1,
                                    Size ="Size"
                                },
                                Uom =new Uom()
                                {
                                    Id =1,
                                    Unit ="Unit"
                                }
                            }
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
