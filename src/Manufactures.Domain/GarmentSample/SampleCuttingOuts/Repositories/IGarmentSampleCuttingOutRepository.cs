using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories
{
    public interface IGarmentSampleCuttingOutRepository : IAggregateRepository<GarmentSampleCuttingOut, GarmentSampleCuttingOutReadModel>
    {
        IQueryable<GarmentSampleCuttingOutReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentSampleCuttingOutReadModel> query);
    }

}
