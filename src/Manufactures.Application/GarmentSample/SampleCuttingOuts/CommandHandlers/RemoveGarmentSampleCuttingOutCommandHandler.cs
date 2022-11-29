using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.CommandHandlers
{
    public class RemoveGarmentSampleCuttingOutCommandHandler : ICommandHandler<RemoveGarmentSampleCuttingOutCommand, GarmentSampleCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingOutRepository _GarmentSampleCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository _GarmentSampleCuttingOutItemRepository;
        private readonly IGarmentSampleCuttingOutDetailRepository _GarmentSampleCuttingOutDetailRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _GarmentSampleCuttingInDetailRepository;
        private readonly IGarmentSampleSewingInRepository _GarmentSampleSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository _GarmentSampleSewingInItemRepository;

        public RemoveGarmentSampleCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            _GarmentSampleCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            _GarmentSampleCuttingOutDetailRepository = storage.GetRepository<IGarmentSampleCuttingOutDetailRepository>();
            _GarmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _GarmentSampleSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            _GarmentSampleSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
        }

        public async Task<GarmentSampleCuttingOut> Handle(RemoveGarmentSampleCuttingOutCommand request, CancellationToken cancellationToken)
        {
            var cutOut = _GarmentSampleCuttingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleCuttingOut(o)).Single();
            var sewingIn = _GarmentSampleSewingInRepository.Query.Where(o => o.CuttingOutId == request.Identity).Select(o => new GarmentSampleSewingIn(o)).Single();

            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();

            _GarmentSampleCuttingOutItemRepository.Find(o => o.CuttingOutId == cutOut.Identity).ForEach(async cutOutItem =>
            {
                _GarmentSampleCuttingOutDetailRepository.Find(o => o.CuttingOutItemId == cutOutItem.Identity).ForEach(async cutOutDetail =>
                {
                    if (cuttingInDetailToBeUpdated.ContainsKey(cutOutItem.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[cutOutItem.CuttingInDetailId] += cutOutDetail.CuttingOutQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(cutOutItem.CuttingInDetailId, cutOutDetail.CuttingOutQuantity);
                    }

                    cutOutDetail.Remove();
                    await _GarmentSampleCuttingOutDetailRepository.Update(cutOutDetail);
                });

                cutOutItem.Remove();
                await _GarmentSampleCuttingOutItemRepository.Update(cutOutItem);
            });

            foreach (var cuttingInItem in cuttingInDetailToBeUpdated)
            {
                var GarmentSampleCuttingInDetail = _GarmentSampleCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInItem.Key).Select(s => new GarmentSampleCuttingInDetail(s)).Single();
                GarmentSampleCuttingInDetail.SetRemainingQuantity(GarmentSampleCuttingInDetail.RemainingQuantity + cuttingInItem.Value);
                GarmentSampleCuttingInDetail.Modify();
                await _GarmentSampleCuttingInDetailRepository.Update(GarmentSampleCuttingInDetail);
            }

            _GarmentSampleSewingInItemRepository.Find(o => o.SewingInId == sewingIn.Identity).ForEach(async sewingInItem =>
            {
                sewingInItem.Remove();
                await _GarmentSampleSewingInItemRepository.Update(sewingInItem);
            });

            sewingIn.Remove();
            await _GarmentSampleSewingInRepository.Update(sewingIn);

            cutOut.Remove();
            await _GarmentSampleCuttingOutRepository.Update(cutOut);

            _storage.Save();

            return cutOut;
        }
    }
}
