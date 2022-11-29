using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Manufactures.Data.EntityFrameworkCore.GarmentDeliveryReturns.Repositories
{
    public class GarmentDeliveryReturnRepository : AggregateRepostory<GarmentDeliveryReturn, GarmentDeliveryReturnReadModel>, IGarmentDeliveryReturnRepository
    {
        public IQueryable<GarmentDeliveryReturnReadModel> Read(string order, List<string> select, string filter)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentDeliveryReturnReadModel>.Filter(Query, FilterDictionary);

            return data;
        }

        protected override GarmentDeliveryReturn Map(GarmentDeliveryReturnReadModel readModel)
        {
            return new GarmentDeliveryReturn(readModel);
        }
    }
}