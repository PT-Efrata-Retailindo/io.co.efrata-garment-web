using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentFinishingOuts
{
    public class GarmentFinishingOutDetailTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentFinishingOutDetail item = new GarmentFinishingOutDetail(new GarmentFinishingOutDetailReadModel(id));
            item.SetQuantity(1);
            item.SetSizeId(new SizeId(1));
            item.SetSizeName("SizeName");
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
