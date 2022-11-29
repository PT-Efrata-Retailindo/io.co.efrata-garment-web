using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentDeliveryReturns
{
    public class GarmentDeliveryReturnItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentDeliveryReturnItem item = new GarmentDeliveryReturnItem(new GarmentDeliveryReturnItemReadModel(id));
            item.setUnitDOItemId(1);
            item.setUENItemId(1);
            item.setProductId(new ProductId(1));
            item.setQuantity(2);
            item.setUomId(new UomId(1));
            item.SetDeleted();
            Assert.NotNull(item);
        }

        }
}
