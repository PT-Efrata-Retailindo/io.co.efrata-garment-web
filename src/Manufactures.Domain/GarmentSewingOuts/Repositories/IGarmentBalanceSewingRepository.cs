using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.Repositories
{
	public interface IGarmentBalanceSewingRepository : IAggregateRepository<GarmentBalanceSewing, GarmentBalanceSewingReadModel>
	{
		IQueryable<GarmentBalanceSewingReadModel> Read(int page, int size, string order, string keyword, string filter);
	}
}
