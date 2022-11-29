using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Commands;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentSubconCuttingOuts;
using Manufactures.Domain.GarmentSubconCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSubconFinishingIns.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentFinishingIns.CommandHandlers
{
    public class RemoveGarmentSubconFinishingInCommandHandler : ICommandHandler<RemoveGarmentSubconFinishingInCommand, GarmentFinishingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentSubconCuttingRepository _garmentSubconCuttingRepository;

        public RemoveGarmentSubconFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            _garmentSubconCuttingRepository = storage.GetRepository<IGarmentSubconCuttingRepository>();
        }

        public async Task<GarmentFinishingIn> Handle(RemoveGarmentSubconFinishingInCommand request, CancellationToken cancellationToken)
        {
            var finIn = _garmentFinishingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentFinishingIn(o)).Single();

            Dictionary<Guid, double> subconCuttingSumQuantities = new Dictionary<Guid, double>();

            var finishingInItems = _garmentFinishingInItemRepository.Find(o => o.FinishingInId == finIn.Identity);
            finishingInItems.ForEach(async item =>
            {
                if (Guid.Empty != item.SubconCuttingId)
                {
                    subconCuttingSumQuantities[item.SubconCuttingId] = subconCuttingSumQuantities.GetValueOrDefault(item.SubconCuttingId) + item.Quantity;
                }

                item.Remove();

                await _garmentFinishingInItemRepository.Update(item);
            });

            //foreach (var item in finishingInItems)
            //{
            //    if (Guid.Empty != item.SubconCuttingId)
            //    {
            //        subconCuttingSumQuantities[item.SubconCuttingId] = subconCuttingSumQuantities.GetValueOrDefault(item.SubconCuttingId) + item.Quantity;
            //    }

            //    item.Remove();

            //    await _garmentFinishingInItemRepository.Update(item);
            //}

            foreach (var sumQuantity in subconCuttingSumQuantities)
            {
                var subconCutting = _garmentSubconCuttingRepository.Query.Where(x => x.Identity == sumQuantity.Key).Select(s => new GarmentSubconCutting(s)).Single();
                subconCutting.SetFinishingInQuantity(subconCutting.FinishingInQuantity - sumQuantity.Value);
                subconCutting.Modify();

                await _garmentSubconCuttingRepository.Update(subconCutting);
            }


            finIn.Remove();
            await _garmentFinishingInRepository.Update(finIn);

            _storage.Save();

            return finIn;
        }
    }
}
