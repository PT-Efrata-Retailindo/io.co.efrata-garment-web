using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentAdjustments
{
    public  class GarmentAdjustmentItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentAdjustmentItem adjustmentItem = new GarmentAdjustmentItem(new GarmentAdjustmentItemReadModel(id));
            adjustmentItem.SetQuantity(1);
            adjustmentItem.SetPrice(1);
            adjustmentItem.Modify();
            Assert.NotNull(adjustmentItem);
        }
    }
}
