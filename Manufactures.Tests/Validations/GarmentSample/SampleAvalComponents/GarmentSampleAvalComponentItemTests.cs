using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSample.SampleAvalComponents
{
    public class GarmentSampleAvalComponentItemTests
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentSampleAvalComponentItem componentItem = new GarmentSampleAvalComponentItem(new GarmentSampleAvalComponentItemReadModel(id));
            Assert.NotNull(componentItem);
        }
    }
}
