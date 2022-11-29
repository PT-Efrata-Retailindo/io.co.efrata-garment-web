using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.Repositories
{
    public interface IGarmentAvalProductItemRepository : IAggregateRepository<GarmentAvalProductItem, GarmentAvalProductItemReadModel>
    {
    }
}