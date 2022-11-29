using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories
{
    public interface IGarmentSampleAvalProductItemRepository : IAggregateRepository<GarmentSampleAvalProductItem, GarmentSampleAvalProductItemReadModel>
    {
    }
}
