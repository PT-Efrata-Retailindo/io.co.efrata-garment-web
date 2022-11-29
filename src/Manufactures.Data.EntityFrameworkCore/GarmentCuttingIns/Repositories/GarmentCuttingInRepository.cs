using Infrastructure.Data.EntityFrameworkCore;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingIns.Repositories
{
    public class GarmentCuttingInRepository : AggregateRepostory<GarmentCuttingIn, GarmentCuttingInReadModel>, IGarmentCuttingInRepository
    {
        public IQueryable<GarmentCuttingInReadModel> Read(int page, int size, string order, string keyword, string filter)
        {
            var data = Query;
            var buyerCode = string.Empty;

            Dictionary<string, object> FilterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            
            if (FilterDictionary.ContainsKey("BuyerCode"))
            { 
                buyerCode = FilterDictionary.FirstOrDefault(k => k.Key == "BuyerCode").Value.ToString();
                FilterDictionary.Remove("BuyerCode");
            }

            data = QueryHelper<GarmentCuttingInReadModel>.Filter(data, FilterDictionary);

            if (!string.IsNullOrEmpty(buyerCode))
            {
                var preparings = storageContext.Set<GarmentPreparingReadModel>();
                var roNo = preparings.Where(x => x.BuyerCode == buyerCode)
                    .Select(s => s.RONo).Distinct().ToList();

                data = data.Where(x => roNo.Contains(x.RONo));
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                data = from d in data
                       where d.CutInNo.Contains(keyword)
                       || d.CuttingType.Contains(keyword)
                       || d.Article.Contains(keyword)
                       || d.RONo.Contains(keyword)
                       || d.UnitCode.Contains(keyword)
                       || d.UnitName.Contains(keyword)
                       || d.Items.Any(item => item.UENNo.Contains(keyword) || item.Details.Any(detail => detail.ProductCode.Contains(keyword) || detail.ProductName.Contains(keyword)))
                       select d;
            }

            Dictionary<string, string> OrderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            data = OrderDictionary.Count == 0 ? data.OrderByDescending(o => o.ModifiedDate) : QueryHelper<GarmentCuttingInReadModel>.Order(data, OrderDictionary);

            //data = data.Skip((page - 1) * size).Take(size);

            return data;
        }

        protected override GarmentCuttingIn Map(GarmentCuttingInReadModel readModel)
        {
            return new GarmentCuttingIn(readModel);
        }

        public IQueryable<object> ReadExecute(IQueryable<GarmentCuttingInReadModel> query) {
            var newQuery = query.Select(x => new {
                Id = x.Identity,
                CutInNo = x.CutInNo,
                CuttingType = x.CuttingType,
                RONo = x.RONo,
                Article = x.Article,
                Unit = new {
                    Id = x.UnitId,
                    Name = x.UnitName,
                    Code = x.UnitCode
                },
                CuttingInDate = x.CuttingInDate,
                FC = x.FC,
                CuttingFrom = x.CuttingFrom,
                CreatedBy = x.CreatedBy,
                CreatedDate = x.CreatedDate,
                LastModifiedBy = x.ModifiedBy,
                LastModifiedDate = x.ModifiedDate,
                Items = x.Items.Select(garmentCuttingInItem => new {
                    Id = garmentCuttingInItem.Identity,
                    CutInId = garmentCuttingInItem.CutInId,
                    PreparingId = garmentCuttingInItem.PreparingId,
                    UENId = garmentCuttingInItem.UENId,
                    UENNo = garmentCuttingInItem.UENNo,
                    SewingOutId = garmentCuttingInItem.SewingOutId,
                    SewingOutNo = garmentCuttingInItem.SewingOutNo,
                    CreatedBy = garmentCuttingInItem.CreatedBy,
                    CreatedDate = garmentCuttingInItem.CreatedDate,
                    LastModifiedBy = garmentCuttingInItem.ModifiedBy,
                    LastModifiedDate = garmentCuttingInItem.ModifiedDate,
                    Details = garmentCuttingInItem.Details.Select(garmentCuttingInDetail => new {
                        Id = garmentCuttingInDetail.Identity,
                        CutInItemId = garmentCuttingInDetail.CutInItemId,
                        PreparingItemId = garmentCuttingInDetail.PreparingItemId,
                        Product = new {
                            Id = garmentCuttingInDetail.ProductId, 
                            Code = garmentCuttingInDetail.ProductCode, 
                            Name =garmentCuttingInDetail.ProductName 
                        },
                        DesignColor = garmentCuttingInDetail.DesignColor,
                        FabricType = garmentCuttingInDetail.FabricType,
                        PreparingQuantity = garmentCuttingInDetail.PreparingQuantity,
                        PreparingUom = new { 
                            Id = garmentCuttingInDetail.PreparingUomId,
                            Unit = garmentCuttingInDetail.PreparingUomUnit 
                        },
                        CuttingInQuantity = garmentCuttingInDetail.CuttingInQuantity,
                        CuttingInUom = new { 
                            Id = garmentCuttingInDetail.CuttingInUomId, 
                            Unit = garmentCuttingInDetail.CuttingInUomUnit 
                        },
                        RemainingQuantity = garmentCuttingInDetail.RemainingQuantity,
                        BasicPrice = garmentCuttingInDetail.BasicPrice,
                        Price = garmentCuttingInDetail.Price,
                        FC = garmentCuttingInDetail.FC,
                        Color = garmentCuttingInDetail.Color,
                        CreatedBy = garmentCuttingInDetail.CreatedBy,
                        CreatedDate = garmentCuttingInDetail.CreatedDate,
                        LastModifiedBy = garmentCuttingInDetail.ModifiedBy,
                        LastModifiedDate = garmentCuttingInDetail.ModifiedDate,
                        PreparingRemainingQuantity = garmentCuttingInDetail.RemainingQuantity,
                    })
                }),
            });
            return newQuery;
        }

    }
}
