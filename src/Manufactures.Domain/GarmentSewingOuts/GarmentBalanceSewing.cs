using Infrastructure.Domain;
using Manufactures.Domain.GarmentSewingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts
{
	public class GarmentBalanceSewing : AggregateRoot<GarmentBalanceSewing, GarmentBalanceSewingReadModel>
	{
		public GarmentBalanceSewing(Guid identity, string roJob, string article, int unitId, string unitCode, string unitName, string buyerCode, double qtyOrder, string style, double hours, double stock, double loadingQtyPcs, double Sewingqty, double remainQty, decimal price, decimal nominal) : base(identity)
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
			LoadingQtyPcs = loadingQtyPcs;
			SewingQtyPcs = Sewingqty;
			RemainQty = remainQty;
			Price = price;
			Nominal = nominal;
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
		public double LoadingQtyPcs { get; private set; }
		public double SewingQtyPcs { get; private set; }
		public string UomUnit { get; private set; }
		public double RemainQty { get; private set; }
		public decimal Price { get; private set; }
		public decimal Nominal { get; private set; }
		public GarmentBalanceSewing(GarmentBalanceSewingReadModel readModel) : base(readModel)
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
			LoadingQtyPcs = readModel.LoadingQtyPcs;
			SewingQtyPcs = readModel.SewingOutQtyPcs;
			RemainQty = readModel.RemainQty;
			Price = readModel.Price;
			Nominal = readModel.Nominal;
		}
		protected override GarmentBalanceSewing GetEntity()
		{
			return this;
		}
	}
}
