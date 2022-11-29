using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories
{
    public interface IGarmentSampleFinishingInItemRepository : IAggregateRepository<GarmentSampleFinishingInItem, GarmentSampleFinishingInItemReadModel>
    {
    }
}
