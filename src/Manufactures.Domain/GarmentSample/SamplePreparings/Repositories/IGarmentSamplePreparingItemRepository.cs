using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SamplePreparings.Repositories
{
    public interface IGarmentSamplePreparingItemRepository : IAggregateRepository<GarmentSamplePreparingItem, GarmentSamplePreparingItemReadModel>
    {
    }
}
