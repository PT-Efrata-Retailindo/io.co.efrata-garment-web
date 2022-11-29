using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingOutItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentSubconCuttingOutItem item = new GarmentSubconCuttingOutItem(new GarmentCuttingOutItemReadModel(id));
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
