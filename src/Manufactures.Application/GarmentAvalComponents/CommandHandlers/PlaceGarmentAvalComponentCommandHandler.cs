using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAvalComponents;
using Manufactures.Domain.GarmentAvalComponents.Commands;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAvalComponents.CommandHandlers
{
    public class PlaceGarmentAvalComponentCommandHandler : ICommandHandler<PlaceGarmentAvalComponentCommand, GarmentAvalComponent>
    {
        private readonly IStorage _storage;

        private readonly IGarmentAvalComponentRepository _garmentAvalComponentRepository;
        private readonly IGarmentAvalComponentItemRepository _garmentAvalComponentItemRepository;

        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        //private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;

        public PlaceGarmentAvalComponentCommandHandler(IStorage storage)
        {
            _storage = storage;

            _garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
            _garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();

            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            //_garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
        }

        public async Task<GarmentAvalComponent> Handle(PlaceGarmentAvalComponentCommand request, CancellationToken cancellationToken)
        {
            GarmentComodity garmentComodity = request.Comodity ?? new GarmentComodity(0, null, null);

            var avalComponentNo = GenerateNo(request);

            GarmentAvalComponent garmentAvalComponent = new GarmentAvalComponent(
                Guid.NewGuid(),
                avalComponentNo,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code, 
                request.Unit.Name,
                request.AvalComponentType, 
                request.RONo,
                request.Article,
                new GarmentComodityId(garmentComodity.Id),
                garmentComodity.Code,
                garmentComodity.Name,
                request.Date.GetValueOrDefault(),
                request.IsReceived
            );

            foreach (var item in request.Items.Where(w => w.IsSave))
            {
                SizeValueObject sizeValueObject = item.Size ?? new SizeValueObject(0, null);

                GarmentAvalComponentItem garmentAvalComponentItem = new GarmentAvalComponentItem(
                    Guid.NewGuid(),
                    garmentAvalComponent.Identity,
                    item.CuttingInDetailId,
                    item.SewingOutItemId,
                    item.SewingOutDetailId,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.Color,
                    item.Quantity,
                    item.Quantity,
                    new SizeId(sizeValueObject.Id),
                    sizeValueObject.Size,
                    item.Price,
                    item.BasicPrice
                );

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
                        cuttingInDetail.SetRemainingQuantity(cuttingInDetail.RemainingQuantity - garmentAvalComponentItem.Quantity);
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
                            sewingOutItem.SetRemainingQuantity(sewingOutItem.RemainingQuantity - garmentAvalComponentItem.Quantity);
                            sewingOutItem.Modify();
                            await _garmentSewingOutItemRepository.Update(sewingOutItem);
                        }
                    //}
                }
            }

            await _garmentAvalComponentRepository.Update(garmentAvalComponent);

            _storage.Save();

            return garmentAvalComponent;
        }

        private string GenerateNo(PlaceGarmentAvalComponentCommand request)
        {
            // AK + Unit Code + 2 digit tahun + 2 digit Bulan + 4 digit urut

            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"AK{request.Unit.Code.Trim()}{year}{month}";

            var lastNo = _garmentAvalComponentRepository.Query.Where(w => w.AvalComponentNo.StartsWith(prefix))
                .OrderByDescending(o => o.AvalComponentNo)
                .Select(s => int.Parse(s.AvalComponentNo.Replace(prefix, "")))
                .FirstOrDefault();
            var curNo = $"{prefix}{(lastNo + 1).ToString("D4")}";

            return curNo;
        }
    }
}
