using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentScrapSources.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources
{
	public class GarmentScrapTransaction : AggregateRoot<GarmentScrapTransaction, GarmentScrapTransactionReadModel>
	{

		public string TransactionNo { get; private set; }
		public DateTimeOffset TransactionDate { get; private set; }
		public string TransactionType { get; private set; }
		public Guid ScrapSourceId { get; private set; }
		public string ScrapSourceName { get; private set; }
		public Guid ScrapDestinationId { get; private set; }
		public string ScrapDestinationName { get; private set; }

		protected override GarmentScrapTransaction GetEntity()
		{
			return this;
		}
		public GarmentScrapTransaction(Guid identity, string transactionNo,string transactionType, DateTimeOffset transactionDate, Guid scrapSourceId, string scrapSourceName, Guid scrapDestinationId, string scrapDestinationName) : base(identity)
		{
			Identity = identity;
			TransactionNo = transactionNo;
			TransactionDate = transactionDate;
			TransactionType = transactionType;
			ScrapDestinationId = scrapDestinationId;
			ScrapDestinationName = scrapDestinationName;
			ScrapSourceId = scrapSourceId;
			ScrapSourceName = scrapSourceName;

			ReadModel = new GarmentScrapTransactionReadModel(Identity)
			{
				TransactionNo = TransactionNo,
				TransactionDate = TransactionDate,
				TransactionType = TransactionType,
				ScrapDestinationId = ScrapDestinationId,
				ScrapDestinationName = ScrapDestinationName,
				ScrapSourceId = ScrapSourceId,
				ScrapSourceName = ScrapSourceName,
			};

			ReadModel.AddDomainEvent(new OnGarmentScrapTransactionPlaced(Identity));
		}
		public GarmentScrapTransaction(GarmentScrapTransactionReadModel readModel) : base(readModel)
		{
			TransactionNo = readModel.TransactionNo;
			TransactionDate = readModel.TransactionDate;
			TransactionType = readModel.TransactionType;
			ScrapDestinationId = readModel.ScrapDestinationId;
			ScrapDestinationName = readModel.ScrapDestinationName;
			ScrapSourceId = readModel.ScrapSourceId;
			ScrapSourceName = readModel.ScrapSourceName;
		}
		public void SetTransactionDate(DateTimeOffset TransactionDate)
		{
			if (this.TransactionDate != TransactionDate)
			{
				this.TransactionDate = TransactionDate;
				ReadModel.TransactionDate = TransactionDate;
			}
		}
		public void Modify()
		{
			MarkModified();
		}
	}
}
