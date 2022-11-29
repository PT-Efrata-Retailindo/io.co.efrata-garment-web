using Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentScrabTransactions
{
    public class GetMutationScrapDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
            GetMutationScrapDto prepareDto = new GetMutationScrapDto();
            GetMutationScrapDto dto = new GetMutationScrapDto(prepareDto);
            Assert.NotNull(dto);

        }
    }
}
