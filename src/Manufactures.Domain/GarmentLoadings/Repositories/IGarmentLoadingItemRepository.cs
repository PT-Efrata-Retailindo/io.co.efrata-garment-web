using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentLoadings.Repositories
{
    public interface IGarmentLoadingItemRepository : IAggregateRepository<GarmentLoadingItem, GarmentLoadingItemReadModel>
    {
    }
}
