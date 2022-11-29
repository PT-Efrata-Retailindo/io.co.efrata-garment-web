using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Infrastructure.Domain.Queries;
using Manufactures.Application.GarmentCuttingOuts.Queries.GetCuttingOutForTraceable;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.Queries.GetSampleCuttingOutForTraceable
{
    public class GetSampleCuttingOutForTraceableQueryHandler :IQueryHandler<GetSampleCuttingOutForTraceableQuery, GetSampleCuttingOutForTraceableListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentSampleCuttingOutDetailRepository _garmentCuttingOutDetailRepository;

        public GetSampleCuttingOutForTraceableQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSampleCuttingOutDetailRepository>();
        }

        public async Task<GetSampleCuttingOutForTraceableListViewModel> Handle(GetSampleCuttingOutForTraceableQuery request, CancellationToken cancellationToken)
        {
            var cutOut = (from a in _garmentCuttingOutRepository.Query
                          join b in _garmentCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
                          join c in _garmentCuttingOutDetailRepository.Query on b.Identity equals c.CuttingOutItemId
                          where a.Deleted == false && b.Deleted == false && c.Deleted == false
                          && request.ro.Contains(a.RONo)
                          select new
                          {
                              RoNo = a.RONo,
                              Qty = c.CuttingOutQuantity
                          }).GroupBy(x => x.RoNo, (key, group) => new
                          {
                              Rono = key,
                              Qty = group.Sum(x => x.Qty)
                          });

            var selectedData = cutOut.Select(x => new GetSampleCuttingOutForTraceableDto
            {
                RONo = x.Rono,
                TotalCuttingOutQuantity = x.Qty
            }).ToList();

            await Task.Yield();
            return new GetSampleCuttingOutForTraceableListViewModel
            {
                data = selectedData
            };
        }
    }
}
