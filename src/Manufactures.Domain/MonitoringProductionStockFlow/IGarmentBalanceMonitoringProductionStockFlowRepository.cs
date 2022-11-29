using Infrastructure.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.MonitoringProductionStockFlow
{
    public  interface IGarmentBalanceMonitoringProductionStockFlowRepository : IAggregateRepository<GarmentBalanceMonitoringProductionStocFlow, GarmentBalanceMonitoringProductionStockReadModel>
    {
        IQueryable<GarmentBalanceMonitoringProductionStockReadModel> Read(string order, List<string> select, string filter, string keyword);
    }
}
