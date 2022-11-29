using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentScrabTransactions
{
    public class ScrapDtoTest
    {
        [Fact]
        public void ShouldSuccess_Instatiate()
        {
            ScrapDto scrapDto = new ScrapDto();
            ScrapDto dto = new ScrapDto(scrapDto);
            Assert.NotNull(dto);
        }
    }
}
