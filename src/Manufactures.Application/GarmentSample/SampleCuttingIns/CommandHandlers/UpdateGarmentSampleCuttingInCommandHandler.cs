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
    public class UpdateGarmentSampleCuttingInCommandHandler : ICommandHandler<UpdateGarmentSampleCuttingInCommand, GarmentSampleCuttingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingInRepository _garmentSampleCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentSampleCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentSampleCuttingInDetailRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;

        public UpdateGarmentSampleCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentSampleCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        public async Task<GarmentSampleCuttingIn> Handle(UpdateGarmentSampleCuttingInCommand request, CancellationToken cancellationToken)
        {
            var cutIn = _garmentSampleCuttingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleCuttingIn(o)).Single();

            Dictionary<Guid, decimal> preparingItemToBeUpdated = new Dictionary<Guid, decimal>();

            _garmentSampleCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).ForEach(async cutInItem =>
            {
                var item = request.Items.Where(o => o.Id == cutInItem.Identity).Single();
                _garmentSampleCuttingInDetailRepository.Find(o => o.CutInItemId == cutInItem.Identity).ForEach(async cutInDetail =>
                {
                    var detail = item.Details.Where(o => o.Id == cutInDetail.Identity).Single();

                    decimal diffPreparingQuantity = (decimal)cutInDetail.PreparingQuantity - (decimal)detail.PreparingQuantity;

                    if (preparingItemToBeUpdated.ContainsKey(cutInDetail.PreparingItemId))
                    {
                        preparingItemToBeUpdated[cutInDetail.PreparingItemId] += diffPreparingQuantity;
                    }
                    else
                    {
                        preparingItemToBeUpdated.Add(cutInDetail.PreparingItemId, diffPreparingQuantity);
                    }

                    cutInDetail.SetCuttingInQuantity(detail.CuttingInQuantity);
                    cutInDetail.SetPreparingQuantity(detail.PreparingQuantity);
                    cutInDetail.SetRemainingQuantity(detail.RemainingQuantity);
                    cutInDetail.SetPrice(detail.Price);
                    cutInDetail.SetFC(detail.FC);

                    cutInDetail.Modify();
                    await _garmentSampleCuttingInDetailRepository.Update(cutInDetail);
                });

                cutInItem.Modify();
                await _garmentSampleCuttingInItemRepository.Update(cutInItem);
            });

            foreach (var preparingItem in preparingItemToBeUpdated)
            {
                var garmentSamplePreparingItem = _garmentSamplePreparingItemRepository.Query.Where(x => x.Identity == preparingItem.Key).Select(s => new GarmentSamplePreparingItem(s)).Single();
                garmentSamplePreparingItem.setRemainingQuantity(Convert.ToDouble((decimal)garmentSamplePreparingItem.RemainingQuantity + preparingItem.Value));
                garmentSamplePreparingItem.SetModified();
                await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItem);
            }
            cutIn.SetFC(request.FC);
            cutIn.Modify();
            await _garmentSampleCuttingInRepository.Update(cutIn);

            _storage.Save();

            return cutIn;
        }
    }
}
