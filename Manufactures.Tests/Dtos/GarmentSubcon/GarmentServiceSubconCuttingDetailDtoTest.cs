using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingDetailDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSubconCuttingDetailDto(new GarmentServiceSubconCuttingDetail(id, Guid.NewGuid(), "ColorD", 1));

            Assert.NotNull(dto);

        }
    }
}
