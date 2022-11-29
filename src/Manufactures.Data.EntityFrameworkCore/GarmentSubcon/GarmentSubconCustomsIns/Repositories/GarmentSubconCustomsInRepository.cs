using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconCustomsIns.Repositories
{
    public class GarmentSubconCustomsInRepository : AggregateRepostory<GarmentSubconCustomsIn, GarmentSubconCustomsInReadModel>, IGarmentSubconCustomsInRepository
    {
        public IQueryable<GarmentSubconCustomsInReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconCustomsInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "BcNo",
                "BcType",
                "SubconType",
            };

            data = QueryHelper<GarmentSubconCustomsInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconCustomsInReadModel>.Order(data, OrderDictionary);


            return data;
        }



        protected override GarmentSubconCustomsIn Map(GarmentSubconCustomsInReadModel readModel)
        {
            return new GarmentSubconCustomsIn(readModel);
        }
    }
}
