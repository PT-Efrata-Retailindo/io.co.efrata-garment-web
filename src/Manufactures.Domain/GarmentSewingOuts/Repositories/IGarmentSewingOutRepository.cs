using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.Repositories
{
    public interface IGarmentSewingOutRepository : IAggregateRepository<GarmentSewingOut, GarmentSewingOutReadModel>
    {
        IQueryable<GarmentSewingOutReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSewingOutReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentSewingOutReadModel> model);
        IQueryable ReadDynamic(string order, string search, string select, string keyword, string filter);
    }
}
