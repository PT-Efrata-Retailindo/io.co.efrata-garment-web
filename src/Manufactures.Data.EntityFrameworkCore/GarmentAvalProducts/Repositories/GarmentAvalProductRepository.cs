using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentAvalProducts;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAvalProducts.Repositories
{
    public class GarmentAvalProductRepository : AggregateRepostory<GarmentAvalProduct, GarmentAvalProductReadModel>, IGarmentAvalProductRepository
    {
        public IQueryable<GarmentAvalProductReadModel> Read(string order, List<string> select, string filter)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentAvalProductReadModel>.Filter(Query, FilterDictionary);

            return data;
        }

        protected override GarmentAvalProduct Map(GarmentAvalProductReadModel readModel)
        {
            return new GarmentAvalProduct(readModel);
        }
    }
}