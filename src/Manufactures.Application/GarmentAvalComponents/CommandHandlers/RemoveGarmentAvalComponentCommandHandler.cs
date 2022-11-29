using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalComponents.CommandHandlers
{
    public class RemoveGarmentAvalComponentCommandHandler : ICommandHandler<RemoveGarmentAvalComponentCommand, GarmentAvalComponent>
    {
        private readonly IStorage _storage;

        private readonly IGarmentAvalComponentRepository _garmentAvalComponentRepository;
        private readonly IGarmentAvalComponentItemRepository _garmentAvalComponentItemRepository;

        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        //private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;

        public RemoveGarmentAvalComponentCommandHandler(IStorage storage)
        {
            _storage = storage;

            _garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
            _garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();

            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            //_garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
        }

        public async Task<GarmentAvalComponent> Handle(RemoveGarmentAvalComponentCommand request, CancellationToken cancellationToken)
        {
            GarmentAvalComponent garmentAvalComponent = _garmentAvalComponentRepository.Query.Where(w => w.Identity == request.Identity).Select(s => new GarmentAvalComponent(s)).Single();

            _garmentAvalComponentItemRepository.Find(f => f.AvalComponentId == garmentAvalComponent.Identity).ForEach(async garmentAvalComponentItem =>
            {
                garmentAvalComponentItem.Remove();
                await _garmentAvalComponentItemRepository.Update(garmentAvalComponentItem);

                if (garmentAvalComponent.AvalComponentType == "CUTTING")
                {
                    var cuttingInDetail = _garmentCuttingInDetailRepository.Find(f => f.Identity == garmentAvalComponentItem.CuttingInDetailId).SingleOrDefault();

                    if (cuttingInDetail == null)
                    {
                        throw new Exception($"CuttingInDetail {garmentAvalComponentItem.CuttingInDetailId} not found");
                    }
                    else
                    {
                        cuttingInDetail.SetRemainingQuantity(cuttingInDetail.RemainingQuantity + garmentAvalComponentItem.Quantity);
                        cuttingInDetail.Modify();
                        await _garmentCuttingInDetailRepository.Update(cuttingInDetail);
                    }
                }
                else if (garmentAvalComponent.AvalComponentType == "SEWING")
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
                        var sewingOutItem = _garmentSewingOutItemRepository.Find(f => f.Identity == garmentAvalComponentItem.SewingOutItemId).SingleOrDefault();

                        if (sewingOutItem == null)
                        {
                            throw new Exception($"SewingOutItem {garmentAvalComponentItem.SewingOutItemId} not found");
                        }
                        else
                        {
                            sewingOutItem.SetRemainingQuantity(sewingOutItem.RemainingQuantity + garmentAvalComponentItem.Quantity);
                            sewingOutItem.Modify();
                            await _garmentSewingOutItemRepository.Update(sewingOutItem);
                        }
                    //}
                }
            });

            garmentAvalComponent.Remove();
            await _garmentAvalComponentRepository.Update(garmentAvalComponent);

            _storage.Save();

            return garmentAvalComponent;
        }
    }
}
