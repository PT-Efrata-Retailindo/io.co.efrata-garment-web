using Manufactures.Application.GarmentSewingOuts.Queries.MonitoringSewing;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSewingOuts
{
   public class GarmentMonitoringSewingDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentMonitoringSewingDto prepareDto = new GarmentMonitoringSewingDto();
            GarmentMonitoringSewingDto dto = new GarmentMonitoringSewingDto(prepareDto);
            Assert.NotNull(dto);

        }
    }
}
