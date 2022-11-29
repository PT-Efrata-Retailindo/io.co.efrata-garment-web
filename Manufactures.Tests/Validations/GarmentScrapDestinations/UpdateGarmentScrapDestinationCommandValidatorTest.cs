using Barebone.Tests;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentScrapDestinations;
using Manufactures.Domain.GarmentScrapDestinations.Commands;
using Manufactures.Domain.GarmentScrapDestinations.ReadModels;
using Manufactures.Domain.GarmentScrapDestinations.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;
using static Manufactures.Domain.GarmentScrapDestinations.Commands.UpdateGarmentScrapDestinationCommand;

namespace Manufactures.Tests.Validations.GarmentScrapDestinations
{
    public class UpdateGarmentScrapDestinationCommandValidatorTest : BaseValidatorUnitTest
    {
        private readonly Mock<IGarmentScrapDestinationRepository> _mockGarmentScrapDestinationRepository;

        public UpdateGarmentScrapDestinationCommandValidatorTest()
        {
            _mockGarmentScrapDestinationRepository = CreateMock<IGarmentScrapDestinationRepository>();

            _MockStorage.SetupStorage(_mockGarmentScrapDestinationRepository);
        }
        private UpdateGarmentScrapDestinationCommandValidator GetValidationRules()
        {
            return new UpdateGarmentScrapDestinationCommandValidator(_MockStorage.Object);
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentScrapDestinationCommand()
            {
                Code = "Code",
                Name = "Name",
                Description = "Description"
            };
            _mockGarmentScrapDestinationRepository.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapDestinationReadModel, bool>>>())).Returns(new List<GarmentScrapDestination>()
            {
               new GarmentScrapDestination(id,"Code","Name","Description")
            });
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
            var unitUnderTest = new UpdateGarmentScrapDestinationCommand()
            {
                Code = "Code",
                Name = "Name",
                Description = "Description"
            };
            _mockGarmentScrapDestinationRepository.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapDestinationReadModel, bool>>>())).Returns(new List<GarmentScrapDestination>()
            {
            });

            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }

    }
}
