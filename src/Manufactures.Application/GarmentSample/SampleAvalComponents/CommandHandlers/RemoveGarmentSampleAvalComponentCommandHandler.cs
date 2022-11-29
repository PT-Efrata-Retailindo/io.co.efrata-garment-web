using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.CommandHandlers
{
    public class RemoveGarmentSampleAvalComponentCommandHandler : ICommandHandler<RemoveGarmentSampleAvalComponentCommand, GarmentSampleAvalComponent>
    {
        private readonly IStorage _storage;

        private readonly IGarmentSampleAvalComponentRepository _garmentSampleAvalComponentRepository;
        private readonly IGarmentSampleAvalComponentItemRepository _garmentSampleAvalComponentItemRepository;

        private readonly IGarmentSampleCuttingInDetailRepository _garmentSampleCuttingInDetailRepository;
        private readonly IGarmentSampleSewingOutItemRepository _garmentSampleSewingOutItemRepository;
        //private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;

        public RemoveGarmentSampleAvalComponentCommandHandler(IStorage storage)
        {
            _storage = storage;

            _garmentSampleAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
            _garmentSampleAvalComponentItemRepository = storage.GetRepository<IGarmentSampleAvalComponentItemRepository>();

            _garmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentSampleSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            //_garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
        }

        public async Task<GarmentSampleAvalComponent> Handle(RemoveGarmentSampleAvalComponentCommand request, CancellationToken cancellationToken)
        {
            GarmentSampleAvalComponent garmentSampleAvalComponent = _garmentSampleAvalComponentRepository.Query.Where(w => w.Identity == request.Identity).Select(s => new GarmentSampleAvalComponent(s)).Single();

            _garmentSampleAvalComponentItemRepository.Find(f => f.SampleAvalComponentId == garmentSampleAvalComponent.Identity).ForEach(async garmentSampleAvalComponentItem =>
            {
                garmentSampleAvalComponentItem.Remove();
                await _garmentSampleAvalComponentItemRepository.Update(garmentSampleAvalComponentItem);

                if (garmentSampleAvalComponent.SampleAvalComponentType == "CUTTING")
                {
                    var cuttingInDetail = _garmentSampleCuttingInDetailRepository.Find(f => f.Identity == garmentSampleAvalComponentItem.SampleCuttingInDetailId).SingleOrDefault();

                    if (cuttingInDetail == null)
                    {
                        throw new Exception($"CuttingInDetail {garmentSampleAvalComponentItem.SampleCuttingInDetailId} not found");
                    }
                    else
                    {
                        cuttingInDetail.SetRemainingQuantity(cuttingInDetail.RemainingQuantity + garmentSampleAvalComponentItem.Quantity);
                        cuttingInDetail.Modify();
                        await _garmentSampleCuttingInDetailRepository.Update(cuttingInDetail);
                    }
                }
                else if (garmentSampleAvalComponent.SampleAvalComponentType == "SEWING")
                {
                    //if (item.IsDifferentSize)
                    //{
                    //    var sewingOutDetail = _garmentSewingOutDetailRepository.Find(f => f.Identity == garmentAvalComponentItem.SewingOutDetailId).SingleOrDefault();

                    //    if (sewingOutDetail == null)
                    //    {
                    //        throw new Exception($"SewingOutDetail {garmentAvalComponentItem.SewingOutDetailId} not found");
                    //    }
                    //    //else
                    //    //{
                    //    //    sewingOutDetail.Modify();
                    //    //    await _garmentSewingOutDetailRepository.Update(sewingOutDetail);
                    //    //}
                    //}
                    //else
                    //{
                    var sewingOutItem = _garmentSampleSewingOutItemRepository.Find(f => f.Identity == garmentSampleAvalComponentItem.SampleSewingOutItemId).SingleOrDefault();

                    if (sewingOutItem == null)
                    {
                        throw new Exception($"SewingOutItem {garmentSampleAvalComponentItem.SampleSewingOutItemId} not found");
                    }
                    else
                    {
                        sewingOutItem.SetRemainingQuantity(sewingOutItem.RemainingQuantity + garmentSampleAvalComponentItem.Quantity);
                        sewingOutItem.Modify();
                        await _garmentSampleSewingOutItemRepository.Update(sewingOutItem);
                    }
                    //}
                }
            });

            garmentSampleAvalComponent.Remove();
            await _garmentSampleAvalComponentRepository.Update(garmentSampleAvalComponent);

            _storage.Save();

            return garmentSampleAvalComponent;
        }
    }
}
