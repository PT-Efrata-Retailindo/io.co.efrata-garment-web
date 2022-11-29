using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSubcon.SubconContracts.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Tests.Validations.GarmentSubcon.GarmentSubconContracts
{
    public class UpdateGarmentSubconContractValidationTest
    {
        private UpdateGarmentSubconContractCommandValidator GetValidationRules()
        {
            return new UpdateGarmentSubconContractCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new UpdateGarmentSubconContractCommand();

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
            var unitUnderTest = new UpdateGarmentSubconContractCommand()
            {
                AgreementNo = "test",
                BPJNo = "test",
                ContractNo = "test",
                ContractType = "test",
                DueDate = DateTimeOffset.Now,
                ContractDate = DateTimeOffset.Now,
                FinishedGoodType = "test",
                JobType = "test",
                Quantity = 1,
                Supplier = new Supplier
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                Buyer = new Buyer
                {
                    Id = 1,
                    Code = "Buyercode",
                    Name = "BuyerName"
                },
                IsUsed = false
            };
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();

        }
    }
}
