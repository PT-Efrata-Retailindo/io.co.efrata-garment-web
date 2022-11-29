using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconFabricWashes.Queries
{
    public class GetXlsServiceSubconFabricWashQuery : IQuery<MemoryStream>
    {
		public int page { get; private set; }
		public int size { get; private set; }
		public string order { get; private set; }
		public string token { get; private set; }
		public DateTime dateFrom { get; private set; }
		public DateTime dateTo { get; private set; }

		public GetXlsServiceSubconFabricWashQuery(int page, int size, string order, DateTime dateFrom, DateTime dateTo, string token)
		{
			this.page = page;
			this.size = size;
			this.order = order;
			this.dateFrom = dateFrom;
			this.dateTo = dateTo;
			this.token = token;
		}
	}
}
