using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleSewingIns.CommandHandler
{
    public class UpdateDatesGarmentSampleSewingInCommandHandler : ICommandHandler<UpdateDatesGarmentSampleSewingInCommand, int>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingInRepository _GarmentSampleSewingInRepository;

        public UpdateDatesGarmentSampleSewingInCommandHandler(IStorage storage)
        {
            _GarmentSampleSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            _storage = storage;
        }

        public async Task<int> Handle(UpdateDatesGarmentSampleSewingInCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var SewIns = _GarmentSampleSewingInRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSampleSewingIn(a)).ToList();

            foreach (var model in SewIns)
            {
                model.setDate(request.Date);
                model.Modify();
                await _GarmentSampleSewingInRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
