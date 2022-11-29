using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
	public class OnGarmentScrapTransactionItemPlaced : IGarmentScrapTransactionEventHanlder
	{
		public OnGarmentScrapTransactionItemPlaced(Guid identity)
		{
			GarmentScrapTransactionItemId = identity;
		}
		public Guid GarmentScrapTransactionItemId { get; }
	}
}
