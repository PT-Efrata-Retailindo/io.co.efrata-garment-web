using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
	public class OnGarmentScrapStockPlaced : IGarmentScrapTransactionEventHanlder
	{
		public OnGarmentScrapStockPlaced(Guid identity)
	{
		GarmentScrapStockId = identity;
	}
	public Guid GarmentScrapStockId { get; }
}
}
