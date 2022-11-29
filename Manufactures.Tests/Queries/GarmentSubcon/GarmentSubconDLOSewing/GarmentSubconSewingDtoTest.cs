using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOSewingReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOSewing
{
    public class GarmentSubconDLOComponentDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconDLOSewingReportDto sewingSubconReportDto = new GarmentSubconDLOSewingReportDto();
            GarmentSubconDLOSewingReportDto dto = new GarmentSubconDLOSewingReportDto(sewingSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
