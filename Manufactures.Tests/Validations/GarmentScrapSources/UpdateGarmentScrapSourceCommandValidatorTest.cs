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
using static Manufactures.Domain.GarmentScrapSources.Commands.UpdateGarmentScrapSourceCommand;

namespace Manufactures.Tests.Validations.GarmentScrapSources
{
    public class UpdateGarmentScrapSourceCommandValidatorTest : BaseValidatorUnitTest
    {
        private readonly Mock<IGarmentScrapSourceRepository> _mockGarmentScrapSourceRepository;

        public UpdateGarmentScrapSourceCommandValidatorTest()
        {
            _mockGarmentScrapSourceRepository = CreateMock<IGarmentScrapSourceRepository>();

            _MockStorage.SetupStorage(_mockGarmentScrapSourceRepository);
        }

        private UpdateGarmentScrapSourceCommandValidator GetValidationRules()
        {
            return new UpdateGarmentScrapSourceCommandValidator(_MockStorage.Object);
        }

        [Fact]
        public void Update_ShouldHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentScrapSourceCommand()
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
        public void Update_ShouldNotHaveError()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var unitUnderTest = new UpdateGarmentScrapSourceCommand()
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
