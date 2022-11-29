using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories
{
    public interface IGarmentExpenditureGoodReturnItemRepository : IAggregateRepository<GarmentExpenditureGoodReturnItem, GarmentExpenditureGoodReturnItemReadModel>
    {
    }
}
