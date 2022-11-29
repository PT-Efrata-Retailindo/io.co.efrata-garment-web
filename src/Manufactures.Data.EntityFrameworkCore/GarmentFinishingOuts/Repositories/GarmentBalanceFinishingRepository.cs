using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Repositories
{
	public class GarmentBalanceFinishingRepository : AggregateRepostory<GarmentBalanceFinishing, GarmentBalanceFinishingReadModel>, IGarmentBalanceFinishingRepository
	{
		public IQueryable<GarmentBalanceFinishingReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentBalanceFinishingReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"UnitCode",
				"RONo",
				"Article",
			};

			data = QueryHelper<GarmentBalanceFinishingReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentBalanceFinishingReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentBalanceFinishing Map(GarmentBalanceFinishingReadModel readModel)
		{
			return new GarmentBalanceFinishing(readModel);
		}
	}
}
