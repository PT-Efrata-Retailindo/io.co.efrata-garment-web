using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
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
    public class RemoveGarmentFinishingOutCommandHandler : ICommandHandler<RemoveGarmentFinishingOutCommand, GarmentFinishingOut>
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

        public RemoveGarmentFinishingOutCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
            _garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
            _garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentFinishingOutDetailRepository>();
            _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
            _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
        }

        public async Task<GarmentFinishingOut> Handle(RemoveGarmentFinishingOutCommand request, CancellationToken cancellationToken)
        {
            var finishOut = _garmentFinishingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentFinishingOut(o)).Single();

            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<GarmentFinishedGoodStock, double> finGood = new Dictionary<GarmentFinishedGoodStock, double>();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == finishOut.UnitToId && new GarmentComodityId( a.ComodityId) == finishOut.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            if (finishOut.FinishingTo == "SEWING")
            {
                Guid sewInId = Guid.Empty;
                _garmentFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).ForEach(async finOutItem =>
                {
                    var sewInItem = _garmentSewingInItemRepository.Query.Where(o => o.FinishingOutItemId == finOutItem.Identity).Select(o => new GarmentSewingInItem(o)).Single();
                    sewInId = sewInItem.SewingInId;
                    if (finishOut.IsDifferentSize)
                    {
                        _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finOutItem.Identity).ForEach(async finOutDetail =>
                        {
                            sewInItem = _garmentSewingInItemRepository.Query.Where(o => o.FinishingOutDetailId == finOutDetail.Identity).Select(o => new GarmentSewingInItem(o)).Single();

                            sewInItem.Remove();
                            await _garmentSewingInItemRepository.Update(sewInItem);
                        });
                    }
                    else
                    {
                        sewInItem.Remove();
                        await _garmentSewingInItemRepository.Update(sewInItem);
                    }
                });
                var sewIn = _garmentSewingInRepository.Query.Where(a => a.Identity == sewInId).Select(o => new GarmentSewingIn(o)).Single();
                sewIn.Remove();
                await _garmentSewingInRepository.Update(sewIn);
            }

            _garmentFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).ForEach(async finishOutItem =>
            {
                if (finishOut.IsDifferentSize)
                {
                    _garmentFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).ForEach(async finishOutDetail =>
                    {
                        if (finishingInItemToBeUpdated.ContainsKey(finishOutItem.FinishingInItemId))
                        {
                            finishingInItemToBeUpdated[finishOutItem.FinishingInItemId] += finishOutDetail.Quantity;
                        }
                        else
                        {
                            finishingInItemToBeUpdated.Add(finishOutItem.FinishingInItemId, finishOutDetail.Quantity);
                        }

                        if(finishOut.FinishingTo=="GUDANG JADI")
                        {
                            var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                            a => a.RONo == finishOut.RONo &&
                                a.Article == finishOut.Article &&
                                a.BasicPrice == finishOutItem.BasicPrice &&
                                new UnitDepartmentId(a.UnitId) == finishOut.UnitToId &&
                                new SizeId(a.SizeId) == finishOutDetail.SizeId &&
                                new GarmentComodityId(a.ComodityId) == finishOut.ComodityId &&
                                new UomId(a.UomId) == finishOutDetail.UomId
                            ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                            if (finGood.ContainsKey(garmentFinishedGoodExist))
                            {
                                finGood[garmentFinishedGoodExist] += finishOutDetail.Quantity;
                            }
                            else
                            {
                                finGood.Add(garmentFinishedGoodExist, finishOutDetail.Quantity);
                            }

                            GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(a => a.FinishingOutDetailId == finishOutDetail.Identity).Select(a => new GarmentFinishedGoodStockHistory(a)).Single();
                            garmentFinishedGoodStockHistory.Remove();
                            await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);

                        }

                        finishOutDetail.Remove();
                        await _garmentFinishingOutDetailRepository.Update(finishOutDetail);
                    });
                }
                else
                {
                    if (finishingInItemToBeUpdated.ContainsKey(finishOutItem.FinishingInItemId))
                    {
                        finishingInItemToBeUpdated[finishOutItem.FinishingInItemId] += finishOutItem.Quantity;
                    }
                    else
                    {
                        finishingInItemToBeUpdated.Add(finishOutItem.FinishingInItemId, finishOutItem.Quantity);
                    }

                    if (finishOut.FinishingTo == "GUDANG JADI")
                    {
                        var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                            a => a.RONo == finishOut.RONo &&
                                a.Article == finishOut.Article &&
                                a.BasicPrice == finishOutItem.BasicPrice &&
                                new UnitDepartmentId(a.UnitId) == finishOut.UnitToId &&
                                new SizeId(a.SizeId) == finishOutItem.SizeId &&
                                new GarmentComodityId(a.ComodityId) == finishOut.ComodityId &&
                                new UomId(a.UomId) == finishOutItem.UomId
                            ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                        if (finGood.ContainsKey(garmentFinishedGoodExist))
                        {
                            finGood[garmentFinishedGoodExist] += finishOutItem.Quantity;
                        }
                        else
                        {
                            finGood.Add(garmentFinishedGoodExist, finishOutItem.Quantity);
                        }
                        GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(a => a.FinishingOutItemId == finishOutItem.Identity).Select(a => new GarmentFinishedGoodStockHistory(a)).Single();
                        garmentFinishedGoodStockHistory.Remove();
                        
                        await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);

                    }

                }


                finishOutItem.Remove();
                await _garmentFinishingOutItemRepository.Update(finishOutItem);
            });

            foreach (var finInItem in finishingInItemToBeUpdated)
            {
                var garmentSewInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finInItem.Key).Select(s => new GarmentFinishingInItem(s)).Single();
                garmentSewInItem.SetRemainingQuantity(garmentSewInItem.RemainingQuantity + finInItem.Value);
                garmentSewInItem.Modify();
                await _garmentFinishingInItemRepository.Update(garmentSewInItem);
            }
            if (finishOut.FinishingTo == "GUDANG JADI")
            {
                foreach (var finGoodStock in finGood)
                {
                    var garmentFinishedGoodExist = _garmentFinishedGoodStockRepository.Query.Where(
                        a => a.Identity == finGoodStock.Key.Identity
                        ).Select(s => new GarmentFinishedGoodStock(s)).Single();

                    var qty = garmentFinishedGoodExist.Quantity - finGoodStock.Value;

                    garmentFinishedGoodExist.SetQuantity(qty);
                    garmentFinishedGoodExist.SetPrice((garmentFinishedGoodExist.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                    garmentFinishedGoodExist.Modify();

                    await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodExist);
                }
            }
            

            finishOut.Remove();
            await _garmentFinishingOutRepository.Update(finishOut);

            _storage.Save();

            return finishOut;
        }
    }
}
