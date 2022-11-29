using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories
{
    public interface IGarmentSampleSewingOutItemRepository : IAggregateRepository<GarmentSampleSewingOutItem, GarmentSampleSewingOutItemReadModel>
    {
    }
}
