using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Commands;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentPreparings;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingIns.CommandHandlers
{
    public class UpdateGarmentCuttingInCommandHandler : ICommandHandler<UpdateGarmentCuttingInCommand, GarmentCuttingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentPreparingItemRepository _garmentPreparingItemRepository;

        public UpdateGarmentCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
        }

        public async Task<GarmentCuttingIn> Handle(UpdateGarmentCuttingInCommand request, CancellationToken cancellationToken)
        {
            var cutIn = _garmentCuttingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentCuttingIn(o)).Single();

            Dictionary<Guid, decimal> preparingItemToBeUpdated = new Dictionary<Guid, decimal>();

            _garmentCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).ForEach(async cutInItem =>
            {
                var item = request.Items.Where(o => o.Id == cutInItem.Identity).Single();
                _garmentCuttingInDetailRepository.Find(o => o.CutInItemId == cutInItem.Identity).ForEach(async cutInDetail =>
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
                    await _garmentCuttingInDetailRepository.Update(cutInDetail);
                });

                cutInItem.Modify();
                await _garmentCuttingInItemRepository.Update(cutInItem);
            });

            foreach (var preparingItem in preparingItemToBeUpdated)
            {
                var garmentPreparingItem = _garmentPreparingItemRepository.Query.Where(x => x.Identity == preparingItem.Key).Select(s => new GarmentPreparingItem(s)).Single();
                garmentPreparingItem.setRemainingQuantity(Convert.ToDouble((decimal)garmentPreparingItem.RemainingQuantity + preparingItem.Value));
                garmentPreparingItem.SetModified();
                await _garmentPreparingItemRepository.Update(garmentPreparingItem);
            }
            cutIn.SetFC(request.FC);
            cutIn.Modify();
            await _garmentCuttingInRepository.Update(cutIn);

            _storage.Save();

            return cutIn;
        }
    }
}
