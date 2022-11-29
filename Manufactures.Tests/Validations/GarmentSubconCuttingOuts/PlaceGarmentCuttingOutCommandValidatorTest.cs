using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubconCuttingOuts.Commands;
using Manufactures.Domain.GarmentSubconCuttingOuts.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubconCuttingOuts
{
    public class PlaceGarmentCuttingOutCommandValidatorTest
    {
        private PlaceGarmentCuttingOutCommandValidator GetValidationRules()
        {
            return new PlaceGarmentCuttingOutCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentSubconCuttingOutCommand();

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
            var unitUnderTest = new PlaceGarmentSubconCuttingOutCommand();
            unitUnderTest.CuttingOutDate = DateTimeOffset.Now.AddDays(-7);
            unitUnderTest.CuttingInDate = DateTimeOffset.Now;

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
            var unitUnderTest = new PlaceGarmentSubconCuttingOutCommand()
            {
                Article = "Article",
                Price = 1,
                RONo = "RONo",
                Comodity = new GarmentComodity()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Unit = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                CutOutNo = "CutOutNo",
                CuttingOutDate = DateTimeOffset.Now,
                CuttingInDate= DateTimeOffset.Now,
                CuttingOutType = "CuttingOutType",
                EPOId = 1,
                EPOItemId = 1,
                PlanPORemainingQuantity = 1,
                POSerialNumber = "POSerialNumber",
                TotalQty = 1,
                UnitFrom = new UnitDepartment()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name"
                },
                Items = new List<GarmentSubconCuttingOutItemValueObject>()
               {
                   new GarmentSubconCuttingOutItemValueObject()
                   {
                       CutOutId =id,
                       CuttingInDetailId =id,
                       CuttingInId =id,
                       DesignColor ="DesignColor",
                       Id =id,
                       IsSave =true,
                       Product =new Product()
                       {
                           Id =1,
                           Code ="Code",
                           Name ="Name"
                       },
                       TotalCuttingOut =1,
                       TotalCuttingOutQuantity =1,
                       TotalRemainingQuantityCuttingInItem =2,
                       Details =new List<GarmentSubconCuttingOutDetailValueObject>()
                       {
                           new GarmentSubconCuttingOutDetailValueObject()
                           {
                               Color ="Color",
                               BasicPrice =1,
                               CutOutItemId =id,
                               CuttingOutQuantity =1,
                               CuttingOutUom =new Uom()
                               {
                                   Id =1,
                                   Unit="Unit"
                               },
                               Id =id,
                               Price =1,
                               RemainingQuantity =2,
                               Remark ="Remark",
                               Size =new SizeValueObject()
                               {
                                   Id=1,
                                   Size ="Size"
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
