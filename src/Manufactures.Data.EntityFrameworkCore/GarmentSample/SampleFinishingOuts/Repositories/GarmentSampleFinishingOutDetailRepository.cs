using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingOuts.Repositories
{
    public class GarmentSampleFinishingOutDetailRepository : AggregateRepostory<GarmentSampleFinishingOutDetail, GarmentSampleFinishingOutDetailReadModel>, IGarmentSampleFinishingOutDetailRepository
    {
        protected override GarmentSampleFinishingOutDetail Map(GarmentSampleFinishingOutDetailReadModel readModel)
        {
            return new GarmentSampleFinishingOutDetail(readModel);
        }
    }
}
