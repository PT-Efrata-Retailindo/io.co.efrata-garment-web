using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Commands;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Tests.Validations.GarmentSubcon.GarmentSubconDeliveryLetterOuts
{
    public class PlaceGarmentSubconDeliveryLetterOutValidationTests
    {
        private PlaceGarmentSubconDeliveryLetterOutCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSubconDeliveryLetterOutCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldNotHaveError_BB()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractType = "SUBCON GARMENT",
                SubconCategory="SUBCON CUTTING SEWING",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                EPOItemId = 1,
                PONo = "test",
                Remark = "test",
                UENId = 1,
                TotalQty = 1,
                UENNo = "test",
                UsedQty = 1,
                EPONo = "test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        ProductRemark="test",
                        FabricType="test",
                        UENItemId=1,
                        Uom=new Uom()
                        {
                            Id=1,
                            Unit="test"
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

        [Fact]
        public void Place_HaveError_Qty_BB()
        {
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractType = "SUBCON BAHAN BAKU",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                EPOItemId = 1,
                PONo = "test",
                Remark = "test",
                UENId = 1,
                TotalQty = 9,
                UENNo = "test",
                UsedQty = 1,
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =1,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        ProductRemark="test",
                        FabricType="test",
                        UENItemId=1,
                        Uom=new Uom()
                        {
                            Id=1,
                            Unit="test"
                        }
                    }
                }
            };
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();

        }

        [Fact]
        public void Place_HaveError_Qty_Item_BB()
        {
            // Arrange
            var validator = GetValidationRules();
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractType = "SUBCON BAHAN BAKU",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                EPOItemId = 1,
                PONo = "test",
                Remark = "test",
                UENId = 1,
                TotalQty = 1,
                UENNo = "test",
                UsedQty = 1,
                SubconCategory= "SUBCON CUTTING SEWING",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Product =new Product()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name"
                        },
                        Quantity =0,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        ProductRemark="test",
                        FabricType="test",
                        UENItemId=1,
                        Uom=new Uom()
                        {
                            Id=1,
                            Unit="test"
                        }
                    }
                }
            };

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_ShouldNotHaveError_CUTTING()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractType = "SUBCON CUTTING",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                EPOItemId = 1,
                PONo = "test",
                Remark = "test",
                TotalQty = 1,
                UsedQty = 1,
                EPONo ="test",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Quantity =1,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        SubconId=new Guid(),
                        POSerialNumber="test",
                        RONo="RONo",
                        SubconNo="no"
                    }
                }
            };
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();

        }

        [Fact]
        public void Place_HaveError_Qty_CUTTING()
        {
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractType = "SUBCON CUTTING",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                Remark = "test",
                TotalQty = 9,
                UsedQty = 1,
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Quantity =1,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                    }
                }
            };
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();

        }

        [Fact]
        public void Place_HaveError_Qty_Item_CUTTING()
        {
            // Arrange
            var validator = GetValidationRules();
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentSubconDeliveryLetterOutCommand()
            {
                IsUsed = true,
                ContractType = "SUBCON CUTTING",
                DLDate = DateTimeOffset.Now,
                DLType = "test",
                Remark = "test",
                TotalQty = 1,
                UsedQty = 1,
                SubconCategory="SUBCON SEWING",
                Items = new List<GarmentSubconDeliveryLetterOutItemValueObject>()
                {
                    new GarmentSubconDeliveryLetterOutItemValueObject()
                    {
                        DesignColor ="DesignColor",
                        Id =id,
                        Quantity =0,
                        SubconDeliveryLetterOutId =id,
                        ContractQuantity=1,
                        SubconId=new Guid(),
                        POSerialNumber="test",
                        RONo="RONo",
                        SubconNo="no"
                    }
                }
            };

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }
    }
}
