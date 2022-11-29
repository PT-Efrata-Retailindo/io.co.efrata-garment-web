using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalProducts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.Repositories
{
    public interface IGarmentSampleAvalProductRepository : IAggregateRepository<GarmentSampleAvalProduct, GarmentSampleAvalProductReadModel>
    {
        IQueryable<GarmentSampleAvalProductReadModel> Read(string order, List<string> select, string filter);
    }
}
