using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingIns.Repositories
{
    public class GarmentSampleCuttingInDetailRepository : AggregateRepostory<GarmentSampleCuttingInDetail, GarmentSampleCuttingInDetailReadModel>, IGarmentSampleCuttingInDetailRepository
    {
        protected override GarmentSampleCuttingInDetail Map(GarmentSampleCuttingInDetailReadModel readModel)
        {
            return new GarmentSampleCuttingInDetail(readModel);
        }
    }
}
