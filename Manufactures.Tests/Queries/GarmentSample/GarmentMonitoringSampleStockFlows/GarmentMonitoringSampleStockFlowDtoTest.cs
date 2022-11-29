using Manufactures.Application.GarmentSample.GarmentMonitoringSampleStockFlows.Queries;
using Xunit;

namespace Manufactures.Tests.Queries.GarmentSample.GarmentMonitoringSampleStockFlows
{
    public class GarmentMonitoringSampleStockFlowDtoTest
    {
        [Fact]
        public void ShouldSucces_Instantiate()
        {
			GarmentMonitoringSampleStockFlowDto flowDto = new GarmentMonitoringSampleStockFlowDto();
			GarmentMonitoringSampleStockFlowDto dto = new GarmentMonitoringSampleStockFlowDto(flowDto);
            Assert.NotNull(dto);

        }
    }
}
