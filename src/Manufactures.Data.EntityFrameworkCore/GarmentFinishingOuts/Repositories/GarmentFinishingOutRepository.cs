using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Repositories
{
    public class GarmentFinishingOutRepository : AggregateRepostory<GarmentFinishingOut, GarmentFinishingOutReadModel>, IGarmentFinishingOutRepository
    {
        public IQueryable<GarmentFinishingOutReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentFinishingOutReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "FinishingOutNo",
                "UnitCode",
                "UnitToCode",
                "RONo",
                "Article",
                "GarmentFinishingOutItem.ProductCode",
                "GarmentFinishingOutItem.Color",
                "FinishingTo"
            };

            data = QueryHelper<GarmentFinishingOutReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentFinishingOutReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }
		public IQueryable<GarmentFinishingOutReadModel> ReadColor(int page, int size, string order, string keyword, string filter)
		{
			var data = Query;

			Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
			data = QueryHelper<GarmentFinishingOutReadModel>.Filter(data, FilterDictionary);

			List<string> SearchAttributes = new List<string>
			{
				 
				"GarmentFinishingOutItem.Color" 
				 
			};

			data = QueryHelper<GarmentFinishingOutReadModel>.Search(data, SearchAttributes, keyword);

			Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
			data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentFinishingOutReadModel>.Order(data, OrderDictionary);

			//data = data.Skip((page - 1) * size).Take(size);

			return data;
		}

        public IQueryable<object> ReadExecute(IQueryable<GarmentFinishingOutReadModel> query)
        {
            var newQuery = query.Select(garmentFinishingOutList => new
            {
                Id = garmentFinishingOutList.Identity,
                FinishingOutNo = garmentFinishingOutList.FinishingOutNo,
                UnitTo = new
                {
                    Id = garmentFinishingOutList.UnitToId,
                    Code = garmentFinishingOutList.UnitToCode,
                    Name = garmentFinishingOutList.UnitToName
                },
                Unit = new
                {
                    Id = garmentFinishingOutList.UnitId,
                    Code = garmentFinishingOutList.UnitCode,
                    Name = garmentFinishingOutList.UnitName
                },
                RONo = garmentFinishingOutList.RONo,
                Article = garmentFinishingOutList.Article,
                FinishingOutDate = garmentFinishingOutList.FinishingOutDate,
                FinishingTo = garmentFinishingOutList.FinishingTo,
                Comodity = new
                {
                    Id = garmentFinishingOutList.ComodityId,
                    Code = garmentFinishingOutList.ComodityCode,
                    Name = garmentFinishingOutList.ComodityName
                },
                IsDifferentSize = garmentFinishingOutList.IsDifferentSize,

                Items = garmentFinishingOutList.GarmentFinishingOutItem.Select(garmentFinishingOutItem => new {
                    Id = garmentFinishingOutItem.Identity,
                    FinishingOutId = garmentFinishingOutItem.FinishingOutId,
                    FinishingInId = garmentFinishingOutItem.FinishingInId,
                    FinishingInItemId = garmentFinishingOutItem.FinishingInItemId,
                    Product = new
                    {
                        Id = garmentFinishingOutItem.ProductId,
                        Code = garmentFinishingOutItem.ProductCode,
                        Name = garmentFinishingOutItem.ProductName
                    },
                    Size = new
                    {
                        Id = garmentFinishingOutItem.SizeId,
                        Size = garmentFinishingOutItem.SizeName,
                    },
                    DesignColor = garmentFinishingOutItem.DesignColor,
                    Quantity = garmentFinishingOutItem.Quantity,
                    Uom = new {
                        Id = garmentFinishingOutItem.UomId,
                        Unit = garmentFinishingOutItem.UomUnit
                    },
                    Color = garmentFinishingOutItem.Color,
                    RemainingQuantity = garmentFinishingOutItem.RemainingQuantity,
                    BasicPrice = garmentFinishingOutItem.BasicPrice,
                    Price = garmentFinishingOutItem.Price,

                    Details = garmentFinishingOutItem.GarmentFinishingOutDetail.Select(garmentFinishingOutDetail => new {
                        Id = garmentFinishingOutDetail.Identity,
                        FinishingOutItemId = garmentFinishingOutDetail.FinishingOutItemId,
                        Size = new
                        {
                            Id = garmentFinishingOutDetail.SizeId,
                            Size = garmentFinishingOutDetail.SizeName,
                        },
                        Quantity = garmentFinishingOutDetail.Quantity,
                        Uom = new
                        {
                            Id = garmentFinishingOutDetail.UomId,
                            Unit = garmentFinishingOutDetail.UomUnit
                        },

                    })
                })

            });
            return newQuery;
        }

        protected override GarmentFinishingOut Map(GarmentFinishingOutReadModel readModel)
        {
            return new GarmentFinishingOut(readModel);
        }
    }
}