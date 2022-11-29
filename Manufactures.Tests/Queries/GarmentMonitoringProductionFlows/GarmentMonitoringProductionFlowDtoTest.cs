
using Manufactures.Application.GarmentLoadings.Queries;
using Manufactures.Application.GarmentMonitoringProductionFlows.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentMonitoringProductionFlows
{
  public  class GarmentMonitoringProductionFlowDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentMonitoringLoadingDto flowDto = new GarmentMonitoringLoadingDto();
            GarmentMonitoringLoadingDto dto = new GarmentMonitoringLoadingDto(flowDto);
            Assert.NotNull(dto);

        }
    }
}
