using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Commands;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SamplePreparings.CommandHandlers
{
    public class RemoveGarmentSamplePreparingCommandHandler : ICommandHandler<RemoveGarmentSamplePreparingCommand, GarmentSamplePreparing>
    {
        private readonly IGarmentSamplePreparingRepository _garmentSamplePreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;
        private readonly IStorage _storage;

        public RemoveGarmentSamplePreparingCommandHandler(IStorage storage)
        {
            _garmentSamplePreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentSamplePreparing> Handle(RemoveGarmentSamplePreparingCommand request, CancellationToken cancellationToken)
        {
            var garmentSamplePreparing = _garmentSamplePreparingRepository.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (garmentSamplePreparing == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Id));

            var garmentSamplePreparingItems = _garmentSamplePreparingItemRepository.Find(x => x.GarmentSamplePreparingId == request.Id);

            foreach (var item in garmentSamplePreparingItems)
            {
                item.Remove();
                await _garmentSamplePreparingItemRepository.Update(item);
            }

            garmentSamplePreparing.Remove();

            await _garmentSamplePreparingRepository.Update(garmentSamplePreparing);

            _storage.Save();

            return garmentSamplePreparing;
        }
    }
}
