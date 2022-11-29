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

namespace Manufactures.Data.EntityFrameworkCore.GarmentScrapTransactions.Repositories
{
	public class GarmentScrapTransactionItemRepository : AggregateRepostory<GarmentScrapTransactionItem, GarmentScrapTransactionItemReadModel>, IGarmentScrapTransactionItemRepository
	{
		public IQueryable<GarmentScrapTransactionItemReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentScrapTransactionItemReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"ScrapClassificationName"
			};

			data = QueryHelper<GarmentScrapTransactionItemReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentScrapTransactionItemReadModel>.Order(data, OrderDictionary);

			data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentScrapTransactionItem Map(GarmentScrapTransactionItemReadModel readModel)
		{
			return new GarmentScrapTransactionItem(readModel);
		}
	}
}
