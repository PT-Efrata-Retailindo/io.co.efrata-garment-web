using Manufactures.Application.GarmentCuttingOuts.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentCuttingOuts
{
   public class GarmentMonitoringCuttingDtoTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            GarmentMonitoringCuttingDto garmentMonitoring = new GarmentMonitoringCuttingDto();

            GarmentMonitoringCuttingDto dto = new GarmentMonitoringCuttingDto(garmentMonitoring);

            Assert.NotNull(dto);
        }
    }
}
