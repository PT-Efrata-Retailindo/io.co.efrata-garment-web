using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingDtoSizeTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSubconCuttingSizeDto(new GarmentServiceSubconCuttingSize(id, new SizeId(1), "", 1, new UomId(1), "", "ColorD", Guid.NewGuid(), Guid.Empty, Guid.Empty, new ProductId(1), "", ""));

            Assert.NotNull(dto.Size);
            Assert.NotNull(dto.Uom);
            Assert.NotNull(dto.Product);

        }
    }
}
