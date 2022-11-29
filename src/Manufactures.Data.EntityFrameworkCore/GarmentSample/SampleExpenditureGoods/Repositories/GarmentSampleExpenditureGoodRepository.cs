using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleExpenditureGoods.Repositories
{
    public class GarmentSampleExpenditureGoodRepository : AggregateRepostory<GarmentSampleExpenditureGood, GarmentSampleExpenditureGoodReadModel>, IGarmentSampleExpenditureGoodRepository
    {
        public IQueryable<GarmentSampleExpenditureGoodReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleExpenditureGoodReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ExpenditureGoodNo",
                "ExpenditureType",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName",
                "ContractNo",
                "Invoice",
                "BuyerName"
            };
            data = QueryHelper<GarmentSampleExpenditureGoodReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleExpenditureGoodReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSampleExpenditureGoodReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleExpenditureGoodReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };
            data = QueryHelper<GarmentSampleExpenditureGoodReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleExpenditureGoodReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public double BasicPriceByRO(string Keyword = null, string Filter = "{}")
        {
            Dictionary<string, string> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(Filter);
            long unitId = 0;
            bool hasUnitFilter = FilterDictionary.ContainsKey("UnitId") && long.TryParse(FilterDictionary["UnitId"], out unitId);
            bool hasRONoFilter = FilterDictionary.ContainsKey("RONo");
            string RONo = hasRONoFilter ? (FilterDictionary["RONo"] ?? "").Trim() : "";

            var dataHeader = Query.Where(a => a.RONo == RONo && a.UnitId == unitId).Include(a => a.Items);

            double priceTotal = 0;
            double qtyTotal = 0;

            foreach (var data in dataHeader)
            {
                priceTotal += data.Items.Sum(a => a.Price);
                qtyTotal += data.Items.Sum(a => a.Quantity);
            }

            double basicPrice = priceTotal / qtyTotal;

            return basicPrice;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSampleExpenditureGoodReadModel> query)
        {
            var newQuery = query.Select(GarmentSampleExpenditureGood => new
            {
                Id = GarmentSampleExpenditureGood.Identity,
                ExpenditureGoodNo = GarmentSampleExpenditureGood.ExpenditureGoodNo,
                RONo = GarmentSampleExpenditureGood.RONo,
                Article = GarmentSampleExpenditureGood.Article,
                Unit = new
                {
                    Id = GarmentSampleExpenditureGood.UnitId,
                    Code = GarmentSampleExpenditureGood.UnitCode,
                    Name = GarmentSampleExpenditureGood.UnitName
                },
                ExpenditureDate = GarmentSampleExpenditureGood.ExpenditureDate,
                ExpenditureType = GarmentSampleExpenditureGood.ExpenditureType,
                Comodity = new
                {
                    Id = GarmentSampleExpenditureGood.ComodityId,
                    Code = GarmentSampleExpenditureGood.ComodityCode,
                    Name = GarmentSampleExpenditureGood.ComodityName
                },
                Buyer = new
                {
                    Id = GarmentSampleExpenditureGood.BuyerId,
                    Code = GarmentSampleExpenditureGood.BuyerCode,
                    Name = GarmentSampleExpenditureGood.BuyerName
                },
                Invoice = GarmentSampleExpenditureGood.Invoice,
                ContractNo = GarmentSampleExpenditureGood.ContractNo,
                Carton = GarmentSampleExpenditureGood.Carton,
                Description = GarmentSampleExpenditureGood.Description,
                IsReceived = GarmentSampleExpenditureGood.IsReceived,
                PackingListId = GarmentSampleExpenditureGood.PackingListId,

                Items = GarmentSampleExpenditureGood.Items.Select(GarmentSampleExpenditureGoodItem => new {
                    Id = GarmentSampleExpenditureGoodItem.Identity,
                    ExpenditureGoodId = GarmentSampleExpenditureGoodItem.ExpenditureGoodId,
                    FinishedGoodStockId = GarmentSampleExpenditureGoodItem.FinishedGoodStockId,
                    Size = new
                    {
                        Id = GarmentSampleExpenditureGoodItem.SizeId,
                        Size = GarmentSampleExpenditureGoodItem.SizeName,
                    },
                    Quantity = GarmentSampleExpenditureGoodItem.Quantity,
                    Uom = new
                    {
                        Id = GarmentSampleExpenditureGoodItem.UomId,
                        Unit = GarmentSampleExpenditureGoodItem.UomUnit
                    },
                    Description = GarmentSampleExpenditureGoodItem.Description,
                    BasicPrice = GarmentSampleExpenditureGoodItem.BasicPrice,
                    Price = GarmentSampleExpenditureGoodItem.Price,
                    ReturQuantity = GarmentSampleExpenditureGoodItem.ReturQuantity,

                })

            });
            return newQuery;
        }

        protected override GarmentSampleExpenditureGood Map(GarmentSampleExpenditureGoodReadModel readModel)
        {
            return new GarmentSampleExpenditureGood(readModel);
        }

        public IQueryable<GarmentSampleExpenditureGoodReadModel> ReadignoreFilter(int page, int size, string order, string keyword, string filter)
        {
            var data = Query.IgnoreQueryFilters().Where(eg => (eg.Deleted == true && eg.DeletedBy == "L") || (eg.Deleted == false));

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleExpenditureGoodReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "ExpenditureGoodNo",
                "ExpenditureType",
                "Article",
                "RONo",
                "UnitCode",
                "UnitName",
                "ContractNo",
                "Invoice",
                "BuyerName"
            };
            data = QueryHelper<GarmentSampleExpenditureGoodReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleExpenditureGoodReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }
    }
}
