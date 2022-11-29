using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Repositories
{
    public class GarmentCuttingOutItemRepository : AggregateRepostory<GarmentCuttingOutItem, GarmentCuttingOutItemReadModel>, IGarmentCuttingOutItemRepository
    {
        protected override GarmentCuttingOutItem Map(GarmentCuttingOutItemReadModel readModel)
        {
            return new GarmentCuttingOutItem(readModel);
        }
    }
}