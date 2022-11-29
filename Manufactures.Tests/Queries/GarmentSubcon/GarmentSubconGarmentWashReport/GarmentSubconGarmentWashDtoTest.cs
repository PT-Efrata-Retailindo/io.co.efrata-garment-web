using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconGarmentWashReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconGarmentWashReport
{
    public class GarmentSubconGarmentWashReportDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconGarmentWashReportDto garmentSubconGarmentWashReportDto = new GarmentSubconGarmentWashReportDto();
            GarmentSubconGarmentWashReportDto dto = new GarmentSubconGarmentWashReportDto(garmentSubconGarmentWashReportDto);
            Assert.NotNull(dto);
        }
    }
}
