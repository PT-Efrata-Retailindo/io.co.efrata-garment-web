using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
  public  class GarmentFinishingOutDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();
            var department = new UnitDepartmentId(1);
            var garmentFinishingOut = new GarmentFinishingOut(id, "finishingPrinting", new UnitDepartmentId(1), "unitToCode", "unitToCode", "FinishingTo", DateTimeOffset.Now, "roNo", "Article", new UnitDepartmentId(1), "unitCode", "unitName", new GarmentComodityId(1), "comodityCode", "comodityName", true);

            GarmentFinishingOutDto dto = new GarmentFinishingOutDto(garmentFinishingOut);
            Assert.Equal("FinishingTo", dto.FinishingTo);
            Assert.NotNull(dto.Unit);
            Assert.NotNull(dto.UnitTo);
            Assert.NotNull(dto.Comodity);

        }
    }
}
