using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingIns.CommandHandlers
{
    public class UpdateDatesGarmentCuttingInCommandHandler : ICommandHandler<UpdateDatesGarmentCuttingInCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;

        public UpdateDatesGarmentCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentCuttingInCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var CutIns = _garmentCuttingInRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentCuttingIn(a)).ToList();

            foreach (var model in CutIns)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _garmentCuttingInRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
