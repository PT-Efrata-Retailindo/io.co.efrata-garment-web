using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentScrapSources.ReadModels
{
	public class GarmentScrapTransactionReadModel : ReadModelBase
	{
		public GarmentScrapTransactionReadModel(Guid identity) : base(identity)
		{

		}	
		public string TransactionNo { get; internal set; }
		public DateTimeOffset TransactionDate { get; internal set; }
		public string TransactionType { get; internal set; }
		public Guid ScrapSourceId { get; internal set; }
		public string ScrapSourceName { get; internal set; }
		public Guid ScrapDestinationId { get; internal set; }
		public string ScrapDestinationName { get; internal set; }
		public virtual List<GarmentScrapTransactionItemReadModel> GarmentScrapTransactionItem { get; internal set; }
	}
}
