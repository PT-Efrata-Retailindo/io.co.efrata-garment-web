using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.ReadModels;
using Manufactures.Domain.GarmentSample.SampleDeliveryReturns.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleDeliveryReturns.Repositories
{
    public class GarmentSampleDeliveryReturnItemRepository : AggregateRepostory<GarmentSampleDeliveryReturnItem, GarmentSampleDeliveryReturnItemReadModel>, IGarmentSampleDeliveryReturnItemRepository
    {
        protected override GarmentSampleDeliveryReturnItem Map(GarmentSampleDeliveryReturnItemReadModel readModel)
        {
            return new GarmentSampleDeliveryReturnItem(readModel);
        }
    }
}
