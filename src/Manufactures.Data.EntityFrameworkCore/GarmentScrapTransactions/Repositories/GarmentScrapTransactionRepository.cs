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
	public class GarmentScrapTransactionRepository : AggregateRepostory<GarmentScrapTransaction, GarmentScrapTransactionReadModel>, IGarmentScrapTransactionRepository
	{
		public IQueryable<GarmentScrapTransactionReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentScrapTransactionReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"TransactionNo",
				"ScrapSourceName",
				"ScrapDestinationName"
			};

			data = QueryHelper<GarmentScrapTransactionReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentScrapTransactionReadModel>.Order(data, OrderDictionary);

			data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentScrapTransaction Map(GarmentScrapTransactionReadModel readModel)
		{
			return new GarmentScrapTransaction(readModel);
		}
	}
}
