using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentScrapClassifications;
using Manufactures.Domain.GarmentScrapClassifications.ReadModels;
using Manufactures.Domain.GarmentScrapClassifications.Repositories;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentScrapClassifications.Repositories
{
	public class GarmentScrapClassficationRepository : AggregateRepostory<GarmentScrapClassification, GarmentScrapClassificationReadModel>, IGarmentScrapClassificationRepository
	{
		public IQueryable<GarmentScrapClassificationReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentScrapClassificationReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"Code",
				"Name"
			};

			data = QueryHelper<GarmentScrapClassificationReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentScrapClassificationReadModel>.Order(data, OrderDictionary);

			data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentScrapClassification Map(GarmentScrapClassificationReadModel readModel)
		{
			return new GarmentScrapClassification(readModel);
		}
	}
}
