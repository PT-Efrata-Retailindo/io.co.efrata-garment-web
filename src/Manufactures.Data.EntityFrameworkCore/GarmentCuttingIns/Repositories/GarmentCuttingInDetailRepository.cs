using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingIns.Repositories
{
    public class GarmentCuttingInDetailRepository : AggregateRepostory<GarmentCuttingInDetail, GarmentCuttingInDetailReadModel>, IGarmentCuttingInDetailRepository
    {
        protected override GarmentCuttingInDetail Map(GarmentCuttingInDetailReadModel readModel)
        {
            return new GarmentCuttingInDetail(readModel);
        }
    }
}
