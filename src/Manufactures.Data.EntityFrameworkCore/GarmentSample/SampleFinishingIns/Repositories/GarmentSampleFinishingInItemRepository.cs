using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingIns.Repositories
{
    public class GarmentSampleFinishingInItemRepository : AggregateRepostory<GarmentSampleFinishingInItem, GarmentSampleFinishingInItemReadModel>, IGarmentSampleFinishingInItemRepository
    {
        protected override GarmentSampleFinishingInItem Map(GarmentSampleFinishingInItemReadModel readModel)
        {
            return new GarmentSampleFinishingInItem(readModel);
        }
    }
}
