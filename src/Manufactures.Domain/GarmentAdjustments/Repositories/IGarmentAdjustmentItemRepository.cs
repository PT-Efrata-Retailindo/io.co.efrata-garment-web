using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAdjustments.Repositories
{
    public interface IGarmentAdjustmentItemRepository : IAggregateRepository<GarmentAdjustmentItem, GarmentAdjustmentItemReadModel>
    {
    }
}
