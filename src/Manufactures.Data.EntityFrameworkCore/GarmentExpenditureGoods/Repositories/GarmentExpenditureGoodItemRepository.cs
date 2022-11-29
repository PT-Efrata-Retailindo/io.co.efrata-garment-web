using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoods.Repositories
{
    public class GarmentExpenditureGoodItemRepository : AggregateRepostory<GarmentExpenditureGoodItem, GarmentExpenditureGoodItemReadModel>, IGarmentExpenditureGoodItemRepository
    {
        protected override GarmentExpenditureGoodItem Map(GarmentExpenditureGoodItemReadModel readModel)
        {
            return new GarmentExpenditureGoodItem(readModel);
        }
    }
}
