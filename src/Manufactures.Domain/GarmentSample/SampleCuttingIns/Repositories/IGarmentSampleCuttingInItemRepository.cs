using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories
{
    public interface IGarmentSampleCuttingInItemRepository : IAggregateRepository<GarmentSampleCuttingInItem, GarmentSampleCuttingInItemReadModel>
    {
    }
}
