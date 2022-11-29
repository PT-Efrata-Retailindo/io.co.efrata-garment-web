using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleCuttingIns.CommandHandlers
{
    public class RemoveGarmentSampleCuttingInCommandHandler : ICommandHandler<RemoveGarmentSampleCuttingInCommand, GarmentSampleCuttingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingInRepository _garmentSampleCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentSampleCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentSampleCuttingInDetailRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;

        public RemoveGarmentSampleCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentSampleCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        public async Task<GarmentSampleCuttingIn> Handle(RemoveGarmentSampleCuttingInCommand request, CancellationToken cancellationToken)
        {
            var cutIn = _garmentSampleCuttingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleCuttingIn(o)).Single();

            Dictionary<Guid, decimal> preparingItemToBeUpdated = new Dictionary<Guid, decimal>();

            _garmentSampleCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).ForEach(async cutInItem =>
            {
                _garmentSampleCuttingInDetailRepository.Find(o => o.CutInItemId == cutInItem.Identity).ForEach(async cutInDetail =>
                {
                    if (preparingItemToBeUpdated.ContainsKey(cutInDetail.PreparingItemId))
                    {
                        preparingItemToBeUpdated[cutInDetail.PreparingItemId] += (decimal)cutInDetail.PreparingQuantity;
                    }
                    else
                    {
                        preparingItemToBeUpdated.Add(cutInDetail.PreparingItemId, (decimal)cutInDetail.PreparingQuantity);
                    }

                    cutInDetail.Remove();
                    await _garmentSampleCuttingInDetailRepository.Update(cutInDetail);
                });

                cutInItem.Remove();
                await _garmentSampleCuttingInItemRepository.Update(cutInItem);
            });

            foreach (var preparingItem in preparingItemToBeUpdated)
            {
                var garmentSamplePreparingItem = _garmentSamplePreparingItemRepository.Query.Where(x => x.Identity == preparingItem.Key).Select(s => new GarmentSamplePreparingItem(s)).Single();
                garmentSamplePreparingItem.setRemainingQuantity(Convert.ToDouble((decimal)garmentSamplePreparingItem.RemainingQuantity + preparingItem.Value));
                garmentSamplePreparingItem.SetModified();
                await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItem);
            }

            cutIn.Remove();
            await _garmentSampleCuttingInRepository.Update(cutIn);

            _storage.Save();

            return cutIn;
        }
    }
}
