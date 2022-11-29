using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
	public class OnGarmentScrapSourcePlaced : IGarmentScrapTransactionEventHanlder
	{
		public OnGarmentScrapSourcePlaced(Guid identity)
		{
			GarmentScrapSourceId = identity;
		}
		public Guid GarmentScrapSourceId { get; }
	}
}
