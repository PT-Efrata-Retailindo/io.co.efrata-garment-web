using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoods.Repositories
{
	public class GarmentExpenditureGoodInvoiceRelationRepository : AggregateRepostory<GarmentExpenditureGoodInvoiceRelation, GarmentExpenditureGoodInvoiceRelationReadModel>, IGarmentExpenditureGoodInvoiceRelationRepository
	{
		public IQueryable<GarmentExpenditureGoodInvoiceRelationReadModel> Read(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentExpenditureGoodInvoiceRelationReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				"ExpenditureGoodNo",
				"RONo",
				"UnitCode",
				"InvoiceNo"
			};
			data = QueryHelper<GarmentExpenditureGoodInvoiceRelationReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentExpenditureGoodInvoiceRelationReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

		protected override GarmentExpenditureGoodInvoiceRelation Map(GarmentExpenditureGoodInvoiceRelationReadModel readModel)
		{
			return new GarmentExpenditureGoodInvoiceRelation(readModel);
		}
	}
}
