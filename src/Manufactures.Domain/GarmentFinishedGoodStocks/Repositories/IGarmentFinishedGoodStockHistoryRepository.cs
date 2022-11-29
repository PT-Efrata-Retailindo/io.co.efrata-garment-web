using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishedGoodStocks.Repositories
{
    public interface IGarmentFinishedGoodStockHistoryRepository : IAggregateRepository<GarmentFinishedGoodStockHistory, GarmentFinishedGoodStockHistoryReadModel>
    {
    }
}
