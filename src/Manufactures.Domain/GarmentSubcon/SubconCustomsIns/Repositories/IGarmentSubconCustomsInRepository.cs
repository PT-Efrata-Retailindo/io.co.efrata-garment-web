using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.SubconCustomsIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSubcon.SubconCustomsIns.Repositories
{
    public interface IGarmentSubconCustomsInRepository : IAggregateRepository<GarmentSubconCustomsIn, GarmentSubconCustomsInReadModel>
    {
        IQueryable<GarmentSubconCustomsInReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
