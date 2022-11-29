using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingIns.Repositories
{
    public class GarmentFinishingInItemRepository : AggregateRepostory<GarmentFinishingInItem, GarmentFinishingInItemReadModel>, IGarmentFinishingInItemRepository
    {
        protected override GarmentFinishingInItem Map(GarmentFinishingInItemReadModel readModel)
        {
            return new GarmentFinishingInItem(readModel);
        }
    }
}
