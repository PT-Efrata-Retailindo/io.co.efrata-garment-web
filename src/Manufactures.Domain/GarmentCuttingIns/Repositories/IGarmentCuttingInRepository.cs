using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentCuttingIns.ReadModels;
using System.Linq;

namespace Manufactures.Domain.GarmentCuttingIns.Repositories
{
    public interface IGarmentCuttingInRepository : IAggregateRepository<GarmentCuttingIn, GarmentCuttingInReadModel>
    {
        IQueryable<GarmentCuttingInReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<object> ReadExecute(IQueryable<GarmentCuttingInReadModel> query);
    }
}
