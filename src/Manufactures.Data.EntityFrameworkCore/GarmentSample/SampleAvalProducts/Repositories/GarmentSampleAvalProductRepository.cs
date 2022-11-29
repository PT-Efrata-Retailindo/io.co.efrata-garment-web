using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleAvalProducts;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalProducts.Repositories
{
    public class GarmentSampleAvalProductRepository : AggregateRepostory<GarmentSampleAvalProduct, GarmentSampleAvalProductReadModel>, IGarmentSampleAvalProductRepository
    {
        public IQueryable<GarmentSampleAvalProductReadModel> Read(string order, List<string> select, string filter)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleAvalProductReadModel>.Filter(Query, FilterDictionary);

            return data;
        }

        protected override GarmentSampleAvalProduct Map(GarmentSampleAvalProductReadModel readModel)
        {
            return new GarmentSampleAvalProduct(readModel);
        }
    }
}
