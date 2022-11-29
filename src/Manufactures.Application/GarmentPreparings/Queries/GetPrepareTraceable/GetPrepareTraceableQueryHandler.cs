using ExtCore.Data.Abstractions;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Infrastructure.Domain.Queries;
using System.Linq;

namespace Manufactures.Application.GarmentPreparings.Queries.GetPrepareTraceable
{
    public class GetPrepareTraceableQueryHandler : IQueryHandler<GetPrepareTraceableQuery, GetPrepareTraceableListViewModel>
    {
        protected readonly IHttpClientService _http;
        private readonly IStorage _storage;

        private readonly IGarmentPreparingRepository garmentPreparingRepository;
        private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;

        public GetPrepareTraceableQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage = storage;
            garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
            garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();


            _http = serviceProvider.GetService<IHttpClientService>();
        }

        public async Task<GetPrepareTraceableListViewModel> Handle(GetPrepareTraceableQuery request, CancellationToken cancellationToken)
        {
            var ro = request.Ro.Contains(",") ? request.Ro.Split(",").ToList() : new List<string> { request.Ro };

            var preparing = (from a in garmentPreparingRepository.Query
                            join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
                            where ro.Contains(a.RONo)
                            select new
                            {
                                //a.UENId,
                                //a.UENNo,
                                a.RONo,
                                //a.Article,
                                //a.IsCuttingIn,
                                //a.CreatedBy,
                                Quantity = b.Quantity - b.RemainingQuantity,
                                
                                b.ProductCode,
                                //b.UENItemId
                            }).GroupBy(x=> new { x.RONo, x.ProductCode },(key,group) => new {
                                RONo = key.RONo,
                                ProductCode = key.ProductCode,
                                Quantity = group.Sum(x=>x.Quantity)

                            });

            GetPrepareTraceableListViewModel getPrepareTraceableListView = new GetPrepareTraceableListViewModel();

            List<GetPrepareTraceableDto> dto = new List<GetPrepareTraceableDto>();

            foreach (var i in preparing)
            {
                GetPrepareTraceableDto qtyTraceableDto = new GetPrepareTraceableDto
                {
                    //Article = i.Article,
                    //CreatedBy = i.CreatedBy,
                    //IsCuttingIn = i.IsCuttingIn,
                    Quantity = i.Quantity,
                    //RemainingQuantity = i.RemainingQuantity,
                    RONo = i.RONo,
                    //UENId = i.UENId,
                    ProductCode = i.ProductCode,
                    //UENItemId = i.UENItemId,
                    //UENNo = i.UENNo
                };

                dto.Add(qtyTraceableDto);
            }

            getPrepareTraceableListView.getPrepareTraceableDtos = dto;

            return getPrepareTraceableListView;
        }
    }
}
