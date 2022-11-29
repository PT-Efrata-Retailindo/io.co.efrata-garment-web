using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingIns.Repositories
{
    public interface IGarmentSewingInItemRepository : IAggregateRepository<GarmentSewingInItem, GarmentSewingInItemReadModel>
    {
    }
}