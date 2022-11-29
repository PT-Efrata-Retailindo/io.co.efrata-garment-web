using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentExpenditureGoodReturns
{
    public class GarmentExpenditureGoodReturnItemTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentExpenditureGoodReturnItem item = new GarmentExpenditureGoodReturnItem(new GarmentExpenditureGoodReturnItemReadModel(id));
            item.Modify();
            Assert.NotNull(item);
        }

    }
}
