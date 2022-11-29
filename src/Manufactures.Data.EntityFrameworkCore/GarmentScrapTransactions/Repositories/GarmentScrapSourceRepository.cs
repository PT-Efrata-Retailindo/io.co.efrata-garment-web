using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentScrapSources.Repositories
{
	public class GarmentScrapSourceRepository : AggregateRepostory<GarmentScrapSource, GarmentScrapSourceReadModel>, IGarmentScrapSourceRepository
	{
		public IQueryable<GarmentScrapSourceReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentScrapSourceReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"Code",
				"Name"
			};

			data = QueryHelper<GarmentScrapSourceReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentScrapSourceReadModel>.Order(data, OrderDictionary);

			data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentScrapSource Map(GarmentScrapSourceReadModel readModel)
		{
			return new GarmentScrapSource(readModel);
		}
	}
}
