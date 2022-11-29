using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSample.SampleRequests;
using Manufactures.Domain.GarmentSample.SampleRequests.ReadModels;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleRequests.Repositories
{
    public class GarmentSampleRequestSpecificationRepository : AggregateRepostory<GarmentSampleRequestSpecification, GarmentSampleRequestSpecificationReadModel>, IGarmentSampleRequestSpecificationRepository
    {
        protected override GarmentSampleRequestSpecification Map(GarmentSampleRequestSpecificationReadModel readModel)
        {
            return new GarmentSampleRequestSpecification(readModel);
        }
    }
}