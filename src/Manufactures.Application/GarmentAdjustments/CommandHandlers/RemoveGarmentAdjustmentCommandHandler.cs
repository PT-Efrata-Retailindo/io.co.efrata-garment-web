using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentAdjustments;
using Manufactures.Domain.GarmentAdjustments.Commands;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentComodityPrices;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentFinishedGoodStocks;
using Manufactures.Domain.GarmentFinishedGoodStocks.Repositories;
using Manufactures.Domain.GarmentFinishingIns;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentSewingDOs;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentSewingIns;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentAdjustments.CommandHandlers
{
    public class RemoveGarmentAdjustmentCommandHandler : ICommandHandler<RemoveGarmentAdjustmentCommand, GarmentAdjustment>
    {
        private readonly IStorage _storage;
        private readonly IGarmentAdjustmentRepository _garmentAdjustmentRepository;
        private readonly IGarmentAdjustmentItemRepository _garmentAdjustmentItemRepository;
        private readonly IGarmentSewingDOItemRepository _garmentSewingDOItemRepository;
        private readonly IGarmentSewingInItemRepository _garmentSewingInItemRepository;
        private readonly IGarmentFinishingInItemRepository _garmentFinishingInItemRepository;
		private readonly IGarmentFinishedGoodStockRepository _garmentFinishedGoodStockRepository;
		private readonly IGarmentFinishedGoodStockHistoryRepository _garmentFinishedGoodStockHistoryRepository;
        private readonly IGarmentComodityPriceRepository _garmentComodityPriceRepository;
    

    public RemoveGarmentAdjustmentCommandHandler(IStorage storage)
    {
        _storage = storage;
        _garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
        _garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
        _garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
        _garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
        _garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
        _garmentFinishedGoodStockRepository = storage.GetRepository<IGarmentFinishedGoodStockRepository>();
        _garmentFinishedGoodStockHistoryRepository = storage.GetRepository<IGarmentFinishedGoodStockHistoryRepository>();
        _garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
    }

        public async Task<GarmentAdjustment> Handle(RemoveGarmentAdjustmentCommand request, CancellationToken cancellationToken)
        {
            var adjustment = _garmentAdjustmentRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentAdjustment(o)).Single();
            GarmentComodityPrice garmentComodityPrice = _garmentComodityPriceRepository.Query.Where(a => a.IsValid == true && new UnitDepartmentId(a.UnitId) == adjustment.UnitId && new GarmentComodityId(a.ComodityId) == adjustment.ComodityId).Select(s => new GarmentComodityPrice(s)).Single();

            Dictionary<Guid, double> sewingDOItemToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<Guid, double> sewingInItemToBeUpdated = new Dictionary<Guid, double>();
            Dictionary<Guid, double> finishingInItemToBeUpdated = new Dictionary<Guid, double>();
			Dictionary<Guid, double> finishedGoodItemToBeUpdated = new Dictionary<Guid, double>();
			List<GarmentFinishedGoodStock> finGoodStocks = new List<GarmentFinishedGoodStock>();

			_garmentAdjustmentItemRepository.Find(o => o.AdjustmentId == adjustment.Identity).ForEach(async adjustmentItem =>
            {
                if (adjustment.AdjustmentType == "LOADING")
                {
                    if (sewingDOItemToBeUpdated.ContainsKey(adjustmentItem.SewingDOItemId))
                    {
                        sewingDOItemToBeUpdated[adjustmentItem.SewingDOItemId] += adjustmentItem.Quantity;
                    }
                    else
                    {
                        sewingDOItemToBeUpdated.Add(adjustmentItem.SewingDOItemId, adjustmentItem.Quantity);
                    }
                }
                else if (adjustment.AdjustmentType == "SEWING")
                {
                    if (sewingInItemToBeUpdated.ContainsKey(adjustmentItem.SewingInItemId))
                    {
                        sewingInItemToBeUpdated[adjustmentItem.SewingInItemId] += adjustmentItem.Quantity;
                    }
                    else
                    {
                        sewingInItemToBeUpdated.Add(adjustmentItem.SewingInItemId, adjustmentItem.Quantity);
                    }
                }
                else if (adjustment.AdjustmentType == "FINISHING")
                {
                    if (finishingInItemToBeUpdated.ContainsKey(adjustmentItem.FinishingInItemId))
                    {
                        finishingInItemToBeUpdated[adjustmentItem.FinishingInItemId] += adjustmentItem.Quantity;
                    }
                    else
                    {
                        finishingInItemToBeUpdated.Add(adjustmentItem.FinishingInItemId, adjustmentItem.Quantity);
                    }
                }else
                {
					if (finishedGoodItemToBeUpdated.ContainsKey(adjustmentItem.FinishedGoodStockId))
					{
						finishedGoodItemToBeUpdated[adjustmentItem.FinishedGoodStockId] += adjustmentItem.Quantity;
					}
					else
					{

						finishedGoodItemToBeUpdated.Add(adjustmentItem.FinishedGoodStockId, adjustmentItem.Quantity);
					}
				}

                adjustmentItem.Remove();

                await _garmentAdjustmentItemRepository.Update(adjustmentItem);
            });

			if (adjustment.AdjustmentType == "LOADING")
			{
				foreach (var sewingDOItem in sewingDOItemToBeUpdated)
				{
					var garmentSewingDOItem = _garmentSewingDOItemRepository.Query.Where(x => x.Identity == sewingDOItem.Key).Select(s => new GarmentSewingDOItem(s)).Single();
					garmentSewingDOItem.setRemainingQuantity(garmentSewingDOItem.RemainingQuantity + sewingDOItem.Value);
					garmentSewingDOItem.Modify();

					await _garmentSewingDOItemRepository.Update(garmentSewingDOItem);
				}
			}
			else if (adjustment.AdjustmentType == "SEWING")
			{
				foreach (var sewingInItem in sewingInItemToBeUpdated)
				{
					var garmentSewingInItem = _garmentSewingInItemRepository.Query.Where(x => x.Identity == sewingInItem.Key).Select(s => new GarmentSewingInItem(s)).Single();
					garmentSewingInItem.SetRemainingQuantity(garmentSewingInItem.RemainingQuantity + sewingInItem.Value);
					garmentSewingInItem.Modify();

					await _garmentSewingInItemRepository.Update(garmentSewingInItem);
				}
			}
			else if (adjustment.AdjustmentType == "FINISHING")
			{
				foreach (var finishingInItem in finishingInItemToBeUpdated)
				{
					var garmentFinishingInItem = _garmentFinishingInItemRepository.Query.Where(x => x.Identity == finishingInItem.Key).Select(s => new GarmentFinishingInItem(s)).Single();
					garmentFinishingInItem.SetRemainingQuantity(garmentFinishingInItem.RemainingQuantity + finishingInItem.Value);
					garmentFinishingInItem.Modify();

					await _garmentFinishingInItemRepository.Update(garmentFinishingInItem);
				}
			}
			else
			{
				foreach (var data in finishedGoodItemToBeUpdated)
				{
					var garmentFinishedGoodstock = _garmentFinishedGoodStockRepository.Query.Where(x => x.Identity == data.Key).Select(s => new GarmentFinishedGoodStock(s)).Single();
                    var qty = garmentFinishedGoodstock.Quantity + data.Value;
                    garmentFinishedGoodstock.SetQuantity(qty);
                    garmentFinishedGoodstock.SetPrice((garmentFinishedGoodstock.BasicPrice + (double)garmentComodityPrice.Price) * (qty));

                    garmentFinishedGoodstock.Modify();
					await _garmentFinishedGoodStockRepository.Update(garmentFinishedGoodstock);
				}
				var stockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(o => o.AdjustmentId == adjustment.Identity).Select(o => new GarmentFinishedGoodStockHistory(o));
				foreach (var data in stockHistory)
				{
					var dataStockHistory = _garmentFinishedGoodStockHistoryRepository.Query.Where(o => o.Identity == data.Identity).Select(o => new GarmentFinishedGoodStockHistory(o)).Single();
					dataStockHistory.Remove();
					await _garmentFinishedGoodStockHistoryRepository.Update(dataStockHistory);
				}
			}
			
			adjustment.Remove();
            await _garmentAdjustmentRepository.Update(adjustment);
            _storage.Save();
            return adjustment;
        }
    }
}