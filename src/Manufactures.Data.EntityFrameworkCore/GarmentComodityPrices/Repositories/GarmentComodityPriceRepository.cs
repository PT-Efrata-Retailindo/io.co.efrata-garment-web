using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentComodityPrices.Repositories
{
    public class GarmentComodityPriceRepository : AggregateRepostory<GarmentComodityPrice, GarmentComodityPriceReadModel>, IGarmentComodityPriceRepository
    {
        public IQueryable<GarmentComodityPriceReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query.Where(a=>a.IsValid==true);

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentComodityPriceReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "UnitCode",
                "UnitName",
                "ComodityName",
                "ComodityCode"
            };
            data = QueryHelper<GarmentComodityPriceReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentComodityPriceReadModel>.Order(data, OrderDictionary);

           // data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentComodityPrice Map(GarmentComodityPriceReadModel readModel)
        {
            return new GarmentComodityPrice(readModel);
        }
    }
}
