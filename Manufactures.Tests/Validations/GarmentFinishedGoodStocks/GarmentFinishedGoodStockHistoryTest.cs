using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.GarmentFinishedGoodStocks
{
    public class GarmentFinishedGoodStockHistoryTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentFinishedGoodStockHistory item = new GarmentFinishedGoodStockHistory(new GarmentFinishedGoodStockHistoryReadModel(id));
            item.Modify();
            Assert.NotNull(item);
        }
    }
}
