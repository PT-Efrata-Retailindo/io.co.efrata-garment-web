using Barebone.Tests;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;
using static Manufactures.Domain.GarmentScrapSources.Commands.PlaceGarmentScrapSourceCommand;

namespace Manufactures.Tests.Validations.GarmentScrapSources
{
  public  class PlaceGarmentScrapSourceCommandValidatorTest : BaseValidatorUnitTest
    {
        private readonly Mock<IGarmentScrapSourceRepository> _mockGarmentScrapSourceRepository;

        public PlaceGarmentScrapSourceCommandValidatorTest()
        {
            _mockGarmentScrapSourceRepository = CreateMock<IGarmentScrapSourceRepository>();

            _MockStorage.SetupStorage(_mockGarmentScrapSourceRepository);
        }

        private PlaceGarmentScrapSourceCommandValidator GetValidationRules()
        {
            return new PlaceGarmentScrapSourceCommandValidator(_MockStorage.Object);
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentScrapSourceCommand()
            {
                Code = "Code",
                Name = "Name",
                Description = "Description"
            };
            _mockGarmentScrapSourceRepository.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapSourceReadModel, bool>>>())).Returns(new List<GarmentScrapSource>()
            {
               new GarmentScrapSource(id,"Code","Name","Description")

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
            var unitUnderTest = new PlaceGarmentScrapSourceCommand()
            {
                Code = "Code",
                Name = "Name",
                Description = "Description"
            };
            _mockGarmentScrapSourceRepository.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapSourceReadModel, bool>>>())).Returns(new List<GarmentScrapSource>()
            {

            });

            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}
