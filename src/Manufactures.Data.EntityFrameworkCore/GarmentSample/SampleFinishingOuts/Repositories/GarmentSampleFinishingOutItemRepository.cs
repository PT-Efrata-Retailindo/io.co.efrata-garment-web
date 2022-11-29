using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingOuts.Repositories
{
    public class GarmentSampleFinishingOutItemRepository : AggregateRepostory<GarmentSampleFinishingOutItem, GarmentSampleFinishingOutItemReadModel>, IGarmentSampleFinishingOutItemRepository
    {
        protected override GarmentSampleFinishingOutItem Map(GarmentSampleFinishingOutItemReadModel readModel)
        {
            return new GarmentSampleFinishingOutItem(readModel);
        }
    }
}
