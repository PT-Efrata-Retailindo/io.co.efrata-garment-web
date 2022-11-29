using Barebone.Tests;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentAdjustments
{
   public class PlaceGarmentAdjustmentCommandValidatorTest : BaseValidatorUnitTest
    {
        private PlaceGarmentAdjustmentCommandValidator GetValidationRules()
        {
            return new PlaceGarmentAdjustmentCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentAdjustmentCommand();
            unitUnderTest.Items = new List<GarmentAdjustmentItemValueObject>()
            {
                new GarmentAdjustmentItemValueObject()
                {
                    Quantity =1,
                    RemainingQuantity =0
                }
            };
            var validator = GetValidationRules();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentAdjustmentCommand();
            unitUnderTest.AdjustmentDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.ProcessDate = DateTimeOffset.Now;
            unitUnderTest.Items = new List<GarmentAdjustmentItemValueObject>()
            {
                new GarmentAdjustmentItemValueObject()
                {
                    Quantity =1,
                    RemainingQuantity =0
                }
            };
            var validator = GetValidationRules();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentAdjustmentCommand();
            Guid id = Guid.NewGuid();
            unitUnderTest.AdjustmentDate = DateTimeOffset.Now.AddDays(-1);
            unitUnderTest.AdjustmentDesc = "AdjustmentDesc";
            unitUnderTest.AdjustmentNo = "AdjustmentNo";
            unitUnderTest.AdjustmentType = "BARANG JADI";
            unitUnderTest.Article = "Article";
            unitUnderTest.Comodity = new GarmentComodity()
            {
                Id =1,
                Code ="Code",
                Name = "Name"
            };
            unitUnderTest.Price = 1;
            unitUnderTest.RONo = "RONo";
            unitUnderTest.Unit = new UnitDepartment()
            {
                Id = 1,
                Code = "Code",
                Name = "Name"
            };

            unitUnderTest.Items = new List<GarmentAdjustmentItemValueObject>()
            {
                new GarmentAdjustmentItemValueObject()
                {
                    AdjustmentType ="BARANG JADI",
                    BasicPrice =1,
                    Color ="Color",
                    DesignColor ="DesignColor",
                    IsSave =true,
                    Price =1,
                    Product =new Product()
                    {
                        Id =1,
                        Code ="Code",
                        Name ="Name"
                    },
                    Quantity =1,
                    Size =new SizeValueObject()
                    {
                        Id =1,
                        Size ="Size"
                    },
                    Uom =new Uom()
                    {
                        Id =1,
                        Unit ="Unit"
                    },
                    RemainingQuantity =2,
                    AdjustmentId =id
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
