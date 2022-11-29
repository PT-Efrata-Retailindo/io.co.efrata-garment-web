using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetAllGarmentSampleAvalComponents
{
    public class GetAllGarmentSampleAvalComponentsQueryHandler : IQueryHandler<GetAllGarmentSampleAvalComponentsQuery, GarmentSampleAvalComponentsListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleAvalComponentRepository _garmentSampleAvalComponentRepository;
        private readonly IGarmentSampleAvalComponentItemRepository _garmentSampleAvalComponentItemRepository;

        public GetAllGarmentSampleAvalComponentsQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
            _garmentSampleAvalComponentItemRepository = storage.GetRepository<IGarmentSampleAvalComponentItemRepository>();
        }

        public async Task<GarmentSampleAvalComponentsListViewModel> Handle(GetAllGarmentSampleAvalComponentsQuery request, CancellationToken cancellationToken)
        {
            var Query = _garmentSampleAvalComponentRepository.ReadList(request.order, request.keyword, request.filter);

            int total = Query.Count();
            Query = Query.Skip((request.page - 1) * request.size).Take(request.size);

            List<GarmentSampleAvalComponentDto> garmentSampleAvalComponentDtos = _garmentSampleAvalComponentRepository.Find(Query)
                .Select(s => new GarmentSampleAvalComponentDto(s))
                .ToList();

            var garmentSampleAvalComponentIds = garmentSampleAvalComponentDtos.Select(s => s.Id).ToHashSet();

            var itemQuantity = _garmentSampleAvalComponentItemRepository.Query
                .Where(w => garmentSampleAvalComponentIds.Contains(w.SampleAvalComponentId))
                .Select(s => new { s.SampleAvalComponentId, s.Quantity })
                .ToList();

            Parallel.ForEach(garmentSampleAvalComponentDtos, dto =>
            {
                dto.Quantities = itemQuantity.Where(w => w.SampleAvalComponentId == dto.Id).Sum(s => s.Quantity);
            });

            await Task.Yield();
            return new GarmentSampleAvalComponentsListViewModel
            {
                total = total,
                GarmentSampleAvalComponents = garmentSampleAvalComponentDtos
            };
        }
    }
}
