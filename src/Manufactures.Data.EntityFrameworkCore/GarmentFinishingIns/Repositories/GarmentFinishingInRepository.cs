using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingIns.Repositories
{
    public class GarmentFinishingInRepository : AggregateRepostory<GarmentFinishingIn, GarmentFinishingInReadModel>, IGarmentFinishingInRepository
    {
        public IQueryable<GarmentFinishingInReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentFinishingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "FinishingInNo",
                "FinishingInType",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName",
                "UnitFromCode",
                "UnitFromName",
                "Items.ProductName"
            };
            data = QueryHelper<GarmentFinishingInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentFinishingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentFinishingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentFinishingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };
            data = QueryHelper<GarmentFinishingInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentFinishingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentFinishingIn Map(GarmentFinishingInReadModel readModel)
        {
            return new GarmentFinishingIn(readModel);
        }
    }
}
