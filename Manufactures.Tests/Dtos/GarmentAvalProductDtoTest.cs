using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentAvalProductDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();

            GarmentAvalProductDto dto = new GarmentAvalProductDto();
            dto.LastModifiedBy="LastModifiedBy";
            dto.CreatedBy = "CreatedBy";
            dto.Unit = new UnitDepartment();
            dto.AvalDate = DateTimeOffset.Now;
            Assert.Equal("LastModifiedBy", dto.LastModifiedBy);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.CreatedBy);
            Assert.True(dto.AvalDate > DateTimeOffset.MinValue);
          

        }
    }
}
