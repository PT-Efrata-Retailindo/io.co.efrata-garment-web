using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentCuttingAdjustments;
using Manufactures.Domain.GarmentCuttingAdjustments.ReadModels;
using Manufactures.Domain.GarmentCuttingAdjustments.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingAdjustments.Repositories
{
    public class GarmentCuttingAdjustmentRepository : AggregateRepostory<GarmentCuttingAdjustment, GarmentCuttingAdjustmentReadModel>, IGarmentCuttingAdjustmentRepository
    {
        public IQueryable<GarmentCuttingAdjustmentReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentCuttingAdjustmentReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "CutInNo",
                "UnitCode",
                "RONo",
                "AdjustmentNo",
            };

            data = QueryHelper<GarmentCuttingAdjustmentReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentCuttingAdjustmentReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentCuttingAdjustment Map(GarmentCuttingAdjustmentReadModel readModel)
        {
            return new GarmentCuttingAdjustment(readModel);
        }
    }
}