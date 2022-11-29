using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOCuttingSewingReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOCuttingSewing
{
    public class GarmentSubconDLOCuttingSewingDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconDLOCuttingSewingReportDto CuttingSewingSubconReportDto = new GarmentSubconDLOCuttingSewingReportDto();
            GarmentSubconDLOCuttingSewingReportDto dto = new GarmentSubconDLOCuttingSewingReportDto(CuttingSewingSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
