using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingOuts.Repositories
{
    public class GarmentSampleSewingOutRepository : AggregateRepostory<GarmentSampleSewingOut, GarmentSampleSewingOutReadModel>, IGarmentSampleSewingOutRepository
    {
        public IQueryable<GarmentSampleSewingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SewingOutNo",
                "UnitCode",
                "UnitToCode",
                "RONo",
                "Article",
                "GarmentSewingOutItem.ProductCode",
                "GarmentSewingOutItem.Color"
            };

            data = QueryHelper<GarmentSampleSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleSewingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<GarmentSampleSewingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "RONo",
            };

            data = QueryHelper<GarmentSampleSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleSewingOutReadModel>.Order(data, OrderDictionary);

            //var data2 = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable ReadDynamic(string order, string search, string select, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleSewingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = JsonConvert.DeserializeObject<List<string>>(search);
            if (SearchAttributes.Count == 0)
            {
                SearchAttributes = new List<string>
                {
                    "SewingOutNo",
                    "UnitCode",
                    "UnitToCode",
                    "RONo",
                    "Article",
                    "GarmentSewingOutItem.ProductCode",
                    "GarmentSewingOutItem.Color"
                };
            }
            data = QueryHelper<GarmentSampleSewingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleSewingOutReadModel>.Order(data, OrderDictionary);

            var selectedData = QueryHelper<GarmentSampleSewingOutReadModel>.Select(data, select);

            return selectedData;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSampleSewingOutReadModel> query)
        {
            var newQuery = query.Select(x => new
            {
                Id = x.Identity,
                SewingOutNo = x.SewingOutNo,
                Buyer = new
                {
                    Id = x.BuyerId,
                    Code = x.BuyerCode,
                    Name = x.BuyerName
                },
                Unit = new
                {
                    Id = x.UnitId,
                    Code = x.UnitCode,
                    Name = x.UnitName
                },
                SewingTo = x.SewingTo,
                UnitTo = new
                {
                    Id = x.UnitToId,
                    Code = x.UnitToCode,
                    Name = x.UnitToName
                },
                RONo = x.RONo,
                Article = x.Article,
                Comodity = new
                {
                    Id = x.ComodityId,
                    Code = x.ComodityCode,
                    Name = x.ComodityName
                },
                SewingOutDate = x.SewingOutDate,
                IsDifferentSize = x.IsDifferentSize,
                Items = x.GarmentSewingOutItem.Select(garmentSewingOutItem => new
                {
                    Id = garmentSewingOutItem.Identity,
                    SewingOutId = garmentSewingOutItem.SampleSewingOutId,
                    SewingInId = garmentSewingOutItem.SampleSewingInId,
                    SewingInItemId = garmentSewingOutItem.SampleSewingInItemId,
                    Product = new
                    {
                        Id = garmentSewingOutItem.ProductId,
                        Code = garmentSewingOutItem.ProductCode,
                        Name = garmentSewingOutItem.ProductName
                    },
                    Size = new
                    {
                        Id = garmentSewingOutItem.SizeId,
                        Size = garmentSewingOutItem.SizeName,
                    },
                    DesignColor = garmentSewingOutItem.DesignColor,
                    Quantity = garmentSewingOutItem.Quantity,
                    Uom = new
                    {
                        Id = garmentSewingOutItem.UomId,
                        Unit = garmentSewingOutItem.UomUnit,
                    },
                    Color = garmentSewingOutItem.Color,
                    RemainingQuantity = garmentSewingOutItem.RemainingQuantity,
                    BasicPrice = garmentSewingOutItem.BasicPrice,
                    Price = garmentSewingOutItem.Price,

                    Details = garmentSewingOutItem.GarmentSampleSewingOutDetail.Select(garmentSewingOutDetail => new
                    {
                        Id = garmentSewingOutDetail.Identity,
                        SewingOutItemId = garmentSewingOutDetail.SampleSewingOutItemId,
                        Size = new
                        {
                            Id = garmentSewingOutDetail.SizeId,
                            Size = garmentSewingOutDetail.SizeName,
                        },
                        Quantity = garmentSewingOutDetail.Quantity,
                        Uom = new
                        {
                            Id = garmentSewingOutDetail.UomId,
                            Unit = garmentSewingOutDetail.UomUnit,
                        },
                    })

                })
            });

            return newQuery;
        }

        protected override GarmentSampleSewingOut Map(GarmentSampleSewingOutReadModel readModel)
        {
            return new GarmentSampleSewingOut(readModel);
        }
    }
}