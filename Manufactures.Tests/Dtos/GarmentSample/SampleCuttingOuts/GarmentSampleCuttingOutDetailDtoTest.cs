
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Dtos.GarmentSample.SampleCuttingOuts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample.SampleCuttingOuts
{
    public class GarmentSampleCuttingOutDetailDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSampleCuttingOutDetailDto(new GarmentSampleCuttingOutDetail(id,id, new Domain.Shared.ValueObjects.SizeId(1),
                "a", "a",1,1,new Domain.Shared.ValueObjects.UomId(1), "a",1,1));

            Assert.NotNull(dto);
            Assert.NotNull(dto.CuttingOutUom);
            Assert.NotNull(dto.Size);
        }
    }
}
