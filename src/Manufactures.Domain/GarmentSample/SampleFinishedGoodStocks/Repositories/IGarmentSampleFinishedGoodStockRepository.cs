using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories
{
    public interface IGarmentSampleFinishedGoodStockRepository : IAggregateRepository<GarmentSampleFinishedGoodStock, GarmentSampleFinishedGoodStockReadModel>
    {
        IQueryable<GarmentSampleFinishedGoodStockReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSampleFinishedGoodStockReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}

