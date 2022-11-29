using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.CommandHandlers
{
    public class UpdateDatesGarmentSewingOutCommandHandler : ICommandHandler<UpdateDatesGarmentSewingOutCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;

        public UpdateDatesGarmentSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentSewingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var SewOuts = _garmentSewingOutRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSewingOut(a)).ToList();

            foreach (var model in SewOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _garmentSewingOutRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
