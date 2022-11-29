using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentCuttingInItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();
            GarmentCuttingInItemDto dto = new GarmentCuttingInItemDto(new GarmentCuttingInItem(new GarmentCuttingInItemReadModel(id)));
            dto.LastModifiedBy = "LastModifiedBy";
            dto.CreatedBy = "CreatedBy";
            dto.CutInId = id;
            dto.PreparingId =id;
            dto.UENId =1;
            dto.UENNo = "UENNo";
            dto.SewingOutId = id;
            dto.SewingOutNo = "SewingOutNo";
            dto.Details = new List<GarmentCuttingInDetailDto>();
            Assert.Equal("LastModifiedBy", dto.LastModifiedBy);
            Assert.Equal("UENNo", dto.UENNo);
            Assert.Equal("SewingOutNo", dto.SewingOutNo);
            Assert.Equal(id, dto.Id);
            Assert.Equal(id, dto.SewingOutId);
            Assert.NotNull(dto.Details);
            Assert.NotNull(dto.CreatedBy);
        }
    }
}
