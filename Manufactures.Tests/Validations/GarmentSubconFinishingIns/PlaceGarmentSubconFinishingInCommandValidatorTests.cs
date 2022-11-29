using Barebone.Tests;
using ExtCore.Data.Abstractions;
using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSubconFinishingIns.Commands;
using Manufactures.Domain.GarmentSubconFinishingIns.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubconFinishingIns
{
    public class PlaceGarmentSubconFinishingInCommandValidatorTests : BaseValidatorUnitTest
    {
        private readonly Mock<IGarmentFinishingInRepository> _mockGarmentFinishingInRepository;

        public PlaceGarmentSubconFinishingInCommandValidatorTests()
        {
            _mockGarmentFinishingInRepository = CreateMock<IGarmentFinishingInRepository>();

            _MockStorage.SetupStorage(_mockGarmentFinishingInRepository);
        }

        private PlaceGarmentSubconFinishingInCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSubconFinishingInCommandValidator(_MockStorage.Object);
        }

        [Fact]
        public void Place_HaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentSubconFinishingInCommand();
            unitUnderTest.DOId = 1;
            unitUnderTest.RONo = "RONo";

            _mockGarmentFinishingInRepository.Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInReadModel>
                {
                    new GarmentFinishingIn(Guid.Empty, null, null, new UnitDepartmentId(1), null, null, unitUnderTest.RONo, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, unitUnderTest.DOId, null, null).GetReadModel()
                }.AsQueryable());

            var validator = GetValidationRules();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldHaveError();
        }

        [Fact]
        public void Place_NotHaveError()
        {
            // Arrange
            var unitUnderTest = new PlaceGarmentSubconFinishingInCommand();
            unitUnderTest.Unit = new UnitDepartment(1, "Unit", "Unit");
            unitUnderTest.FinishingInDate = DateTimeOffset.Now.AddDays(-1);
            unitUnderTest.Comodity = new GarmentComodity(1, "Comodity", "Comodity");
            unitUnderTest.Supplier = new { };
            unitUnderTest.DOId = 1;
            unitUnderTest.DONo = "DONo";
            unitUnderTest.RONo = "RONo";
            unitUnderTest.Article = "Article";
            unitUnderTest.TotalQuantity = 0;
            unitUnderTest.Items = new List<GarmentSubconFinishingInItemValueObject>
            {
                new GarmentSubconFinishingInItemValueObject
                {
                    IsSave = true,
                    Quantity = 10,
                    Product = new Product(1, "Product", "Product"),
                    Size = new SizeValueObject(1, "Size")
                }
            };

            _mockGarmentFinishingInRepository.Setup(s => s.Query)
                .Returns(new List<GarmentFinishingInReadModel>
                {
                    new GarmentFinishingIn(Guid.Empty, null, null, new UnitDepartmentId(1), null, null, "Not" + unitUnderTest.RONo, null, new UnitDepartmentId(1), null, null, DateTimeOffset.Now, new GarmentComodityId(1), null, null, unitUnderTest.DOId + 1, "Not" + unitUnderTest.DONo, null).GetReadModel()
                }.AsQueryable());

            var validator = GetValidationRules();

            // Action
            var result = validator.TestValidate(unitUnderTest);

            // Assert
            result.ShouldNotHaveError();
        }
    }
}
