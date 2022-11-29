using System;
using System.Linq;
using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.ReadModels;

namespace Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories
{
    public interface IGarmentServiceSubconSewingItemRepository : IAggregateRepository<GarmentServiceSubconSewingItem, GarmentServiceSubconSewingItemReadModel>
    {
        IQueryable<GarmentServiceSubconSewingItemReadModel> ReadItem(int page, int size, string order, string keyword, string filter);
    }
}
