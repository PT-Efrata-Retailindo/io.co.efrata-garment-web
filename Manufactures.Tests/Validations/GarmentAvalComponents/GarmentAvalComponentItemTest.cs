using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentAvalComponents
{
    public class GarmentAvalComponentItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentAvalComponentItem componentItem = new GarmentAvalComponentItem(new GarmentAvalComponentItemReadModel(id));
            Assert.NotNull(componentItem);
        }
        }
}
