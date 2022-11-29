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

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconFabricWashs.Repositories
{
    public class GarmentServiceSubconFabricWashRepository : AggregateRepostory<GarmentServiceSubconFabricWash, GarmentServiceSubconFabricWashReadModel>, IGarmentServiceSubconFabricWashRepository
    {
        IQueryable<GarmentServiceSubconFabricWashReadModel> IGarmentServiceSubconFabricWashRepository.Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconFabricWashReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ServiceSubconFabricWashNo",
            };

            data = QueryHelper<GarmentServiceSubconFabricWashReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconFabricWashReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSubconFabricWash Map(GarmentServiceSubconFabricWashReadModel readModel)
        {
            return new GarmentServiceSubconFabricWash(readModel);
        }
    }
}
