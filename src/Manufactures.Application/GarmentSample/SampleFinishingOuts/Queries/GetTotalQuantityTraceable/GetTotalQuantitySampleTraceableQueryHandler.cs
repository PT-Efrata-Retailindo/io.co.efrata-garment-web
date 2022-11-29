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
using Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantitySampleTraceable;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;

namespace Manufactures.Application.GarmentFinishingOuts.Queries.GetTotalQuantityTraceable
{
    public class GetTotalQuantitySampleTraceableQueryHandler : IQueryHandler<GetTotalQuantitySampleTraceableQuery, GarmentTotalQtyTraceableListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleFinishingOutRepository garmentSampleFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository garmentSampleFinishingOutItemRepository;
        //private readonly IGarmentFinishingInRepository garmentFinishingInRepository ;
        //private readonly IGarmentFinishingInItemRepository  garmentFinishingInItemRepository;

        public GetTotalQuantitySampleTraceableQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentSampleFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            garmentSampleFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            //garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            //garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
        }

        public async Task<GarmentTotalQtyTraceableListViewModel> Handle(GetTotalQuantitySampleTraceableQuery request, CancellationToken cancellationToken)
        {
            var ro = request.rono.Contains(",") ? request.rono.Split(",").ToList() : new List<string> { request.rono };
            var finishingout = from a in (from aa in garmentSampleFinishingOutRepository.Query
                                          where ro.Contains(aa.RONo)
                                          select aa)
                               join b in garmentSampleFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                               select new 
                               {
                                   a.RONo,
                                   a.FinishingTo,
                                   //d.FinishingInType,
                                   b.Quantity

                               };

            var finishingouts = finishingout.GroupBy(x => new { x.RONo, x.FinishingTo }, (key, group) => new
            {

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
