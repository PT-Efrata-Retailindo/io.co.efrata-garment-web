using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.Repositories
{
	public interface IGarmentExpenditureGoodInvoiceRelationRepository : IAggregateRepository<GarmentExpenditureGoodInvoiceRelation, GarmentExpenditureGoodInvoiceRelationReadModel>
	{
		IQueryable<GarmentExpenditureGoodInvoiceRelationReadModel> Read(int page, int size, string order, string keyword, string filter);

	}
}
