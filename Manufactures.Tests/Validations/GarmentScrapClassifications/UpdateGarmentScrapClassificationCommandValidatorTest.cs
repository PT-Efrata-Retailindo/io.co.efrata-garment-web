using Barebone.Tests;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using static Manufactures.Domain.GarmentScrapClassifications.Commands.UpdateGarmentScrapClassificationCommand;

namespace Manufactures.Tests.Validations.GarmentScrapClassifications
{
   public class UpdateGarmentScrapClassificationCommandValidatorTest : BaseValidatorUnitTest
    {
        private readonly Mock<IGarmentScrapClassificationRepository> _mockGarmentScrapClassificationRepository;

        public UpdateGarmentScrapClassificationCommandValidatorTest()
        {
            _mockGarmentScrapClassificationRepository = CreateMock<IGarmentScrapClassificationRepository>();

            _MockStorage.SetupStorage(_mockGarmentScrapClassificationRepository);
        }
        private UpdateGarmentScrapClassificationCommandValidator GetValidationRules()
        {
            return new UpdateGarmentScrapClassificationCommandValidator(_MockStorage.Object);
        }

        [Fact]
        public void Update_ShouldHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentScrapClassificationCommand();

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Update_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentScrapClassificationCommand()
            {
                Code = "Code",
                Description = "Description",
                Name = "Name"
            };

            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}
