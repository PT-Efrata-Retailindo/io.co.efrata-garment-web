using Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentPreparings
{
    public class GarmentMonitoringPrepareDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentMonitoringPrepareDto prepareDto = new GarmentMonitoringPrepareDto();
            GarmentMonitoringPrepareDto dto = new GarmentMonitoringPrepareDto(prepareDto);
            Assert.NotNull(dto);

        }
    }
    }
