using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;


namespace Manufactures.Domain.GarmentSewingDOs.Repositories
{
    public interface IGarmentSewingDOItemRepository : IAggregateRepository<GarmentSewingDOItem, GarmentSewingDOItemReadModel>
    {
    }
}