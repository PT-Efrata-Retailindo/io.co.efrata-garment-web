using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingIns.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSewingIns.Repositories
{
    public interface IGarmentSewingInRepository : IAggregateRepository<GarmentSewingIn, GarmentSewingInReadModel>
    {
        IQueryable<GarmentSewingInReadModel> Read(int page, int size, string order, string keyword, string filter);
        IQueryable<GarmentSewingInReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);
    }
}