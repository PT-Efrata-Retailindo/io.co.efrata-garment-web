using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using System.Linq;

namespace Manufactures.Domain.GarmentAvalComponents.Repositories
{
    public interface IGarmentAvalComponentRepository : IAggregateRepository<GarmentAvalComponent, GarmentAvalComponentReadModel>
    {
        IQueryable<GarmentAvalComponentReadModel> ReadList(string order, string keyword, string filter);
    }
}
