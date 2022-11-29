using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleExpenditureGoods.Repositories
{
    public class GarmentSampleExpenditureGoodItemRepository : AggregateRepostory<GarmentSampleExpenditureGoodItem, GarmentSampleExpenditureGoodItemReadModel>, IGarmentSampleExpenditureGoodItemRepository
    {
        protected override GarmentSampleExpenditureGoodItem Map(GarmentSampleExpenditureGoodItemReadModel readModel)
        {
            return new GarmentSampleExpenditureGoodItem(readModel);
        }
    }
}
