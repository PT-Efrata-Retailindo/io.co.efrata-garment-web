using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingIns.Repositories
{
    public class GarmentSampleSewingInItemRepository : AggregateRepostory<GarmentSampleSewingInItem, GarmentSampleSewingInItemReadModel>, IGarmentSampleSewingInItemRepository
    {
        protected override GarmentSampleSewingInItem Map(GarmentSampleSewingInItemReadModel readModel)
        {
            return new GarmentSampleSewingInItem(readModel);
        }
    }
}
