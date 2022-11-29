using Infrastructure.Domain.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories
{
    public interface IGarmentSampleExpenditureGoodRepository : IAggregateRepository<GarmentSampleExpenditureGood, GarmentSampleExpenditureGoodReadModel>
    {
        IQueryable<GarmentSampleExpenditureGoodReadModel> Read(int page, int size, string order, string keyword, string filter);

        IQueryable<GarmentSampleExpenditureGoodReadModel> ReadComplete(int page, int size, string order, string keyword, string filter);

        double BasicPriceByRO(string Keyword = null, string Filter = "{}");
        IQueryable<object> ReadExecute(IQueryable<GarmentSampleExpenditureGoodReadModel> query);

        IQueryable<GarmentSampleExpenditureGoodReadModel> ReadignoreFilter(int page, int size, string order, string keyword, string filter);
    }

}
