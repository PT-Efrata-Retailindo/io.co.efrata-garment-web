using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns.Repositories
{
    public interface IGarmentFinishingInItemRepository : IAggregateRepository<GarmentFinishingInItem, GarmentFinishingInItemReadModel>
    {
    }
}
