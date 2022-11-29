using Infrastructure.Domain;
using Manufactures.Domain.GarmentLoadings.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentLoadings
{
	public class GarmentBalanceLoading : AggregateRoot<GarmentBalanceLoading, GarmentBalanceLoadingReadModel>
	{
		public GarmentBalanceLoading(Guid identity, string roJob, string article, int unitId, string unitCode, string unitName, string buyerCode, double qtyOrder, string style, double hours, double stock, double cuttingQtyPcs, double loadingqty, double remainQty, decimal price, decimal nominal) : base(identity)
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
			CuttingQtyPcs = cuttingQtyPcs;
			LoadingQtyPcs = loadingqty;
			RemainQty = remainQty;
			Price = price;
			Nominal = nominal;
            Identity = identity;

            ReadModel = new GarmentBalanceLoadingReadModel(Identity)
            {
                RoJob = roJob,
                Article = article,
                UnitId = unitId,
                UnitCode = unitCode,
                UnitName = unitName,
                BuyerCode = buyerCode,
                QtyOrder = qtyOrder,
                Style = style,
                Hours = hours,
                Stock = stock,
                CuttingQtyPcs = cuttingQtyPcs,
                LoadingQtyPcs = loadingqty,
                RemainQty = remainQty,
                Price = price,
                Nominal = nominal

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
		public double CuttingQtyPcs { get; private set; }
		public double LoadingQtyPcs { get; private set; }
		public string UomUnit { get; private set; }
		public double RemainQty { get; private set; }
		public decimal Price { get; private set; }
		public decimal Nominal { get; private set; }
		public GarmentBalanceLoading(GarmentBalanceLoadingReadModel readModel) : base(readModel)
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
			CuttingQtyPcs = readModel.CuttingQtyPcs;
			LoadingQtyPcs = readModel.LoadingQtyPcs;
			RemainQty = readModel.RemainQty;
			Price = readModel.Price;
			Nominal = readModel.Nominal;
		}
		protected override GarmentBalanceLoading GetEntity()
		{
			return this;
		}
	}
}
