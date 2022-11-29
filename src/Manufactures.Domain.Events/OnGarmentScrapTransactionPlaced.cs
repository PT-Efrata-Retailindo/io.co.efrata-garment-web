using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
	public class OnGarmentScrapTransactionPlaced : IGarmentScrapTransactionEventHanlder
	{
		public OnGarmentScrapTransactionPlaced(Guid identity)
		{
			GarmentScrapTransactionId = identity;
		}
		public Guid GarmentScrapTransactionId { get; }
	}
}
