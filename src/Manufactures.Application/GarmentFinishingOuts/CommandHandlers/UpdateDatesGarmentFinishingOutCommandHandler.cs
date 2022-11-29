using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentFinishingOuts.CommandHandlers
{
    public class UpdateDatesGarmentFinishingOutCommandHandler : ICommandHandler<UpdateDatesGarmentFinishingOutCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingOutRepository _garmentFinishingOutRepository;

        public UpdateDatesGarmentFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentFinishingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var FinOuts = _garmentFinishingOutRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentFinishingOut(a)).ToList();

            foreach (var model in FinOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _garmentFinishingOutRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
