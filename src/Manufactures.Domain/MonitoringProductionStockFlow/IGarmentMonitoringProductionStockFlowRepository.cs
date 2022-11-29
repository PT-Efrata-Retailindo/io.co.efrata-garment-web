using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.MonitoringProductionStockFlow
{
	public interface IGarmentMonitoringProductionStockFlowRepository : IAggregateRepository<GarmentMonitoringProductionStocFlow, GarmentMonitoringProductionStockReadModel>
	{
		IQueryable<GarmentMonitoringProductionStockReadModel> Read(string order, List<string> select, string filter, string keyword);
	}
}
