using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SamplePreparings.Repositories
{
    public interface IGarmentSamplePreparingRepository : IAggregateRepository<GarmentSamplePreparing, GarmentSamplePreparingReadModel>
    {
        IQueryable<GarmentSamplePreparingReadModel> Read(string order, List<string> select, string filter);
        IQueryable<GarmentSamplePreparingReadModel> ReadOptimized(string order, string filter, string keyword);
        IQueryable<object> ReadExecute(IQueryable<GarmentSamplePreparingReadModel> model, string keyword);
        bool RoChecking(IEnumerable<string> roList, string buyerCode);
    }
}
