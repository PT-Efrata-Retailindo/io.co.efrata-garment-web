using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconDeliveryLetterOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconDeliveryLetterOuts.Repositories
{
    public class GarmentSubconDeliveryLetterOutRepository : AggregateRepostory<GarmentSubconDeliveryLetterOut, GarmentSubconDeliveryLetterOutReadModel>, IGarmentSubconDeliveryLetterOutRepository
    {
        public IQueryable<GarmentSubconDeliveryLetterOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSubconDeliveryLetterOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "DLNo",
                "DLType",
                "ContractType",
                "EPONo",
                "UENNo",
            };

            data = QueryHelper<GarmentSubconDeliveryLetterOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSubconDeliveryLetterOutReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentSubconDeliveryLetterOut Map(GarmentSubconDeliveryLetterOutReadModel readModel)
        {
            return new GarmentSubconDeliveryLetterOut(readModel);
        }
    }
}