using Manufactures.Application.GarmentExpenditureGoods.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentExpenditureGoods
{
  public  class GarmentMonitoringExpenditureGoodDtoTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            GarmentMonitoringExpenditureGoodDto garmentMonitoring = new GarmentMonitoringExpenditureGoodDto();
           
            GarmentMonitoringExpenditureGoodDto dto = new GarmentMonitoringExpenditureGoodDto(garmentMonitoring);

            Assert.NotNull(dto);
        }
    }
}
