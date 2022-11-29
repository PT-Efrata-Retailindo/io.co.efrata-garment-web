using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.Repositories
{
    public interface IGarmentSubconCuttingRepository : IAggregateRepository<GarmentSubconCutting, GarmentSubconCuttingReadModel>
    {
    }
}
