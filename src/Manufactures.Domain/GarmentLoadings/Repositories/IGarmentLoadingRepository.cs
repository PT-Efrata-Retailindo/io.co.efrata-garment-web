using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentLoadings.Repositories
{
    public interface IGarmentLoadingRepository : IAggregateRepository<GarmentLoading, GarmentLoadingReadModel>
    {
        IQueryable<GarmentLoadingReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<object> ReadExecute(IQueryable<GarmentLoadingReadModel> model);
    }
}
