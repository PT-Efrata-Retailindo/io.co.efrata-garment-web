using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories
{
    public interface IGarmentServiceSubconFabricWashItemRepository : IAggregateRepository<GarmentServiceSubconFabricWashItem, GarmentServiceSubconFabricWashItemReadModel>
    {
        IQueryable<GarmentServiceSubconFabricWashItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
