using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentLoadings;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Commands;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingIns.CommandHandlers
{
    public class RemoveGarmentSewingInCommandHandler : ICommandHandler<RemoveGarmentSewingInCommand, GarmentSewingIn>
    {
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentLoadingItemRepository _garmentLoadingItemRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IStorage _storage;

        public RemoveGarmentSewingInCommandHandler(IStorage storage)
        {
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            _storage = storage;
        }

        public async Task<GarmentSewingIn> Handle(RemoveGarmentSewingInCommand request, CancellationToken cancellationToken)
        {
            var garmentSewingIn = _garmentSewingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSewingIn(o)).Single();

            if (garmentSewingIn == null)
                throw Validator.ErrorValidation(("Id", "Invalid Id: " + request.Identity));

            var garmentSewingInItems = _garmentSewingInItemRepository.Find(x => x.SewingInId == request.Identity);

            foreach (var item in garmentSewingInItems)
            {
                item.Remove();

                if (garmentSewingIn.SewingFrom == "CUTTING")
                {
                    var garmentLoadingItem = _garmentLoadingItemRepository.Query.Where(o => o.Identity == item.LoadingItemId).Select(s => new GarmentLoadingItem(s)).Single();

                    garmentLoadingItem.SetRemainingQuantity(garmentLoadingItem.RemainingQuantity + item.Quantity);

                    garmentLoadingItem.Modify();
                    await _garmentLoadingItemRepository.Update(garmentLoadingItem);
                }
                else if(garmentSewingIn.SewingFrom == "SEWING")
                {
                    var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(s => s.Identity == item.SewingOutItemId).Select(s => new GarmentSewingOutItem(s)).Single();

                    garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity + item.Quantity);

                    garmentSewingOutItem.Modify();
                    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                }
                else if (garmentSewingIn.SewingFrom == "FINISHING")
                {
                    var garmentFinishingOutItem = _garmentFinishingOutItemRepository.Query.Where(s => s.Identity == item.FinishingOutItemId).Select(s => new GarmentFinishingOutItem(s)).Single();

                    garmentFinishingOutItem.SetRemainingQuantity(garmentFinishingOutItem.RemainingQuantity + item.Quantity);

                    garmentFinishingOutItem.Modify();
                    await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                }

                await _garmentSewingInItemRepository.Update(item);
            }

            garmentSewingIn.Remove();

            await _garmentSewingInRepository.Update(garmentSewingIn);

            _storage.Save();

            return garmentSewingIn;
        }
    }
}