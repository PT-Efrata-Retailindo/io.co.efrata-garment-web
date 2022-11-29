using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories
{
    public interface IGarmentSampleAvalComponentRepository : IAggregateRepository<GarmentSampleAvalComponent, GarmentSampleAvalComponentReadModel>
    {
        IQueryable<GarmentSampleAvalComponentReadModel> ReadList(string order, string keyword, string filter);
    }
}
