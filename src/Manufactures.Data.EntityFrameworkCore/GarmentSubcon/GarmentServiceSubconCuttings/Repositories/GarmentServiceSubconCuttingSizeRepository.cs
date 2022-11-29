using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.ReadModels;
using Manufactures.Domain.GarmentSubcon.ServiceSubconCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Repositories
{
    public class GarmentServiceSubconCuttingSizeRepository : AggregateRepostory<GarmentServiceSubconCuttingSize, GarmentServiceSubconCuttingSizeReadModel>, IGarmentServiceSubconCuttingSizeRepository
    {
        protected override GarmentServiceSubconCuttingSize Map(GarmentServiceSubconCuttingSizeReadModel readModel)
        {
            return new GarmentServiceSubconCuttingSize(readModel);
        }
    }
}