using Manufactures.Application.GarmentSubcon.Queries.GarmentSubconDLORawMaterialReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconDLORawMaterial
{
    public class GarmentSubconDLORawMaterialDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentSubconDLORawMaterialReportDto rawMaterialSubconReportDto = new GarmentSubconDLORawMaterialReportDto();
            GarmentSubconDLORawMaterialReportDto dto = new GarmentSubconDLORawMaterialReportDto(rawMaterialSubconReportDto);
            Assert.NotNull(dto);

        }
    }
}
