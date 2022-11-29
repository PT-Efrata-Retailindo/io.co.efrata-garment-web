using Manufactures.Application.GarmentPreparings.Queries.GetPrepareTraceable;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentPreparings.ForTraceableOut
{
    public class GetPrepareTraceableDtoTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            GetPrepareTraceableDto garmentMonitoring = new GetPrepareTraceableDto();

            GetPrepareTraceableDto dto = new GetPrepareTraceableDto(garmentMonitoring);

            Assert.NotNull(dto);
        }
    }
}
