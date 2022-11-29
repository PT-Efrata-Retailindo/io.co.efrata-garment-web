using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapSources.CommandHandler
{
    public class PlaceGarmentScrapSourceCommandHandler : ICommandHandler<PlaceGarmentScrapSourceCommand, GarmentScrapSource>
    {
        private readonly IStorage _storage;

        private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;

        public PlaceGarmentScrapSourceCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
        }

        public async Task<GarmentScrapSource> Handle(PlaceGarmentScrapSourceCommand request, CancellationToken cancellationToken)
        {

            GarmentScrapSource garmentScrapSource = new GarmentScrapSource(
                Guid.NewGuid(),
                request.Code,
                request.Name,
                request.Description
            );


            await _garmentScrapSourceRepository.Update(garmentScrapSource);

            _storage.Save();

            return garmentScrapSource;
        }

    }
}
