using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Repositories
{
    public class GarmentFinishingOutDetailRepository : AggregateRepostory<GarmentFinishingOutDetail, GarmentFinishingOutDetailReadModel>, IGarmentFinishingOutDetailRepository
    {
        protected override GarmentFinishingOutDetail Map(GarmentFinishingOutDetailReadModel readModel)
        {
            return new GarmentFinishingOutDetail(readModel);
        }
    }
}
