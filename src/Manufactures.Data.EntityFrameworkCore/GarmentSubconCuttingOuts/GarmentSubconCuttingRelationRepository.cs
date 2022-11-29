using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Data.EntityFrameworkCore.GarmentSubconCuttingOuts
{
    public class GarmentSubconCuttingRelationRepository : AggregateRepostory<GarmentSubconCuttingRelation, GarmentSubconCuttingRelationReadModel>, IGarmentSubconCuttingRelationRepository
    {
        protected override GarmentSubconCuttingRelation Map(GarmentSubconCuttingRelationReadModel readModel)
        {
            return new GarmentSubconCuttingRelation(readModel);
        }
    }
}
