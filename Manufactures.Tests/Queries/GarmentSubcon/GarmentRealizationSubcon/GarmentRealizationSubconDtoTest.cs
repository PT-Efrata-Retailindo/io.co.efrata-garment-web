using Manufactures.Application.GarmentSubcon.Queries.GarmentRealizationSubconReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentRealizationSubcon
{
    public class GarmentRealizationSubconDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentRealizationSubconReportDto realizationSubconReportDto = new GarmentRealizationSubconReportDto();
            GarmentRealizationSubconReportDto dto = new GarmentRealizationSubconReportDto(realizationSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
