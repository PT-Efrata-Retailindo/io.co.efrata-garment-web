using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Infrastructure.Domain.Queries;

namespace Manufactures.Application.GarmentCuttingOuts.Queries.GetCuttingOutForTraceable
{
    public class GetCuttingOutForTraceableQueryHandler : IQueryHandler<GetCuttingOutForTraceableQuery, GetCuttingOutForTraceableListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;

        public GetCuttingOutForTraceableQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
        }

        public async Task<GetCuttingOutForTraceableListViewModel> Handle(GetCuttingOutForTraceableQuery request, CancellationToken cancellationToken)
        {
            var cutOut = (from a in _garmentCuttingOutRepository.Query
                          join b in _garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                          join c in _garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
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

            var selectedData = cutOut.Select(x => new GetCuttingOutForTraceableDto
            {
                RONo = x.Rono,
                TotalCuttingOutQuantity = x.Qty
            }).ToList();

            await Task.Yield();
            return new GetCuttingOutForTraceableListViewModel
            {
                data = selectedData
            };
        }
    }
}
