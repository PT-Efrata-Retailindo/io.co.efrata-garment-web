using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentComodityPrices;

namespace Manufactures.Application.GarmentSample.SampleCuttingOuts.CommandHandlers
{
    public class PlaceGarmentSampleCuttingOutCommandHandler : ICommandHandler<PlaceGarmentSampleCuttingOutCommand, GarmentSampleCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleCuttingOutRepository _GarmentSampleCuttingOutRepository;
        private readonly IGarmentSampleCuttingOutItemRepository _GarmentSampleCuttingOutItemRepository;
        private readonly IGarmentSampleCuttingOutDetailRepository _GarmentSampleCuttingOutDetailRepository;
        private readonly IGarmentSampleSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentSampleCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
            _GarmentSampleCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
            _GarmentSampleCuttingOutDetailRepository = storage.GetRepository<IGarmentSampleCuttingOutDetailRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentSampleCuttingOut> Handle(PlaceGarmentSampleCuttingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true && item.Details.Count() > 0).ToList();

            GarmentSampleCuttingOut GarmentSampleCuttingOut = new GarmentSampleCuttingOut(
                Guid.NewGuid(),
                GenerateCutOutNo(request),
                request.CuttingOutType,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                request.CuttingOutDate.GetValueOrDefault(),
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                true
            );

            GarmentSampleSewingIn garmentSewingIn = new GarmentSampleSewingIn(
                Guid.NewGuid(),
                GenerateSewingInNo(request),
                "CUTTING",
                GarmentSampleCuttingOut.Identity,
                GarmentSampleCuttingOut.CutOutNo,
                new UnitDepartmentId(request.UnitFrom.Id),
                request.UnitFrom.Code,
                request.UnitFrom.Name,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.CuttingOutDate.GetValueOrDefault()
            );

            Dictionary<Guid, double> cuttingInDetailToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                GarmentSampleCuttingOutItem GarmentSampleCuttingOutItem = new GarmentSampleCuttingOutItem(
                    Guid.NewGuid(),
                    item.CuttingInId,
                    item.CuttingInDetailId,
                    GarmentSampleCuttingOut.Identity,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.TotalCuttingOutQuantity
                );

                foreach (var detail in item.Details)
                {
                    GarmentSampleCuttingOutDetail GarmentSampleCuttingOutDetail = new GarmentSampleCuttingOutDetail(
                        Guid.NewGuid(),
                        GarmentSampleCuttingOutItem.Identity,
                        new SizeId(detail.Size.Id),
                        detail.Size.Size,
                        detail.Color.ToUpper(),
                        0,
                        detail.CuttingOutQuantity,
                        new UomId(detail.CuttingOutUom.Id),
                        detail.CuttingOutUom.Unit,
                        detail.BasicPrice,
                        detail.Price
                    );

                    if (cuttingInDetailToBeUpdated.ContainsKey(item.CuttingInDetailId))
                    {
                        cuttingInDetailToBeUpdated[item.CuttingInDetailId] += detail.CuttingOutQuantity;
                    }
                    else
                    {
                        cuttingInDetailToBeUpdated.Add(item.CuttingInDetailId, detail.CuttingOutQuantity);
                    }

                    await _GarmentSampleCuttingOutDetailRepository.Update(GarmentSampleCuttingOutDetail);

                    GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).OrderBy(o => o.ModifiedDate).Select(s => new GarmentComodityPrice(s)).Last();
                    double price = (detail.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * detail.CuttingOutQuantity;
                    GarmentSampleSewingInItem garmentSewingInItem = new GarmentSampleSewingInItem(
                        Guid.NewGuid(),
                        garmentSewingIn.Identity,
                        GarmentSampleCuttingOutDetail.Identity,
                        GarmentSampleCuttingOutItem.Identity,
                        Guid.Empty,
                        Guid.Empty,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        new SizeId(detail.Size.Id),
                        detail.Size.Size,
                        detail.CuttingOutQuantity,
                        new UomId(detail.CuttingOutUom.Id),
                        detail.CuttingOutUom.Unit,
                        detail.Color.ToUpper(),
                        detail.CuttingOutQuantity,
                        detail.BasicPrice,
                        price
                    );

                    await _garmentSewingInItemRepository.Update(garmentSewingInItem);

                }
                await _GarmentSampleCuttingOutItemRepository.Update(GarmentSampleCuttingOutItem);
            }

            foreach (var cuttingInDetail in cuttingInDetailToBeUpdated)
            {
                var GarmentSampleCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInDetail.Key).Select(s => new GarmentSampleCuttingInDetail(s)).Single();
                GarmentSampleCuttingInDetail.SetRemainingQuantity(GarmentSampleCuttingInDetail.RemainingQuantity - cuttingInDetail.Value);
                GarmentSampleCuttingInDetail.Modify();

                await _garmentCuttingInDetailRepository.Update(GarmentSampleCuttingInDetail);
            }

            await _GarmentSampleCuttingOutRepository.Update(GarmentSampleCuttingOut);

            await _garmentSewingInRepository.Update(garmentSewingIn);

            _storage.Save();

            return GarmentSampleCuttingOut;
        }

        private string GenerateCutOutNo(PlaceGarmentSampleCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"CR{request.UnitFrom.Code}{year}{month}";

            var lastCutOutNo = _GarmentSampleCuttingOutRepository.Query.Where(w => w.CutOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.CutOutNo)
                .Select(s => int.Parse(s.CutOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutOutNo = $"{prefix}{(lastCutOutNo + 1).ToString("D4")}";

            return CutOutNo;
        }

        private string GenerateSewingInNo(PlaceGarmentSampleCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"SI{request.Unit.Code}{year}{month}";

            var lastSewingInNo = _garmentSewingInRepository.Query.Where(w => w.SewingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingInNo)
                .Select(s => int.Parse(s.SewingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingInNo = $"{prefix}{(lastSewingInNo + 1).ToString("D4")}";

            return SewingInNo;
        }
    }
}
