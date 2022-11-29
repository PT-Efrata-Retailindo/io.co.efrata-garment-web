using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Repositories
{
    public class GarmentCuttingOutRepository : AggregateRepostory<GarmentCuttingOut, GarmentCuttingOutReadModel>, IGarmentCuttingOutRepository
    {
        public IQueryable<GarmentCuttingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;//.Where(d => d.CuttingOutType != "SUBKON");

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentCuttingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "CutOutNo",
                "UnitCode",
                "RONo",
                "Article",
            };

            data = QueryHelper<GarmentCuttingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentCuttingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentCuttingOut Map(GarmentCuttingOutReadModel readModel)
        {
            return new GarmentCuttingOut(readModel);
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentCuttingOutReadModel> query)
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

                Items = x.GarmentCuttingOutItem.Select(garmentCuttingOutItem => new {
                    Id = garmentCuttingOutItem.Identity,
                    CutOutId = garmentCuttingOutItem.CutOutId,
                    CuttingInId = garmentCuttingOutItem.CuttingInId,
                    CuttingInDetailId = garmentCuttingOutItem.CuttingInDetailId,
                    Product = new
                    {
                        Id = garmentCuttingOutItem.ProductId,
                        Code = garmentCuttingOutItem.ProductCode,
                        Name = garmentCuttingOutItem.ProductName
                    },
                    DesignColor = garmentCuttingOutItem.DesignColor,
                    TotalCuttingOut = garmentCuttingOutItem.TotalCuttingOut,

                    Details = garmentCuttingOutItem.GarmentCuttingOutDetail.Select(garmentCuttingOutDetail => new {
                        Id = garmentCuttingOutDetail.Identity,
                        CutOutItemId = garmentCuttingOutDetail.CutOutItemId,
                        Size = new
                        {
                            Id = garmentCuttingOutDetail.SizeId,
                            Size = garmentCuttingOutDetail.SizeName
                        },
                        CuttingOutQuantity = garmentCuttingOutDetail.CuttingOutQuantity,
                        CuttingOutUom = new {
                            Id = garmentCuttingOutDetail.CuttingOutUomId,
                            Unit = garmentCuttingOutDetail.CuttingOutUomUnit
                        },
                        Color = garmentCuttingOutDetail.Color,
                        RemainingQuantity = garmentCuttingOutDetail.RemainingQuantity,
                        BasicPrice = garmentCuttingOutDetail.BasicPrice,
                        Price = garmentCuttingOutDetail.Price

                    })

                })

            });
            return newQuery;
        }
    }
}