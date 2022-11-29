using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentCuttingOuts.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentCuttingOuts
{
	public class GarmentBalanceCutting : AggregateRoot<GarmentBalanceCutting, GarmentBalanceCuttingReadModel>
	{
		public GarmentBalanceCutting(Guid identity,string roJob, string article, int unitId, string unitCode, string unitName, string buyerCode, double qtyOrder, string style, double hours, double stock, double cuttingQtyMeter, double cuttingQtyPcs, double fc, double expenditure, double remainQty, decimal price, decimal nominal) : base(identity)
		{
            this.MarkTransient();

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
			CuttingQtyMeter = cuttingQtyMeter;
			CuttingQtyPcs = cuttingQtyPcs;
			Fc = fc;
			Expenditure = expenditure;
			RemainQty = remainQty;
			Price = price;
			Nominal = nominal;
            this.Identity = identity;
            ReadModel = new GarmentBalanceCuttingReadModel(Identity)
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
                CuttingQtyMeter = CuttingQtyMeter,
                CuttingQtyPcs = CuttingQtyPcs,
                Fc = Fc,
                Expenditure = Expenditure,
                RemainQty = RemainQty,
                Price = Price,
                Nominal = Nominal

            };
            ReadModel.AddDomainEvent(new OnGarmentBalanceCuttingPlaced(this.Identity));
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
		public double CuttingQtyMeter { get; private set; }
		public double CuttingQtyPcs { get; private set; }
		public double Fc { get; private set; }
		public double Expenditure { get; private set; }
		public double RemainQty { get; private set; }
		public decimal Price { get; private set; }
		public decimal Nominal { get; private set; }
		public GarmentBalanceCutting(GarmentBalanceCuttingReadModel readModel) : base(readModel)
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
			CuttingQtyMeter = readModel.CuttingQtyMeter;
			CuttingQtyPcs = readModel.CuttingQtyPcs;
			Fc = readModel.Fc;
			Expenditure = readModel.Expenditure;
			RemainQty = readModel.RemainQty;
			Price = readModel.Price;
			Nominal = readModel.Nominal;
            Identity = readModel.Identity;
           
        }
		protected override GarmentBalanceCutting GetEntity()
		{
			return this;
		}
	}
}
