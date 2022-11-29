using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingDOs.Repositories
{
    public class GarmentSewingDORepository : AggregateRepostory<GarmentSewingDO, GarmentSewingDOReadModel>, IGarmentSewingDORepository
    {
        public IQueryable<GarmentSewingDOReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            data = QueryHelper<GarmentSewingDOReadModel>.Filter(data, FilterDictionary);

            List<string> SearchAttributes = new List<string>
            {
                "SewingDONo",
                "Article",
                "RONo",
                "UnitCode",
                "GarmentSewingDOItem.ProductCode"
            };

            data = QueryHelper<GarmentSewingDOReadModel>.Search(data, SearchAttributes, keyword);

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentSewingDOReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentSewingDO Map(GarmentSewingDOReadModel readModel)
        {
            return new GarmentSewingDO(readModel);
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentSewingDOReadModel> query) {
            var newQuery = query.Select(x => new {
                Id = x.Identity,
                SewingDONo = x.SewingDONo,
                CuttingOutId = x.CuttingOutId,
                UnitFrom = new {
                    Id = x.UnitFromId, 
                    Code = x.UnitFromCode, 
                    Name = x.UnitFromName
                },
                Unit = new { 
                    Id = x.UnitId, 
                    Code = x.UnitCode, 
                    Name =x.UnitName 
                },
                RONo = x.RONo,
                Article = x.Article,
                Comodity = new {
                    Id = x.ComodityId, 
                    Code = x.ComodityCode, 
                    Name =x.ComodityName
                },
                SewingDODate = x.SewingDODate,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastModifiedBy = x.ModifiedBy,
                LastModifiedDate = x.ModifiedDate,
                Items = x.GarmentSewingDOItem.Select(y => new {
                    Id = y.Identity,
                    SewingDOId = y.SewingDOId,
                    CuttingOutDetailId = y.CuttingOutDetailId,
                    CuttingOutItemId = y.CuttingOutItemId,
                    Product = new { 
                        Id = y.ProductId, 
                        Code = y.ProductCode, 
                        Name = y.ProductName
                    },
                    DesignColor = y.DesignColor,
                    Size = new { 
                        Id = y.SizeId, 
                        Size = y.SizeName
                    },
                    Quantity = y.Quantity,
                    Uom = new {
                        Id = y.UomId, 
                        Unit = y.UomUnit
                    },
                    Color = y.Color,
                    RemainingQuantity = y.RemainingQuantity,
                    BasicPrice = y.BasicPrice,
                    Price = y.Price,
                    CreatedBy = y.CreatedBy,
                    CreatedDate = y.CreatedDate,
                    LastModifiedBy = y.ModifiedBy,
                    LastModifiedDate = y.ModifiedDate,
                })
            });
            return newQuery;
        }
    }
}