using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories
{
    public interface IGarmentServiceSubconCuttingItemRepository : IAggregateRepository<GarmentServiceSubconCuttingItem, GarmentServiceSubconCuttingItemReadModel>
    {

        IQueryable<GarmentServiceSubconCuttingItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
