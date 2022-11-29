using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingOuts.CommandHandlers
{
    public class UpdateDatesGarmentCuttingOutCommandHandler : ICommandHandler<UpdateDatesGarmentCuttingOutCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;

        public UpdateDatesGarmentCuttingOutCommandHandler(IStorage storage)
        {
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateDatesGarmentCuttingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var CutOuts = _garmentCuttingOutRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentCuttingOut(a)).ToList();

            foreach (var model in CutOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _garmentCuttingOutRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
