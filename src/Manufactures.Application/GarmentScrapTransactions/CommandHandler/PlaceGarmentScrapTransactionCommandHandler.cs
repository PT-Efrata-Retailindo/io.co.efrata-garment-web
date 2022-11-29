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

	public class PlaceGarmentScrapTransactionCommandHandler : ICommandHandler<PlaceGarmentScrapTransactionCommand, GarmentScrapTransaction>
	{
		private readonly IStorage _storage;
		private readonly IGarmentScrapTransactionRepository _garmentScrapTransactionRepository;
		private readonly IGarmentScrapTransactionItemRepository _garmentScrapTransactionItemRepository;
		//private readonly IGarmentScrapSourceRepository _garmentScrapSourceRepository;
		//private readonly IGarmentScrapDestinationRepository _garmentScrapDestinationRepository;
		private readonly IGarmentScrapStockRepository _garmentScrapStockRepository;

		public PlaceGarmentScrapTransactionCommandHandler(IStorage storage)
		{
			_storage = storage;
			_garmentScrapTransactionRepository = storage.GetRepository<IGarmentScrapTransactionRepository>();
			_garmentScrapTransactionItemRepository = storage.GetRepository<IGarmentScrapTransactionItemRepository>();
			//_garmentScrapSourceRepository = storage.GetRepository<IGarmentScrapSourceRepository>();
			//_garmentScrapDestinationRepository = storage.GetRepository<IGarmentScrapDestinationRepository>();
			_garmentScrapStockRepository = storage.GetRepository<IGarmentScrapStockRepository>();
			 
		}
		public async Task<GarmentScrapTransaction> Handle(PlaceGarmentScrapTransactionCommand request, CancellationToken cancellationToken)
		{

			if (request.TransactionType == "IN")
			{
				request.Items = request.Items.Where(item => item.Quantity > 0).ToList();

				Guid identity = Guid.NewGuid();
				GarmentScrapTransaction transaction = new GarmentScrapTransaction(
					identity,
					GenerateTransactionNo(request),
					request.TransactionType,
					request.TransactionDate,
					request.ScrapSourceId,
					request.ScrapSourceName,
					request.ScrapDestinationId,
					request.ScrapDestinationName
				);
				await _garmentScrapTransactionRepository.Update(transaction);
				foreach (var item in request.Items)
				{
					if (item.Quantity > 0)
					{
						Guid itemIdentity = Guid.NewGuid();
						GarmentScrapTransactionItem garmentScrapTransactionItem = new GarmentScrapTransactionItem(
							itemIdentity,
							transaction.Identity,
							item.ScrapClassificationId,
							item.ScrapClassificationName,
							item.Quantity,
							item.UomId,
							item.UomUnit,
							item.Description
						);
						var stock = _garmentScrapStockRepository.Query.Where(s => s.ScrapClassificationId == item.ScrapClassificationId && s.ScrapDestinationId == request.ScrapDestinationId).Select(s => new GarmentScrapStock(s)).FirstOrDefault();
						if (stock != null)
						{
							stock.SetQuantity(stock.Quantity + item.Quantity);
							stock.Modify();
							await _garmentScrapStockRepository.Update(stock);
						}
						else
						{
							Guid stockIdentity = Guid.NewGuid();
							GarmentScrapStock garmentScrapStock = new GarmentScrapStock(
								stockIdentity,
								transaction.ScrapDestinationId,
								transaction.ScrapDestinationName,
								item.ScrapClassificationId,
								item.ScrapClassificationName,
								item.Quantity,
								item.UomId,
								item.UomUnit
							);
							await _garmentScrapStockRepository.Update(garmentScrapStock);
						}

						await _garmentScrapTransactionItemRepository.Update(garmentScrapTransactionItem);
						_storage.Save();
					}
				}
				return transaction;
			}
			else
			{
				request.Items = request.Items.Where(item => item.Quantity > 0).ToList();

				Guid identity = Guid.NewGuid();
				GarmentScrapTransaction transaction = new GarmentScrapTransaction(
					identity,
					GenerateTransactionNoOut(request),
					request.TransactionType,
					request.TransactionDate,
					request.ScrapSourceId,
					request.ScrapSourceName,
					request.ScrapDestinationId,
					request.ScrapDestinationName
				);
				await _garmentScrapTransactionRepository.Update(transaction);
				foreach (var item in request.Items)
				{
					if (item.Quantity > 0)
					{
						Guid itemIdentity = Guid.NewGuid();
						GarmentScrapTransactionItem garmentScrapTransactionItem = new GarmentScrapTransactionItem(
							itemIdentity,
							transaction.Identity,
							item.ScrapClassificationId,
							item.ScrapClassificationName,
							item.Quantity,
							item.UomId,
							item.UomUnit,
							item.Description
						);
						var stock = _garmentScrapStockRepository.Query.Where(s => s.ScrapClassificationId == item.ScrapClassificationId && s.ScrapDestinationId == request.ScrapDestinationId).Select(s => new GarmentScrapStock(s)).FirstOrDefault();
						if (stock != null)
						{
							stock.SetQuantity(stock.Quantity - item.Quantity);
							stock.Modify();
							await _garmentScrapStockRepository.Update(stock);
						}
						else
						{
							Guid stockIdentity = Guid.NewGuid();
							GarmentScrapStock garmentScrapStock = new GarmentScrapStock(
								stockIdentity,
								transaction.ScrapDestinationId,
								transaction.ScrapDestinationName,
								item.ScrapClassificationId,
								item.ScrapClassificationName,
								item.Quantity,
								item.UomId,
								item.UomUnit
							);
							await _garmentScrapStockRepository.Update(garmentScrapStock);
						}

						await _garmentScrapTransactionItemRepository.Update(garmentScrapTransactionItem);
						_storage.Save();
					}
				}
				return transaction;
			}
		}

		private string GenerateTransactionNo(PlaceGarmentScrapTransactionCommand request)
		{
			var now = DateTime.Now;
			var year = now.ToString("yy");
			var month = now.ToString("MM");

			var prefix = $"GV-BUM{year}";

			var lastNo = _garmentScrapTransactionRepository.Query.Where(w => w.TransactionNo.StartsWith(prefix))
				.OrderByDescending(o => o.TransactionNo)
				.Select(s => int.Parse(s.TransactionNo.Replace(prefix, "")))
				.FirstOrDefault();
			var transNo = $"{prefix}{(lastNo + 1).ToString("D5")}";

			return transNo;
		}

		private string GenerateTransactionNoOut(PlaceGarmentScrapTransactionCommand request)
		{
			var now = DateTime.Now;
			var year = now.ToString("yy");
			var month = now.ToString("MM");

			var prefix = $"GV-BUK{year}";

			var lastNo = _garmentScrapTransactionRepository.Query.Where(w => w.TransactionNo.StartsWith(prefix))
				.OrderByDescending(o => o.TransactionNo)
				.Select(s => int.Parse(s.TransactionNo.Replace(prefix, "")))
				.FirstOrDefault();
			var transNo = $"{prefix}{(lastNo + 1).ToString("D5")}";

			return transNo;
		}
	}
}
