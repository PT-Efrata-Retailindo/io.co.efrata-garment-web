using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentMonitoringProductionFlows.Queries
{
	public class GarmentMonitoringProductionFlowListViewModel
	{
		public List<GarmentMonitoringProductionFlowDto> garmentMonitorings { get; set; }
		public int count { get; set; }
	}
}
