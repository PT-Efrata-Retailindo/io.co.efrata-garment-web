using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAvalComponents.Repositories
{
    public class GarmentAvalComponentRepository : AggregateRepostory<GarmentAvalComponent, GarmentAvalComponentReadModel>, IGarmentAvalComponentRepository
    {
        protected override GarmentAvalComponent Map(GarmentAvalComponentReadModel readModel)
        {
            return new GarmentAvalComponent(readModel);
        }

        public IQueryable<GarmentAvalComponentReadModel> ReadList(string order, string keyword, string filter)
        {
            var Query = this.Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            Query = QueryHelper<GarmentAvalComponentReadModel>.Filter(Query, FilterDictionary);

            List<string> SearchAttributes = new List<string> { "AvalComponentNo", "UnitCode", "UnitName", "AvalComponentType", "RONo", "Article" };
            Query = QueryHelper<GarmentAvalComponentReadModel>.Search(Query, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            Query = OrderDictionary.Count == 0 ? Query.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentAvalComponentReadModel>.Order(Query, OrderDictionary);

            return Query;
        }
    }
}
