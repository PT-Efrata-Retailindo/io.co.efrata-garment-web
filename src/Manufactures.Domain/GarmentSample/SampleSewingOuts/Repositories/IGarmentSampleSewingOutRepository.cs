using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories
{
    public interface IGarmentSampleSewingOutRepository : IAggregateRepository<GarmentSampleSewingOut, GarmentSampleSewingOutReadModel>
    {
        IQueryable<GarmentSampleSewingOutReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSampleSewingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentSampleSewingOutReadModel> model);
        IQueryable ReadDynamic(string order, string search, string select, string keyword, string filter);
    }
}
