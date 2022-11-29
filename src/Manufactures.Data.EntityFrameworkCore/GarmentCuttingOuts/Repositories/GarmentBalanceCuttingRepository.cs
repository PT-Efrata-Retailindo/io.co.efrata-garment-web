using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Repositories
{
	public class GarmentBalanceCuttingRepository : AggregateRepostory<GarmentBalanceCutting, GarmentBalanceCuttingReadModel>, IGarmentBalanceCuttingRepository
	{
		public IQueryable<GarmentBalanceCuttingReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentBalanceCuttingReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"CutOutNo",
				"UnitCode",
				"RONo",
				"Article",
			};

			data = QueryHelper<GarmentBalanceCuttingReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentBalanceCuttingReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentBalanceCutting Map(GarmentBalanceCuttingReadModel readModel)
		{
			return new GarmentBalanceCutting(readModel);
		}
	}

}