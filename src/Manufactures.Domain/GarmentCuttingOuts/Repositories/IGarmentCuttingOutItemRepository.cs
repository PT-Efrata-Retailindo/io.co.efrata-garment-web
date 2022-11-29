using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.Repositories
{
    public interface IGarmentCuttingOutItemRepository : IAggregateRepository<GarmentCuttingOutItem, GarmentCuttingOutItemReadModel>
    {
    }
}
