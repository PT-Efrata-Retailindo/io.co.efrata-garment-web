using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Commands;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleFinishingOuts.CommandHandlers
{
    public class PlaceGarmentSampleFinishingOutCommandHandler : ICommandHandler<PlaceGarmentSampleFinishingOutCommand, GarmentSampleFinishingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleFinishingOutRepository _GarmentSampleFinishingOutRepository;
        private readonly IGarmentSampleFinishingOutItemRepository _GarmentSampleFinishingOutItemRepository;
        private readonly IGarmentSampleFinishingOutDetailRepository _GarmentSampleFinishingOutDetailRepository;
        private readonly IGarmentSampleFinishingInItemRepository _GarmentSampleFinishingInItemRepository;
        private readonly IGarmentSampleFinishedGoodStockRepository _GarmentSampleFinishedGoodStockRepository;
        private readonly IGarmentSampleFinishedGoodStockHistoryRepository _GarmentSampleFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentSampleSewingInRepository _GarmentSampleSewingInRepository;
        private readonly IGarmentSampleSewingInItemRepository _GarmentSampleSewingInItemRepository;

        public PlaceGarmentSampleFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
            _GarmentSampleFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
            _GarmentSampleFinishingOutDetailRepository = storage.GetRepository<IGarmentSampleFinishingOutDetailRepository>();
            _GarmentSampleFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
            _GarmentSampleFinishedGoodStockRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockRepository>();
            _GarmentSampleFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _GarmentSampleSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
            _GarmentSampleSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
        }

        public async Task<GarmentSampleFinishingOut> Handle(PlaceGarmentSampleFinishingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.UnitTo.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();
            Guid GarmentSampleFinishingOutId = Guid.NewGuid();
            GarmentSampleFinishingOut GarmentSampleFinishingOut = new GarmentSampleFinishingOut(
                GarmentSampleFinishingOutId,
                GenerateFinOutNo(request),
                new UnitDepartmentId(request.UnitTo.Id),
                request.UnitTo.Code,
                request.UnitTo.Name,
                request.FinishingTo,
                request.FinishingOutDate.GetValueOrDefault(),
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

            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();

            Dictionary<string, double> finGood = new Dictionary<string, double>();

            foreach (var item in request.Items)
            {
                if (item.IsSave)
                {
                    Guid GarmentSampleFinishingOutItemId = Guid.NewGuid();
                    GarmentSampleFinishingOutItem GarmentSampleFinishingOutItem = new GarmentSampleFinishingOutItem(
                        GarmentSampleFinishingOutItemId,
                        GarmentSampleFinishingOut.Identity,
                        item.FinishingInId,
                        item.FinishingInItemId,
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
                        request.IsDifferentSize ? item.TotalQuantity : item.Quantity,
                        item.BasicPrice,
                        item.Price
                    );
                    item.Id = GarmentSampleFinishingOutItemId;
                    if (request.IsDifferentSize)
                    {
                        foreach (var detail in item.Details)
                        {
                            Guid GarmentSampleFinishingOutDetailId = Guid.NewGuid();
                            GarmentSampleFinishingOutDetail GarmentSampleFinishingOutDetail = new GarmentSampleFinishingOutDetail(
                                GarmentSampleFinishingOutDetailId,
                                GarmentSampleFinishingOutItem.Identity,
                                new SizeId(detail.Size.Id),
                                detail.Size.Size,
                                detail.Quantity,
                                new UomId(detail.Uom.Id),
                                detail.Uom.Unit
                            );
                            detail.Id = GarmentSampleFinishingOutDetailId;
                            if (finishingInItemToBeUpdated.ContainsKey(item.FinishingInItemId))
                            {
                                finishingInItemToBeUpdated[item.FinishingInItemId] += detail.Quantity;
                            }
                            else
                            {
                                finishingInItemToBeUpdated.Add(item.FinishingInItemId, detail.Quantity);
                            }

                            await _GarmentSampleFinishingOutDetailRepository.Update(GarmentSampleFinishingOutDetail);

                            if (request.FinishingTo == "GUDANG JADI")
                            {
                                string finStock = detail.Size.Id + "~" + detail.Size.Size + "~" + detail.Uom.Id + "~" + detail.Uom.Unit + "~" + item.BasicPrice;

                                if (finGood.ContainsKey(finStock))
                                {
                                    finGood[finStock] += detail.Quantity;
                                }
                                else
                                {
                                    finGood.Add(finStock, detail.Quantity);
                                }
                            }

                        }
                    }
                    else
                    {
                        if (finishingInItemToBeUpdated.ContainsKey(item.FinishingInItemId))
                        {
                            finishingInItemToBeUpdated[item.FinishingInItemId] += item.Quantity;
                        }
                        else
                        {
                            finishingInItemToBeUpdated.Add(item.FinishingInItemId, item.Quantity);
                        }

                        if (request.FinishingTo == "GUDANG JADI")
                        {
                            string finStock = item.Size.Id + "~" + item.Size.Size + "~" + item.Uom.Id + "~" + item.Uom.Unit + "~" + item.BasicPrice;

                            if (finGood.ContainsKey(finStock))
                            {
                                finGood[finStock] += item.Quantity;
                            }
                            else
                            {
                                finGood.Add(finStock, item.Quantity);
                            }
                        }

                    }
                    await _GarmentSampleFinishingOutItemRepository.Update(GarmentSampleFinishingOutItem);
                }
            }

            foreach (var finInItem in finishingInItemToBeUpdated)
            {
                var garmentSampleFinishingInItem = _GarmentSampleFinishingInItemRepository.Query.Where(x => x.Identity == finInItem.Key).Select(s => new GarmentSampleFinishingInItem(s)).Single();
                garmentSampleFinishingInItem.SetRemainingQuantity(garmentSampleFinishingInItem.RemainingQuantity - finInItem.Value);
                garmentSampleFinishingInItem.Modify();

                await _GarmentSampleFinishingInItemRepository.Update(garmentSampleFinishingInItem);
            }

            if (request.FinishingTo == "GUDANG JADI")
            {
                int count = 1;
                List<GarmentSampleFinishedGoodStock> finGoodStocks = new List<GarmentSampleFinishedGoodStock>();
                foreach (var finGoodStock in finGood)
                {
                    SizeId sizeId = new SizeId(Convert.ToInt32(finGoodStock.Key.Split("~")[0]));
                    string sizeName = finGoodStock.Key.Split("~")[1];
                    UomId uomId = new UomId(Convert.ToInt32(finGoodStock.Key.Split("~")[2]));
                    string uomUnit = finGoodStock.Key.Split("~")[3];
                    double basicPrice = Convert.ToDouble(finGoodStock.Key.Split("~")[4]);
                    var GarmentSampleFinishedGoodExist = _GarmentSampleFinishedGoodStockRepository.Query.Where(
                        a => a.RONo == request.RONo &&
                            a.Article == request.Article &&
                            a.BasicPrice == basicPrice &&
                            a.UnitId == request.UnitTo.Id &&
                            new SizeId(a.SizeId) == sizeId &&
                            a.ComodityId == request.Comodity.Id &&
                            new UomId(a.UomId) == uomId
                        ).Select(s => new GarmentSampleFinishedGoodStock(s)).SingleOrDefault();

                    double qty = GarmentSampleFinishedGoodExist == null ? finGoodStock.Value : (finGoodStock.Value + GarmentSampleFinishedGoodExist.Quantity);

                    double price = (basicPrice + (double)garmentComodityPrice.Price) * qty;

                    if (GarmentSampleFinishedGoodExist == null)
                    {
                        var now = DateTime.Now;
                        var year = now.ToString("yy");
                        var month = now.ToString("MM");
                        var prefix = $"ST{request.UnitTo.Code.Trim()}{year}{month}";

                        var lastFnGoodNo = _GarmentSampleFinishedGoodStockRepository.Query.Where(w => w.FinishedGoodStockNo.StartsWith(prefix))
                        .OrderByDescending(o => o.FinishedGoodStockNo)
                        .Select(s => int.Parse(s.FinishedGoodStockNo.Replace(prefix, "")))
                        .FirstOrDefault();
                        var FinGoodNo = $"{prefix}{(lastFnGoodNo + count).ToString("D4")}";
                        GarmentSampleFinishedGoodStock finishedGood = new GarmentSampleFinishedGoodStock(
                                        Guid.NewGuid(),
                                        FinGoodNo,
                                        request.RONo,
                                        request.Article,
                                        new UnitDepartmentId(request.UnitTo.Id),
                                        request.UnitTo.Code,
                                        request.UnitTo.Name,
                                        new GarmentComodityId(request.Comodity.Id),
                                        request.Comodity.Code,
                                        request.Comodity.Name,
                                        sizeId,
                                        sizeName,
                                        uomId,
                                        uomUnit,
                                        qty,
                                        basicPrice,
                                        price
                                        );
                        count++;
                        await _GarmentSampleFinishedGoodStockRepository.Update(finishedGood);
                        finGoodStocks.Add(finishedGood);
                    }
                    else
                    {
                        GarmentSampleFinishedGoodExist.SetQuantity(qty);
                        GarmentSampleFinishedGoodExist.SetPrice(price);
                        GarmentSampleFinishedGoodExist.Modify();

                        await _GarmentSampleFinishedGoodStockRepository.Update(GarmentSampleFinishedGoodExist);
                        var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                                 a.Article == request.Article &&
                                 a.BasicPrice == GarmentSampleFinishedGoodExist.BasicPrice &&
                                 a.UnitId == new UnitDepartmentId(request.UnitTo.Id) &&
                                 a.SizeId == GarmentSampleFinishedGoodExist.SizeId &&
                                 a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                                 a.UomId == GarmentSampleFinishedGoodExist.UomId).SingleOrDefault();
                        finGoodStocks.Add(GarmentSampleFinishedGoodExist);
                    }

                }

                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                                 a.Article == request.Article &&
                                 a.BasicPrice == item.BasicPrice &&
                                 a.UnitId == new UnitDepartmentId(request.UnitTo.Id) &&
                                 a.SizeId == new SizeId(detail.Size.Id) &&
                                 a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                                 a.UomId == new UomId(detail.Uom.Id)).Single();

                                double price = (stock.BasicPrice + (double)garmentComodityPrice.Price) * detail.Quantity;

                                GarmentSampleFinishedGoodStockHistory GarmentSampleFinishedGoodStockHistory = new GarmentSampleFinishedGoodStockHistory(
                                        Guid.NewGuid(),
                                        stock.Identity,
                                        item.Id,
                                        detail.Id,
                                        Guid.Empty,
                                        Guid.Empty,
                                        Guid.Empty,
                                        Guid.Empty,
                                        Guid.Empty,
                                        Guid.Empty,
                                        "IN",
                                        stock.RONo,
                                        stock.Article,
                                        stock.UnitId,
                                        stock.UnitCode,
                                        stock.UnitName,
                                        stock.ComodityId,
                                        stock.ComodityCode,
                                        stock.ComodityName,
                                        stock.SizeId,
                                        stock.SizeName,
                                        stock.UomId,
                                        stock.UomUnit,
                                        detail.Quantity,
                                        stock.BasicPrice,
                                        price
                                    );
                                await _GarmentSampleFinishedGoodStockHistoryRepository.Update(GarmentSampleFinishedGoodStockHistory);
                            }
                        }
                        else
                        {
                            var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                             a.Article == request.Article &&
                             a.BasicPrice == item.BasicPrice &&
                             a.UnitId == new UnitDepartmentId(request.UnitTo.Id) &&
                             a.SizeId == new SizeId(item.Size.Id) &&
                             a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                             a.UomId == new UomId(item.Uom.Id)).Single();

                            double price = (stock.BasicPrice + (double)garmentComodityPrice.Price) * item.Quantity;

                            GarmentSampleFinishedGoodStockHistory GarmentSampleFinishedGoodStockHistory = new GarmentSampleFinishedGoodStockHistory(
                                    Guid.NewGuid(),
                                    stock.Identity,
                                    item.Id,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    Guid.Empty,
                                    "IN",
                                    stock.RONo,
                                    stock.Article,
                                    stock.UnitId,
                                    stock.UnitCode,
                                    stock.UnitName,
                                    stock.ComodityId,
                                    stock.ComodityCode,
                                    stock.ComodityName,
                                    stock.SizeId,
                                    stock.SizeName,
                                    stock.UomId,
                                    stock.UomUnit,
                                    item.Quantity,
                                    stock.BasicPrice,
                                    price
                                );
                            await _GarmentSampleFinishedGoodStockHistoryRepository.Update(GarmentSampleFinishedGoodStockHistory);
                        }
                    }

                }
            }
            #region Create SewingIn
            if (request.FinishingTo == "SEWING")
            {
                GarmentSampleSewingIn garmentSampleSewingIn = new GarmentSampleSewingIn(
                    Guid.NewGuid(),
                    GenerateSewingInNo(request),
                    "FINISHING",
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
                    request.FinishingOutDate.GetValueOrDefault()
                );
                await _GarmentSampleSewingInRepository.Update(garmentSampleSewingIn);

                foreach (var item in request.Items)
                {
                    if (item.IsSave)
                    {
                        if (request.IsDifferentSize)
                        {
                            foreach (var detail in item.Details)
                            {
                                GarmentSampleSewingInItem garmentSampleSewingInItem = new GarmentSampleSewingInItem(
                                    Guid.NewGuid(),
                                    garmentSampleSewingIn.Identity,
                                    Guid.Empty,
                                    Guid.Empty,
                                    item.Id,
                                    detail.Id,
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
                                await _GarmentSampleSewingInItemRepository.Update(garmentSampleSewingInItem);
                            }
                        }
                        else
                        {
                            GarmentSampleSewingInItem GarmentSampleSewingInItem = new GarmentSampleSewingInItem(
                                    Guid.NewGuid(),
                                    garmentSampleSewingIn.Identity,
                                    Guid.Empty,
                                    Guid.Empty,
                                    item.Id,
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
                            await _GarmentSampleSewingInItemRepository.Update(GarmentSampleSewingInItem);
                        }
                    }
                }
            }
            #endregion

            await _GarmentSampleFinishingOutRepository.Update(GarmentSampleFinishingOut);

            _storage.Save();

            return GarmentSampleFinishingOut;
        }

        private string GenerateFinOutNo(PlaceGarmentSampleFinishingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"FO{request.Unit.Code.Trim()}{year}{month}";

            var lastFinOutNo = _GarmentSampleFinishingOutRepository.Query.Where(w => w.FinishingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingOutNo)
                .Select(s => int.Parse(s.FinishingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewOutNo = $"{prefix}{(lastFinOutNo + 1).ToString("D4")}";

            return SewOutNo;
        }

        private string GenerateFinGoodNo(GarmentSampleFinishedGoodStock request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"ST{request.UnitCode.Trim()}{year}{month}";

            var lastFnGoodNo = _GarmentSampleFinishedGoodStockRepository.Query.Where(w => w.FinishedGoodStockNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishedGoodStockNo)
                .Select(s => int.Parse(s.FinishedGoodStockNo.Replace(prefix, "")))
                .FirstOrDefault();
            var FinGoodNo = $"{prefix}{(lastFnGoodNo + 1).ToString("D4")}";

            return FinGoodNo;
        }

        private string GenerateSewingInNo(PlaceGarmentSampleFinishingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var prefix = $"SI{request.UnitTo.Code}{year}{month}";

            var lastSewingInNo = _GarmentSampleSewingInRepository.Query.Where(w => w.SewingInNo.StartsWith(prefix))
                .OrderByDescending(o => o.SewingInNo)
                .Select(s => int.Parse(s.SewingInNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewingInNo = $"{prefix}{(lastSewingInNo + 1).ToString("D4")}";

            return SewingInNo;
        }

    }

}