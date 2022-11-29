using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingOuts.Repositories
{
	public class GarmentBalanceSewingRepository : AggregateRepostory<GarmentBalanceSewing, GarmentBalanceSewingReadModel>, IGarmentBalanceSewingRepository
	{
		public IQueryable<GarmentBalanceSewingReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentBalanceSewingReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"UnitCode",
				"RONo",
				"Article",
			};

			data = QueryHelper<GarmentBalanceSewingReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentBalanceSewingReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentBalanceSewing Map(GarmentBalanceSewingReadModel readModel)
		{
			return new GarmentBalanceSewing(readModel);
		}
	}
}
