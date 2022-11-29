using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleRequests.Repositories
{
    public class GarmentSampleRequestProductRepository : AggregateRepostory<GarmentSampleRequestProduct, GarmentSampleRequestProductReadModel>, IGarmentSampleRequestProductRepository
    {
        protected override GarmentSampleRequestProduct Map(GarmentSampleRequestProductReadModel readModel)
        {
            return new GarmentSampleRequestProduct(readModel);
        }
    }
}