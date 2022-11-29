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
    public class RemoveGarmentExpenditureGoodCommandHandler : ICommandHandler<RemoveGarmentExpenditureGoodCommand, GarmentExpenditureGood>
    {
        private readonly IStorage _storage;
        private readonly IGarmentExpenditureGoodRepository _garmentExpenditureGoodRepository;
        private readonly IGarmentExpenditureGoodItemRepository _garmentExpenditureGoodItemRepository;
        private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
        private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
		private readonly IGarmentExpenditureGoodInvoiceRelationRepository _garmentExpenditureGoodInvoiceRelationRepository;

		public RemoveGarmentExpenditureGoodCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
            _garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
            _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
            _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
            _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			_garmentExpenditureGoodInvoiceRelationRepository = storage.GetRepository<IGarmentExpenditureGoodInvoiceRelationRepository>();

		}

		public async Task<GarmentExpenditureGood> Handle(RemoveGarmentExpenditureGoodCommand request, CancellationToken cancellationToken)
        {
            var ExpenditureGood = _garmentExpenditureGoodRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentExpenditureGood(o)).Single();
			var invoiceRelation = _garmentExpenditureGoodInvoiceRelationRepository.Query.Where(o => o.ExpenditureGoodId == ExpenditureGood.Identity).Select(o => new GarmentExpenditureGoodInvoiceRelation(o)).Single();
			GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == ExpenditureGood.UnitId && new GarmentComodityId(a.ComodityId) == ExpenditureGood.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            Dictionary<Guid, double> finStockToBeUpdated = new Dictionary<Guid, double>();

            _garmentExpenditureGoodItemRepository.Find(o => o.ExpenditureGoodId == ExpenditureGood.Identity).ForEach(async expenditureItem =>
            {
                if (finStockToBeUpdated.ContainsKey(expenditureItem.FinishedGoodStockId))
                {
                    finStockToBeUpdated[expenditureItem.FinishedGoodStockId] += expenditureItem.Quantity;
                }
                else
                {
                    finStockToBeUpdated.Add(expenditureItem.FinishedGoodStockId, expenditureItem.Quantity);
                }

                GarmentFinishedGoodStockHistory garmentFinishedGoodStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(a => a.ExpenditureGoodItemId == expenditureItem.Identity).Select(a => new GarmentFinishedGoodStockHistory(a)).Single();
                garmentFinishedGoodStockHistory.Remove();
                await _garmentFinishedGoodStockHistoryRepository.Update(garmentFinishedGoodStockHistory);

                expenditureItem.Remove();
                await _garmentExpenditureGoodItemRepository.Update(expenditureItem);
            });

            foreach (var finStock in finStockToBeUpdated)
            {
                var garmentFinishingGoodStockItem = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == finStock.Key).Select(s => new GarmentFinishedGoodStock(s)).Single();
                var qty = garmentFinishingGoodStockItem.Quantity + finStock.Value;
                garmentFinishingGoodStockItem.SetQuantity(qty);
                garmentFinishingGoodStockItem.SetPrice((garmentFinishingGoodStockItem.BasicPrice + (double)garmentComodityPrice.Price) * (qty));
                garmentFinishingGoodStockItem.Modify();

                await _garmentFinishedGoodStockRepository.Update(garmentFinishingGoodStockItem);
            }
			if (invoiceRelation != null)
			{
				invoiceRelation.Remove();
				await _garmentExpenditureGoodInvoiceRelationRepository.Update(invoiceRelation);
			}
            ExpenditureGood.Remove();
            await _garmentExpenditureGoodRepository.Update(ExpenditureGood);

            _storage.Save();

            return ExpenditureGood;
        }
    }
}
