using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.Repositories
{
	public interface IGarmentScrapStockRepository : IAggregateRepository<GarmentScrapStock, GarmentScrapStockReadModel>
	{
		IQueryable<GarmentScrapStockReadModel> Read(int page, int size, string order, string keyword, string filter);
	}
}
