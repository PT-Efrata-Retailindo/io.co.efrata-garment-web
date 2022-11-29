using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentScrapDestinations.ReadModels;
using System.Linq;

namespace Manufactures.Domain.GarmentScrapDestinations.Repositories
{
    public interface IGarmentScrapDestinationRepository : IAggregateRepository<GarmentScrapDestination, GarmentScrapDestinationReadModel>
	{
		IQueryable<GarmentScrapDestinationReadModel> Read(int page, int size, string order, string keyword, string filter);
	}
}
