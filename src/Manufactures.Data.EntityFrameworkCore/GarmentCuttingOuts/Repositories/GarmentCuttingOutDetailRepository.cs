using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Repositories
{
    public class GarmentCuttingOutDetailRepository : AggregateRepostory<GarmentCuttingOutDetail, GarmentCuttingOutDetailReadModel>, IGarmentCuttingOutDetailRepository
    {
        protected override GarmentCuttingOutDetail Map(GarmentCuttingOutDetailReadModel readModel)
        {
            return new GarmentCuttingOutDetail(readModel);
        }
    }
}