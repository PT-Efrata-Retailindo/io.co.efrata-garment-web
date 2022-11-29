using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;

namespace Manufactures.Domain.GarmentAvalComponents.Repositories
{
    public interface IGarmentAvalComponentItemRepository : IAggregateRepository<GarmentAvalComponentItem, GarmentAvalComponentItemReadModel>
    {
    }
}
