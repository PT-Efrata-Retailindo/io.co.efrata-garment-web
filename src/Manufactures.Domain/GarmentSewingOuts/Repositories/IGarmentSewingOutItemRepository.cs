using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.Repositories
{
    public interface IGarmentSewingOutItemRepository : IAggregateRepository<GarmentSewingOutItem, GarmentSewingOutItemReadModel>
    {
    }
}
