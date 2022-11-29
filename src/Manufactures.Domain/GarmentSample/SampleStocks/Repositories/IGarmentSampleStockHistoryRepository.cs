using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleStocks.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleStocks.Repositories
{
    public interface IGarmentSampleStockHistoryRepository : IAggregateRepository<GarmentSampleStockHistory, GarmentSampleStockHistoryReadModel>
    {
    }
}
