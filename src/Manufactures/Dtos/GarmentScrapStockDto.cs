using Manufactures.Domain.GarmentScrapSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos
{
	 class GarmentScrapStockDto : BaseDto
	{
		public GarmentScrapStockDto(GarmentScrapStock transaction)
		{
			Id = transaction.Identity;
			ScrapClassificationId = transaction.ScrapClassificationId;
			ScrapClassificationName = transaction.ScrapClassificationName;
			ScrapDestinationId = transaction.ScrapDestinationId;
			ScrapDestinationName = transaction.ScrapDestinationName;
			UomId = transaction.UomId;
			UomUnit = transaction.UomUnit;
			Quantity = transaction.Quantity ;
		}
		public Guid Id { get; internal set; }
		public double Quantity { get; internal set; }
		public string UomUnit { get; internal set; }
		public int UomId { get; internal set; }
		public Guid ScrapClassificationId { get; internal set; }
		public string ScrapClassificationName { get; internal set; }
		public Guid ScrapDestinationId { get; internal set; }
		public string ScrapDestinationName { get; internal set; }

	}
}
