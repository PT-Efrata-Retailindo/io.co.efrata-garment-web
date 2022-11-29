using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingOuts;
using Manufactures.Domain.GarmentCuttingOuts.Commands;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentCuttingOuts.CommandHandlers
{
    public class PlaceGarmentCuttingOutCommandHandler : ICommandHandler<PlaceGarmentCuttingOutCommand, GarmentCuttingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentCuttingOutRepository _garmentCuttingOutRepository;
        private readonly IGarmentCuttingOutItemRepository _garmentCuttingOutItemRepository;
        private readonly IGarmentCuttingOutDetailRepository _garmentCuttingOutDetailRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSewingDORepository _garmentSewingDORepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentCuttingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
            _garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
            _garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
            _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
            _garmentComodityPriceRepository= storage.GetRepository<IGarmentComodityPriceRepository>();
        }

        public async Task<GarmentCuttingOut> Handle(PlaceGarmentCuttingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true && item.Details.Count() > 0).ToList();

            GarmentCuttingOut garmentCuttingOut = new GarmentCuttingOut(
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
                request.IsUsed
            );

            GarmentSewingDO garmentSewingDO = new GarmentSewingDO(
                Guid.NewGuid(),
                GenerateSewingDONo(request),
                garmentCuttingOut.Identity,
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
                GarmentCuttingOutItem garmentCuttingOutItem = new GarmentCuttingOutItem(
                    Guid.NewGuid(),
                    item.CuttingInId,
                    item.CuttingInDetailId,
                    garmentCuttingOut.Identity,
                    new ProductId(item.Product.Id),
                    item.Product.Code,
                    item.Product.Name,
                    item.DesignColor,
                    item.TotalCuttingOutQuantity
                );

                foreach (var detail in item.Details)
                {
                    GarmentCuttingOutDetail garmentCuttingOutDetail = new GarmentCuttingOutDetail(
                        Guid.NewGuid(),
                        garmentCuttingOutItem.Identity,
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

                    await _garmentCuttingOutDetailRepository.Update(garmentCuttingOutDetail);

                    GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).OrderBy(o => o.ModifiedDate).Select(s => new GarmentComodityPrice(s)).Last();
                    double price = (detail.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * detail.CuttingOutQuantity;
                    GarmentSewingDOItem garmentSewingDOItem = new GarmentSewingDOItem(
                        Guid.NewGuid(),
                        garmentSewingDO.Identity,
                        garmentCuttingOutDetail.Identity,
                        garmentCuttingOutItem.Identity,
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

                    await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);

                }
                await _garmentCuttingOutItemRepository.Update(garmentCuttingOutItem);
            }

            foreach (var cuttingInDetail in cuttingInDetailToBeUpdated)
            {
                var garmentCuttingInDetail = _garmentCuttingInDetailRepository.Query.Where(x => x.Identity == cuttingInDetail.Key).Select(s => new GarmentCuttingInDetail(s)).Single();
                garmentCuttingInDetail.SetRemainingQuantity(garmentCuttingInDetail.RemainingQuantity - cuttingInDetail.Value);
                garmentCuttingInDetail.Modify();

                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
            }

            await _garmentCuttingOutRepository.Update(garmentCuttingOut);

            await _garmentSewingDORepository.Update(garmentSewingDO);

            _storage.Save();

            return garmentCuttingOut;
        }

        private string GenerateCutOutNo(PlaceGarmentCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"CR{request.UnitFrom.Code}{year}{month}";

            var lastCutOutNo = _garmentCuttingOutRepository.Query.Where(w => w.CutOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.CutOutNo)
                .Select(s => int.Parse(s.CutOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var CutOutNo = $"{prefix}{(lastCutOutNo + 1).ToString("D4")}";

            return CutOutNo;
        }

        private string GenerateSewingDONo(PlaceGarmentCuttingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"DS{request.Unit.Code}{year}{month}";

            var lastSewingDONo = _garmentSewingDORepository.Query.Where(w => w.SewingDONo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingDONo)
                .Select(s => int.Parse(s.SewingDONo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingDONo = $"{prefix}{(lastSewingDONo + 1).ToString("D4")}";

            return SewingDONo;
        }
    }
}