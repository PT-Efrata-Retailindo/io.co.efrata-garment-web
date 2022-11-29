using Infrastructure.Domain;
using Manufactures.Domain.GarmentFinishingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentFinishingOuts
{
	public class GarmentBalanceFinishing : AggregateRoot<GarmentBalanceFinishing, GarmentBalanceFinishingReadModel>
	{
		public GarmentBalanceFinishing(Guid identity, string roJob, string article, int unitId, string unitCode, string unitName, string buyerCode, double qtyOrder, string style, double hours, double stock, double Sewingqty, double finishingQtyPcs, double remainQty, decimal price, decimal nominal) : base(identity)
		{
			RoJob = roJob;
			Article = article;
			UnitId = unitId;
			UnitCode = unitCode;
			UnitName = unitName;
			BuyerCode = buyerCode;
			QtyOrder = qtyOrder;
			Style = style;
			Hours = hours;
			Stock = stock;
			FinishingQtyPcs = finishingQtyPcs;
			SewingQtyPcs = Sewingqty;
			RemainQty = remainQty;
			Price = price;
			Nominal = nominal;
            Identity = identity;

            ReadModel = new GarmentBalanceFinishingReadModel(Identity)
            {

                RoJob = RoJob,
                Article = Article,
                UnitId = UnitId,
                UnitCode = UnitCode,
                UnitName = UnitName,
                BuyerCode = BuyerCode,
                QtyOrder = QtyOrder,
                Style = Style,
                Hours = Hours,
                Stock = Stock,
                FinishingQty = FinishingQtyPcs,
                SewingQtyPcs = SewingQtyPcs,
                RemainQty = RemainQty,
                Price = Price,
                Nominal = Nominal
            };
		}

		public string RoJob { get; private set; }
		public string Article { get; private set; }
		public int UnitId { get; private set; }
		public string UnitCode { get; private set; }
		public string UnitName { get; private set; }
		public string BuyerCode { get; private set; }
		public double QtyOrder { get; private set; }
		public string Style { get; private set; }
		public double Hours { get; private set; }
		public double Stock { get; private set; }
		public double FinishingQtyPcs { get; private set; }
		public double SewingQtyPcs { get; private set; }
		public string UomUnit { get; private set; }
		public double RemainQty { get; private set; }
		public decimal Price { get; private set; }
		public decimal Nominal { get; private set; }
		public GarmentBalanceFinishing(GarmentBalanceFinishingReadModel readModel) : base(readModel)
		{
			RoJob = readModel.RoJob;
			Article = readModel.Article;
			UnitId = readModel.UnitId;
			UnitCode = readModel.UnitCode;
			UnitName = readModel.UnitName;
			BuyerCode = readModel.BuyerCode;
			QtyOrder = readModel.QtyOrder;
			Style = readModel.Style;
			Hours = readModel.Hours;
			Stock = readModel.Stock;
			FinishingQtyPcs = readModel.FinishingQty;
			SewingQtyPcs = readModel.SewingQtyPcs;
			RemainQty = readModel.RemainQty;
			Price = readModel.Price;
			Nominal = readModel.Nominal;
		}
		protected override GarmentBalanceFinishing GetEntity()
		{
			return this;
		}
	}
}
