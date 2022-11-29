using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentAdjustments.Repositories
{
    public class GarmentAdjustmentItemRepository : AggregateRepostory<GarmentAdjustmentItem, GarmentAdjustmentItemReadModel>, IGarmentAdjustmentItemRepository
    {
        protected override GarmentAdjustmentItem Map(GarmentAdjustmentItemReadModel readModel)
        {
            return new GarmentAdjustmentItem(readModel);
        }
    }
}