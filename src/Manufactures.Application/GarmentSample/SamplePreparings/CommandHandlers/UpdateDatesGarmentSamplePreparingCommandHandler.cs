using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SamplePreparings.CommandHandlers
{
    public class UpdateDatesGarmentSamplePreparingCommandHandler : ICommandHandler<UpdateDatesGarmentSamplePreparingCommand, int>
    {
        private readonly IGarmentSamplePreparingRepository _garmentPreparingRepository;
        private readonly IStorage _storage;

        public UpdateDatesGarmentSamplePreparingCommandHandler(IStorage storage)
        {
            _garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public async Task<int> Handle(UpdateDatesGarmentSamplePreparingCommand request, CancellationToken cancellationToken)
        {
            List<Guid> guids = new List<Guid>();
            foreach (var id in request.Identities)
            {
                guids.Add(Guid.Parse(id));
            }
            var Preparings = _garmentPreparingRepository.Query.Where(a => guids.Contains(a.Identity)).Select(a => new GarmentSamplePreparing(a)).ToList();

            foreach (var model in Preparings)
            {
                model.setProcessDate(request.Date);
                model.SetModified();
                await _garmentPreparingRepository.Update(model);
            }
            _storage.Save();

            return guids.Count();
        }
    }
}
