using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Dtos.GarmentSample.SampleRequest;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample.SampleRequests
{
    public class GarmentSampleRequestSpecificationDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSampleRequestSpecificationDto(new GarmentSampleRequestSpecification(id, id, "a", "a", 1, "a", new Domain.Shared.ValueObjects.UomId(1),"a",1));

            Assert.NotNull(dto);
            Assert.NotNull(dto.Uom);
        }
    }
}
