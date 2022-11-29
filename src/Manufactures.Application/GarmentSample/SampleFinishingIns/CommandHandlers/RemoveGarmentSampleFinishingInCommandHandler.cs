using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleFinishingIns.CommandHandlers
{
    public class RemoveGarmentSampleFinishingInCommandHandler : ICommandHandler<RemoveGarmentSampleFinishingInCommand, GarmentSampleFinishingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentSampleFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentSampleSewingOutItemRepository _garmentSewingOutItemRepository;

        public RemoveGarmentSampleFinishingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingInRepository = storage.GetRepository<IGarmentSampleFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
        }

        public async Task<GarmentSampleFinishingIn> Handle(RemoveGarmentSampleFinishingInCommand request, CancellationToken cancellationToken)
        {
            var finIn = _garmentFinishingInRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleFinishingIn(o)).Single();

            Dictionary<Guid, double> sewingOutItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentFinishingInItemRepository.Find(o => o.FinishingInId == finIn.Identity).ForEach(async finishingInItem =>
            {
                if (sewingOutItemToBeUpdated.ContainsKey(finishingInItem.SewingOutItemId))
                {
                    sewingOutItemToBeUpdated[finishingInItem.SewingOutItemId] += finishingInItem.Quantity;
                }
                else
                {
                    sewingOutItemToBeUpdated.Add(finishingInItem.SewingOutItemId, finishingInItem.Quantity);
                }

                finishingInItem.Remove();

                await _garmentFinishingInItemRepository.Update(finishingInItem);
            });

            foreach (var sewingOutItem in sewingOutItemToBeUpdated)
            {
                var garmentSewingOutItem = _garmentSewingOutItemRepository.Query.Where(x => x.Identity == sewingOutItem.Key).Select(s => new GarmentSampleSewingOutItem(s)).Single();
                garmentSewingOutItem.SetRemainingQuantity(garmentSewingOutItem.RemainingQuantity + sewingOutItem.Value);
                garmentSewingOutItem.Modify();

                await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
            }

            finIn.Remove();
            await _garmentFinishingInRepository.Update(finIn);

            _storage.Save();

            return finIn;
        }
    }
}
