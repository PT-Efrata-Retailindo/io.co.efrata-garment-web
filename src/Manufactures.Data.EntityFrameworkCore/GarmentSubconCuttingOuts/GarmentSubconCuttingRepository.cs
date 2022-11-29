using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingRepository : AggregateRepostory<GarmentSubconCutting, GarmentSubconCuttingReadModel>, IGarmentSubconCuttingRepository
    {
        protected override GarmentSubconCutting Map(GarmentSubconCuttingReadModel readModel)
        {
            return new GarmentSubconCutting(readModel);
        }
    }
}
