using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.Repositories
{
    public interface IGarmentExpenditureGoodRepository : IAggregateRepository<GarmentExpenditureGood, GarmentExpenditureGoodReadModel>
    {
        IQueryable<GarmentExpenditureGoodReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentExpenditureGoodReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);

        double BasicPriceByRO(string Keyword = null, string Filter = "{}");
        IQueryable<object> ReadExecute(IQueryable<GarmentExpenditureGoodReadModel> query);

        IQueryable<GarmentExpenditureGoodReadModel> ReadignoreFilter(int page, int size, string order, string keyword, string filter);
    }

}
