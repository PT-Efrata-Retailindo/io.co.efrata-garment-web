using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories
{
    public interface IGarmentServiceSubconCuttingDetailRepository : IAggregateRepository<GarmentServiceSubconCuttingDetail, GarmentServiceSubconCuttingDetailReadModel>
    {
    }
}
