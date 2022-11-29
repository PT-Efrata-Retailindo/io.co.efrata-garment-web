using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSample.SampleRequest;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSample.SampleRequests
{
    public class GarmentSampleRequestDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSampleRequestDto(new GarmentSampleRequest(id, "a", "a", "a", "a", DateTimeOffset.Now, new BuyerId(1), "a", "a",
                new GarmentComodityId(1), "a", "a", "a", "a", DateTimeOffset.Now, "a", "a", "", false, false, DateTimeOffset.Now, "", false, DateTimeOffset.Now,
                null, null, false, DateTimeOffset.Now, null, null, null, null, null, null, new SectionId(1), null, null));

            Assert.NotNull(dto);
            Assert.NotNull(dto.Buyer);
            Assert.NotNull(dto.Comodity);
            Assert.NotNull(dto.Section);
        }
    }
}
