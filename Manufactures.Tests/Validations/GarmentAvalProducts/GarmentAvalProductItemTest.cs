using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentAvalProducts
{
    public class GarmentAvalProductItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentAvalProductItem productItem = new GarmentAvalProductItem(new GarmentAvalProductItemReadModel(id));
            productItem.setDesignColor("designCollor");
            productItem.SetIsReceived(true);
            productItem.setPreparingId(new GarmentPreparingId("value"));
            productItem.setPreparingItemId(new GarmentPreparingItemId("value"));
            productItem.setProductCode("code");
            productItem.setProductId(new ProductId(1));
            productItem.setProductName("newProductName");
            productItem.setQuantity(1);
            productItem.setUomId(new UomId(1));
            productItem.setUomUnit("newUomUnit");
            productItem.SetDeleted();


            Assert.NotNull(productItem);
        }
    }
}
