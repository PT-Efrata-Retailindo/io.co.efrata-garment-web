using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories
{
    public interface IGarmentSampleDeliveryReturnRepository : IAggregateRepository<GarmentSampleDeliveryReturn, GarmentSampleDeliveryReturnReadModel>
    {
        IQueryable<GarmentSampleDeliveryReturnReadModel> Read(string order, List<string> select, string filter);
    }
}
