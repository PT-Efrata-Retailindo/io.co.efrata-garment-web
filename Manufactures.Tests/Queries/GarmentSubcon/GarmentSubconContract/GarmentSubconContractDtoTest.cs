using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconContactReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconContact
{
    public class GarmentSubconContractDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconContactReportDto realizationSubconReportDto = new GarmentSubconContactReportDto();
            GarmentSubconContactReportDto dto = new GarmentSubconContactReportDto(realizationSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
