using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentCuttingAdjustments;
using Manufactures.Domain.GarmentCuttingAdjustments.ReadModels;
using Manufactures.Domain.GarmentCuttingAdjustments.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingAdjustments.Repositories
{
    public class GarmentCuttingAdjustmentItemRepository : AggregateRepostory<GarmentCuttingAdjustmentItem, GarmentCuttingAdjustmentItemReadModel>, IGarmentCuttingAdjustmentItemRepository
    {
        protected override GarmentCuttingAdjustmentItem Map(GarmentCuttingAdjustmentItemReadModel readModel)
        {
            return new GarmentCuttingAdjustmentItem(readModel);
        }
    }
}
