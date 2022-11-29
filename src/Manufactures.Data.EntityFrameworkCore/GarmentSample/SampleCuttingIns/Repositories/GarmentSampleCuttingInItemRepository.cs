using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingIns.Repositories
{
    public class GarmentSampleCuttingInItemRepository : AggregateRepostory<GarmentSampleCuttingInItem, GarmentSampleCuttingInItemReadModel>, IGarmentSampleCuttingInItemRepository
    {
        protected override GarmentSampleCuttingInItem Map(GarmentSampleCuttingInItemReadModel readModel)
        {
            return new GarmentSampleCuttingInItem(readModel);
        }
    }
}
