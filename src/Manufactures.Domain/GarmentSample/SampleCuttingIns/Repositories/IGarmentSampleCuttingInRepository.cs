using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories
{
    public interface IGarmentSampleCuttingInRepository : IAggregateRepository<GarmentSampleCuttingIn, GarmentSampleCuttingInReadModel>
    {
        IQueryable<GarmentSampleCuttingInReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<object> ReadExecute(IQueryable<GarmentSampleCuttingInReadModel> query);
    }
}
