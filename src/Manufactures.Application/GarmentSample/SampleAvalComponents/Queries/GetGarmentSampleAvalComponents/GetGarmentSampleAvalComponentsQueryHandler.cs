using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.Queries.GetGarmentSampleAvalComponents
{
    public class GetGarmentSampleAvalComponentsQueryHandler : IQueryHandler<GetGarmentSampleAvalComponentQuery, GarmentSampleAvalComponentViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleAvalComponentRepository _garmentSampleAvalComponentRepository;
        private readonly IGarmentSampleAvalComponentItemRepository _garmentSampleAvalComponentItemRepository;

        public GetGarmentSampleAvalComponentsQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
            _garmentSampleAvalComponentItemRepository = storage.GetRepository<IGarmentSampleAvalComponentItemRepository>();
        }

        public async Task<GarmentSampleAvalComponentViewModel> Handle(GetGarmentSampleAvalComponentQuery request, CancellationToken cancellationToken)
        {
            var data = _garmentSampleAvalComponentRepository.Find(f => f.Identity == request.Identity)
                .Select(s => new GarmentSampleAvalComponentViewModel(s))
                .Single();

            data.Items = _garmentSampleAvalComponentItemRepository.Find(f => f.SampleAvalComponentId == request.Identity)
                .OrderBy(o => o.SizeName)
                .Select(s => new GarmentSampleAvalComponentItemDto(s))
                .ToList();

            await Task.Yield();
            return data;
        }
    }
}
