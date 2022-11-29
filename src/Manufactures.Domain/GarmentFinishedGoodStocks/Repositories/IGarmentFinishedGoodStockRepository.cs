using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentFinishedGoodStocks.Repositories
{
    public interface IGarmentFinishedGoodStockRepository : IAggregateRepository<GarmentFinishedGoodStock, GarmentFinishedGoodStockReadModel>
    {
        IQueryable<GarmentFinishedGoodStockReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentFinishedGoodStockReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}
