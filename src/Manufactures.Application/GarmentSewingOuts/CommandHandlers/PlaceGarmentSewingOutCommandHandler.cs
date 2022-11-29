using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentSewingOuts;
using Manufactures.Domain.GarmentSewingOuts.Commands;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSewingOuts.CommandHandlers
{
    public class PlaceGarmentSewingOutCommandHandler : ICommandHandler<PlaceGarmentSewingOutCommand, GarmentSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSewingOutDetailRepository _garmentSewingOutDetailRepository;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;

        public PlaceGarmentSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSewingOutDetailRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
        }

        public async Task<GarmentSewingOut> Handle(PlaceGarmentSewingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            Guid sewingOutId = Guid.NewGuid();
            string sewingOutNo = GenerateSewOutNo(request);

            
            GarmentSewingOut garmentSewingOut = new GarmentSewingOut(
                sewingOutId,
                sewingOutNo,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                new UnitDepartmentId(request.UnitTo.Id),
                request.UnitTo.Code,
                request.UnitTo.Name,
                request.SewingTo,
                request.SewingOutDate.GetValueOrDefault(),
                request.RONo,
                request.Article,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name, 
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                request.IsDifferentSize
            );

            Dictionary<Guid, double> sewingInItemToBeUpdated = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    GarmentSewingOutItem garmentSewingOutItem = new GarmentSewingOutItem(
                        Guid.NewGuid(),
                        garmentSewingOut.Identity,
                        item.SewingInId,
                        item.SewingInItemId,
                        new ProductId(item.Product.Id),
                        item.Product.Code,
                        item.Product.Name,
                        item.DesignColor,
                        new SizeId(item.Size.Id),
                        item.Size.Size,
                        request.IsDifferentSize? item.TotalQuantity : item.Quantity,
                        new UomId(item.Uom.Id),
                        item.Uom.Unit,
                        item.Color,
                        request.SewingTo == "CUTTING" ? item.RemainingQuantity : 0,
                        item.BasicPrice,
                        item.Price
                    );
                    item.Id = garmentSewingOutItem.Identity;

                    if (request.IsDifferentSize)
                    {
                        foreach (var detail in item.Details)
                        {
                            GarmentSewingOutDetail garmentSewingOutDetail = new GarmentSewingOutDetail(
                                Guid.NewGuid(),
                                garmentSewingOutItem.Identity,
                                new SizeId(detail.Size.Id),
                                detail.Size.Size,
                                detail.Quantity,
                                new UomId(detail.Uom.Id),
                                detail.Uom.Unit
                            );
                            detail.Id = garmentSewingOutDetail.Identity;

                            if (sewingInItemToBeUpdated.ContainsKey(item.SewingInItemId))
                            {
                                sewingInItemToBeUpdated[item.SewingInItemId] += detail.Quantity;
                            }
                            else
                            {
                                sewingInItemToBeUpdated.Add(item.SewingInItemId, detail.Quantity);
                            }

                            await _garmentSewingOutDetailRepository.Update(garmentSewingOutDetail);

                        }
                    }
                    else
                    {
                        if (sewingInItemToBeUpdated.ContainsKey(item.SewingInItemId))
                        {
                            sewingInItemToBeUpdated[item.SewingInItemId] += item.Quantity;
                        }
                        else
                        {
                            sewingInItemToBeUpdated.Add(item.SewingInItemId, item.Quantity);
                        }
                    }
                    await _garmentSewingOutItemRepository.Update(garmentSewingOutItem);
                }
                
            }

            foreach (var sewInItem in sewingInItemToBeUpdated)
            {
                var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
                garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity - sewInItem.Value);
                garmentSewingInItem.Modify();

                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
            }


            await _garmentSewingOutRepository.Update(garmentSewingOut);

            #region CreateCuttingIn

            if (request.SewingTo == "CUTTING")
            {
                GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

                var now = DateTime.Now;
                var year = now.ToString("yy");
                var month = now.ToString("MM");

                var prefix = $"DC{request.UnitTo.Code.Trim()}{year}{month}";

                var lastCutInNo = _garmentCuttingInRepository.Query.Where(w => w.CutInNo.StartsWith(prefix))
                    .OrderByDescending(o => o.CutInNo)
                    .Select(s => int.Parse(s.CutInNo.Replace(prefix, "")))
                    .FirstOrDefault();
                var CutInNo = $"{prefix}{(lastCutInNo + 1).ToString("D4")}";

                GarmentCuttingIn garmentCuttingIn = new GarmentCuttingIn(
                    Guid.NewGuid(),
                    CutInNo,
                    null,
                    "SEWING",
                    request.RONo,
                    request.Article,
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    request.SewingOutDate.GetValueOrDefault(),
                    0
                    );

                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        GarmentCuttingInItem garmentCuttingInItem = new GarmentCuttingInItem(
                            Guid.NewGuid(),
                            garmentCuttingIn.Identity,
                            Guid.Empty,
                            0,
                            null,
                            sewingOutId,
                            sewingOutNo
                            );

                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(
                                    Guid.NewGuid(),
                                    garmentCuttingInItem.Identity,
                                    Guid.Empty,
                                    item.Id,
                                    detail.Id,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    null,
                                    0,
                                    new UomId(0),
                                    null,
                                    Convert.ToInt32(detail.Quantity),
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit,
                                    detail.Quantity,
                                    item.BasicPrice,
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price*25/100))*detail.Quantity ,
                                    0,
                                    item.Color
                                    );

                                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                            }
                        }
                        else
                        {
                            GarmentCuttingInDetail garmentCuttingInDetail = new GarmentCuttingInDetail(
                                    Guid.NewGuid(),
                                    garmentCuttingInItem.Identity,
                                    Guid.Empty,
                                    item.Id,
                                    Guid.Empty,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    null,
                                    0,
                                    new UomId(0),
                                    null,
                                    Convert.ToInt32(item.Quantity),
                                    new UomId(item.Uom.Id),
                                    item.Uom.Unit,
                                    item.Quantity,
                                    item.BasicPrice,
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * item.Quantity,
                                    0,
                                    item.Color
                                    );
                            await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                        }

                        await _garmentCuttingInItemRepository.Update(garmentCuttingInItem);
                    }
                }

                await _garmentCuttingInRepository.Update(garmentCuttingIn);
            }

            #endregion

            #region Create SewingIn
            if (request.SewingTo == "SEWING")
            {
                GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
                    Guid.NewGuid(),
                    GenerateSewingInNo(request),
                    "SEWING",
                    Guid.Empty,
                    "",
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    request.RONo,
                    request.Article,
                    new GarmentComodityId(request.Comodity.Id),
                    request.Comodity.Code,
                    request.Comodity.Name,
                    request.SewingOutDate.GetValueOrDefault()
                );
                await _garmentSewingInRepository.Update(garmentSewingIn);
                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(
                                    Guid.NewGuid(),
                                    garmentSewingIn.Identity,
                                    item.Id,
                                    detail.Id,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit,
                                    item.Color,
                                    detail.Quantity,
                                    item.BasicPrice,
                                    item.Price
                                );
                                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                            }
                        }
                        else
                        {
                            GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(
                                    Guid.NewGuid(),
                                    garmentSewingIn.Identity,
                                    item.Id,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    new SizeId(item.Size.Id),
                                    item.Size.Size,
                                    item.Quantity,
                                    new UomId(item.Uom.Id),
                                    item.Uom.Unit,
                                    item.Color,
                                    item.Quantity,
                                    item.BasicPrice,
                                    item.Price
                                );
                            await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                        }
                    }
                }
            }
            #endregion

            #region Create FinishingIn
            if (request.SewingTo == "FINISHING")
            {
                GarmentFinishingIn garmentFinishingIn = new GarmentFinishingIn(
                    Guid.NewGuid(),
                    GenerateFinishingInNo(request),
                    "SEWING",
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    request.RONo,
                    request.Article,
                    new UnitDepartmentId(request.UnitTo.Id),
                    request.UnitTo.Code,
                    request.UnitTo.Name,
                    request.SewingOutDate.GetValueOrDefault(),
                    new GarmentComodityId(request.Comodity.Id),
                    request.Comodity.Code,
                    request.Comodity.Name,
                    0,
                    null,
                    null
                );
                await _garmentFinishingInRepository.Update(garmentFinishingIn);
                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                GarmentFinishingInItem garmentFinishingInItem = new GarmentFinishingInItem(
                                    Guid.NewGuid(),
                                    garmentFinishingIn.Identity,
                                    item.Id,
                                    detail.Id,
                                    Guid.Empty,
                                    new SizeId(detail.Size.Id),
                                    detail.Size.Size,
                                    new ProductId(item.Product.Id),
                                    item.Product.Code,
                                    item.Product.Name,
                                    item.DesignColor,
                                    detail.Quantity,
                                    detail.Quantity,
                                    new UomId(detail.Uom.Id),
                                    detail.Uom.Unit,
                                    item.Color,
                                    item.BasicPrice,
                                    item.Price
                                );
                                await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                            }
                        }
                        else
                        {
                            GarmentFinishingInItem garmentFinishingInItem = new GarmentFinishingInItem(
                                Guid.NewGuid(),
                                garmentFinishingIn.Identity,
                                item.Id,
                                Guid.Empty,
                                Guid.Empty,
                                new SizeId(item.Size.Id),
                                item.Size.Size,
                                new ProductId(item.Product.Id),
                                item.Product.Code,
                                item.Product.Name,
                                item.DesignColor,
                                item.Quantity,
                                item.RemainingQuantity,
                                new UomId(item.Uom.Id),
                                item.Uom.Unit,
                                item.Color,
                                item.BasicPrice,
                                item.Price
                            );
                            await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                        }
                    }
                }
            }
            #endregion
            _storage.Save();

            return garmentSewingOut;
        }

        private string GenerateSewOutNo(PlaceGarmentSewingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"SO{request.Unit.Code.Trim()}{year}{month}";

            var lastSewOutNo = _garmentSewingOutRepository.Query.Where(w => w.SewingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingOutNo)
                .Select(s => int.Parse(s.SewingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewOutNo = $"{prefix}{(lastSewOutNo + 1).ToString("D4")}";

            return SewOutNo;
        }

        private string GenerateSewingInNo(PlaceGarmentSewingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"SI{request.UnitTo.Code}{year}{month}";

            var lastSewingInNo = _garmentSewingInRepository.Query.Where(w => w.SewingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingInNo)
                .Select(s => int.Parse(s.SewingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingInNo = $"{prefix}{(lastSewingInNo + 1).ToString("D4")}";

            return SewingInNo;
        }

        private string GenerateFinishingInNo(PlaceGarmentSewingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.UnitTo.Code;

            var prefix = $"FI{unitcode}{year}{month}";

            var lastFinishingInNo = _garmentFinishingInRepository.Query.Where(w => w.FinishingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingInNo)
                .Select(s => int.Parse(s.FinishingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastFinishingInNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}