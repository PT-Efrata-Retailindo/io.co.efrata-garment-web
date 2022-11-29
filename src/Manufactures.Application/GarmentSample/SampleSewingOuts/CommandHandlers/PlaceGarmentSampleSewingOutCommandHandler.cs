using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleSewingOuts.CommandHandlers
{
    public class PlaceGarmentSampleSewingOutCommandHandler : ICommandHandler<PlaceGarmentSampleSewingOutCommand, GarmentSampleSewingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleSewingOutRepository _garmentSewingOutRepository;
        private readonly IGarmentSampleSewingOutItemRepository _garmentSewingOutItemRepository;
        private readonly IGarmentSampleSewingOutDetailRepository _garmentSewingOutDetailRepository;
       // private readonly IGarmentSampleSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentSampleCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentSampleFinishingInRepository _garmentFinishingInRepository;
        private readonly IGarmentSampleFinishingInItemRepository _garmentFinishingInItemRepository;

        public PlaceGarmentSampleSewingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
            _garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
            _garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSampleSewingOutDetailRepository>();
           // _garmentSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentFinishingInRepository = storage.GetRepository<IGarmentSampleFinishingInRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();

        }

        public async Task<GarmentSampleSewingOut> Handle(PlaceGarmentSampleSewingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            Guid sewingOutId = Guid.NewGuid();
            string sewingOutNo = GenerateSewOutNo(request);


            GarmentSampleSewingOut garmentSewingOut = new GarmentSampleSewingOut(
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
                    GarmentSampleSewingOutItem garmentSewingOutItem = new GarmentSampleSewingOutItem(
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
                        request.IsDifferentSize ? item.TotalQuantity : item.Quantity,
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
                            GarmentSampleSewingOutDetail garmentSewingOutDetail = new GarmentSampleSewingOutDetail(
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
                var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewInItem.Key).Select(s => new GarmentSampleSewingInItem(s)).Single();
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

                GarmentSampleCuttingIn garmentCuttingIn = new GarmentSampleCuttingIn(
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
                        GarmentSampleCuttingInItem garmentCuttingInItem = new GarmentSampleCuttingInItem(
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
                                GarmentSampleCuttingInDetail garmentCuttingInDetail = new GarmentSampleCuttingInDetail(
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
                                    (item.BasicPrice + ((double)garmentComodityPrice.Price * 25 / 100)) * detail.Quantity,
                                    0,
                                    item.Color
                                    );

                                await _garmentCuttingInDetailRepository.Update(garmentCuttingInDetail);
                            }
                        }
                        else
                        {
                            GarmentSampleCuttingInDetail garmentCuttingInDetail = new GarmentSampleCuttingInDetail(
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



            #region Create FinishingIn
            if (request.SewingTo == "FINISHING")
            {
                GarmentSampleFinishingIn garmentFinishingIn = new GarmentSampleFinishingIn(
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
                                GarmentSampleFinishingInItem garmentFinishingInItem = new GarmentSampleFinishingInItem(
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
                                    item.Price,0
                                );
                                await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
                            }
                        }
                        else
                        {
                             
                            GarmentSampleFinishingInItem garmentFinishingInItem = new GarmentSampleFinishingInItem(
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
                                item.Price,
                                0
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

        private string GenerateSewOutNo(PlaceGarmentSampleSewingOutCommand request)
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

      

        private string GenerateFinishingInNo(PlaceGarmentSampleSewingOutCommand request)
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

