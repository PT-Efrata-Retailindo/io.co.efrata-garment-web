using Manufactures.Domain.GarmentScrapSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
	class GarmentScrapTransactionDto : BaseDto
	{
		public GarmentScrapTransactionDto(GarmentScrapTransaction transaction)
		{
			Id = transaction.Identity;
			TransactionNo = transaction.TransactionNo;
			TransactionDate = transaction.TransactionDate;
			TransactionType = transaction.TransactionType;
			ScrapSourceId = transaction.ScrapSourceId;
			ScrapSourceName = transaction.ScrapSourceName;
			ScrapDestinationId = transaction.ScrapDestinationId;
			ScrapDestinationName = transaction.ScrapDestinationName;
            CreatedBy = transaction.AuditTrail.CreatedBy;
			
			Items = new List<GarmentScrapTransactionItemDto>();
		}
		public Guid Id { get; internal set; }
		public string TransactionNo { get; internal set; }
		public DateTimeOffset TransactionDate { get; internal set; }
		public string TransactionType { get; internal set; }
		public Guid ScrapSourceId { get; internal set; }
		public string ScrapSourceName { get; internal set; }
		public Guid ScrapDestinationId { get; internal set; }
		public string ScrapDestinationName { get; internal set; }

		public virtual List<GarmentScrapTransactionItemDto> Items { get; internal set; }
	}
}
