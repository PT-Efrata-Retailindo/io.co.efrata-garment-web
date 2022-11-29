using Manufactures.Domain.GarmentDeliveryReturns.ValueObjects;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentDeliveryReturnDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();
            GarmentDeliveryReturnDto dto = new GarmentDeliveryReturnDto();
            dto.LastModifiedBy = "LastModifiedBy";
            dto.Id = id;
            dto.CreatedBy = "CreatedBy";
            dto.RONo = "RONo";
            dto.PreparingId = "PreparingId";
            dto.RONo = "RONo";
            dto.Article = "Article";
            dto.ReturnDate = DateTimeOffset.Now;
            dto.UnitDOId = 1;
            dto.IsUsed = true;
            dto.UENId = 1;
            dto.Unit = new UnitDepartment();
            dto.ReturnType = "ReturnType";
            Assert.True(dto.IsUsed);
            Assert.True(DateTimeOffset.MinValue < dto.ReturnDate);
            Assert.Equal("PreparingId", dto.PreparingId);
            Assert.Equal("LastModifiedBy", dto.LastModifiedBy);
            Assert.Equal("Article", dto.Article);
            Assert.Equal("RONo", dto.RONo);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.CreatedBy);
            Assert.Equal(id, dto.Id);
            Assert.Equal(1, dto.UnitDOId);
            Assert.Equal(1, dto.UENId);
            

        }
    }
}
