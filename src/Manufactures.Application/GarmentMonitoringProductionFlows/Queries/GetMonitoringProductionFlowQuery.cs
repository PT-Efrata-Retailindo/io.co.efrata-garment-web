using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentMonitoringProductionFlows.Queries
{
	public class GetMonitoringProductionFlowQuery : IQuery<GarmentMonitoringProductionFlowListViewModel>
	{
		public int page { get; private set; }
		public int size { get; private set; }
		public string order { get; private set; }
		public string token { get; private set; }
		public int unit { get; private set; }
		public DateTime date { get; private set; }
		public string ro { get; private set; }

		public GetMonitoringProductionFlowQuery(int page, int size, string order, int unit, DateTime date, string  ro, string token)
		{
			this.page = page;
			this.size = size;
			this.order = order;
			this.unit = unit;
			this.date = date;
			this.ro = ro;
			this.token = token;
		}
	}
}
