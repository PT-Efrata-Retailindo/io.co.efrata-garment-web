using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentLoadings
{
    public class GarmentLoadingItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentLoadingItem item = new GarmentLoadingItem(new GarmentLoadingItemReadModel(id));
            item.SetQuantity(1);
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
