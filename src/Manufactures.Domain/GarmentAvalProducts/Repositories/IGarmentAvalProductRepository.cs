using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.Repositories
{
    public interface IGarmentAvalProductRepository : IAggregateRepository<GarmentAvalProduct, GarmentAvalProductReadModel>
    {
        IQueryable<GarmentAvalProductReadModel> Read(string order, List<string> select, string filter);
    }
}