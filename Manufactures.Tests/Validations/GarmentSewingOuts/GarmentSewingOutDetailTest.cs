using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentSewingOuts
{
    public class GarmentSewingOutDetailTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentSewingOutDetail item = new GarmentSewingOutDetail(new GarmentSewingOutDetailReadModel(id));
            item.SetSizeName("SizeName");
            item.SetSizeId(new Domain.Shared.ValueObjects.SizeId(1));
            item.SetQuantity(1);
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
