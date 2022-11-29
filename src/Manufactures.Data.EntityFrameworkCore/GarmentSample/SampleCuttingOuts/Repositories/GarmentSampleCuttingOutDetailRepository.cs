using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingOuts.Repositories
{
    public class GarmentSampleCuttingOutDetailRepository : AggregateRepostory<GarmentSampleCuttingOutDetail, GarmentSampleCuttingOutDetailReadModel>, IGarmentSampleCuttingOutDetailRepository
    {
        protected override GarmentSampleCuttingOutDetail Map(GarmentSampleCuttingOutDetailReadModel readModel)
        {
            return new GarmentSampleCuttingOutDetail(readModel);
        }
    }
}
