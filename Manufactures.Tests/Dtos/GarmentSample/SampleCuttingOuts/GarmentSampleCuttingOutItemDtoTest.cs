using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Dtos.GarmentSample.SampleCuttingOuts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample.SampleCuttingOuts
{
    public class GarmentSampleCuttingOutItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSampleCuttingOutItemDto(new GarmentSampleCuttingOutItem(id,id,id,id,new Domain.Shared.ValueObjects.ProductId(1),
                "a", "a", "a",1));

            Assert.NotNull(dto);
            Assert.NotNull(dto.Product);
        }
    }
}
