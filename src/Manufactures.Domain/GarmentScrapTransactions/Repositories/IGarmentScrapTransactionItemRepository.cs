using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.Repositories
{
	public interface IGarmentScrapTransactionItemRepository : IAggregateRepository<GarmentScrapTransactionItem, GarmentScrapTransactionItemReadModel>
	{
		IQueryable<GarmentScrapTransactionItemReadModel> Read(int page, int size, string order, string keyword, string filter);
	}
}
