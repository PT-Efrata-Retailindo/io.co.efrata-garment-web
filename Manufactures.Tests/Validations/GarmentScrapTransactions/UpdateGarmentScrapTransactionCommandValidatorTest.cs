using Barebone.Tests;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Manufactures.Domain.GarmentScrapSources.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static Manufactures.Domain.GarmentScrapSources.Commands.UpdateGarmentScrapTransactionCommand;

namespace Manufactures.Tests.Validations.GarmentScrapTransactions
{
    public class UpdateGarmentScrapTransactionCommandValidatorTest : BaseValidatorUnitTest
    {
        private readonly Mock<IGarmentScrapTransactionRepository> _mockGarmentScrapTransactionRepository;

        public UpdateGarmentScrapTransactionCommandValidatorTest()
        {
            _mockGarmentScrapTransactionRepository = CreateMock<IGarmentScrapTransactionRepository>();

            _MockStorage.SetupStorage(_mockGarmentScrapTransactionRepository);
        }

        private UpdateGarmentScrapTransactionCommandValidator GetValidationRules()
        {
            return new UpdateGarmentScrapTransactionCommandValidator(_MockStorage.Object);
        }

        [Fact]
        public void Update_ShouldHaveError()
        {
            // Arrange
            var validator = GetValidationRules();
            var unitUnderTest = new UpdateGarmentScrapTransactionCommand();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Update_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var validator = GetValidationRules();
            var unitUnderTest = new UpdateGarmentScrapTransactionCommand()
            {
                ScrapDestinationId = id,
                ScrapDestinationName = "ScrapDestinationName",
                ScrapSourceId = id,
                ScrapSourceName = "ScrapSourceName",
                TransactionDate = DateTimeOffset.Now.AddDays(-1),
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
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }

    }
}
