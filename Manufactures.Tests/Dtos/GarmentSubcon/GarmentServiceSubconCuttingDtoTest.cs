using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.GarmentSubcon;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos.GarmentSubcon
{
    public class GarmentServiceSubconCuttingDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            Guid id = Guid.NewGuid();
            var dto = new GarmentServiceSubconCuttingDto(new GarmentServiceSubconCutting(id, "ServiceSubconCuttingNo", "type", new UnitDepartmentId(1), "unitToCode", "unitToName",  DateTimeOffset.Now, false, new BuyerId(1), "buyerCode", "buyerName", new UomId(1), "uomUnit", 1));

            Assert.NotNull(dto.Unit);

        }
    }
}
