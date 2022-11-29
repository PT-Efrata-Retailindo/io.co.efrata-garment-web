using System;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSubcon;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentServiceSubconSewingItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSubconSewingDto(new GarmentServiceSubconSewing(id, "serviceSubconSewingNo",   DateTimeOffset.Now, false, new BuyerId(1), "BuyerCode", "BuyerName", 5, "uomUnit"));

            Assert.NotNull(dto);
        }
    }
}
