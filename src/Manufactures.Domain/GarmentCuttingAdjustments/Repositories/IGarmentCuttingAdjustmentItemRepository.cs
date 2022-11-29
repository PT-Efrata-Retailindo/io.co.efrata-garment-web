using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingAdjustments.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingAdjustments.Repositories
{
    public interface IGarmentCuttingAdjustmentItemRepository : IAggregateRepository<GarmentCuttingAdjustmentItem, GarmentCuttingAdjustmentItemReadModel>
    {
    }
}
