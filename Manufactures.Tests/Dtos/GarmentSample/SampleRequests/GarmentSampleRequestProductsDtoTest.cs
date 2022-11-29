using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSample.SampleRequest;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample.SampleRequests
{
    public class GarmentSampleRequestProductsDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSampleRequestProductDto(new GarmentSampleRequestProduct(id,id, "a", "a","a", new SizeId(1), "a", "a",1,1));

            Assert.NotNull(dto);
            Assert.NotNull(dto.Size);
        }
    }
}
