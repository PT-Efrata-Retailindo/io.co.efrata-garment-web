using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.CommandHandlers
{
    public class UpdateDatesGarmentSampleCuttingOutCommandHandler : ICommandHandler<UpdateDatesGarmentSampleCuttingOutCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingOutRepository _GarmentSampleCuttingOutRepository;

        public UpdateDatesGarmentSampleCuttingOutCommandHandler(IStorage storage)
        {
            _GarmentSampleCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateDatesGarmentSampleCuttingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var CutOuts = _GarmentSampleCuttingOutRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleCuttingOut(a)).ToList();

            foreach (var model in CutOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _GarmentSampleCuttingOutRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
