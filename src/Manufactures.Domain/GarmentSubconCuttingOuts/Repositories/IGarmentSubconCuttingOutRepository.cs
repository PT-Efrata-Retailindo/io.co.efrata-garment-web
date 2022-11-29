using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.Repositories
{
    public interface IGarmentSubconCuttingOutRepository : IAggregateRepository<GarmentSubconCuttingOut, GarmentCuttingOutReadModel>
    {
        IQueryable<GarmentCuttingOutReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
