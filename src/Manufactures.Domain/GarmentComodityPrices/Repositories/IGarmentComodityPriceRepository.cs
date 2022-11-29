using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentComodityPrices.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentComodityPrices.Repositories
{
    public interface IGarmentComodityPriceRepository : IAggregateRepository<GarmentComodityPrice, GarmentComodityPriceReadModel>
    {
        IQueryable<GarmentComodityPriceReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
