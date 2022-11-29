using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconFabricWashes.Repositories
{
    public class GarmentServiceSubconFabricWashItemRepository : AggregateRepostory<GarmentServiceSubconFabricWashItem, GarmentServiceSubconFabricWashItemReadModel>, IGarmentServiceSubconFabricWashItemRepository
    {
        IQueryable<GarmentServiceSubconFabricWashItemReadModel> IGarmentServiceSubconFabricWashItemRepository.ReadItem(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconFabricWashItemReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "UnitExpenditureNo"
            };

            data = QueryHelper<GarmentServiceSubconFabricWashItemReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconFabricWashItemReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSubconFabricWashItem Map(GarmentServiceSubconFabricWashItemReadModel readModel)
        {
            return new GarmentServiceSubconFabricWashItem(readModel);
        }
    }
}
