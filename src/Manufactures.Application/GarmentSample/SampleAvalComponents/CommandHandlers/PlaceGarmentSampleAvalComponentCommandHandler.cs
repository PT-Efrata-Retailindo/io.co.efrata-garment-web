using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Commands;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleAvalComponents.CommandHandlers
{
    public class PlaceGarmentSampleAvalComponentCommandHandler : ICommandHandler<PlaceGarmentSampleAvalComponentCommand, GarmentSampleAvalComponent>
    {
        private readonly IStorage _storage;

        private readonly IGarmentSampleAvalComponentRepository _garmentSampleAvalComponentRepository;
        private readonly IGarmentSampleAvalComponentItemRepository _garmentSampleAvalComponentItemRepository;

        private readonly IGarmentSampleCuttingInDetailRepository _garmentSampleCuttingInDetailRepository;
        private readonly IGarmentSampleSewingOutItemRepository _garmentSampleSewingOutItemRepository;
        //private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;

        public PlaceGarmentSampleAvalComponentCommandHandler(IStorage storage)
        {
            _storage = storage;

            _garmentSampleAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
            _garmentSampleAvalComponentItemRepository = storage.GetRepository<IGarmentSampleAvalComponentItemRepository>();

            _garmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentSampleSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            //_garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
        }

        public async Task<GarmentSampleAvalComponent> Handle(PlaceGarmentSampleAvalComponentCommand request, CancellationToken cancellationToken)
        {
            GarmentComodity garmentComodity = request.Comodity ?? new GarmentComodity(0, null, null);

            var avalComponentNo = GenerateNo(request);

            GarmentSampleAvalComponent garmentSampleAvalComponent = new GarmentSampleAvalComponent(
                Guid.NewGuid(),
                avalComponentNo,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.SampleAvalComponentType,
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

                GarmentSampleAvalComponentItem garmentSampleAvalComponentItem = new GarmentSampleAvalComponentItem(
                    Guid.NewGuid(),
                    garmentSampleAvalComponent.Identity,
                    item.SampleCuttingInDetailId,
                    item.SampleSewingOutItemId,
                    item.SampleSewingOutDetailId,
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
                        cuttingInDetail.SetRemainingQuantity(cuttingInDetail.RemainingQuantity - garmentSampleAvalComponentItem.Quantity);
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
                        sewingOutItem.SetRemainingQuantity(sewingOutItem.RemainingQuantity - garmentSampleAvalComponentItem.Quantity);
                        sewingOutItem.Modify();
                        await _garmentSampleSewingOutItemRepository.Update(sewingOutItem);
                    }
                    //}
                }
            }

            await _garmentSampleAvalComponentRepository.Update(garmentSampleAvalComponent);

            _storage.Save();

            return garmentSampleAvalComponent;
        }

        private string GenerateNo(PlaceGarmentSampleAvalComponentCommand request)
        {
            // AK + Unit Code + 2 digit tahun + 2 digit Bulan + 4 digit urut

            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"AK{request.Unit.Code.Trim()}{year}{month}";

            var lastNo = _garmentSampleAvalComponentRepository.Query.Where(w => w.SampleAvalComponentNo.StartsWith(prefix))
                .OrderByDescending(o => o.SampleAvalComponentNo)
                .Select(s => int.Parse(s.SampleAvalComponentNo.Replace(prefix, "")))
                .FirstOrDefault();
            var curNo = $"{prefix}{(lastNo + 1).ToString("D4")}";

            return curNo;
        }
    }
}
