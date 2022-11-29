using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.CustomsOuts;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.CustomsOuts.Repositories
{
    public class GarmentSubconCustomsOutItemRepository : AggregateRepostory<GarmentSubconCustomsOutItem, GarmentSubconCustomsOutItemReadModel>, IGarmentSubconCustomsOutItemRepository
    {
        protected override GarmentSubconCustomsOutItem Map(GarmentSubconCustomsOutItemReadModel readModel)
        {
            return new GarmentSubconCustomsOutItem(readModel);
        }
    }
}
