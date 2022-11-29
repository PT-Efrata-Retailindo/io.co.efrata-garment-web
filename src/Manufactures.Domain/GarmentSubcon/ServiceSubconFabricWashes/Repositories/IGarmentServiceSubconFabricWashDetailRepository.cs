using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories
{
    public interface IGarmentServiceSubconFabricWashDetailRepository : IAggregateRepository<GarmentServiceSubconFabricWashDetail, GarmentServiceSubconFabricWashDetailReadModel>
    {
    }
}
