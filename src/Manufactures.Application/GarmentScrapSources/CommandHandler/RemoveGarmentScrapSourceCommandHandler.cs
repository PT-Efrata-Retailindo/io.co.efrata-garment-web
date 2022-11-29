using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapSources.CommandHandler
{
    public class RemoveGarmentScrapSourceCommandHandler : ICommandHandler<RemoveGarmentScrapSourceCommand, GarmentScrapSource>
    {
        private readonly IStorage _storage;

        private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;

        public RemoveGarmentScrapSourceCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
        }

        public async Task<GarmentScrapSource> Handle(RemoveGarmentScrapSourceCommand request, CancellationToken cancellationToken)
        {
            var gscrapSource = _garmentScrapSourceRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentScrapSource(o)).Single();

            if (gscrapSource == null)
                throw Validator.ErrorValidation(("Identity", "Invalid Id: " + request.Identity));

            gscrapSource.Remove();
            await _garmentScrapSourceRepository.Update(gscrapSource);
            _storage.Save();
            return gscrapSource;
        }
    }
}
