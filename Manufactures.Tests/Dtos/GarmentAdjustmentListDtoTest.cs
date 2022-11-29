using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentAdjustmentListDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();

            GarmentAdjustmentListDto dto = new GarmentAdjustmentListDto(new GarmentAdjustment(id, "AdjustmentNo","AdjustmentType", "RONo", "Article",new UnitDepartmentId(1),"unitCode","unitName",DateTimeOffset.Now,new GarmentComodityId(1),"comodityCode","ComodityName","AdjustmenDesc"));
            dto.LastModifiedBy = "LastModifiedBy";
            dto.CreatedBy = "CreatedBy";
            dto.TotalRemainingQuantity = 1;
            dto.TotalAdjustmentQuantity = 1;


            Assert.Equal("LastModifiedBy", dto.LastModifiedBy);
            Assert.Equal("AdjustmentNo", dto.AdjustmentNo);
            Assert.Equal("RONo", dto.RONo);
            Assert.Equal("Article", dto.Article);
            Assert.Equal(id, dto.Id);
            Assert.NotNull(dto.Comodity);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.CreatedBy);
            Assert.NotNull(dto.AdjustmentNo);
            Assert.Equal(1, dto.TotalRemainingQuantity);
            Assert.Equal(1, dto.TotalAdjustmentQuantity);
            Assert.True(dto.AdjustmentDate > DateTimeOffset.MinValue);


        }
    }
}
