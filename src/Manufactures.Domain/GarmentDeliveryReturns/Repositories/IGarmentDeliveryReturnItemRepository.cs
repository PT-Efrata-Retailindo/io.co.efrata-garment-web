using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentDeliveryReturns.Repositories
{
    public interface IGarmentDeliveryReturnItemRepository : IAggregateRepository<GarmentDeliveryReturnItem, GarmentDeliveryReturnItemReadModel>
    {
    }
}