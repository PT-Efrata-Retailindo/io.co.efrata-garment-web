
using Manufactures.Application.GarmentMonitoringProductionFlows.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentLoadings
{
 public   class GarmentMonitoringLoadingDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentMonitoringProductionFlowDto garmentMonitoring = new GarmentMonitoringProductionFlowDto();
            GarmentMonitoringProductionFlowDto dto = new GarmentMonitoringProductionFlowDto(garmentMonitoring);
            Assert.NotNull(dto);
           
        }
      

    }
}
