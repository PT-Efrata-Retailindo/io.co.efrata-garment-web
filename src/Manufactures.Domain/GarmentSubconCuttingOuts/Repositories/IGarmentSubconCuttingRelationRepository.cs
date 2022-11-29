using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts.ReadModels;

namespace Manufactures.Domain.GarmentSubconCuttingOuts.Repositories
{
    public interface IGarmentSubconCuttingRelationRepository : IAggregateRepository<GarmentSubconCuttingRelation, GarmentSubconCuttingRelationReadModel>
    {
    }
}
