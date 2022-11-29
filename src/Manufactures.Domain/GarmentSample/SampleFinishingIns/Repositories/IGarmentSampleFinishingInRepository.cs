using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories
{
    public interface IGarmentSampleFinishingInRepository : IAggregateRepository<GarmentSampleFinishingIn, GarmentSampleFinishingInReadModel>
    {
        IQueryable<GarmentSampleFinishingInReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSampleFinishingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}
