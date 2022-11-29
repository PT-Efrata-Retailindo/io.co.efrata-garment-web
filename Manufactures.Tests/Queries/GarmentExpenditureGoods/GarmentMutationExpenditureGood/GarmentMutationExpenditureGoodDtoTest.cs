using Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentExpenditureGoods.GarmentMutationExpenditureGood
{
    public class GarmentMutationExpenditureGoodDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentMutationExpenditureGoodDto wIPDto = new GarmentMutationExpenditureGoodDto();
            GarmentMutationExpenditureGoodDto dto = new GarmentMutationExpenditureGoodDto(wIPDto);
            Assert.NotNull(dto);

        }
    }
}
