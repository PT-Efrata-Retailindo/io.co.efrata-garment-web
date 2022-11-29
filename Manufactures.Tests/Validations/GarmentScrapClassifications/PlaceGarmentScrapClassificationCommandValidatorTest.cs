using Barebone.Tests;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentScrapClassifications;
using Manufactures.Domain.GarmentScrapClassifications.Commands;
using Manufactures.Domain.GarmentScrapClassifications.ReadModels;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;
using static Manufactures.Domain.GarmentScrapClassifications.Commands.PlaceGarmentScrapClassificationCommand;

namespace Manufactures.Tests.Validations.GarmentScrapClassifications
{
 public   class PlaceGarmentScrapClassificationCommandValidatorTest: BaseValidatorUnitTest
    {
        private readonly Mock<IGarmentScrapClassificationRepository> _mockGarmentScrapClassificationRepository;

       public PlaceGarmentScrapClassificationCommandValidatorTest()
        {
            _mockGarmentScrapClassificationRepository = CreateMock<IGarmentScrapClassificationRepository>();

            _MockStorage.SetupStorage(_mockGarmentScrapClassificationRepository);
        }
        private PlaceGarmentScrapClassificationCommandValidator GetValidationRules()
        {
            return new PlaceGarmentScrapClassificationCommandValidator(_MockStorage.Object);
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new PlaceGarmentScrapClassificationCommand()
            {
                Code = "Code",
                Name = "Name",
                Description = "Description"
            };
            _mockGarmentScrapClassificationRepository.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapClassificationReadModel, bool>>>())).Returns(new List<GarmentScrapClassification>()
            {
               new GarmentScrapClassification(id,"Code","Name","Description"),
               new GarmentScrapClassification(id,"Code","Name","Description")
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
            var unitUnderTest = new PlaceGarmentScrapClassificationCommand()
            {
                Code = "Code",
                Name = "Name",
                Description = "Description"
            };
            _mockGarmentScrapClassificationRepository.Setup(s => s.Find(It.IsAny<Expression<Func<GarmentScrapClassificationReadModel, bool>>>())).Returns(new List<GarmentScrapClassification>()
            {
            
            });
            // Action
            var validator = GetValidationRules();
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }

    }
}
