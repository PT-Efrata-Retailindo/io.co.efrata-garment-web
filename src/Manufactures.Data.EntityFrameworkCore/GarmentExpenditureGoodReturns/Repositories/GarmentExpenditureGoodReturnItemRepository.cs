using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoodReturns.Repositories
{
    public class GarmentExpenditureGoodReturnItemRepository : AggregateRepostory<GarmentExpenditureGoodReturnItem, GarmentExpenditureGoodReturnItemReadModel>, IGarmentExpenditureGoodReturnItemRepository
    {
        protected override GarmentExpenditureGoodReturnItem Map(GarmentExpenditureGoodReturnItemReadModel readModel)
        {
            return new GarmentExpenditureGoodReturnItem(readModel);
        }
    }
}
