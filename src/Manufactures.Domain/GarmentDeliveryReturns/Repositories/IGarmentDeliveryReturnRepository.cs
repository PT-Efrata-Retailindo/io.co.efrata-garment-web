using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentDeliveryReturns.Repositories
{
    public interface IGarmentDeliveryReturnRepository : IAggregateRepository<GarmentDeliveryReturn, GarmentDeliveryReturnReadModel>
    {
        IQueryable<GarmentDeliveryReturnReadModel> Read(string order, List<string> select, string filter);
    }
}