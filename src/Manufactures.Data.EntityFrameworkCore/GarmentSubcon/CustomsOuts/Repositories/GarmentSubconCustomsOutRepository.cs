using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.CustomsOuts.Repositories
{
    public class GarmentSubconCustomsOutRepository : AggregateRepostory<GarmentSubconCustomsOut, GarmentSubconCustomsOutReadModel>, IGarmentSubconCustomsOutRepository
    {
        public IQueryable<GarmentSubconCustomsOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconCustomsOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SubconContractNo",
                "SubconType",
                "CustomsOutType",
                "CustomsOutNo",
                "SupplierName",
                "SupplierCode",
            };

            data = QueryHelper<GarmentSubconCustomsOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconCustomsOutReadModel>.Order(data, OrderDictionary);


            return data;
        }

        protected override GarmentSubconCustomsOut Map(GarmentSubconCustomsOutReadModel readModel)
        {
            return new GarmentSubconCustomsOut(readModel);
        }
    }
}