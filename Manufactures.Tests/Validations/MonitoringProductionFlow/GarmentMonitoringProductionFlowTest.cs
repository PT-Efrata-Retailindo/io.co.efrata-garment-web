using Manufactures.Domain.MonitoringProductionFlow;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.MonitoringProductionFlow
{
    public class GarmentMonitoringProductionFlowTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentMonitoringProductionFlow item = new GarmentMonitoringProductionFlow(new GarmentMonitoringProductionFlowReadModel(id));
           
            Assert.NotNull(item);

            GarmentMonitoringProductionFlow item2 = new GarmentMonitoringProductionFlow(id, "RO", "BuyerCode", "Article", "Comodity", 1, "Size", 1, 1, 1, 1, 1);
            Assert.Equal(id, item2.Identity);
            Assert.Equal("RO", item2.Ro);
            Assert.Equal("BuyerCode", item2.BuyerCode);
            Assert.Equal("Article", item2.Article);
            Assert.Equal("Comodity", item2.Comodity);
            Assert.Equal(1, item2.QtyOrder);
            Assert.Equal(1, item2.QtyCutting);
            Assert.Equal(1, item2.QtyFinishing);
            Assert.Equal(1, item2.QtyLoading);
            Assert.Equal(1, item2.QtySewing);
            Assert.Equal("Size", item2.Size);
            Assert.NotNull(item2);
        }

    }
}
