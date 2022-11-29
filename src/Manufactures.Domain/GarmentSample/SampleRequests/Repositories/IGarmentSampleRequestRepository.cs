using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleRequests.Repositories
{
    public interface IGarmentSampleRequestRepository : IAggregateRepository<GarmentSampleRequest, GarmentSampleRequestReadModel>
    {
        IQueryable<GarmentSampleRequestReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
