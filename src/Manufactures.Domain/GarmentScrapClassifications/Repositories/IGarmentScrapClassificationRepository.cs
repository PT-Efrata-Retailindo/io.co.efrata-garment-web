using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentScrapClassifications.ReadModels;
using System.Collections.Generic;
using System.Linq;


namespace Manufactures.Domain.GarmentScrapClassifications.Repositories
{
	public interface IGarmentScrapClassificationRepository : IAggregateRepository<GarmentScrapClassification, GarmentScrapClassificationReadModel>
	{
		
		IQueryable<GarmentScrapClassificationReadModel> Read(int page, int size, string order, string keyword, string filter);
	}
}
