using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentSewingOutItemDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentSewingOutDto(new GarmentSewingOut(id, "sewingOutNo", new BuyerId(1), "BuyerCode", "BuyerName", new UnitDepartmentId(1), "unitToCode", "unitToName", "SewingTo", DateTimeOffset.Now, "RoNo", "Article", new UnitDepartmentId(1), "unitCode", "unitName", new GarmentComodityId(1), "comodityCode", "ComodityName", false));

            Assert.Equal("SewingTo", dto.SewingTo);
            Assert.NotNull(dto.Buyer);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.UnitTo);
            Assert.NotNull(dto.Comodity);

        }
    }
}
