using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingOutDetailRepository : AggregateRepostory<GarmentSubconCuttingOutDetail, GarmentCuttingOutDetailReadModel>, IGarmentSubconCuttingOutDetailRepository
    {
        protected override GarmentSubconCuttingOutDetail Map(GarmentCuttingOutDetailReadModel readModel)
        {
            return new GarmentSubconCuttingOutDetail(readModel);
        }
    }
}
