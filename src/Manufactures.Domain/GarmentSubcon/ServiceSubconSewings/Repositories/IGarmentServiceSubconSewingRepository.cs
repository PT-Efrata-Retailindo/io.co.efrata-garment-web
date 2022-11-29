using System;
using System.Linq;
using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories
{
    public interface IGarmentServiceSubconSewingRepository : IAggregateRepository<GarmentServiceSubconSewing, GarmentServiceSubconSewingReadModel>
    {
        IQueryable<GarmentServiceSubconSewingReadModel> Read(int page, int size, string order, string keyword, string filter);
    }
}
