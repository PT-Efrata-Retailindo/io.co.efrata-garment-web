using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SamplePreparings.Repositories
{
    public class GarmentSamplePreparingRepository : AggregateRepostory<GarmentSamplePreparing, GarmentSamplePreparingReadModel>, IGarmentSamplePreparingRepository
    {
        public IQueryable<GarmentSamplePreparingReadModel> Read(string order, List<string> select, string filter)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();
            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSamplePreparingReadModel>.Filter(Query, FilterDictionary);

            return data;
        }

        protected override GarmentSamplePreparing Map(GarmentSamplePreparingReadModel readModel)
        {
            return new GarmentSamplePreparing(readModel);
        }

        public IQueryable<GarmentSamplePreparingReadModel> ReadOptimized(string order, string filter, string keyword)
        {
            var data = Query.OrderByDescending(o => o.CreatedDate).AsQueryable();

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSamplePreparingReadModel>.Filter(Query, FilterDictionary);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSamplePreparingReadModel>.Order(data, OrderDictionary);


            return data;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSamplePreparingReadModel> query, string keyword)
        {
            var newQuery = query.Select(x => new {
                Id = x.Identity,
                LastModifiedDate = x.ModifiedDate ?? x.CreatedDate,
                LastModifiedBy = x.ModifiedBy ?? x.CreatedBy,
                UENId = x.UENId,
                UENNo = x.UENNo,
                Unit = new
                {
                    Id = x.UnitId,
                    Name = x.UnitName,
                    Code = x.UnitCode
                },
                ProcessDate = x.ProcessDate,
                RONo = x.RONo,
                Article = x.Article,
                IsCuttingIn = x.IsCuttingIn,
                CreatedBy = x.CreatedBy,
                Buyer = new
                {
                    Id = x.BuyerId,
                    Code = x.BuyerCode,
                    Name = x.BuyerName
                },
                TotalQuantity = x.GarmentSamplePreparingItem.Sum(b => b.Quantity),
                Items = x.GarmentSamplePreparingItem.Select(y => new {
                    Id = y.Identity,
                    LastModifiedDate = y.ModifiedDate ?? y.CreatedDate,
                    LastModifiedBy = y.ModifiedBy ?? y.CreatedBy,
                    UENItemId = y.UENItemId,
                    Product = new
                    {
                        Id = y.ProductId,
                        Name = y.ProductName,
                        Code = y.ProductCode
                    },
                    DesignColor = y.DesignColor,
                    Quantity = y.Quantity,
                    Uom = new
                    {
                        Id = y.UomId,
                        Unit = y.UomUnit
                    },
                    FabricType = y.FabricType,
                    RemainingQuantity = y.RemainingQuantity,
                    BasicPrice = y.BasicPrice,
                    GarmentSamplePreparingId = y.GarmentSamplePreparingId,
                    ROSource = y.ROSource,
                }).OrderBy(y => y.Id)
            });

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                newQuery = newQuery.Where(d => d.UENNo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || d.RONo.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || d.Unit.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || d.Article != null && d.Article.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                        || d.Items.Any(e => e.Product.Code.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            }
            return newQuery;
        }

        public bool RoChecking(IEnumerable<string> roList, string buyerCode)
        {
            var data = Query.Where(x => roList.Contains(x.RONo) && buyerCode.Contains(x.BuyerCode)).ToList();
            return data.Count > 0;
        }
    }
}
