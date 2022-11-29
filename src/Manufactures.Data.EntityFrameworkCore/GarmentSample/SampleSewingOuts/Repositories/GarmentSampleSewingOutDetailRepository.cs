using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingOuts.Repositories
{
    public class GarmentSampleSewingOutDetailRepository : AggregateRepostory<GarmentSampleSewingOutDetail, GarmentSampleSewingOutDetailReadModel>, IGarmentSampleSewingOutDetailRepository
    {
        protected override GarmentSampleSewingOutDetail Map(GarmentSampleSewingOutDetailReadModel readModel)
        {
            return new GarmentSampleSewingOutDetail(readModel);
        }
    }
}
