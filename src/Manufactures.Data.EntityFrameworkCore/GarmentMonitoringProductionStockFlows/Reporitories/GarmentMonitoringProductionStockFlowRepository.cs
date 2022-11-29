using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.MonitoringProductionStockFlow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentMonitoringProductionStockFlows.Reporitories
{
	public class GarmentMonitoringProductionStockFlowRepository : AggregateRepostory<GarmentMonitoringProductionStocFlow, GarmentMonitoringProductionStockReadModel>, IGarmentMonitoringProductionStockFlowRepository
	{
		public IQueryable<GarmentMonitoringProductionStockReadModel> Read(string order, List<string> select, string filter, string keyword)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentMonitoringProductionStockReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"Ro"
			};

			data = QueryHelper<GarmentMonitoringProductionStockReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentMonitoringProductionStockReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}
		protected override GarmentMonitoringProductionStocFlow Map(GarmentMonitoringProductionStockReadModel readModel)
		{
			return new GarmentMonitoringProductionStocFlow(readModel);
		}
	}
}
