using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using System.Linq;


namespace Manufactures.Domain.GarmentFinishingOuts.Repositories
{
    public interface IGarmentFinishingOutRepository : IAggregateRepository<GarmentFinishingOut, GarmentFinishingOutReadModel>
    {
        IQueryable<GarmentFinishingOutReadModel> Read(int page, int size, string order, string keyword, string filter);
		IQueryable<GarmentFinishingOutReadModel> ReadColor(int page, int size, string order, string keyword, string filter);
        IQueryable<object> ReadExecute(IQueryable<GarmentFinishingOutReadModel> model);
    }
}
