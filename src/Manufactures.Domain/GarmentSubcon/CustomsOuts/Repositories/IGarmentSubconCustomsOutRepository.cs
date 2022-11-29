using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.CustomsOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.CustomsOuts.Repositories
{
    public interface IGarmentSubconCustomsOutRepository : IAggregateRepository<GarmentSubconCustomsOut, GarmentSubconCustomsOutReadModel>
    {
        IQueryable<GarmentSubconCustomsOutReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
