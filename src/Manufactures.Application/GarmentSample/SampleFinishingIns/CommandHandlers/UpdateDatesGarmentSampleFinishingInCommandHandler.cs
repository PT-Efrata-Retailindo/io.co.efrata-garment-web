using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleFinishingIns.CommandHandlers
{
    public class UpdateDatesGarmentSampleFinishingInCommandHandler : ICommandHandler<UpdateDatesGarmentSampleFinishingInCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleFinishingInRepository _garmentFinishingInRepository;

        public UpdateDatesGarmentSampleFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentSampleFinishingInRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentSampleFinishingInCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var FinIns = _garmentFinishingInRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleFinishingIn(a)).ToList();

            foreach (var model in FinIns)
            {
                model.setDate(request.Date);
                model.setSubconType(request.SubconType);
                model.Modify();
                await _garmentFinishingInRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
