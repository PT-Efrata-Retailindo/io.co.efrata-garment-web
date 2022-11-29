using Infrastructure.Domain.Queries;
using System;

namespace Manufactures.Application.GarmentLoadings.Queries
{
	public class GetMonitoringLoadingQuery : IQuery<GarmentMonitoringLoadingListViewModel>
	{
		public int page { get; private set; }
		public int size { get; private set; }
		public string order { get; private set; }
		public string token { get; private set; }
		public int unit { get; private set; }
		public DateTime dateFrom { get; private set; }
		public DateTime dateTo { get; private set; }

		public GetMonitoringLoadingQuery(int page, int size, string order, int unit, DateTime dateFrom, DateTime dateTo, string token)
		{
			this.page = page;
			this.size = size;
			this.order = order;
			this.unit = unit;
			this.dateFrom = dateFrom;
			this.dateTo = dateTo;
			this.token = token;
		}
	}
}
