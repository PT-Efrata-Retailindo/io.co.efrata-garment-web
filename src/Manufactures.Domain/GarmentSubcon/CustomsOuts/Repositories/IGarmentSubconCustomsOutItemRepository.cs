using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories
{
    public interface IGarmentSubconCustomsOutItemRepository : IAggregateRepository<GarmentSubconCustomsOutItem, GarmentSubconCustomsOutItemReadModel>
    {
    }
}
