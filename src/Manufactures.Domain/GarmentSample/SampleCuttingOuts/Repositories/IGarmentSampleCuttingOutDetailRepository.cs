using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories
{
    public interface IGarmentSampleCuttingOutDetailRepository : IAggregateRepository<GarmentSampleCuttingOutDetail, GarmentSampleCuttingOutDetailReadModel>
    {
    }
}
