
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Dtos;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Dtos
{
    public class GarmentCuttingOutListDtoTest
    {
        [Fact]
        public void should_Success_Instantiate()
        {
            var id = Guid.NewGuid();
            GarmentCuttingOut garmentCuttingOut = new GarmentCuttingOut(id,"cutOutNo", "cuttingOutType",new Domain.Shared.ValueObjects.UnitDepartmentId(1), "unitFromCode", "unitFromName",DateTimeOffset.Now,"roNo","article",new Domain.Shared.ValueObjects.UnitDepartmentId(1),"unitCode","unitName",new Domain.Shared.ValueObjects.GarmentComodityId(1),"comodityCode","comodityName", false);

            GarmentCuttingOutListDto dto = new GarmentCuttingOutListDto(garmentCuttingOut);
            dto.LastModifiedBy = "LastModifiedBy";
            dto.CreatedBy = "CreatedBy";
            dto.Article = "";

            Assert.NotNull(dto);
         


        }
    }
}
