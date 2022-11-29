using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories
{
    public interface IGarmentSampleFinishedGoodStockHistoryRepository : IAggregateRepository<GarmentSampleFinishedGoodStockHistory, GarmentSampleFinishedGoodStockHistoryReadModel>
    {
    }
}
