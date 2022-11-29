using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapDestinations;
using Manufactures.Domain.GarmentScrapDestinations.Commands;
using Manufactures.Domain.GarmentScrapDestinations.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapDestinations.CommandHandler
{
    public class PlaceGarmentScrapDestinationCommandHandler : ICommandHandler<PlaceGarmentScrapDestinationCommand, GarmentScrapDestination>
    {
        private readonly IStorage _storage;

        private readonly IGarmentScrapDestinationRepository _garmentScrapDestinationRepository;

        public PlaceGarmentScrapDestinationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentScrapDestinationRepository = storage.GetRepository<IGarmentScrapDestinationRepository>();
        }

        public async Task<GarmentScrapDestination> Handle(PlaceGarmentScrapDestinationCommand request, CancellationToken cancellationToken)
        {

            GarmentScrapDestination garmentScrapDestination = new GarmentScrapDestination(
                Guid.NewGuid(),
                request.Code,
                request.Name,
                request.Description
            );


            await _garmentScrapDestinationRepository.Update(garmentScrapDestination);

            _storage.Save();

            return garmentScrapDestination;
        }
    }
}
