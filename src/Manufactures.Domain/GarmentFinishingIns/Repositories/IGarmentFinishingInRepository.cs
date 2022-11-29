using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingIns.Repositories
{
    public interface IGarmentFinishingInRepository : IAggregateRepository<GarmentFinishingIn, GarmentFinishingInReadModel>
    {
        IQueryable<GarmentFinishingInReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentFinishingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}
