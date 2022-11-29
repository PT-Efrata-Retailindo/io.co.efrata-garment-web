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
    public class RemoveGarmentSampleFinishingOutCommandHandler : ICommandHandler<RemoveGarmentSampleFinishingOutCommand, GarmentSampleFinishingOut>
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

        public RemoveGarmentSampleFinishingOutCommandHandler(IStorage storage)
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

        public async Task<GarmentSampleFinishingOut> Handle(RemoveGarmentSampleFinishingOutCommand request, CancellationToken cancellationToken)
        {
            var finishOut = _GarmentSampleFinishingOutRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentSampleFinishingOut(o)).Single();

            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<GarmentSampleFinishedGoodStock, double> finGood = new Dictionary<GarmentSampleFinishedGoodStock, double>();

            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == finishOut.UnitToId && new GarmentComodityId(a.ComodityId) == finishOut.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            if (finishOut.FinishingTo == "SEWING")
            {
                Guid sewInId = Guid.Empty;
                _GarmentSampleFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).ForEach(async finOutItem =>
                {
                    var sewInItem = _GarmentSampleSewingInItemRepository.Query.Where(o => o.FinishingOutItemId == finOutItem.Identity).Select(o => new GarmentSampleSewingInItem(o)).Single();
                    sewInId = sewInItem.SewingInId;
                    if (finishOut.IsDifferentSize)
                    {
                        _GarmentSampleFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finOutItem.Identity).ForEach(async finOutDetail =>
                        {
                            sewInItem = _GarmentSampleSewingInItemRepository.Query.Where(o => o.FinishingOutDetailId == finOutDetail.Identity).Select(o => new GarmentSampleSewingInItem(o)).Single();

                            sewInItem.Remove();
                            await _GarmentSampleSewingInItemRepository.Update(sewInItem);
                        });
                    }
                    else
                    {
                        sewInItem.Remove();
                        await _GarmentSampleSewingInItemRepository.Update(sewInItem);
                    }
                });
                var sewIn = _GarmentSampleSewingInRepository.Query.Where(a => a.Identity == sewInId).Select(o => new GarmentSampleSewingIn(o)).Single();
                sewIn.Remove();
                await _GarmentSampleSewingInRepository.Update(sewIn);
            }

            _GarmentSampleFinishingOutItemRepository.Find(o => o.FinishingOutId == finishOut.Identity).ForEach(async finishOutItem =>
            {
                if (finishOut.IsDifferentSize)
                {
                    _GarmentSampleFinishingOutDetailRepository.Find(o => o.FinishingOutItemId == finishOutItem.Identity).ForEach(async finishOutDetail =>
                    {
                        if (finishingInItemToBeUpdated.ContainsKey(finishOutItem.FinishingInItemId))
                        {
                            finishingInItemToBeUpdated[finishOutItem.FinishingInItemId] += finishOutDetail.Quantity;
                        }
                        else
                        {
                            finishingInItemToBeUpdated.Add(finishOutItem.FinishingInItemId, finishOutDetail.Quantity);
                        }

                        if (finishOut.FinishingTo == "GUDANG JADI")
                        {
                            var GarmentSampleFinishedGoodExist = _GarmentSampleFinishedGoodStockRepository.Query.Where(
                            a => a.RONo == finishOut.RONo &&
                                a.Article == finishOut.Article &&
                                a.BasicPrice == finishOutItem.BasicPrice &&
                                new UnitDepartmentId(a.UnitId) == finishOut.UnitToId &&
                                new SizeId(a.SizeId) == finishOutDetail.SizeId &&
                                new GarmentComodityId(a.ComodityId) == finishOut.ComodityId &&
                                new UomId(a.UomId) == finishOutDetail.UomId
                            ).Select(s => new GarmentSampleFinishedGoodStock(s)).Single();

                            if (finGood.ContainsKey(GarmentSampleFinishedGoodExist))
                            {
                                finGood[GarmentSampleFinishedGoodExist] += finishOutDetail.Quantity;
                            }
                            else
                            {
                                finGood.Add(GarmentSampleFinishedGoodExist, finishOutDetail.Quantity);
                            }

                            GarmentSampleFinishedGoodStockHistory GarmentSampleFinishedGoodStockHistory = _GarmentSampleFinishedGoodStockHistoryRepository.Query.Where(a => a.FinishingOutDetailId == finishOutDetail.Identity).Select(a => new GarmentSampleFinishedGoodStockHistory(a)).Single();
                            GarmentSampleFinishedGoodStockHistory.Remove();
                            await _GarmentSampleFinishedGoodStockHistoryRepository.Update(GarmentSampleFinishedGoodStockHistory);

                        }

                        finishOutDetail.Remove();
                        await _GarmentSampleFinishingOutDetailRepository.Update(finishOutDetail);
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
                        var GarmentSampleFinishedGoodExist = _GarmentSampleFinishedGoodStockRepository.Query.Where(
                            a => a.RONo == finishOut.RONo &&
                                a.Article == finishOut.Article &&
                                a.BasicPrice == finishOutItem.BasicPrice &&
                                new UnitDepartmentId(a.UnitId) == finishOut.UnitToId &&
                                new SizeId(a.SizeId) == finishOutItem.SizeId &&
                                new GarmentComodityId(a.ComodityId) == finishOut.ComodityId &&
                                new UomId(a.UomId) == finishOutItem.UomId
                            ).Select(s => new GarmentSampleFinishedGoodStock(s)).Single();

                        if (finGood.ContainsKey(GarmentSampleFinishedGoodExist))
                        {
                            finGood[GarmentSampleFinishedGoodExist] += finishOutItem.Quantity;
                        }
                        else
                        {
                            finGood.Add(GarmentSampleFinishedGoodExist, finishOutItem.Quantity);
                        }
                        GarmentSampleFinishedGoodStockHistory GarmentSampleFinishedGoodStockHistory = _GarmentSampleFinishedGoodStockHistoryRepository.Query.Where(a => a.FinishingOutItemId == finishOutItem.Identity).Select(a => new GarmentSampleFinishedGoodStockHistory(a)).Single();
                        GarmentSampleFinishedGoodStockHistory.Remove();

                        await _GarmentSampleFinishedGoodStockHistoryRepository.Update(GarmentSampleFinishedGoodStockHistory);

                    }

                }


                finishOutItem.Remove();
                await _GarmentSampleFinishingOutItemRepository.Update(finishOutItem);
            });

            foreach (var finInItem in finishingInItemToBeUpdated)
            {
                var garmentSampleSewInItem = _GarmentSampleFinishingInItemRepository.Query.Where(x => x.Identity == finInItem.Key).Select(s => new GarmentSampleFinishingInItem(s)).Single();
                garmentSampleSewInItem.SetRemainingQuantity(garmentSampleSewInItem.RemainingQuantity + finInItem.Value);
                garmentSampleSewInItem.Modify();
                await _GarmentSampleFinishingInItemRepository.Update(garmentSampleSewInItem);
            }
            if (finishOut.FinishingTo == "GUDANG JADI")
            {
                foreach (var finGoodStock in finGood)
                {
                    var GarmentSampleFinishedGoodExist = _GarmentSampleFinishedGoodStockRepository.Query.Where(
                        a => a.Identity == finGoodStock.Key.Identity
                        ).Select(s => new GarmentSampleFinishedGoodStock(s)).Single();

                    var qty = GarmentSampleFinishedGoodExist.Quantity - finGoodStock.Value;

                    GarmentSampleFinishedGoodExist.SetQuantity(qty);
                    GarmentSampleFinishedGoodExist.SetPrice((GarmentSampleFinishedGoodExist.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                    GarmentSampleFinishedGoodExist.Modify();

                    await _GarmentSampleFinishedGoodStockRepository.Update(GarmentSampleFinishedGoodExist);
                }
            }


            finishOut.Remove();
            await _GarmentSampleFinishingOutRepository.Update(finishOut);

            _storage.Save();

            return finishOut;
        }
    }
}
