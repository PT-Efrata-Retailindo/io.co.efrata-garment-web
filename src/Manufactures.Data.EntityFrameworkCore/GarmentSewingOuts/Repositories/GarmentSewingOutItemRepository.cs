using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingOuts.Repositories
{
    public class GarmentSewingOutItemRepository : AggregateRepostory<GarmentSewingOutItem, GarmentSewingOutItemReadModel>, IGarmentSewingOutItemRepository
    {
        protected override GarmentSewingOutItem Map(GarmentSewingOutItemReadModel readModel)
        {
            return new GarmentSewingOutItem(readModel);
        }
    }
}