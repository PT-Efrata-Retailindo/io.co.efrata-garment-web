using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories
{
    public interface IGarmentSampleSewingInItemRepository : IAggregateRepository<GarmentSampleSewingInItem, GarmentSampleSewingInItemReadModel>
    {
    }
}
