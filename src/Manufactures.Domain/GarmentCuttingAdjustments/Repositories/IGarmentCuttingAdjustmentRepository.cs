using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingAdjustments.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingAdjustments.Repositories
{
    public interface IGarmentCuttingAdjustmentRepository : IAggregateRepository<GarmentCuttingAdjustment, GarmentCuttingAdjustmentReadModel>
    {
        IQueryable<GarmentCuttingAdjustmentReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
