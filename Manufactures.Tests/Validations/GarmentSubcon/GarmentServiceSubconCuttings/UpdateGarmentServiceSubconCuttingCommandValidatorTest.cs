using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubcon.GarmentServiceSubconCuttings
{
    public class UpdateGarmentServiceSubconCuttingCommandValidatorTest
    {
        private UpdateGarmentServiceSubconCuttingCommandValidator GetValidationRules()
        {
            return new UpdateGarmentServiceSubconCuttingCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new UpdateGarmentServiceSubconCuttingCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentServiceSubconCuttingCommand()
            {
                IsUsed = true,
                SubconDate = DateTimeOffset.Now,
                SubconNo = "CuttingOutNo",
                Uom = new Uom()
                {
                    Id = 1,
                    Unit = "Unit"
                },
                QtyPacking = 1,
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Buyer = new Buyer
                {
                    Id = 1,
                    Code = "Buyercode",
                    Name = "BuyerName"
                },
                Items = new List<GarmentServiceSubconCuttingItemValueObject>()
                {
                    new GarmentServiceSubconCuttingItemValueObject()
                    {
                        Id =id,
                        ServiceSubconCuttingId =id,
                        Article = "Article",
                        RONo = "RONo",
                        Comodity = new GarmentComodity()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Details= new List<GarmentServiceSubconCuttingDetailValueObject>()
                        {
                            new GarmentServiceSubconCuttingDetailValueObject
                            {
                                DesignColor ="DesignColor",
                                IsSave =true,
                                CuttingInQuantity =1,
                                Sizes= new List<GarmentServiceSubconCuttingSizeValueObject>()
                                {
                                    new GarmentServiceSubconCuttingSizeValueObject
                                    {
                                        Size= new SizeValueObject()
                                        {
                                            Id=1,
                                            Size="size"
                                        },
                                        Color="RED",
                                        Quantity=1,
                                        Uom= new Uom
                                        {
                                            Id=1,
                                            Unit="uom"
                                        }
                                    }
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
