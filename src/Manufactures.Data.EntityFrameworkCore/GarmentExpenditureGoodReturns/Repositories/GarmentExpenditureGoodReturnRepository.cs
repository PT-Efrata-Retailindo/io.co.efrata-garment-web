using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoodReturns.Repositories
{
    public class GarmentExpenditureGoodReturnRepository : AggregateRepostory<GarmentExpenditureGoodReturn, GarmentExpenditureGoodReturnReadModel>, IGarmentExpenditureGoodReturnRepository
    {
        public IQueryable<GarmentExpenditureGoodReturnReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentExpenditureGoodReturnReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ReturNo",
                "ExpenditureNo",
                "URNNo",
                "ReturType",
                "Article",
                "RONo",
                "UnitCode",
                "Invoice",
                "UnitName"
            };
            data = QueryHelper<GarmentExpenditureGoodReturnReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentExpenditureGoodReturnReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentExpenditureGoodReturn Map(GarmentExpenditureGoodReturnReadModel readModel)
        {
            return new GarmentExpenditureGoodReturn(readModel);
        }
    }
}
