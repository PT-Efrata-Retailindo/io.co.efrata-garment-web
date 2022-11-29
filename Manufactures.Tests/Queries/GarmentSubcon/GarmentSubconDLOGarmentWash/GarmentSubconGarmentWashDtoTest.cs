using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOGarmentWashReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOGarmentWash
{
    public class GarmentSubconDLOGarmentWashDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconDLOGarmentWashReportDto realizationSubconReportDto = new GarmentSubconDLOGarmentWashReportDto();
            GarmentSubconDLOGarmentWashReportDto dto = new GarmentSubconDLOGarmentWashReportDto(realizationSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
