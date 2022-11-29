using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories
{
    public interface IGarmentSampleAvalComponentItemRepository : IAggregateRepository<GarmentSampleAvalComponentItem, GarmentSampleAvalComponentItemReadModel>
    {
    }
}
