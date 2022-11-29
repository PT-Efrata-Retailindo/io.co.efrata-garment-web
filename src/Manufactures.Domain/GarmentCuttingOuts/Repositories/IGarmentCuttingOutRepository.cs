using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.Repositories
{
    public interface IGarmentCuttingOutRepository : IAggregateRepository<GarmentCuttingOut, GarmentCuttingOutReadModel>
    {
        IQueryable<GarmentCuttingOutReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentCuttingOutReadModel> query);
    }
    
}
