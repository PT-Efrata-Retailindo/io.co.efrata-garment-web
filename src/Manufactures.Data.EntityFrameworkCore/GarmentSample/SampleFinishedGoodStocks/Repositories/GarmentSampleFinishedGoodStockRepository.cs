using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishedGoodStocks.Repositories
{
    public class GarmentSampleFinishedGoodStockRepository : AggregateRepostory<GarmentSampleFinishedGoodStock, GarmentSampleFinishedGoodStockReadModel>, IGarmentSampleFinishedGoodStockRepository
    {
        public IQueryable<GarmentSampleFinishedGoodStockReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleFinishedGoodStockReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "FinishedGoodStockNo",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName",
            };
            data = QueryHelper<GarmentSampleFinishedGoodStockReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleFinishedGoodStockReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSampleFinishedGoodStockReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleFinishedGoodStockReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };
            data = QueryHelper<GarmentSampleFinishedGoodStockReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleFinishedGoodStockReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }


        protected override GarmentSampleFinishedGoodStock Map(GarmentSampleFinishedGoodStockReadModel readModel)
        {
            return new GarmentSampleFinishedGoodStock(readModel);
        }
    }
}
