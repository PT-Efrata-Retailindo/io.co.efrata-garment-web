using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingOuts.Repositories
{
    public class GarmentSewingOutDetailRepository : AggregateRepostory<GarmentSewingOutDetail, GarmentSewingOutDetailReadModel>, IGarmentSewingOutDetailRepository
    {
        protected override GarmentSewingOutDetail Map(GarmentSewingOutDetailReadModel readModel)
        {
            return new GarmentSewingOutDetail(readModel);
        }
    }
}