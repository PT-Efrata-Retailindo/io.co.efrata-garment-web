using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Infrastructure.External.DanLirisClient.Microservice;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Commands;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks;
using Manufactures.Domain.GarmentSample.SampleFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentSample.SampleStocks;
using Manufactures.Domain.GarmentSample.SampleStocks.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentSample.SampleExpenditureGoods.CommandHandlers
{
    public class PlaceGarmentSampleExpenditureGoodCommandHandler : ICommandHandler<PlaceGarmentSampleExpenditureGoodCommand, GarmentSampleExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentSampleExpenditureGoodRepository _GarmentSampleExpenditureGoodRepository;
        private readonly IGarmentSampleExpenditureGoodItemRepository _GarmentSampleExpenditureGoodItemRepository;
        private readonly IGarmentSampleFinishedGoodStockRepository _GarmentSampleFinishedGoodStockRepository;
        private readonly IGarmentSampleFinishedGoodStockHistoryRepository _GarmentSampleFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentSampleStockRepository _GarmentSampleStockRepository;
        private readonly IGarmentSampleStockHistoryRepository _GarmentSampleStockHistoryRepository;

        public PlaceGarmentSampleExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _GarmentSampleExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
            _GarmentSampleExpenditureGoodItemRepository = storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();
            _GarmentSampleFinishedGoodStockRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockRepository>();
            _GarmentSampleFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentSampleFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _GarmentSampleStockRepository = storage.GetRepository<IGarmentSampleStockRepository>();
            _GarmentSampleStockHistoryRepository = storage.GetRepository<IGarmentSampleStockHistoryRepository>();
        }

        public async Task<GarmentSampleExpenditureGood> Handle(PlaceGarmentSampleExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();

            GarmentSampleExpenditureGood GarmentSampleExpenditureGood = new GarmentSampleExpenditureGood(
                Guid.NewGuid(),
                GenerateExpenditureGoodNo(request),
                request.ExpenditureType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.RONo,
                request.Article,
                new GarmentComodityId(request.Comodity.Id),
                request.Comodity.Code,
                request.Comodity.Name,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                request.ExpenditureDate,
                request.Invoice,
                request.ContractNo,
                request.Carton,
                request.Description,
                request.IsReceived,
                request.PackingListId
            );

            Dictionary<string, double> finStockToBeUpdated = new Dictionary<string, double>();
            Dictionary<Guid, double> finstockQty = new Dictionary<Guid, double>();

            foreach (var item in request.Items)
            {
                if (item.isSave)
                {
                    double StockQty = 0;
                    var GarmentSampleFinishingGoodStock = _GarmentSampleFinishedGoodStockRepository.Query.Where(x => x.SizeId == item.Size.Id && x.UomId == item.Uom.Id && x.RONo == request.RONo && x.UnitId == request.Unit.Id && x.Quantity > 0).OrderBy(a => a.CreatedDate).ToList();

                    double qty = item.Quantity;
                    foreach (var finishedGood in GarmentSampleFinishingGoodStock)
                    {
                        if (!finstockQty.ContainsKey(finishedGood.Identity))
                        {
                            finstockQty.Add(finishedGood.Identity, finishedGood.Quantity);
                        }
                        string key = finishedGood.Identity.ToString() + "~" + item.Description;
                        if (qty > 0)
                        {
                            double remainQty = finstockQty[finishedGood.Identity] - qty;
                            if (remainQty < 0)
                            {
                                qty -= finstockQty[finishedGood.Identity];
                                finStockToBeUpdated.Add(key, 0);
                                finstockQty[finishedGood.Identity] = 0;
                            }
                            else if (remainQty == 0)
                            {
                                finStockToBeUpdated.Add(key, 0);
                                finstockQty[finishedGood.Identity] = remainQty;
                                break;
                            }
                            else if (remainQty > 0)
                            {
                                finStockToBeUpdated.Add(key, remainQty);
                                finstockQty[finishedGood.Identity] = remainQty;
                                break;
                            }
                        }
                    }
                }
            }

            int count = 1;
            List<GarmentSampleStock> stocks = new List<GarmentSampleStock>();
            foreach (var finStock in finStockToBeUpdated)
            {
                var keyString = finStock.Key.Split("~");

                var GarmentSampleFinishingGoodStockItem = _GarmentSampleFinishedGoodStockRepository.Query.Where(x => x.Identity == Guid.Parse(keyString[0])).Select(s => new GarmentSampleFinishedGoodStock(s)).Single();

                var item = request.Items.Where(a => new SizeId(a.Size.Id) == GarmentSampleFinishingGoodStockItem.SizeId && new UomId(a.Uom.Id) == GarmentSampleFinishingGoodStockItem.UomId && a.Description == keyString[1]).Single();

                item.Price = (item.BasicPrice + ((double)garmentComodityPrice.Price * 1)) * item.Quantity;
                var qty = GarmentSampleFinishingGoodStockItem.Quantity - finStock.Value;

                GarmentSampleExpenditureGoodItem garmentSampleExpenditureGoodItem = new GarmentSampleExpenditureGoodItem(
                    Guid.NewGuid(),
                    GarmentSampleExpenditureGood.Identity,
                    GarmentSampleFinishingGoodStockItem.Identity,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    qty,
                    0,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Description,
                    GarmentSampleFinishingGoodStockItem.BasicPrice,
                    (GarmentSampleFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * qty
                );

                await _GarmentSampleExpenditureGoodItemRepository.Update(garmentSampleExpenditureGoodItem);

                GarmentSampleFinishedGoodStockHistory garmentSampleFinishedGoodStockHistory = new GarmentSampleFinishedGoodStockHistory(
                                            Guid.NewGuid(),
                                            GarmentSampleFinishingGoodStockItem.Identity,
                                            Guid.Empty,
                                            Guid.Empty,
                                            GarmentSampleExpenditureGood.Identity,
                                            garmentSampleExpenditureGoodItem.Identity,
                                            Guid.Empty,
                                            Guid.Empty,
                                            Guid.Empty,
                                            Guid.Empty,
                                            "OUT",
                                            GarmentSampleExpenditureGood.RONo,
                                            GarmentSampleExpenditureGood.Article,
                                            GarmentSampleExpenditureGood.UnitId,
                                            GarmentSampleExpenditureGood.UnitCode,
                                            GarmentSampleExpenditureGood.UnitName,
                                            GarmentSampleExpenditureGood.ComodityId,
                                            GarmentSampleExpenditureGood.ComodityCode,
                                            GarmentSampleExpenditureGood.ComodityName,
                                            garmentSampleExpenditureGoodItem.SizeId,
                                            garmentSampleExpenditureGoodItem.SizeName,
                                            garmentSampleExpenditureGoodItem.UomId,
                                            garmentSampleExpenditureGoodItem.UomUnit,
                                            garmentSampleExpenditureGoodItem.Quantity,
                                            garmentSampleExpenditureGoodItem.BasicPrice,
                                            garmentSampleExpenditureGoodItem.Price
                                        );
                await _GarmentSampleFinishedGoodStockHistoryRepository.Update(garmentSampleFinishedGoodStockHistory);

                GarmentSampleFinishingGoodStockItem.SetQuantity(finStock.Value);
                GarmentSampleFinishingGoodStockItem.SetPrice((GarmentSampleFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (finStock.Value));
                GarmentSampleFinishingGoodStockItem.Modify();

                await _GarmentSampleFinishedGoodStockRepository.Update(GarmentSampleFinishingGoodStockItem);


                if(request.ExpenditureType== "ARSIP MD" || request.ExpenditureType == "ARSIP SAMPLE")
                {
                    var existStock= _GarmentSampleStockRepository.Query.Where(
                        a => a.RONo == request.RONo 
                        && a.Article == request.Article 
                        && new SizeId(a.SizeId) == new SizeId(item.Size.Id) 
                        && a.ComodityId == request.Comodity.Id 
                        && new UomId(a.UomId) == new UomId(item.Uom.Id) 
                        && a.ArchiveType== request.ExpenditureType
                        && a.Description==item.Description
                        ).Select(s => new GarmentSampleStock(s)).SingleOrDefault();

                    if (existStock == null)
                    {
                        GarmentSampleStock stock = new GarmentSampleStock(
                                        Guid.NewGuid(),
                                        GenerateStockNo(request,count),
                                        request.ExpenditureType,
                                        request.RONo,
                                        request.Article,
                                        new GarmentComodityId(request.Comodity.Id),
                                        request.Comodity.Code,
                                        request.Comodity.Name,
                                        new SizeId(item.Size.Id),
                                        item.Size.Size,
                                        new UomId(item.Uom.Id),
                                        item.Uom.Unit,
                                        qty,
                                        item.Description
                                        );
                        count++;
                        await _GarmentSampleStockRepository.Update(stock);
                        stocks.Add(stock);
                    }
                    else
                    {
                        existStock.SetQuantity(existStock.Quantity+qty);
                        existStock.Modify();

                        await _GarmentSampleStockRepository.Update(existStock);
                        var stock = stocks.Where(a => a.RONo == request.RONo 
                                && a.Article == request.Article 
                                && a.SizeId == new SizeId(item.Size.Id)
                                && a.ComodityId == new GarmentComodityId( request.Comodity.Id)
                                && a.UomId == new UomId(item.Uom.Id)
                                && a.ArchiveType == request.ExpenditureType).SingleOrDefault();
                        stocks.Add(existStock);
                    }

                    GarmentSampleStockHistory garmentSampleStockHistory = new GarmentSampleStockHistory(
                                            Guid.NewGuid(),
                                            GarmentSampleExpenditureGood.Identity,
                                            garmentSampleExpenditureGoodItem.Identity,
                                            "IN",
                                            request.ExpenditureType,
                                            GarmentSampleExpenditureGood.RONo,
                                            GarmentSampleExpenditureGood.Article,
                                            GarmentSampleExpenditureGood.ComodityId,
                                            GarmentSampleExpenditureGood.ComodityCode,
                                            GarmentSampleExpenditureGood.ComodityName,
                                            garmentSampleExpenditureGoodItem.SizeId,
                                            garmentSampleExpenditureGoodItem.SizeName,
                                            garmentSampleExpenditureGoodItem.UomId,
                                            garmentSampleExpenditureGoodItem.UomUnit,
                                            garmentSampleExpenditureGoodItem.Quantity,
                                            item.Description
                                        );
                    await _GarmentSampleStockHistoryRepository.Update(garmentSampleStockHistory);

                }

            }

            await _GarmentSampleExpenditureGoodRepository.Update(GarmentSampleExpenditureGood);

            _storage.Save();

            return GarmentSampleExpenditureGood;
        }

        private string GenerateExpenditureGoodNo(PlaceGarmentSampleExpenditureGoodCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var pre = request.ExpenditureType == "EXPORT" ? "EGE" : request.ExpenditureType == "SISA" ? "EGS" : request.ExpenditureType == "ARSIP MD" ? "EGAM" : "EGAS";

            var prefix = $"{pre}{unitcode}{year}{month}";

            var lastExpenditureGoodNo = _GarmentSampleExpenditureGoodRepository.Query.Where(w => w.ExpenditureGoodNo.StartsWith(prefix))
                .OrderByDescending(o => o.ExpenditureGoodNo)
                .Select(s => int.Parse(s.ExpenditureGoodNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastExpenditureGoodNo + 1).ToString("D4")}";

            return finInNo;
        }

        private string GenerateStockNo(PlaceGarmentSampleExpenditureGoodCommand request,int count)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var prefix = $"STA{request.Unit.Code.Trim()}{year}{month}";

            var lastStockNo = _GarmentSampleStockRepository.Query.Where(w => w.SampleStockNo.StartsWith(prefix))
                .OrderByDescending(o => o.SampleStockNo)
                .Select(s => int.Parse(s.SampleStockNo.Replace(prefix, "")))
                .FirstOrDefault();
            var StockNo = $"{prefix}{(lastStockNo + count).ToString("D4")}";

            return StockNo;
        }
    }
}
