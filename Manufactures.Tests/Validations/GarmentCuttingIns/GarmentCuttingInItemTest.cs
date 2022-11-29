using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentCuttingIns
{
    public class GarmentCuttingInItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentCuttingInItem item = new GarmentCuttingInItem(new GarmentCuttingInItemReadModel(id));
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
