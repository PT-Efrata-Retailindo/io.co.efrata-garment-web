using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleStocks;
using Manufactures.Domain.GarmentSample.SampleStocks.ReadModels;
using Manufactures.Domain.GarmentSample.SampleStocks.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleStocks.Repositories
{
    public class GarmentSampleStockHistoryRepository : AggregateRepostory<GarmentSampleStockHistory, GarmentSampleStockHistoryReadModel>, IGarmentSampleStockHistoryRepository
    {
        protected override GarmentSampleStockHistory Map(GarmentSampleStockHistoryReadModel readModel)
        {
            return new GarmentSampleStockHistory(readModel);
        }
    }
}