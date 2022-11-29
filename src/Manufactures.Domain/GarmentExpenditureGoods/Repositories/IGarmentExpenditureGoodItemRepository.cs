using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.Repositories
{
    public interface IGarmentExpenditureGoodItemRepository : IAggregateRepository<GarmentExpenditureGoodItem, GarmentExpenditureGoodItemReadModel>
    {
    }
}

