using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleStocks.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleStocks.Repositories
{
    public interface IGarmentSampleStockRepository : IAggregateRepository<GarmentSampleStock, GarmentSampleStockReadModel>
    {
        IQueryable<GarmentSampleStockReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSampleStockReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}
