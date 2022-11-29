using System;

namespace Manufactures.Domain.Events
{
	public class OnGarmentScrapDestinationPlaced : IGarmentScrapTransactionEventHanlder
	{
		public OnGarmentScrapDestinationPlaced(Guid identity)
		{
			GarmentScrapDestinationId = identity;
		}
		public Guid GarmentScrapDestinationId { get; }
	}
}
