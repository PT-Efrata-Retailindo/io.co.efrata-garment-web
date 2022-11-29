using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.SampleCuttingIns
{
    public class UpdateGarmentSampleCuttingInCommandValidatorTest
    {
        private UpdateGarmentSampleCuttingInCommandValidator GetValidationRules()
        {
            return new UpdateGarmentSampleCuttingInCommandValidator();
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new UpdateGarmentSampleCuttingInCommand();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_HaveError_Date()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new UpdateGarmentSampleCuttingInCommand();
            unitUnderTest.CuttingInDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.PreparingDate = DateTimeOffset.Now;

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
            var unitUnderTest = new UpdateGarmentSampleCuttingInCommand()
            {
                Article = "Article",
                CutInNo = "CutInNo",
                CuttingFrom = "CuttingFrom",
                CuttingInDate = DateTimeOffset.Now,
                PreparingDate = DateTimeOffset.Now,
                CuttingType = "CuttingType",
                FC = 1,
                RONo = "RONo",
                Unit = new Domain.Shared.ValueObjects.UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSampleCuttingInItemValueObject>()
                {
                    new GarmentSampleCuttingInItemValueObject()
                    {
                        Id =id,
                        PreparingId =id,
                        SewingOutId =id,
                        SewingOutNo ="SewingOutNo",
                        UENId =1,
                        UENNo ="UENNo",
                        Details=new List<GarmentSampleCuttingInDetailValueObject>()
                        {
                            new GarmentSampleCuttingInDetailValueObject()
                            {
                                IsSave =true,
                                BasicPrice =1,
                                CuttingInQuantity =1,
                                CuttingInUom =new Domain.Shared.ValueObjects.Uom()
                                {
                                    Id =1,
                                    Unit ="Unit"
                                },
                                DesignColor ="DesignColor",
                                FabricType ="FABRIC",
                                FC =1,
                                PreparingItemId =id,
                                PreparingQuantity =1,
                                PreparingRemainingQuantity =2,
                                PreparingUom =new Domain.Shared.ValueObjects.Uom()
                                {
                                    Id =1,
                                    Unit ="Unit"
                                },
                                Price =1,
                                Product =new Domain.Shared.ValueObjects.Product()
                                {
                                    Id =1,
                                },
                                RemainingQuantity =1
                            }
                        }
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
