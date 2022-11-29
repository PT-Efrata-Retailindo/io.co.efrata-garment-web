using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Newtonsoft.Json;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Repositories
{
    public class GarmentServiceSubconSewingRepository : AggregateRepostory<GarmentServiceSubconSewing, GarmentServiceSubconSewingReadModel>,IGarmentServiceSubconSewingRepository
    {
        IQueryable<GarmentServiceSubconSewingReadModel> IGarmentServiceSubconSewingRepository.Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentServiceSubconSewingReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ServiceSubconSewingNo",
                //"UnitCode",
            };

            data = QueryHelper<GarmentServiceSubconSewingReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentServiceSubconSewingReadModel>.Order(data, OrderDictionary);

            return data;
        }

        protected override GarmentServiceSubconSewing Map(GarmentServiceSubconSewingReadModel readModel)
        {
            return new GarmentServiceSubconSewing(readModel);
        }
    }
}
