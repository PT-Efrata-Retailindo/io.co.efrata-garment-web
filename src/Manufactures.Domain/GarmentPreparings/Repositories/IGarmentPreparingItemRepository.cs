using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings.Repositories
{
    public interface IGarmentPreparingItemRepository : IAggregateRepository<GarmentPreparingItem, GarmentPreparingItemReadModel>
    {
    }
}