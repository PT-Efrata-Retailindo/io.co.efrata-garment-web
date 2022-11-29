using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Queries;
using Manufactures.Domain.GarmentAvalComponents.Queries.GetGarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Repositories;

namespace Manufactures.Application.GarmentAvalComponents.Queries.GetGarmentAvalComponents
{
    public class GetGarmentAvalComponentsQueryHandler : IQueryHandler<GetGarmentAvalComponentQuery, GarmentAvalComponentViewModel>
    {
        private readonly IStorage _storage;
        private readonly IGarmentAvalComponentRepository _garmentAvalComponentRepository;
        private readonly IGarmentAvalComponentItemRepository _garmentAvalComponentItemRepository;

        public GetGarmentAvalComponentsQueryHandler(IStorage storage)
        {
            _storage = storage;
            _garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
            _garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
        }

        public async Task<GarmentAvalComponentViewModel> Handle(GetGarmentAvalComponentQuery request, CancellationToken cancellationToken)
        {
            var data = _garmentAvalComponentRepository.Find(f => f.Identity == request.Identity)
                .Select(s => new GarmentAvalComponentViewModel(s))
                .Single();

            data.Items = _garmentAvalComponentItemRepository.Find(f => f.AvalComponentId == request.Identity)
                .OrderBy(o => o.SizeName)
                .Select(s => new GarmentAvalComponentItemDto(s))
                .ToList();

            await Task.Yield();
            return data;
        }
    }
}
