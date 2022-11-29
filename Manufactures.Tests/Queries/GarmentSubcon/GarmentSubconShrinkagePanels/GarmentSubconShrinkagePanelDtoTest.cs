using Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels.ExcelTemplates;
using Manufactures.Application.GarmentSubcon.GarmentServiceSubconShrinkagePanels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentServiceSubconShrinkagePanels
{
    public class GarmentSubconDLOShrinkagePanelDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            XlsSubconShrinkageDto shrinkageSubconDto = new XlsSubconShrinkageDto();
            XlsSubconShrinkageDto dto = new XlsSubconShrinkageDto(shrinkageSubconDto);
            Assert.NotNull(dto);

        }
    }
}
