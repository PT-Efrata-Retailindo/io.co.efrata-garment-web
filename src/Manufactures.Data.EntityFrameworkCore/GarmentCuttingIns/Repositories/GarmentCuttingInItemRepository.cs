using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingIns.Repositories
{
    public class GarmentCuttingInItemRepository : AggregateRepostory<GarmentCuttingInItem, GarmentCuttingInItemReadModel>, IGarmentCuttingInItemRepository
    {
        protected override GarmentCuttingInItem Map(GarmentCuttingInItemReadModel readModel)
        {
            return new GarmentCuttingInItem(readModel);
        }
    }
}
