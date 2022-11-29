using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSubconCuttingItemDto(new GarmentServiceSubconCuttingItem(id,new Guid(), "roNo", "art", new Domain.Shared.ValueObjects.GarmentComodityId(1), "comoCode", "comoName"));

            Assert.NotNull(dto.Comodity);

        }
    }
}
