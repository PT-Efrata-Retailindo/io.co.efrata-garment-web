using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingOuts.Repositories
{
    public class GarmentSampleCuttingOutItemRepository : AggregateRepostory<GarmentSampleCuttingOutItem, GarmentSampleCuttingOutItemReadModel>, IGarmentSampleCuttingOutItemRepository
    {
        protected override GarmentSampleCuttingOutItem Map(GarmentSampleCuttingOutItemReadModel readModel)
        {
            return new GarmentSampleCuttingOutItem(readModel);
        }
    }
}
