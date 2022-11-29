using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts.Repositories
{
    public interface IGarmentCuttingOutDetailRepository : IAggregateRepository<GarmentCuttingOutDetail, GarmentCuttingOutDetailReadModel>
    {
    }
}
