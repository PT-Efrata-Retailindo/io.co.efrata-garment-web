using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconFabricWashes.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconFabricWashes.Repositories
{
    public class GarmentServiceSubconFabricWashDetailRepository : AggregateRepostory<GarmentServiceSubconFabricWashDetail, GarmentServiceSubconFabricWashDetailReadModel>, IGarmentServiceSubconFabricWashDetailRepository
    {
        protected override GarmentServiceSubconFabricWashDetail Map(GarmentServiceSubconFabricWashDetailReadModel readModel)
        {
            return new GarmentServiceSubconFabricWashDetail(readModel);
        }
    }
}
