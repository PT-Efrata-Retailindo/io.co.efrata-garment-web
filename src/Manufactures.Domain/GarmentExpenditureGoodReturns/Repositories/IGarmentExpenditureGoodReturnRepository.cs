using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories
{
    public interface IGarmentExpenditureGoodReturnRepository : IAggregateRepository<GarmentExpenditureGoodReturn, GarmentExpenditureGoodReturnReadModel>
    {
        IQueryable<GarmentExpenditureGoodReturnReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
