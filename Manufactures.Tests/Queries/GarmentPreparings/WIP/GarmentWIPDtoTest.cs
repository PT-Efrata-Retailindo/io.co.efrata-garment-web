using Manufactures.Application.GarmentPreparings.Queries.GetWIP;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentPreparings.WIP
{
    public class GarmentWIPDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentWIPDto wIPDto = new GarmentWIPDto();
            GarmentWIPDto dto = new GarmentWIPDto(wIPDto);
            Assert.NotNull(dto);

        }
    }
}
