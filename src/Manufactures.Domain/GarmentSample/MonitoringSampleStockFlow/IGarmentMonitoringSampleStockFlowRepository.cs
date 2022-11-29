using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.MonitoringSampleStockFlow
{
	public interface IGarmentMonitoringSampleStockFlowRepository : IAggregateRepository<GarmentMonitoringSampleStocFlow, GarmentMonitoringSampleStockReadModel>
	{
		IQueryable<GarmentMonitoringSampleStockReadModel> Read(string order, List<string> select, string filter, string keyword);
	}
}
