using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable
{
    public class GetTotalQuantityTraceableQueryHandler : IQueryHandler<GetTotalQuantityTraceableQuery, GarmentTotalQtyTraceableListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
        private readonly IGarmentFinishingInRepository garmentFinishingInRepository ;
        private readonly IGarmentFinishingInItemRepository  garmentFinishingInItemRepository;

        public GetTotalQuantityTraceableQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
        }

        public async Task<GarmentTotalQtyTraceableListViewModel> Handle(GetTotalQuantityTraceableQuery request, CancellationToken cancellationToken)
        {
            var ro = request.rono.Contains(",") ? request.rono.Split(",").ToList() : new List<string> { request.rono };
            var finishingout = from a in (from aa in garmentFinishingOutRepository.Query
                                          where ro.Contains(aa.RONo)
                                          select aa)
                               join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                               join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                               join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                               select new 
                               {
                                   a.RONo,
                                   a.FinishingTo,
                                   d.FinishingInType,
                                   b.Quantity

                               };

            var finishingouts = finishingout.GroupBy(x => new { x.RONo, x.FinishingTo, x.FinishingInType }, (key, group) => new
            {
                FinishingInType = key.FinishingInType,
                FinishingTo = key.FinishingTo,
                RONo = key.RONo,
                TotalQuantity = group.Sum(x => x.Quantity)

            });

            GarmentTotalQtyTraceableListViewModel listViewModel = new GarmentTotalQtyTraceableListViewModel();

            List<GarmentTotalQtyTraceableDto> dto = new List<GarmentTotalQtyTraceableDto>();

            foreach (var i in finishingouts)
            {
                GarmentTotalQtyTraceableDto qtyTraceableDto = new GarmentTotalQtyTraceableDto
                {
                    finishingInType = i.FinishingInType,
                    finishingTo = i.FinishingTo,
                    roJob = i.RONo,
                    totalQty = i.TotalQuantity
                };

                dto.Add(qtyTraceableDto);
            }

            listViewModel.garmentTotalQtyTraceables = dto;

            return listViewModel;
        }

    }
}
