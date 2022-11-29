using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using Manufactures.Domain.GarmentLoadings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentLoadings.Repositories
{
    public class GarmentLoadingItemRepository : AggregateRepostory<GarmentLoadingItem, GarmentLoadingItemReadModel>, IGarmentLoadingItemRepository
    {
        protected override GarmentLoadingItem Map(GarmentLoadingItemReadModel readModel)
        {
            return new GarmentLoadingItem(readModel);
        }
    }
}
