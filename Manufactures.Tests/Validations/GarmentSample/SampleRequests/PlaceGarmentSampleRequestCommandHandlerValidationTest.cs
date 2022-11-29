using FluentValidation.TestHelper;
using Manufactures.Domain.GarmentSample.SampleRequests.Commands;
using Manufactures.Domain.GarmentSample.SampleRequests.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.SampleRequests
{
    public class PlaceGarmentSampleRequestCommandHandlerValidationTest
    {
        private PlaceGarmentSampleRequestCommandValidator GetValidationRules()
        {
            return new PlaceGarmentSampleRequestCommandValidator();
        }

        [Fact]
        public void Place_ShouldHaveError()
        {
            // Arrange

            var unitUnderTest = new PlaceGarmentSampleRequestCommand();

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
            var unitUnderTest = new PlaceGarmentSampleRequestCommand()
            {
                Date = DateTimeOffset.Now,
                SampleCategory = "Commercial Sample",
                Buyer = new Buyer
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                Comodity = new GarmentComodity
                {
                    Code = "test",
                    Id = 1,
                    Name = "test"
                },
                Attached="aa",
                IsPosted=false,
                IsReceived=false,
                Packing="aa",
                POBuyer="aa",
                Remark="aa",
                RONoCC="aa",
                RONoSample="aa",
                SampleRequestNo="aa",
                SampleType="aa",
                SentDate=DateTimeOffset.Now,
                Section= new SectionValueObject
                {

                    Code = "test",
                    Id = 1,
                },
                SampleProducts = new List<GarmentSampleRequestProductValueObject>()
                {
                    new GarmentSampleRequestProductValueObject
                    {
                       Quantity=1,
                       Size=new SizeValueObject
                       {
                           Id=1,
                           Size="s"
                       },
                       Color="aa",
                       SizeDescription="aa",
                       Style="aa",
                       Fabric = "aa"
                    }
                },
                SampleSpecifications = new List<GarmentSampleRequestSpecificationValueObject>()
                {
                    new GarmentSampleRequestSpecificationValueObject
                    {
                        Quantity=1,
                        Inventory="ACC",
                        Remark="aa",
                        SpecificationDetail="aa"
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
