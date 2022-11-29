using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSewingOuts.Queries.MonitoringSewing
{
	public class GarmentMonitoringSewingDto
	{
		public GarmentMonitoringSewingDto()
		{
		}
	
		public string roJob { get; internal set; }
		public string article { get; internal set; }
		public string buyerCode { get; internal set; }
        public string style { get; internal set; }
        public double qtyOrder { get; internal set; }
        public double beginingBalanceSewingQty { get; internal set; }
        public double qtySewingIn { get; internal set; }
        public double qtySewingOut { get; internal set; }
        public double qtySewingInTransfer { get; internal set; }
        public double wipSewingOut { get; internal set; }
        public double wipFinishingOut { get; internal set; }
        public double qtySewingRetur { get; internal set; }
        public double qtySewingAdj { get; internal set; }
        public double endBalanceSewingQty { get; internal set; }
        public double endBalanceSewingPrice { get; internal set; }
        public decimal price { get; internal set; }
	 
		public GarmentMonitoringSewingDto(GarmentMonitoringSewingDto garmentMonitoring)
		{
			 
			roJob = garmentMonitoring.roJob;
			article = garmentMonitoring.article;
			qtyOrder = garmentMonitoring.qtyOrder;
			beginingBalanceSewingQty = garmentMonitoring.beginingBalanceSewingQty;
			qtySewingIn = garmentMonitoring.qtySewingIn;
			qtySewingInTransfer = garmentMonitoring.qtySewingInTransfer;
			qtySewingOut = garmentMonitoring.qtySewingOut;
			qtySewingRetur = garmentMonitoring.qtySewingRetur;
			wipSewingOut = garmentMonitoring.wipSewingOut;
            wipFinishingOut = garmentMonitoring.wipFinishingOut;
            endBalanceSewingQty = garmentMonitoring.endBalanceSewingQty;
            endBalanceSewingPrice = garmentMonitoring.endBalanceSewingPrice;
			price = garmentMonitoring.price;
			buyerCode = garmentMonitoring.buyerCode;
			 
		}
	}
}
