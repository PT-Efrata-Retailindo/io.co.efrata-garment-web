using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapDestinations;
using Manufactures.Domain.GarmentScrapDestinations.Commands;
using Manufactures.Domain.GarmentScrapDestinations.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapDestinations.CommandHandler
{
    public class RemoveGarmentScrapDestinationCommandHandler : ICommandHandler<RemoveGarmentScrapDestinationCommand, GarmentScrapDestination>
    {
        private readonly IStorage _storage;

        private readonly IGarmentScrapDestinationRepository _garmentScrapDestinationRepository;

        public RemoveGarmentScrapDestinationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentScrapDestinationRepository = storage.GetRepository<IGarmentScrapDestinationRepository>();
        }

        public async Task<GarmentScrapDestination> Handle(RemoveGarmentScrapDestinationCommand request, CancellationToken cancellationToken)
        {
            var gscrapDestination = _garmentScrapDestinationRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentScrapDestination(o)).Single();

            if (gscrapDestination == null)
                throw Validator.ErrorValidation(("Identity", "Invalid Id: " + request.Identity));

            gscrapDestination.Remove();
            await _garmentScrapDestinationRepository.Update(gscrapDestination);
            _storage.Save();
            return gscrapDestination;
        }
    }
}
