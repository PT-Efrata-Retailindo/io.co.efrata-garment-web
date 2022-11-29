using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Repositories
{
    public class GarmentFinishingOutItemRepository : AggregateRepostory<GarmentFinishingOutItem, GarmentFinishingOutItemReadModel>, IGarmentFinishingOutItemRepository
    {
        protected override GarmentFinishingOutItem Map(GarmentFinishingOutItemReadModel readModel)
        {
            return new GarmentFinishingOutItem(readModel);
        }
    }
}