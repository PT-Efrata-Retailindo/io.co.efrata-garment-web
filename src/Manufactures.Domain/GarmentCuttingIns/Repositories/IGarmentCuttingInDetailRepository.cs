using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingIns.Repositories
{
    public interface IGarmentCuttingInDetailRepository : IAggregateRepository<GarmentCuttingInDetail, GarmentCuttingInDetailReadModel>
    {
    }
}
