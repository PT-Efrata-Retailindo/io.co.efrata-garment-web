using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentComodityPriceDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();
            GarmentComodityPriceDto dto = new GarmentComodityPriceDto(new GarmentComodityPrice(id,true,DateTimeOffset.Now,new UnitDepartmentId(1),"unitCode","unitName",new GarmentComodityId(1),"comodityCode","comodityName",1));
            dto.LastModifiedBy = "LastModifiedBy";
            dto.CreatedBy = "CreatedBy";
            dto.Price = 1;
            

            Assert.True(dto.Date > DateTimeOffset.MinValue);
            Assert.True(dto.IsValid);
            Assert.Equal("LastModifiedBy", dto.LastModifiedBy);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.Comodity);
            Assert.Equal(1, dto.Price);
        }
        }
}
