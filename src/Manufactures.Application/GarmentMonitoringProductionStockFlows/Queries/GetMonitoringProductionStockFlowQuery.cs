using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries
{
	public class GetMonitoringProductionStockFlowQuery : IQuery<GarmentMonitoringProductionStockFlowListViewModel>
	{
		public int page { get; private set; }
		public int size { get; private set; }
		public string order { get; private set; }
		public string token { get; private set; }
		public int unit { get; private set; }
		public string ro { get; private set; }
		public DateTime dateFrom { get; private set; }
		public DateTime dateTo { get; private set; }

		public GetMonitoringProductionStockFlowQuery(int page, int size, string order, int unit, string ro, DateTime dateFrom, DateTime dateTo, string token)
		{
			this.page = page;
			this.size = size;
			this.order = order;
			this.unit = unit;
			this.ro = ro;
			this.dateTo = dateTo;
			this.dateFrom = dateFrom;
			this.token = token;
		}
	}
}
