using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.SamplePreparings;

namespace Manufactures.Application.GarmentSample.SampleCuttingIns.CommandHandlers
{
    public class PlaceGarmentSampleCuttingInCommandHandler : ICommandHandler<PlaceGarmentSampleCuttingInCommand, GarmentSampleCuttingIn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingInRepository _garmentSampleCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentSampleCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentSampleCuttingInDetailRepository;
        private readonly IGarmentSamplePreparingItemRepository _garmentSamplePreparingItemRepository;

        public PlaceGarmentSampleCuttingInCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSampleCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentSampleCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentSampleCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentSamplePreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
        }

        public async Task<GarmentSampleCuttingIn> Handle(PlaceGarmentSampleCuttingInCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.Details.Where(detail => detail.IsSave).Count() > 0).ToList();

            GarmentSampleCuttingIn garmentSampleCuttingIn = new GarmentSampleCuttingIn(
                Guid.NewGuid(),
                GenerateCutInNo(request),
                request.CuttingType,
                request.CuttingFrom,
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.CuttingInDate.GetValueOrDefault(),
                request.FC
            );

            Dictionary<Guid, decimal> preparingItemToBeUpdated = new Dictionary<Guid, decimal>();

            foreach (var item in request.Items)
            {
                GarmentSampleCuttingInItem garmentSampleCuttingInItem = new GarmentSampleCuttingInItem(
                    Guid.NewGuid(),
                    garmentSampleCuttingIn.Identity,
                    item.PreparingId,
                    item.UENId,
                    item.UENNo,
                    item.SewingOutId,
                    item.SewingOutNo
                );

                foreach (var detail in item.Details)
                {
                    if (detail.IsSave)
                    {
                        GarmentSampleCuttingInDetail garmentSampleCuttingInDetail = new GarmentSampleCuttingInDetail(
                            Guid.NewGuid(),
                            garmentSampleCuttingInItem.Identity,
                            detail.PreparingItemId,
                            Guid.Empty,
                            Guid.Empty,
                            new ProductId(detail.Product.Id),
                            detail.Product.Code,
                            detail.Product.Name,
                            detail.DesignColor,
                            detail.FabricType,
                            detail.PreparingQuantity,
                            new UomId(detail.PreparingUom.Id),
                            detail.PreparingUom.Unit,
                            detail.CuttingInQuantity,
                            new UomId(detail.CuttingInUom.Id),
                            detail.CuttingInUom.Unit,
                            detail.RemainingQuantity,
                            detail.BasicPrice,
                            detail.Price,
                            detail.FC,
                            null
                        );

                        if (preparingItemToBeUpdated.ContainsKey(detail.PreparingItemId))
                        {
                            preparingItemToBeUpdated[detail.PreparingItemId] += (decimal)detail.PreparingQuantity;
                        }
                        else
                        {
                            preparingItemToBeUpdated.Add(detail.PreparingItemId, (decimal)detail.PreparingQuantity);
                        }

                        await _garmentSampleCuttingInDetailRepository.Update(garmentSampleCuttingInDetail);
                    }
                }

                await _garmentSampleCuttingInItemRepository.Update(garmentSampleCuttingInItem);
            }

            foreach (var preparingItem in preparingItemToBeUpdated)
            {
                var garmentSamplePreparingItem = _garmentSamplePreparingItemRepository.Query.Where(x => x.Identity == preparingItem.Key).Select(s => new GarmentSamplePreparingItem(s)).Single();
                garmentSamplePreparingItem.setRemainingQuantity(Convert.ToDouble((decimal)garmentSamplePreparingItem.RemainingQuantity - preparingItem.Value));
                garmentSamplePreparingItem.SetModified();

                await _garmentSamplePreparingItemRepository.Update(garmentSamplePreparingItem);
            }

            await _garmentSampleCuttingInRepository.Update(garmentSampleCuttingIn);

            _storage.Save();

            return garmentSampleCuttingIn;
        }

        private string GenerateCutInNo(PlaceGarmentSampleCuttingInCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"DC{request.Unit.Code.Trim()}{year}{month}";

            var lastCutInNo = _garmentSampleCuttingInRepository.Query.Where(w => w.CutInNo.StartsWith(prefix))
                .OrderByDescending(o => o.CutInNo)
                .Select(s => int.Parse(s.CutInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutInNo = $"{prefix}{(lastCutInNo + 1).ToString("D4")}";

            return CutInNo;
        }
    }
}
