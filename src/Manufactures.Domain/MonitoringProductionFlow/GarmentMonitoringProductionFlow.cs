using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.MonitoringProductionFlow
{
	public class GarmentMonitoringProductionFlow : AggregateRoot<GarmentMonitoringProductionFlow, GarmentMonitoringProductionFlowReadModel>
	{

		public string Ro { get; private set; }
		public string BuyerCode { get; private set; }
		public string Article { get; private set; }
		public string Comodity { get; private set; }
		public double QtyOrder { get; private set; }
		public string Size { get; private set; }
		public double QtyCutting { get; private set; }
		public double QtyLoading { get; private set; }
		public double QtySewing { get; private set; }
		public double QtyFinishing { get; private set; }
		public double Wip { get; private set; }

		protected override GarmentMonitoringProductionFlow GetEntity()
		{
			return this;
		}
		public GarmentMonitoringProductionFlow(Guid identity, string ro, string buyerCode, string article, string comodity, double qtyOrder, string size, double qtyCutting, double qtyLoading, double qtySewing, double qtyFinishing, double wip) : base(identity)
		{
			Identity = identity;
			Ro = ro;
			BuyerCode = buyerCode;
			Article = article;
			Comodity = comodity;
			QtyOrder = qtyOrder;
			Size = size;
			QtyCutting = qtyCutting;
			QtyLoading = qtyLoading;
			QtySewing = qtySewing;
			QtyFinishing = qtyFinishing;
			Wip = wip;
			ReadModel = new GarmentMonitoringProductionFlowReadModel(Identity)
			{
				Ro = ro,
				BuyerCode = buyerCode,
				Article = article,
				Comodity = comodity,
				QtyOrder = qtyOrder,
				Size = size,
				QtyCutting = qtyCutting,
				QtyLoading = qtyLoading,
				QtySewing = qtySewing,
				QtyFinishing = qtyFinishing,
				Wip = wip
			};
		}

		public GarmentMonitoringProductionFlow(GarmentMonitoringProductionFlowReadModel readModel) : base(readModel)
		{
			Ro = readModel.Ro;
			BuyerCode = readModel.BuyerCode;
			Article = readModel.Article;
			Comodity = readModel.Comodity;
			QtyOrder = readModel.QtyOrder;
			Size = readModel.Size;
			QtyCutting = readModel.QtyCutting;
			QtyLoading = readModel.QtyLoading;
			QtySewing = readModel.QtySewing;
			QtyFinishing = readModel.QtyFinishing;
			Wip = readModel.Wip;
		}
	}
}
