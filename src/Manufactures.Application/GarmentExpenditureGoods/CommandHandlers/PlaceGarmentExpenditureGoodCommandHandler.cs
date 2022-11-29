using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods;
using Manufactures.Domain.GarmentExpenditureGoods.Commands;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentExpenditureGoods.CommandHandlers
{
    public class PlaceGarmentExpenditureGoodCommandHandler : ICommandHandler<PlaceGarmentExpenditureGoodCommand, GarmentExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
		private readonly IGarmentExpenditureGoodInvoiceRelationRepository _garmentExpenditureGoodInvoiceRelationRepository;
		private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;

        public PlaceGarmentExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			_garmentExpenditureGoodInvoiceRelationRepository = storage.GetRepository<IGarmentExpenditureGoodInvoiceRelationRepository>();
		}

        public async Task<GarmentExpenditureGood> Handle(PlaceGarmentExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.ToList();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && a.UnitId == request.Unit.Id && a.ComodityId == request.Comodity.Id).Select(s => new GarmentComodityPrice(s)).Single();
			string ExpenditureGoodNo = GenerateExpenditureGoodNo(request);

			GarmentExpenditureGood garmentExpenditureGood = new GarmentExpenditureGood(
                Guid.NewGuid(),
			   ExpenditureGoodNo,
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
                    var garmentFinishingGoodStock = _garmentFinishedGoodStockRepository.Query.Where(x => x.SizeId == item.Size.Id && x.UomId == item.Uom.Id && x.RONo == request.RONo && x.UnitId == request.Unit.Id && x.Quantity > 0).OrderBy(a => a.CreatedDate).ToList();

                    double qty = item.Quantity;
                    foreach (var finishedGood in garmentFinishingGoodStock)
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

            foreach (var finStock in finStockToBeUpdated)
            {
                var keyString = finStock.Key.Split("~");

                var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == Guid.Parse(keyString[0])).Select(s => new GarmentFinishedGoodStock(s)).Single();

                var item = request.Items.Where(a => new SizeId(a.Size.Id) == garmentFinishingGoodStockItem.SizeId && new UomId(a.Uom.Id) == garmentFinishingGoodStockItem.UomId && a.Description == keyString[1]).Single();

                item.Price = (item.BasicPrice + ((double)garmentComodityPrice.Price * 1)) * item.Quantity;
                var qty = garmentFinishingGoodStockItem.Quantity - finStock.Value;

                GarmentExpenditureGoodItem garmentExpenditureGoodItem = new GarmentExpenditureGoodItem(
                    Guid.NewGuid(),
                    garmentExpenditureGood.Identity,
                    garmentFinishingGoodStockItem.Identity,
                    new SizeId(item.Size.Id),
                    item.Size.Size,
                    qty,
                    0,
                    new UomId(item.Uom.Id),
                    item.Uom.Unit,
                    item.Description,
                    garmentFinishingGoodStockItem.BasicPrice,
                    (garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * qty
                );

                await _garmentExpenditureGoodItemRepository.Update(garmentExpenditureGoodItem);

                GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = new GarmentFinishedGoodStockHistory(
                                            Guid.NewGuid(),
                                            garmentFinishingGoodStockItem.Identity,
                                            Guid.Empty,
                                            Guid.Empty,
                                            garmentExpenditureGood.Identity,
                                            garmentExpenditureGoodItem.Identity,
                                            Guid.Empty,
                                            Guid.Empty,
                                            Guid.Empty,
                                            Guid.Empty,
                                            "OUT",
                                            garmentExpenditureGood.RONo,
                                            garmentExpenditureGood.Article,
                                            garmentExpenditureGood.UnitId,
                                            garmentExpenditureGood.UnitCode,
                                            garmentExpenditureGood.UnitName,
                                            garmentExpenditureGood.ComodityId,
                                            garmentExpenditureGood.ComodityCode,
                                            garmentExpenditureGood.ComodityName,
                                            garmentExpenditureGoodItem.SizeId,
                                            garmentExpenditureGoodItem.SizeName,
                                            garmentExpenditureGoodItem.UomId,
                                            garmentExpenditureGoodItem.UomUnit,
                                            garmentExpenditureGoodItem.Quantity,
                                            garmentExpenditureGoodItem.BasicPrice,
                                            garmentExpenditureGoodItem.Price
                                        );
                await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);

                garmentFinishingGoodStockItem.SetQuantity(finStock.Value);
                garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (finStock.Value));
                garmentFinishingGoodStockItem.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);
				


			}

            await _garmentExpenditureGoodRepository.Update(garmentExpenditureGood);

            _storage.Save();
			var _expend = (from a in _garmentExpenditureGoodRepository.Query
						   where a.ExpenditureGoodNo == ExpenditureGoodNo
						   select a.Identity).FirstOrDefault();
			GarmentExpenditureGoodInvoiceRelation invoiceRelation = new GarmentExpenditureGoodInvoiceRelation(
				Guid.NewGuid(),
				_expend,
				ExpenditureGoodNo,
				request.Unit.Code,
				request.RONo,
				request.TotalQty,
				request.PackingListId,
				request.InvoiceId,
				request.Invoice

			);

			await _garmentExpenditureGoodInvoiceRelationRepository.Update(invoiceRelation);
			_storage.Save();

			return garmentExpenditureGood;
        }

        private string GenerateExpenditureGoodNo(PlaceGarmentExpenditureGoodCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var day = now.ToString("dd");
            var unitcode = request.Unit.Code;

            var pre = request.ExpenditureType == "EXPORT" ? "EGE" : request.ExpenditureType == "SISA" ? "EGS" : request.ExpenditureType == "LOKAL" ? "EGC" : "EGL";

            var prefix = $"{pre}{unitcode}{year}{month}";

            var lastExpenditureGoodNo = _garmentExpenditureGoodRepository.Query.Where(w => w.ExpenditureGoodNo.StartsWith(prefix))
                .OrderByDescending(o => o.ExpenditureGoodNo)
                .Select(s => int.Parse(s.ExpenditureGoodNo.Replace(prefix, "")))
                .FirstOrDefault();
            var finInNo = $"{prefix}{(lastExpenditureGoodNo + 1).ToString("D4")}";

            return finInNo;
        }
    }
}

