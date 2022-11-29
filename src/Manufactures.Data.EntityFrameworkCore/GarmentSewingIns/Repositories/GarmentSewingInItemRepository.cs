using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingIns.Repositories
{
    public class GarmentSewingInItemRepository : AggregateRepostory<GarmentSewingInItem, GarmentSewingInItemReadModel>, IGarmentSewingInItemRepository
    {
        protected override GarmentSewingInItem Map(GarmentSewingInItemReadModel readModel)
        {
            return new GarmentSewingInItem(readModel);
        }
    }
}