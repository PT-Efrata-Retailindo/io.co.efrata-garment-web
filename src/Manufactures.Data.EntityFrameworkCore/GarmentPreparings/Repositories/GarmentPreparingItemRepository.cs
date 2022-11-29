using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentPreparings.Repositories
{
    public class GarmentPreparingItemRepository : AggregateRepostory<GarmentPreparingItem, GarmentPreparingItemReadModel>, IGarmentPreparingItemRepository
    {
        protected override GarmentPreparingItem Map(GarmentPreparingItemReadModel readModel)
        {
            return new GarmentPreparingItem(readModel);
        }
    }
}