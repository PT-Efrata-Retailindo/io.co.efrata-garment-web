using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleFinishingOuts.CommandHandlers
{
    public class UpdateDatesGarmentSampleFinishingOutCommandHandler : ICommandHandler<UpdateDatesGarmentSampleFinishingOutCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleFinishingOutRepository _GarmentSampleFinishingOutRepository;

        public UpdateDatesGarmentSampleFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
        }

        public async Task<int> Handle(UpdateDatesGarmentSampleFinishingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var FinOuts = _GarmentSampleFinishingOutRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleFinishingOut(a)).ToList();

            foreach (var model in FinOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _GarmentSampleFinishingOutRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
