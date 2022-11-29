using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentLoadings.Repositories
{
	public class GarmentBalanceLoadingRepository : AggregateRepostory<GarmentBalanceLoading, GarmentBalanceLoadingReadModel>, IGarmentBalanceLoadingRepository
	{
		public IQueryable<GarmentBalanceLoadingReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentBalanceLoadingReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"UnitCode",
				"RONo",
				"Article",
			};

			data = QueryHelper<GarmentBalanceLoadingReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentBalanceLoadingReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentBalanceLoading Map(GarmentBalanceLoadingReadModel readModel)
		{
			return new GarmentBalanceLoading(readModel);
		}
	}
}
