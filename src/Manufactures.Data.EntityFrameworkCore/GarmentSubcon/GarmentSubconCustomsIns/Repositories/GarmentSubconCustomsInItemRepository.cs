using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconCustomsIns.Repositories
{
    public class GarmentSubconCustomsInItemRepository : AggregateRepostory<GarmentSubconCustomsInItem, GarmentSubconCustomsInItemReadModel>, IGarmentSubconCustomsInItemRepository
    {
        protected override GarmentSubconCustomsInItem Map(GarmentSubconCustomsInItemReadModel readModel)
        {
            return new GarmentSubconCustomsInItem(readModel);
        }
    }
}
