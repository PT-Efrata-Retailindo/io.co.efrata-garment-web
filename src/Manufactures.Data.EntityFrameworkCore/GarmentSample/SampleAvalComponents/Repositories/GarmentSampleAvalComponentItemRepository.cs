using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalComponents.Repositories
{
    public class GarmentSampleAvalComponentItemRepository : AggregateRepostory<GarmentSampleAvalComponentItem, GarmentSampleAvalComponentItemReadModel>, IGarmentSampleAvalComponentItemRepository
    {
        protected override GarmentSampleAvalComponentItem Map(GarmentSampleAvalComponentItemReadModel readModel)
        {
            return new GarmentSampleAvalComponentItem(readModel);
        }
    }
}
