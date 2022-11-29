using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentCuttingOuts
{
    public class GarmentCuttingOutItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentCuttingOutItem item = new GarmentCuttingOutItem(new GarmentCuttingOutItemReadModel(id));
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
