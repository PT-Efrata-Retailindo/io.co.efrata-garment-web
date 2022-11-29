using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapSources.CommandHandler
{
    public class UpdateGarmentScrapSourceCommandHandler : ICommandHandler<UpdateGarmentScrapSourceCommand, GarmentScrapSource>
    {
        private readonly IStorage _storage;

        private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;
        public UpdateGarmentScrapSourceCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
        }

        public async Task<GarmentScrapSource> Handle(UpdateGarmentScrapSourceCommand request, CancellationToken cancellationToken)
        {
            var gscrapSource = _garmentScrapSourceRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentScrapSource(o)).Single();

            if (gscrapSource == null)
                throw Validator.ErrorValidation(("Identity", "Invalid Id: " + request.Identity));

            gscrapSource.setCode(request.Code);
            gscrapSource.setName(request.Name);
            gscrapSource.setDescription(request.Description);
            gscrapSource.Modify();
            await _garmentScrapSourceRepository.Update(gscrapSource);
            _storage.Save();
            return gscrapSource;
        }
    }
}
