using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Data.EntityFrameworkCore.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentFinishingOuts;
using Manufactures.Domain.GarmentFinishingOuts.Commands;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentFinishingOuts.CommandHandlers
{
    public class PlaceGarmentFinishingOutCommandHandler : ICommandHandler<PlaceGarmentFinishingOutCommand, GarmentFinishingOut>
    {
        private readonly IStorage _storage;
        private readonly IGarmentFinishingOutRepository _garmentFinishingOutRepository;
        private readonly IGarmentFinishingOutItemRepository _garmentFinishingOutItemRepository;
        private readonly IGarmentFinishingOutDetailRepository _garmentFinishingOutDetailRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentSewingInRepository _garmentSewingInRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;

        public PlaceGarmentFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository= storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        public async Task<GarmentFinishingOut> Handle(PlaceGarmentFinishingOutCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.IsSave == true).ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.UnitTo.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();
            Guid garmentFinishingOutId = Guid.NewGuid();
            GarmentFinishingOut garmentFinishingOut = new GarmentFinishingOut(
                garmentFinishingOutId,
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
                    Guid garmentFinishingOutItemId = Guid.NewGuid();
                    GarmentFinishingOutItem garmentFinishingOutItem = new GarmentFinishingOutItem(
                        garmentFinishingOutItemId,
                        garmentFinishingOut.Identity,
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
                    item.Id = garmentFinishingOutItemId;
                    if (request.IsDifferentSize)
                    {
                        foreach (var detail in item.Details)
                        {
                            Guid garmentFinishingOutDetailId = Guid.NewGuid();
                            GarmentFinishingOutDetail garmentFinishingOutDetail = new GarmentFinishingOutDetail(
                                garmentFinishingOutDetailId,
                                garmentFinishingOutItem.Identity,
                                new SizeId(detail.Size.Id),
                                detail.Size.Size,
                                detail.Quantity,
                                new UomId(detail.Uom.Id),
                                detail.Uom.Unit
                            );
                            detail.Id = garmentFinishingOutDetailId;
                            if (finishingInItemToBeUpdated.ContainsKey(item.FinishingInItemId))
                            {
                                finishingInItemToBeUpdated[item.FinishingInItemId] += detail.Quantity;
                            }
                            else
                            {
                                finishingInItemToBeUpdated.Add(item.FinishingInItemId, detail.Quantity);
                            }

                            await _garmentFinishingOutDetailRepository.Update(garmentFinishingOutDetail);

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
                    await _garmentFinishingOutItemRepository.Update(garmentFinishingOutItem);
                }
            }

            foreach (var finInItem in finishingInItemToBeUpdated)
            {
                var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finInItem.Key).Select(s => new GarmentFinishingInItem(s)).Single();
                garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity - finInItem.Value);
                garmentFinishingInItem.Modify();

                await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
            }

            if(request.FinishingTo=="GUDANG JADI")
            {
                int count = 1;
                List<GarmentFinishedGoodStock> finGoodStocks = new List<GarmentFinishedGoodStock>();
                foreach (var finGoodStock in finGood)
                {
                    SizeId sizeId = new SizeId(Convert.ToInt32(finGoodStock.Key.Split("~")[0]));
                    string sizeName = finGoodStock.Key.Split("~")[1];
                    UomId uomId = new UomId(Convert.ToInt32(finGoodStock.Key.Split("~")[2]));
                    string uomUnit = finGoodStock.Key.Split("~")[3];
                    double basicPrice = Convert.ToDouble(finGoodStock.Key.Split("~")[4]);
                    var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                        a => a.RONo == request.RONo &&
                            a.Article == request.Article &&
                            a.BasicPrice == basicPrice &&
                            a.UnitId == request.UnitTo.Id &&
                            new SizeId(a.SizeId) == sizeId &&
                            a.ComodityId == request.Comodity.Id &&
                            new UomId(a.UomId) == uomId
                        ).Select(s => new GarmentFinishedGoodStock(s)).SingleOrDefault();

                    double qty = garmentFinishedGoodExist == null ? finGoodStock.Value : (finGoodStock.Value + garmentFinishedGoodExist.Quantity);

                    double price = (basicPrice + (double)garmentComodityPrice.Price) * qty;

                    if (garmentFinishedGoodExist == null)
                    {
                        var now = DateTime.Now;
                        var year = now.ToString("yy");
                        var month = now.ToString("MM");
                        var prefix = $"ST{request.UnitTo.Code.Trim()}{year}{month}";

                        var lastFnGoodNo = _garmentFinishedGoodStockRepository.Query.Where(w => w.FinishedGoodStockNo.StartsWith(prefix))
                        .OrderByDescending(o => o.FinishedGoodStockNo)
                        .Select(s => int.Parse(s.FinishedGoodStockNo.Replace(prefix, "")))
                        .FirstOrDefault();
                        var FinGoodNo = $"{prefix}{(lastFnGoodNo + count).ToString("D4")}";
                        GarmentFinishedGoodStock finishedGood = new GarmentFinishedGoodStock(
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
                        await _garmentFinishedGoodStockRepository.Update(finishedGood);
                        finGoodStocks.Add(finishedGood);
                    }
                    else
                    {
                        garmentFinishedGoodExist.SetQuantity(qty);
                        garmentFinishedGoodExist.SetPrice(price);
                        garmentFinishedGoodExist.Modify();

                        await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodExist);
                        var stock = finGoodStocks.Where(a => a.RONo == request.RONo &&
                                 a.Article == request.Article &&
                                 a.BasicPrice == garmentFinishedGoodExist.BasicPrice &&
                                 a.UnitId == new UnitDepartmentId(request.UnitTo.Id) &&
                                 a.SizeId == garmentFinishedGoodExist.SizeId &&
                                 a.ComodityId == new GarmentComodityId(request.Comodity.Id) &&
                                 a.UomId == garmentFinishedGoodExist.UomId).SingleOrDefault();
                        finGoodStocks.Add(garmentFinishedGoodExist);
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

                                GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
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
                                await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
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

                            GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
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
                            await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);
                        }
                    }

                }
            }
            #region Create SewingIn
            if (request.FinishingTo == "SEWING")
            {
                GarmentSewingIn garmentSewingIn = new GarmentSewingIn(
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
                                    Guid.Empty,
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
                                await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                            }
                        }
                        else
                        {
                            GarmentSewingInItem garmentSewingInItem = new GarmentSewingInItem(
                                    Guid.NewGuid(),
                                    garmentSewingIn.Identity,
                                    Guid.Empty,
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
                            await _garmentSewingInItemRepository.Update(garmentSewingInItem);
                        }
                    }
                }
            }
            #endregion

            await _garmentFinishingOutRepository.Update(garmentFinishingOut);

            _storage.Save();

            return garmentFinishingOut;
        }

        private string GenerateFinOutNo(PlaceGarmentFinishingOutCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"FO{request.Unit.Code.Trim()}{year}{month}";

            var lastFinOutNo = _garmentFinishingOutRepository.Query.Where(w => w.FinishingOutNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishingOutNo)
                .Select(s => int.Parse(s.FinishingOutNo.Replace(prefix, "")))
                .FirstOrDefault();
            var SewOutNo = $"{prefix}{(lastFinOutNo + 1).ToString("D4")}";

            return SewOutNo;
        }

        private string GenerateFinGoodNo(GarmentFinishedGoodStock request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"ST{request.UnitCode.Trim()}{year}{month}";

            var lastFnGoodNo = _garmentFinishedGoodStockRepository.Query.Where(w => w.FinishedGoodStockNo.StartsWith(prefix))
                .OrderByDescending(o => o.FinishedGoodStockNo)
                .Select(s => int.Parse(s.FinishedGoodStockNo.Replace(prefix, "")))
                .FirstOrDefault();
            var FinGoodNo = $"{prefix}{(lastFnGoodNo + 1).ToString("D4")}";

            return FinGoodNo;
        }

        private string GenerateSewingInNo(PlaceGarmentFinishingOutCommand request)
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

    }

    //class FinishedGood
    //{
    //    public GarmentComodity comodity;
    //    public SizeValueObject size;
    //    public UnitDepartment unit;
    //    public double basicPrice;
    //}
}