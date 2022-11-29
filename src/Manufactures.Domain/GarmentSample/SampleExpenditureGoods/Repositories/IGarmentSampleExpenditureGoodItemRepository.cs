using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories
{
    public interface IGarmentSampleExpenditureGoodItemRepository : IAggregateRepository<GarmentSampleExpenditureGoodItem, GarmentSampleExpenditureGoodItemReadModel>
    {
    }
}
