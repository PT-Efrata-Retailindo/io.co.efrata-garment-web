using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Repositories
{
    public class GarmentServiceSubconSewingItemRepository : AggregateRepostory<GarmentServiceSubconSewingItem, GarmentServiceSubconSewingItemReadModel>, IGarmentServiceSubconSewingItemRepository
    {
        IQueryable<GarmentServiceSubconSewingItemReadModel> IGarmentServiceSubconSewingItemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconSewingItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo"
            };

            data = QueryHelper<GarmentServiceSubconSewingItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconSewingItemReadModel>.Order(data, OrderDictionary);

            return data;
        }
        protected override GarmentServiceSubconSewingItem Map(GarmentServiceSubconSewingItemReadModel readModel)
        {
            return new GarmentServiceSubconSewingItem(readModel);
        }
    }
}
