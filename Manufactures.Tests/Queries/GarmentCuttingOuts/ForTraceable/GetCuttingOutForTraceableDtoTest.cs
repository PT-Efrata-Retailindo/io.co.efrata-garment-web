using Manufactures.Application.GarmentCuttingOuts.Queries.GetCuttingOutForTraceable;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentCuttingOuts
{
    public class GetCuttingOutForTraceableDtoTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            GetCuttingOutForTraceableDto garmentMonitoring = new GetCuttingOutForTraceableDto();

            GetCuttingOutForTraceableDto dto = new GetCuttingOutForTraceableDto(garmentMonitoring);

            Assert.NotNull(dto);
        }
    }
}
