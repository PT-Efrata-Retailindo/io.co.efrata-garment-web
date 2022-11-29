using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalComponents.Repositories
{
    public class GarmentSampleAvalComponentRepository : AggregateRepostory<GarmentSampleAvalComponent, GarmentSampleAvalComponentReadModel>, IGarmentSampleAvalComponentRepository
    {
        protected override GarmentSampleAvalComponent Map(GarmentSampleAvalComponentReadModel readModel)
        {
            return new GarmentSampleAvalComponent(readModel);
        }

        public IQueryable<GarmentSampleAvalComponentReadModel> ReadList(string order, string keyword, string filter)
        {
            var Query = this.Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<GarmentSampleAvalComponentReadModel>.Filter(Query, FilterDictionary);

            List<string> SearchAttributes = new List<string> { "SampleAvalComponentNo", "UnitCode", "UnitName", "SampleAvalComponentType", "RONo", "Article" };
            Query = QueryHelper<GarmentSampleAvalComponentReadModel>.Search(Query, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = OrderDictionary.Count == 0 ? Query.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleAvalComponentReadModel>.Order(Query, OrderDictionary);

            return Query;
        }
    }
}
