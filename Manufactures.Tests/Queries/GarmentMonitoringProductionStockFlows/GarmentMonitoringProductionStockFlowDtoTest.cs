using Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentMonitoringProductionStockFlows
{
    public class GarmentMonitoringProductionStockFlowDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentMonitoringProductionStockFlowDto flowDto = new GarmentMonitoringProductionStockFlowDto();
            GarmentMonitoringProductionStockFlowDto dto = new GarmentMonitoringProductionStockFlowDto(flowDto);
            Assert.NotNull(dto);

        }
    }
}
