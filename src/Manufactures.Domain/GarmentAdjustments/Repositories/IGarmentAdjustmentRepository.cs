using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentAdjustments.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentAdjustments.Repositories
{
    public interface IGarmentAdjustmentRepository : IAggregateRepository<GarmentAdjustment, GarmentAdjustmentReadModel>
    {
        IQueryable<GarmentAdjustmentReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
