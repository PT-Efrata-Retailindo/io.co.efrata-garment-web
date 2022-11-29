using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentDeliveryReturns;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentDeliveryReturns.Repositories
{
    public class GarmentDeliveryReturnItemRepository : AggregateRepostory<GarmentDeliveryReturnItem, GarmentDeliveryReturnItemReadModel>, IGarmentDeliveryReturnItemRepository
    {
        protected override GarmentDeliveryReturnItem Map(GarmentDeliveryReturnItemReadModel readModel)
        {
            return new GarmentDeliveryReturnItem(readModel);
        }
    }
}