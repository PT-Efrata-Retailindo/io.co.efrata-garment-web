using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingOuts.Repositories
{
    public class GarmentSampleFinishingOutRepository : AggregateRepostory<GarmentSampleFinishingOut, GarmentSampleFinishingOutReadModel>, IGarmentSampleFinishingOutRepository
    {
        public IQueryable<GarmentSampleFinishingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleFinishingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "FinishingOutNo",
                "UnitCode",
                "UnitToCode",
                "RONo",
                "Article",
                "GarmentSampleFinishingOutItem.ProductCode",
                "GarmentSampleFinishingOutItem.Color",
                "FinishingTo"
            };

            data = QueryHelper<GarmentSampleFinishingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleFinishingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }
        public IQueryable<GarmentSampleFinishingOutReadModel> ReadColor(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleFinishingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {

                "GarmentSampleFinishingOutItem.Color"

            };

            data = QueryHelper<GarmentSampleFinishingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleFinishingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSampleFinishingOutReadModel> query)
        {
            var newQuery = query.Select(GarmentSampleFinishingOutList => new
            {
                Id = GarmentSampleFinishingOutList.Identity,
                FinishingOutNo = GarmentSampleFinishingOutList.FinishingOutNo,
                UnitTo = new
                {
                    Id = GarmentSampleFinishingOutList.UnitToId,
                    Code = GarmentSampleFinishingOutList.UnitToCode,
                    Name = GarmentSampleFinishingOutList.UnitToName
                },
                Unit = new
                {
                    Id = GarmentSampleFinishingOutList.UnitId,
                    Code = GarmentSampleFinishingOutList.UnitCode,
                    Name = GarmentSampleFinishingOutList.UnitName
                },
                RONo = GarmentSampleFinishingOutList.RONo,
                Article = GarmentSampleFinishingOutList.Article,
                FinishingOutDate = GarmentSampleFinishingOutList.FinishingOutDate,
                FinishingTo = GarmentSampleFinishingOutList.FinishingTo,
                Comodity = new
                {
                    Id = GarmentSampleFinishingOutList.ComodityId,
                    Code = GarmentSampleFinishingOutList.ComodityCode,
                    Name = GarmentSampleFinishingOutList.ComodityName
                },
                IsDifferentSize = GarmentSampleFinishingOutList.IsDifferentSize,

                Items = GarmentSampleFinishingOutList.GarmentSampleFinishingOutItem.Select(GarmentSampleFinishingOutItem => new {
                    Id = GarmentSampleFinishingOutItem.Identity,
                    FinishingOutId = GarmentSampleFinishingOutItem.FinishingOutId,
                    FinishingInId = GarmentSampleFinishingOutItem.FinishingInId,
                    FinishingInItemId = GarmentSampleFinishingOutItem.FinishingInItemId,
                    Product = new
                    {
                        Id = GarmentSampleFinishingOutItem.ProductId,
                        Code = GarmentSampleFinishingOutItem.ProductCode,
                        Name = GarmentSampleFinishingOutItem.ProductName
                    },
                    Size = new
                    {
                        Id = GarmentSampleFinishingOutItem.SizeId,
                        Size = GarmentSampleFinishingOutItem.SizeName,
                    },
                    DesignColor = GarmentSampleFinishingOutItem.DesignColor,
                    Quantity = GarmentSampleFinishingOutItem.Quantity,
                    Uom = new
                    {
                        Id = GarmentSampleFinishingOutItem.UomId,
                        Unit = GarmentSampleFinishingOutItem.UomUnit
                    },
                    Color = GarmentSampleFinishingOutItem.Color,
                    RemainingQuantity = GarmentSampleFinishingOutItem.RemainingQuantity,
                    BasicPrice = GarmentSampleFinishingOutItem.BasicPrice,
                    Price = GarmentSampleFinishingOutItem.Price,

                    Details = GarmentSampleFinishingOutItem.GarmentSampleFinishingOutDetail.Select(GarmentSampleFinishingOutDetail => new {
                        Id = GarmentSampleFinishingOutDetail.Identity,
                        FinishingOutItemId = GarmentSampleFinishingOutDetail.FinishingOutItemId,
                        Size = new
                        {
                            Id = GarmentSampleFinishingOutDetail.SizeId,
                            Size = GarmentSampleFinishingOutDetail.SizeName,
                        },
                        Quantity = GarmentSampleFinishingOutDetail.Quantity,
                        Uom = new
                        {
                            Id = GarmentSampleFinishingOutDetail.UomId,
                            Unit = GarmentSampleFinishingOutDetail.UomUnit
                        },

                    })
                })

            });
            return newQuery;
        }

        protected override GarmentSampleFinishingOut Map(GarmentSampleFinishingOutReadModel readModel)
        {
            return new GarmentSampleFinishingOut(readModel);
        }
    }
}
