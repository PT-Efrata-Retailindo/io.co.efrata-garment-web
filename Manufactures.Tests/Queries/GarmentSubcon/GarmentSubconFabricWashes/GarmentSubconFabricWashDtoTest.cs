using Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSubcon.GarmentSubconFabricWashes
{
    public class GarmentSubconFabricWashDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            ServiceSubconFabricWashDto fabricWashSubconDto = new ServiceSubconFabricWashDto();
            ServiceSubconFabricWashDto dto = new ServiceSubconFabricWashDto(fabricWashSubconDto);
            Assert.NotNull(dto);

        }
    }
}
