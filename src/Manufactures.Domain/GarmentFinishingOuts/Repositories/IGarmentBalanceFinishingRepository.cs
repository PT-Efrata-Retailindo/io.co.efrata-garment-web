using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts.Repositories
{
	public interface IGarmentBalanceFinishingRepository : IAggregateRepository<GarmentBalanceFinishing, GarmentBalanceFinishingReadModel>
	{
		IQueryable<GarmentBalanceFinishingReadModel> Read(int page, int size, string order, string keyword, string filter);
	}
}
