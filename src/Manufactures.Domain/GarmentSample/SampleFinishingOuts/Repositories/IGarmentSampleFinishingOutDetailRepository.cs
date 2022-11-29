using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories
{
    public interface IGarmentSampleFinishingOutDetailRepository : IAggregateRepository<GarmentSampleFinishingOutDetail, GarmentSampleFinishingOutDetailReadModel>
    {
    }
}
