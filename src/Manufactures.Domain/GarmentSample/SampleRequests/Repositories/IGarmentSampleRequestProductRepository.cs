using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.Repositories
{
    public interface IGarmentSampleRequestProductRepository : IAggregateRepository<GarmentSampleRequestProduct, GarmentSampleRequestProductReadModel>
    {
    }
}
