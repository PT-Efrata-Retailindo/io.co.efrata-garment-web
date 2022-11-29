using Manufactures.Application.GarmentFinishingOuts.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentFinishingOuts
{
   public class GarmentMonitoringFinishingDtoTest
    {
        [Fact]
        public void Should_Success_Intantiate()
        {
            GarmentMonitoringFinishingDto garmentMonitoring = new GarmentMonitoringFinishingDto();
            GarmentMonitoringFinishingDto dto = new GarmentMonitoringFinishingDto(garmentMonitoring);

           Assert.NotNull(dto);
            
        }
    }
}
