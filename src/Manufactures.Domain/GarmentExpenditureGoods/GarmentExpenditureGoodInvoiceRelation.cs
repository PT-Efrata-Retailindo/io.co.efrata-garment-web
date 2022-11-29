using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentExpenditureGoods.ReadModels;
using Moonlay;
using System;
 

namespace Manufactures.Domain.GarmentExpenditureGoods
{
	public class GarmentExpenditureGoodInvoiceRelation : AggregateRoot<GarmentExpenditureGoodInvoiceRelation, GarmentExpenditureGoodInvoiceRelationReadModel>
	{
		public Guid ExpenditureGoodId { get; private set; }
		public string ExpenditureGoodNo { get; private set; }
		public string UnitCode { get; private set; }
		public string RONo { get; private set; }
		public double Qty { get; private set; }
		public int PackingListId { get; private set; }
		public int InvoiceId { get; private set; }
		public string InvoiceNo { get; private set; }
		public GarmentExpenditureGoodInvoiceRelation(Guid identity, Guid expenditureGoodId, string expenditureGoodNo, string unitCode,  string rONo, double qty, int packingListId, int invoiceId,string invoiceNo) : base(identity)
		{ 
			//MarkTransient();
			ExpenditureGoodNo = expenditureGoodNo;
			Identity = identity;
			ExpenditureGoodId = expenditureGoodId;
			UnitCode = unitCode;
			RONo = rONo;
			Qty = qty;
			PackingListId = packingListId;
			InvoiceId = invoiceId;
			InvoiceNo = invoiceNo;

			ReadModel = new GarmentExpenditureGoodInvoiceRelationReadModel(Identity)
			{
				ExpenditureGoodNo = ExpenditureGoodNo,
				ExpenditureGoodId = ExpenditureGoodId,
				 
				RONo = RONo,
				UnitCode = UnitCode,
				Qty = Qty,
				PackingListId = PackingListId,
				InvoiceId = InvoiceId,
				InvoiceNo = InvoiceNo 
			};

			ReadModel.AddDomainEvent(new OnGarmentExpenditureGoodInvoiceRelationPlaced(Identity));
		}

		public GarmentExpenditureGoodInvoiceRelation(GarmentExpenditureGoodInvoiceRelationReadModel readModel) : base(readModel)
		{
			ExpenditureGoodNo = readModel.ExpenditureGoodNo;
			ExpenditureGoodId = readModel.ExpenditureGoodId;
			UnitCode = readModel.UnitCode;
			RONo = readModel.RONo;
			Qty = readModel.Qty;
			PackingListId = readModel.PackingListId;
			InvoiceId = readModel.InvoiceId;
			InvoiceNo = readModel.InvoiceNo;	 
		}

		public void SetInvoiceId(int InvoiceId)
		{
			if (this.InvoiceId != InvoiceId)
			{
				this.InvoiceId = InvoiceId;
				ReadModel.InvoiceId = InvoiceId;
			}
		}

		public void SetInvoiceNo(string  InvoiceNo)
		{
			if (this.InvoiceNo != InvoiceNo)
			{
				this.InvoiceNo = InvoiceNo;
				ReadModel.InvoiceNo = InvoiceNo;
			}
		}
		public void Modify()
		{
			MarkModified();
		}

		protected override GarmentExpenditureGoodInvoiceRelation GetEntity()
		{
			return this;
		}

		public void SetPackingListId(int PackingListId)
		{
			if (this.PackingListId != PackingListId)
			{
				this.PackingListId = PackingListId;
				ReadModel.PackingListId = PackingListId;
			}
		}
	}
}
