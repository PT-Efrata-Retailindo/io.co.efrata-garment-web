using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Dtos.GarmentSample.SampleCuttingIns;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample.SampleCuttingIns
{
    public class GarmentSampleCuttingInItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();
            GarmentSampleCuttingInItemDto dto = new GarmentSampleCuttingInItemDto(new GarmentSampleCuttingInItem(new GarmentSampleCuttingInItemReadModel(id)));
            dto.LastModifiedBy = "LastModifiedBy";
            dto.CreatedBy = "CreatedBy";
            dto.CutInId = id;
            dto.PreparingId = id;
            dto.UENId = 1;
            dto.UENNo = "UENNo";
            dto.SewingOutId = id;
            dto.SewingOutNo = "SewingOutNo";
            dto.Details = new List<GarmentSampleCuttingInDetailDto>();
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
