using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories
{
    public interface IGarmentSampleDeliveryReturnItemRepository : IAggregateRepository<GarmentSampleDeliveryReturnItem, GarmentSampleDeliveryReturnItemReadModel>
    {
    }
}
