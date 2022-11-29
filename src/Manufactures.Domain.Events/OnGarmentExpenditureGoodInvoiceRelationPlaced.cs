using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Events
{
	public class OnGarmentExpenditureGoodInvoiceRelationPlaced : IGarmentExpenditureGoodInvoiceRelationEvent
	{
		public OnGarmentExpenditureGoodInvoiceRelationPlaced(Guid identity)
		{
			OnGarmentExpenditureGoodId = identity;
		}
		public Guid OnGarmentExpenditureGoodId { get; }
	}
}
