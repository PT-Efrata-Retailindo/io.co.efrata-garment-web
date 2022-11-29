using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLOComponentServiceReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLOComponent
{
    public class GarmentSubconDLOComponentDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconDLOComponentServiceReportDto realizationSubconReportDto = new GarmentSubconDLOComponentServiceReportDto();
            GarmentSubconDLOComponentServiceReportDto dto = new GarmentSubconDLOComponentServiceReportDto(realizationSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
