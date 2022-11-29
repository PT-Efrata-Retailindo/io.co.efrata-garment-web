using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAdjustments.Repositories
{
    public class GarmentAdjustmentRepository : AggregateRepostory<GarmentAdjustment, GarmentAdjustmentReadModel>, IGarmentAdjustmentRepository
    {
        public IQueryable<GarmentAdjustmentReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentAdjustmentReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "AdjustmentNo",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName"
            };
            data = QueryHelper<GarmentAdjustmentReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentAdjustmentReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentAdjustment Map(GarmentAdjustmentReadModel readModel)
        {
            return new GarmentAdjustment(readModel);
        }
    }
}