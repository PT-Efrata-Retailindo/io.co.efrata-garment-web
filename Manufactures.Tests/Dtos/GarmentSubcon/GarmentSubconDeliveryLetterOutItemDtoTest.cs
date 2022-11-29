using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Dtos.GarmentSubcon;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentSubconDeliveryLetterOutItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSubconDeliveryLetterOutItemDto(new GarmentSubconDeliveryLetterOutItem(id,new Guid(),1,new Domain.Shared.ValueObjects.ProductId(1),"code","name","remark","color",1,new Domain.Shared.ValueObjects.UomId(1),"unit", new Domain.Shared.ValueObjects.UomId(1), "unit", "fabType", new Guid(), "", "", "", It.IsAny<int>(), ""));

            Assert.NotNull(dto.Product);
            Assert.NotNull(dto.Uom);

        }
    }
}
