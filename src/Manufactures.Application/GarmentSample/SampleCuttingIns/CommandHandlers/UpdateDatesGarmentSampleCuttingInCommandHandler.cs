using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleCuttingIns.CommandHandlers
{
    public class UpdateDatesGarmentSampleCuttingInCommandHandler : ICommandHandler<UpdateDatesGarmentSampleCuttingInCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingInRepository _garmentCuttingInRepository;

        public UpdateDatesGarmentSampleCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentSampleCuttingInCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var CutIns = _garmentCuttingInRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleCuttingIn(a)).ToList();

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
