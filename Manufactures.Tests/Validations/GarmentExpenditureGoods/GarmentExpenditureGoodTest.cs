using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentExpenditureGoods
{
    public class GarmentExpenditureGoodTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentExpenditureGood item = new GarmentExpenditureGood(new GarmentExpenditureGoodReadModel(id));
            item.SetCarton(1);
            item.SetInvoice("Invoice");
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
