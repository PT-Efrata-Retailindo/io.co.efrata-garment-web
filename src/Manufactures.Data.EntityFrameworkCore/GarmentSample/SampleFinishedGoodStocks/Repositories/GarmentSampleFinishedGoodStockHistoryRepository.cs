using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishedGoodStocks.Repositories
{
    public class GarmentSampleFinishedGoodStockHistoryRepository : AggregateRepostory<GarmentSampleFinishedGoodStockHistory, GarmentSampleFinishedGoodStockHistoryReadModel>, IGarmentSampleFinishedGoodStockHistoryRepository
    {
        protected override GarmentSampleFinishedGoodStockHistory Map(GarmentSampleFinishedGoodStockHistoryReadModel readModel)
        {
            return new GarmentSampleFinishedGoodStockHistory(readModel);
        }
    }
}
