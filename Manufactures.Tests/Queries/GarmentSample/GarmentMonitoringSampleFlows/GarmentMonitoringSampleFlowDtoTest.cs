
using Manufactures.Application.GarmentLoadings.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.GarmentMonitoringSampleFlows
{
  public  class GarmentMonitoringSampleFlowDtoTest
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
