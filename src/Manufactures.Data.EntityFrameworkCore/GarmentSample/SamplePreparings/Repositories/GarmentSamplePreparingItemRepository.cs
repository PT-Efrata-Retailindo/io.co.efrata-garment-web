using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SamplePreparings.Repositories
{
    public class GarmentSamplePreparingItemRepository : AggregateRepostory<GarmentSamplePreparingItem, GarmentSamplePreparingItemReadModel>, IGarmentSamplePreparingItemRepository
    {
        protected override GarmentSamplePreparingItem Map(GarmentSamplePreparingItemReadModel readModel)
        {
            return new GarmentSamplePreparingItem(readModel);
        }
    }
}
