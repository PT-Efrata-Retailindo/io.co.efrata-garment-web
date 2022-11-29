using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingOutItemRepository : AggregateRepostory<GarmentSubconCuttingOutItem, GarmentCuttingOutItemReadModel>, IGarmentSubconCuttingOutItemRepository
    {
        protected override GarmentSubconCuttingOutItem Map(GarmentCuttingOutItemReadModel readModel)
        {
            return new GarmentSubconCuttingOutItem(readModel);
        }
    }
}
