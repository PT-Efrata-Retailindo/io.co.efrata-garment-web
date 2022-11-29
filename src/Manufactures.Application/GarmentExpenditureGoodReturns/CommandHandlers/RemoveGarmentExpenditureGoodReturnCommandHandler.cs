using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Commands;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods;
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

namespace Manufactures.Application.GarmentExpenditureGoodReturns.CommandHandlers
{
    public class RemoveGarmentExpenditureGoodReturnCommandHandler : ICommandHandler<RemoveGarmentExpenditureGoodReturnCommand, GarmentExpenditureGoodReturn>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodReturnRepository _garmentExpenditureGoodReturnRepository;
        private readonly IGarmentExpenditureGoodReturnItemRepository _garmentExpenditureGoodReturnItemRepository;
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;

        public RemoveGarmentExpenditureGoodReturnCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
            _garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
            _garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
        }

        public async Task<GarmentExpenditureGoodReturn> Handle(RemoveGarmentExpenditureGoodReturnCommand request, CancellationToken cancellationToken)
        {
            var ExpenditureGoodReturn = _garmentExpenditureGoodReturnRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentExpenditureGoodReturn(o)).Single();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == ExpenditureGoodReturn.UnitId && new GarmentComodityId(a.ComodityId) == ExpenditureGoodReturn.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            Dictionary<Guid, double> finStockToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<Guid, double> exGoodToBeUpdated = new Dictionary<Guid, double>();

            _garmentExpenditureGoodReturnItemRepository.Find(o => o.ReturId == ExpenditureGoodReturn.Identity).ForEach(async expenditureReturnItem =>
            {
                if (finStockToBeUpdated.ContainsKey(expenditureReturnItem.FinishedGoodStockId))
                {
                    finStockToBeUpdated[expenditureReturnItem.FinishedGoodStockId] += expenditureReturnItem.Quantity;
                }
                else
                {
                    finStockToBeUpdated.Add(expenditureReturnItem.FinishedGoodStockId, expenditureReturnItem.Quantity);
                }

                if (exGoodToBeUpdated.ContainsKey(expenditureReturnItem.ExpenditureGoodItemId))
                {
                    exGoodToBeUpdated[expenditureReturnItem.ExpenditureGoodItemId] += expenditureReturnItem.Quantity;
                }
                else
                {
                    exGoodToBeUpdated.Add(expenditureReturnItem.ExpenditureGoodItemId, expenditureReturnItem.Quantity);
                }

                GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(a => a.ExpenditureGoodReturnItemId == expenditureReturnItem.Identity).Select(a => new GarmentFinishedGoodStockHistory(a)).Single();
                garmentFinishedGoodStockHistory.Remove();
                await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);

                expenditureReturnItem.Remove();
                await _garmentExpenditureGoodReturnItemRepository.Update(expenditureReturnItem);
            });

            foreach (var finStock in finStockToBeUpdated)
            {
                var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == finStock.Key).Select(s => new GarmentFinishedGoodStock(s)).Single();
                var qty = garmentFinishingGoodStockItem.Quantity - finStock.Value;
                garmentFinishingGoodStockItem.SetQuantity(qty);
                garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                garmentFinishingGoodStockItem.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);
            }

            foreach (var exGood in exGoodToBeUpdated)
            {
                var garmentExpenditureGoodItem = _garmentExpenditureGoodItemRepository.Query.Where(x => x.Identity == exGood.Key).Select(s => new GarmentExpenditureGoodItem(s)).Single();
                var qty = garmentExpenditureGoodItem.ReturQuantity - exGood.Value;
                garmentExpenditureGoodItem.SetReturQuantity(qty);
                garmentExpenditureGoodItem.Modify();

                await _garmentExpenditureGoodItemRepository.Update(garmentExpenditureGoodItem);
            }

            ExpenditureGoodReturn.Remove();
            await _garmentExpenditureGoodReturnRepository.Update(ExpenditureGoodReturn);

            _storage.Save();

            return ExpenditureGoodReturn;
        }
    }
}