using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentPreparings
{
    public class GarmentPreparingItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentPreparingItem item = new GarmentPreparingItem(new GarmentPreparingItemReadModel(id));
            //item.setUenItemId(1);
            item.setProduct(new ProductId(1));
            item.setProductCode("newProductCode");
            item.setProductName("newProductName");
            item.setDesignColor("newDesignColor");
            //item.setQuantity(1);
            item.setUomId(new UomId(1));
            item.setUomUnit("newUomUnit");
            item.setFabricType("newFabricType");
            //item.setBasicPrice(1);
            item.setProduct(new ProductId(1));
            Assert.NotNull(item);
        }

    }
}
