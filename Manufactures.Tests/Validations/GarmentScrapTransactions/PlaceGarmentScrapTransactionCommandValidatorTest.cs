using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentScrapTransactions
{
    public class PlaceGarmentScrapTransactionCommandValidatorTest
    {
        private PlaceGarmentScrapTransactionCommandValidator GetValidationRules()
        {
            return new PlaceGarmentScrapTransactionCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new PlaceGarmentScrapTransactionCommand();

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
            var unitUnderTest = new PlaceGarmentScrapTransactionCommand()
            {
                ScrapDestinationId = id,
                ScrapDestinationName = "ScrapDestinationName",
                ScrapSourceId = id,
                ScrapSourceName = "ScrapSourceName",
                TransactionDate = DateTimeOffset.Now,
                TransactionNo = "TransactionNo",
                TransactionType = "TransactionType",
                Items = new List<GarmentScrapTransactionItemValueObject>()
                {
                    new GarmentScrapTransactionItemValueObject()
                    {
                         Id=id,
                         IsEdit =true,
                         Quantity =1,
                         Description ="Description",
                         RemainingQuantity =2,
                         ScrapClassificationId =id,
                         ScrapClassificationName ="ScrapClassificationName",
                         ScrapTransactionId =id,
                         TransactionType ="TransactionType",
                         UomId =1,
                         UomUnit ="UomUnit",
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
