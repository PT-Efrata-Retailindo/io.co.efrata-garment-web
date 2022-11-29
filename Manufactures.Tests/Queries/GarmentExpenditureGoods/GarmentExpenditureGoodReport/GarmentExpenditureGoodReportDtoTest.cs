using Manufactures.Application.GarmentExpenditureGoods.Queries.GetMutationExpenditureGoods;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentExpenditureGoods.GarmentMutationExpenditureGood
{
    public class GarmentExpenditureGoodReportDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GarmentReportExpenditureGoodDto wIPDto = new GarmentReportExpenditureGoodDto();
            GarmentReportExpenditureGoodDto dto = new GarmentReportExpenditureGoodDto(wIPDto);
            Assert.NotNull(dto);

        }
    }
}
