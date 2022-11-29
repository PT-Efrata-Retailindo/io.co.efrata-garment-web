using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingOuts.Repositories
{
    public class GarmentSampleCuttingOutRepository : AggregateRepostory<GarmentSampleCuttingOut, GarmentSampleCuttingOutReadModel>, IGarmentSampleCuttingOutRepository
    {
        public IQueryable<GarmentSampleCuttingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSampleCuttingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "CutOutNo",
                "UnitCode",
                "RONo",
                "Article",
            };

            data = QueryHelper<GarmentSampleCuttingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSampleCuttingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentSampleCuttingOut Map(GarmentSampleCuttingOutReadModel readModel)
        {
            return new GarmentSampleCuttingOut(readModel);
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSampleCuttingOutReadModel> query)
        {
            var newQuery = query.Select(x => new
            {
                Id = x.Identity,
                CutOutNo = x.CutOutNo,
                CuttingOutType = x.CuttingOutType,
                UnitFrom = new
                {
                    Id = x.UnitFromId,
                    Name = x.UnitFromName,
                    Code = x.UnitFromCode
                },
                CuttingOutDate = x.CuttingOutDate,
                RONo = x.RONo,
                Article = x.Article,
                Unit = new
                {
                    Id = x.UnitId,
                    Name = x.UnitName,
                    Code = x.UnitCode
                },
                Comodity = new
                {
                    Id = x.ComodityId,
                    Code = x.ComodityCode,
                    Name = x.ComodityName
                },

                Items = x.GarmentSampleCuttingOutItem.Select(GarmentSampleCuttingOutItem => new {
                    Id = GarmentSampleCuttingOutItem.Identity,
                    CutOutId = GarmentSampleCuttingOutItem.CuttingOutId,
                    CuttingInId = GarmentSampleCuttingOutItem.CuttingInId,
                    CuttingInDetailId = GarmentSampleCuttingOutItem.CuttingInDetailId,
                    Product = new
                    {
                        Id = GarmentSampleCuttingOutItem.ProductId,
                        Code = GarmentSampleCuttingOutItem.ProductCode,
                        Name = GarmentSampleCuttingOutItem.ProductName
                    },
                    DesignColor = GarmentSampleCuttingOutItem.DesignColor,
                    TotalCuttingOut = GarmentSampleCuttingOutItem.TotalCuttingOut,

                    Details = GarmentSampleCuttingOutItem.GarmentSampleCuttingOutDetail.Select(GarmentSampleCuttingOutDetail => new {
                        Id = GarmentSampleCuttingOutDetail.Identity,
                        CutOutItemId = GarmentSampleCuttingOutDetail.CuttingOutItemId,
                        Size = new
                        {
                            Id = GarmentSampleCuttingOutDetail.SizeId,
                            Size = GarmentSampleCuttingOutDetail.SizeName
                        },
                        CuttingOutQuantity = GarmentSampleCuttingOutDetail.CuttingOutQuantity,
                        CuttingOutUom = new
                        {
                            Id = GarmentSampleCuttingOutDetail.CuttingOutUomId,
                            Unit = GarmentSampleCuttingOutDetail.CuttingOutUomUnit
                        },
                        Color = GarmentSampleCuttingOutDetail.Color,
                        RemainingQuantity = GarmentSampleCuttingOutDetail.RemainingQuantity,
                        BasicPrice = GarmentSampleCuttingOutDetail.BasicPrice,
                        Price = GarmentSampleCuttingOutDetail.Price

                    })

                })

            });
            return newQuery;
        }
    }
}
