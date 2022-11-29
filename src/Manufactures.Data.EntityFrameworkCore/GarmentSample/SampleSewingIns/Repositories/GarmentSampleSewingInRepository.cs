using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingIns.Repositories
{
    public class GarmentSampleSewingInRepository : AggregateRepostory<GarmentSampleSewingIn, GarmentSampleSewingInReadModel>, IGarmentSampleSewingInRepository
    {
        public IQueryable<GarmentSampleSewingInReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleSewingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SewingInNo",
                "Article",
                "RONo",
                "UnitCode",
                "UnitFromCode"
            };

            data = QueryHelper<GarmentSampleSewingInReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleSewingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSampleSewingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;
            //var buyerCode = string.Empty;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);

            //if (FilterDictionary.ContainsKey("BuyerCode"))
            //{
            //    buyerCode = FilterDictionary.FirstOrDefault(k => k.Key == "BuyerCode").Value.ToString();
            //    FilterDictionary.Remove("BuyerCode");
            //}

            data = QueryHelper<GarmentSampleSewingInReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };

            data = QueryHelper<GarmentSampleSewingInReadModel>.Search(data, SearchAttributes, keyword);

            //if (!string.IsNullOrEmpty(buyerCode))
            //{
            //    var preparings = storageContext.Set<GarmentPreparingReadModel>();
            //    var roNo = preparings.Where(x => x.BuyerCode == buyerCode)
            //        .Select(s => s.RONo).Distinct().ToList();

            //    data = data.Where(x => roNo.Contains(x.RONo));
            //}

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleSewingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentSampleSewingIn Map(GarmentSampleSewingInReadModel readModel)
        {
            return new GarmentSampleSewingIn(readModel);
        }
    }
}
