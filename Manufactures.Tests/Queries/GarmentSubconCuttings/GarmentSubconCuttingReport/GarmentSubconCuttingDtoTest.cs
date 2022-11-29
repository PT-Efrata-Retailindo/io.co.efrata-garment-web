using Manufactures.Application.GarmentSubcon.GarmentServiceSubconCuttings.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubconCuttings.GarmentSubconCuttingReport
{
    public class GarmentSubconCuttingDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentMonitoringServiceSubconCuttingDto garmentSubconCuttingReportDto = new GarmentMonitoringServiceSubconCuttingDto();
            GarmentMonitoringServiceSubconCuttingDto dto = new GarmentMonitoringServiceSubconCuttingDto(garmentSubconCuttingReportDto);
            Assert.NotNull(dto);
        }
    }
}
