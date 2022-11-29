using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Repositories
{
    public class GarmentServiceSubconCuttingRepository : AggregateRepostory<GarmentServiceSubconCutting, GarmentServiceSubconCuttingReadModel>, IGarmentServiceSubconCuttingRepository
    {
        public IQueryable<GarmentServiceSubconCuttingReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconCuttingReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SubconNo",
                "UnitCode",
                "SubconType",
            };

            data = QueryHelper<GarmentServiceSubconCuttingReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconCuttingReadModel>.Order(data, OrderDictionary);


            return data;
        }

        

        protected override GarmentServiceSubconCutting Map(GarmentServiceSubconCuttingReadModel readModel)
        {
            return new GarmentServiceSubconCutting(readModel);
        }
    }
}