using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories
{
    public interface IGarmentSampleFinishingOutRepository : IAggregateRepository<GarmentSampleFinishingOut, GarmentSampleFinishingOutReadModel>
    {
        IQueryable<GarmentSampleFinishingOutReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<object> ReadExecute(IQueryable<GarmentSampleFinishingOutReadModel> model);
    }
}

