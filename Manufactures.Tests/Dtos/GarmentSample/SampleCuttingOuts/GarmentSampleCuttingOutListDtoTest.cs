using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Dtos.GarmentSample.SampleCuttingOuts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample.SampleCuttingOuts
{
    public class GarmentSampleCuttingOutListDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSampleCuttingOutListDto(new GarmentSampleCuttingOut(id, "a", "a", new Domain.Shared.ValueObjects.UnitDepartmentId(1),
                "a", "a", DateTimeOffset.Now, "a", "a",new Domain.Shared.ValueObjects.UnitDepartmentId(1), "a", "a",
                new Domain.Shared.ValueObjects.GarmentComodityId(1), "a", "a",true));

            Assert.NotNull(dto);
            Assert.NotNull(dto.Comodity);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.UnitFrom);
        }
    }
}
