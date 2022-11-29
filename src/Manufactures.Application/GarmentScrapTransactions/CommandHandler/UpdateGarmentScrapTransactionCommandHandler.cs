using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentScrapSources;
using Manufactures.Domain.GarmentScrapSources.Commands;
using Manufactures.Domain.GarmentScrapSources.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.GarmentScrapTransactions.CommandHandler
{
	public class UpdateGarmentScrapTransactionCommandHandler : ICommandHandler<UpdateGarmentScrapTransactionCommand, GarmentScrapTransaction>
	{
		private readonly IStorage _storage;
		private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;
		private readonly IGarmentScrapTransactionItemRepository _garmentScrapTransactionItemRepository;
		private readonly IGarmentScrapStockRepository _garmentScrapStockRepository;

		public UpdateGarmentScrapTransactionCommandHandler(IStorage storage)
		{
			_storage = storage;
			_garmentScrapTransactionRepository = storage.GetRepository<IGarmentScrapTransactionRepository>();
			_garmentScrapTransactionItemRepository = storage.GetRepository<IGarmentScrapTransactionItemRepository>();
			_garmentScrapStockRepository = storage.GetRepository<IGarmentScrapStockRepository>();

		}

		public async Task<GarmentScrapTransaction> Handle(UpdateGarmentScrapTransactionCommand request, CancellationToken cancellationToken)
		{
			var scrapTransaction = _garmentScrapTransactionRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentScrapTransaction(o)).Single();
			_garmentScrapTransactionItemRepository.Find(o => o.ScrapTransactionId == scrapTransaction.Identity).ForEach(async item =>
			{
				var gStock = _garmentScrapStockRepository.Query.Where(s => s.ScrapDestinationId == scrapTransaction.ScrapDestinationId && s.ScrapClassificationId == item.ScrapClassificationId).Select(i => new GarmentScrapStock(i)).Single();
				var items = request.Items.Where(o => o.Id == item.Identity).Single();
				if (scrapTransaction.TransactionType == "IN")
				{
					gStock.SetQuantity(gStock.Quantity - item.Quantity + items.Quantity);
					await _garmentScrapTransactionItemRepository.Update(item);
					gStock.Modify();
					await _garmentScrapStockRepository.Update(gStock);
				}
				else
				{
					gStock.SetQuantity(gStock.Quantity + item.Quantity - items.Quantity);
					await _garmentScrapTransactionItemRepository.Update(item);
					gStock.Modify();
					await _garmentScrapStockRepository.Update(gStock);
				}
				item.SetQuantity(items.Quantity);
				item.SetDescription(items.Description);
				item.Modify();
				await _garmentScrapTransactionItemRepository.Update(item);
			});
			scrapTransaction.SetTransactionDate(request.TransactionDate);
			scrapTransaction.Modify();
			await _garmentScrapTransactionRepository.Update(scrapTransaction);
			_storage.Save();
			return scrapTransaction;
		}
	}
}
