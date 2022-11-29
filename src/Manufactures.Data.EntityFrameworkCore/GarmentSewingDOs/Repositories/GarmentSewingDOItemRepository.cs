using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSewingDOs.Repositories
{
    public class GarmentSewingDOItemRepository : AggregateRepostory<GarmentSewingDOItem, GarmentSewingDOItemReadModel>, IGarmentSewingDOItemRepository
    {
        protected override GarmentSewingDOItem Map(GarmentSewingDOItemReadModel readModel)
        {
            return new GarmentSewingDOItem(readModel);
        }
    }
}