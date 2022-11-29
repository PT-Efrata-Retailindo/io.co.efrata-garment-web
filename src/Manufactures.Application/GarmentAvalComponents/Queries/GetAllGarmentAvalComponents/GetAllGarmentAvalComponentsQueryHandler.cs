using ExtCore.Data.Abstractions;
using Infrastructure.Data.EntityFrameworkCore.Utilities;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentAvalComponents.ReadModels;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalComponents.Queries.GetAllGarmentAvalComponents
{
    public class GetAllGarmentAvalComponentsQueryHandler : IQueryHandler<GetAllGarmentAvalComponentsQuery, GarmentAvalComponentsListViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentAvalComponentRepository _garmentAvalComponentRepository;
        private readonly IGarmentAvalComponentItemRepository _garmentAvalComponentItemRepository;

        public GetAllGarmentAvalComponentsQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
            _garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
        }

        public async Task<GarmentAvalComponentsListViewModel> Handle(GetAllGarmentAvalComponentsQuery request, CancellationToken cancellationToken)
        {
            var Query = _garmentAvalComponentRepository.ReadList(request.order, request.keyword, request.filter);

            int total = Query.Count();
            Query = Query.Skip((request.page - 1) * request.size).Take(request.size);

            List<GarmentAvalComponentDto> garmentAvalComponentDtos = _garmentAvalComponentRepository.Find(Query)
                .Select(s => new GarmentAvalComponentDto(s))
                .ToList();

            var garmentAvalComponentIds = garmentAvalComponentDtos.Select(s => s.Id).ToHashSet();

            var itemQuantity = _garmentAvalComponentItemRepository.Query
                .Where(w => garmentAvalComponentIds.Contains(w.AvalComponentId))
                .Select(s => new { s.AvalComponentId, s.Quantity })
                .ToList();

            Parallel.ForEach(garmentAvalComponentDtos, dto =>
            {
                dto.Quantities = itemQuantity.Where(w => w.AvalComponentId == dto.Id).Sum(s => s.Quantity);
            });

            await Task.Yield();
            return new GarmentAvalComponentsListViewModel
            {
                total = total,
                GarmentAvalComponents = garmentAvalComponentDtos
            };
        }
    }
}
