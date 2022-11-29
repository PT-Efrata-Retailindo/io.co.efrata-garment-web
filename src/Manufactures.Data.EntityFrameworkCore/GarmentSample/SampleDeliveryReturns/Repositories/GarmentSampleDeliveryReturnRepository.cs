using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleDeliveryReturns.Repositories
{
    public class GarmentSampleDeliveryReturnRepository : AggregateRepostory<GarmentSampleDeliveryReturn, GarmentSampleDeliveryReturnReadModel>, IGarmentSampleDeliveryReturnRepository
    {
        public IQueryable<GarmentSampleDeliveryReturnReadModel> Read(string order, List<string> select, string filter)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleDeliveryReturnReadModel>.Filter(Query, FilterDictionary);

            return data;
        }

        protected override GarmentSampleDeliveryReturn Map(GarmentSampleDeliveryReturnReadModel readModel)
        {
            return new GarmentSampleDeliveryReturn(readModel);
        }
    }
}
