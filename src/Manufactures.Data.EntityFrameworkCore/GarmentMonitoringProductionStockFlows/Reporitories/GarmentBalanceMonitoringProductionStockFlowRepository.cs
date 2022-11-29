using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.MonitoringProductionStockFlow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentMonitoringProductionStockFlows.Repositories
{
    public class GarmentBalanceMonitoringProductionStockFlowRepository : AggregateRepostory<GarmentBalanceMonitoringProductionStocFlow, GarmentBalanceMonitoringProductionStockReadModel>, IGarmentBalanceMonitoringProductionStockFlowRepository
    {
        public IQueryable<GarmentBalanceMonitoringProductionStockReadModel> Read(string order, List<string> select, string filter, string keyword)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentBalanceMonitoringProductionStockReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "Ro"
            };

            data = QueryHelper<GarmentBalanceMonitoringProductionStockReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentBalanceMonitoringProductionStockReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }
        protected override GarmentBalanceMonitoringProductionStocFlow Map(GarmentBalanceMonitoringProductionStockReadModel readModel)
        {
            return new GarmentBalanceMonitoringProductionStocFlow(readModel);
        }
    }
}
