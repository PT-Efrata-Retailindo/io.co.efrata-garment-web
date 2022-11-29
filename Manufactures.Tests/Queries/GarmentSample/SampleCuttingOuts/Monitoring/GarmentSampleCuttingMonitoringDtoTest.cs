using Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.Monitoring;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.SampleCuttingOuts.Monitoring
{
    public class GarmentSampleCuttingMonitoringDtoTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            GarmentSampleCuttingMonitoringDto garmentMonitoring = new GarmentSampleCuttingMonitoringDto();

            GarmentSampleCuttingMonitoringDto dto = new GarmentSampleCuttingMonitoringDto(garmentMonitoring);

            Assert.NotNull(dto);
        }
    }
}
