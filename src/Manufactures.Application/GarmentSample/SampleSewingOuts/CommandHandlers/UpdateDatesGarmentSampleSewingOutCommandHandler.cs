using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.CommandHandlers
{
    public class UpdateDatesGarmentSampleSewingOutCommandHandler : ICommandHandler<UpdateDatesGarmentSampleSewingOutCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingOutRepository _GarmentSampleSewingOutRepository;

        public UpdateDatesGarmentSampleSewingOutCommandHandler(IStorage storage)
        {
            _GarmentSampleSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateDatesGarmentSampleSewingOutCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var SewOuts = _GarmentSampleSewingOutRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleSewingOut(a)).ToList();

            foreach (var model in SewOuts)
            {
                model.SetDate(request.Date);
                model.Modify();
                await _GarmentSampleSewingOutRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
