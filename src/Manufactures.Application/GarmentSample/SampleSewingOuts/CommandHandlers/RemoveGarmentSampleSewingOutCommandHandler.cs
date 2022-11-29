using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
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
    public class RemoveGarmentSampleSewingOutCommandHandler : ICommandHandler<RemoveGarmentSampleSewingOutCommand, GarmentSampleSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSampleSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSampleSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentSampleCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSampleFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentSampleFinishingInItemRepository _garmentFinishingInItemRepository;

        public RemoveGarmentSampleSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSampleSewingOutDetailRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentFinishingInRepository = storage.GetRepository<IGarmentSampleFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
        }

        public async Task<GarmentSampleSewingOut> Handle(RemoveGarmentSampleSewingOutCommand request, CancellationToken cancellationToken)
        {
            var sewOut = _garmentSewingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleSewingOut(o)).Single();

            if (sewOut.SewingTo == "CUTTING")
            {
                Guid cutInId = _garmentCuttingInItemRepository.Query.Where(a => a.SewingOutId == sewOut.Identity).First().CutInId;

                var cutIn = _garmentCuttingInRepository.Query.Where(a => a.Identity == cutInId).Select(a => new GarmentSampleCuttingIn(a)).Single();

                _garmentCuttingInItemRepository.Find(o => o.CutInId == cutIn.Identity).ForEach(async cutInItem =>
                {
                    _garmentCuttingInDetailRepository.Find(o => o.CutInItemId == cutInItem.Identity).ForEach(async cutInDetail =>
                    {

                        cutInDetail.Remove();
                        await _garmentCuttingInDetailRepository.Update(cutInDetail);
                    });

                    cutInItem.Remove();
                    await _garmentCuttingInItemRepository.Update(cutInItem);
                });

                cutIn.Remove();
                await _garmentCuttingInRepository.Update(cutIn);
            }
            
            if (sewOut.SewingTo == "FINISHING")
            {
                Guid finInId = Guid.Empty;
                _garmentSewingOutItemRepository.Find(o => o.SampleSewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
                {
                    var finInItem = _garmentFinishingInItemRepository.Query.Where(o => o.SewingOutItemId == sewOutItem.Identity).Select(o => new GarmentSampleFinishingInItem(o)).Single();
                    finInId = finInItem.FinishingInId;
                    if (sewOut.IsDifferentSize)
                    {
                        _garmentSewingOutDetailRepository.Find(o => o.SampleSewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                        {
                            finInItem = _garmentFinishingInItemRepository.Query.Where(o => o.SewingOutDetailId == sewOutDetail.Identity).Select(o => new GarmentSampleFinishingInItem(o)).Single();

                            finInItem.Remove();
                            await _garmentFinishingInItemRepository.Update(finInItem);
                        });
                    }
                    else
                    {
                        finInItem.Remove();
                        await _garmentFinishingInItemRepository.Update(finInItem);
                    }
                });
                var finIn = _garmentFinishingInRepository.Query.Where(a => a.Identity == finInId).Select(o => new GarmentSampleFinishingIn(o)).Single();
                finIn.Remove();
                await _garmentFinishingInRepository.Update(finIn);
            }

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentSewingOutItemRepository.Find(o => o.SampleSewingOutId == sewOut.Identity).ForEach(async sewOutItem =>
            {
                if (sewOut.IsDifferentSize)
                {
                    _garmentSewingOutDetailRepository.Find(o => o.SampleSewingOutItemId == sewOutItem.Identity).ForEach(async sewOutDetail =>
                    {
                        if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SampleSewingInItemId))
                        {
                            sewInItemToBeUpdated[sewOutItem.SampleSewingInItemId ] += sewOutDetail.Quantity;
                        }
                        else
                        {
                            sewInItemToBeUpdated.Add(sewOutItem.SampleSewingInItemId, sewOutDetail.Quantity);
                        }

                        sewOutDetail.Remove();
                        await _garmentSewingOutDetailRepository.Update(sewOutDetail);
                    });
                }
                else
                {
                    if (sewInItemToBeUpdated.ContainsKey(sewOutItem.SampleSewingInItemId))
                    {
                        sewInItemToBeUpdated[sewOutItem.SampleSewingInItemId] += sewOutItem.Quantity;
                    }
                    else
                    {
                        sewInItemToBeUpdated.Add(sewOutItem.SampleSewingInItemId, sewOutItem.Quantity);
                    }
                }


                sewOutItem.Remove();
                await _garmentSewingOutItemRepository.Update(sewOutItem);
            });

            foreach (var sewingInItem in sewInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSampleSewingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + sewingInItem.Value);
                garmentSewInItem.Modify();
                await _garmentSewingInItemRepository.Update(garmentSewInItem);
            }

            sewOut.Remove();
            await _garmentSewingOutRepository.Update(sewOut);

            _storage.Save();

            return sewOut;
        }
    }
}
