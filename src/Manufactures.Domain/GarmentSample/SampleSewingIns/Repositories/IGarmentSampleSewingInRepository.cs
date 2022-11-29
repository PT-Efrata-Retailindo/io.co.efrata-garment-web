using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories
{
    public interface IGarmentSampleSewingInRepository : IAggregateRepository<GarmentSampleSewingIn, GarmentSampleSewingInReadModel>
    {
        IQueryable<GarmentSampleSewingInReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<GarmentSampleSewingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }

}
