using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingDOs.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSewingDOs.Repositories
{
    public interface IGarmentSewingDORepository : IAggregateRepository<GarmentSewingDO, GarmentSewingDOReadModel>
    {
        IQueryable<GarmentSewingDOReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentSewingDOReadModel> model);

    }
}