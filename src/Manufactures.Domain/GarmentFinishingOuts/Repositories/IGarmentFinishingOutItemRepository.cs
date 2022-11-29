using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts.Repositories
{
    public interface IGarmentFinishingOutItemRepository : IAggregateRepository<GarmentFinishingOutItem, GarmentFinishingOutItemReadModel>
    {
    }
}
