using Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentFinishingOuts.GarmentTotalQtyForTraceable
{
    public class GarmentTotalQtyTraceableDtoTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            GarmentTotalQtyTraceableDto garmentMonitoring = new GarmentTotalQtyTraceableDto();

            GarmentTotalQtyTraceableDto dto = new GarmentTotalQtyTraceableDto(garmentMonitoring);

            Assert.NotNull(dto);
        }
    }
}
