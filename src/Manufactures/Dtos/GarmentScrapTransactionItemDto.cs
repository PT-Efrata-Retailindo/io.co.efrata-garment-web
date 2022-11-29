using Manufactures.Domain.GarmentScrapSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
	class GarmentScrapTransactionItemDto : BaseDto
	{
		public GarmentScrapTransactionItemDto(GarmentScrapTransactionItem transaction)
		{
			Id = transaction.Identity;
			ScrapTransactionId = transaction.ScrapTransactionId;
			ScrapClassificationId = transaction.ScrapClassificationId;
			ScrapClassificationName = transaction.ScrapClassificationName;
			Quantity = transaction.Quantity;
			UomId = transaction.UomId;
			UomUnit = transaction.UomUnit;
			Description = transaction.Description;
		}
		public Guid Id { get; internal set; }
		public Guid ScrapTransactionId { get; internal set; }
		public Guid ScrapClassificationId { get; internal set; }
		public string ScrapClassificationName { get; internal set; }
		public double Quantity { get; internal set; }
		public int UomId { get; internal set; }
		public string UomUnit { get; internal set; }
		public string Description { get; internal set; }
	}
}
