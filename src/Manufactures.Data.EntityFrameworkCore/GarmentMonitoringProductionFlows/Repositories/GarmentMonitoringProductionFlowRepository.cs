using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;

using Manufactures.Domain.MonitoringProductionFlow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentMonitoringProductionFlows.Repositories
{
	public class GarmentMonitoringProductionFlowRepository : AggregateRepostory<GarmentMonitoringProductionFlow, GarmentMonitoringProductionFlowReadModel>, IGarmentMonitoringProductionFlowRepository
	{
		public IQueryable<GarmentMonitoringProductionFlowReadModel> Read(string order, List<string> select, string filter,string keyword)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentMonitoringProductionFlowReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"Ro"
			};

			data = QueryHelper<GarmentMonitoringProductionFlowReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentMonitoringProductionFlowReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}
		protected override GarmentMonitoringProductionFlow Map(GarmentMonitoringProductionFlowReadModel readModel)
		{
			return new GarmentMonitoringProductionFlow(readModel);
		}
	}
}
