using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishedGoodStocks.Repositories
{
    public class GarmentFinishedGoodStockHistoryRepository : AggregateRepostory<GarmentFinishedGoodStockHistory, GarmentFinishedGoodStockHistoryReadModel>, IGarmentFinishedGoodStockHistoryRepository
    {
        protected override GarmentFinishedGoodStockHistory Map(GarmentFinishedGoodStockHistoryReadModel readModel)
        {
            return new GarmentFinishedGoodStockHistory(readModel);
        }
    }
}