using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentExpenditureGoods.ReadModels
{
	public class GarmentExpenditureGoodInvoiceRelationReadModel : ReadModelBase
	{
		public GarmentExpenditureGoodInvoiceRelationReadModel(Guid identity) : base(identity)
		{

		}
		public Guid ExpenditureGoodId { get; internal set; }
		public string ExpenditureGoodNo { get; internal set; }
		public string UnitCode { get; internal set; }
		public string RONo { get; internal set; }
		public double Qty { get; internal set; }
		public int PackingListId { get; internal set; }
		public int InvoiceId { get; internal set; }
		public string InvoiceNo { get; internal set; }
	}
}
