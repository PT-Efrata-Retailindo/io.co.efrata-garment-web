using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleRequests.Repositories
{
    public class GarmentSampleRequestRepository : AggregateRepostory<GarmentSampleRequest, GarmentSampleRequestReadModel>, IGarmentSampleRequestRepository
    {
        public IQueryable<GarmentSampleRequestReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleRequestReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONoSample",
                "SampleCategory",
                "SampleRequestNo",
                "BuyerCode",
                "BuyerName",
                "POBuyer",
            };

            data = QueryHelper<GarmentSampleRequestReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderBy(o => o.IsReceived).ThenByDescending(o=>o.Date) : QueryHelper<GarmentSampleRequestReadModel>.Order(data, OrderDictionary);


            return data;
        }

        protected override GarmentSampleRequest Map(GarmentSampleRequestReadModel readModel)
        {
            return new GarmentSampleRequest(readModel);
        }
    }
}