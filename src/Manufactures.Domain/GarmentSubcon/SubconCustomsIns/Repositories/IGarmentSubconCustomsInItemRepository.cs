using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories
{
    public interface IGarmentSubconCustomsInItemRepository : IAggregateRepository<GarmentSubconCustomsInItem, GarmentSubconCustomsInItemReadModel>
    {
    }
}
