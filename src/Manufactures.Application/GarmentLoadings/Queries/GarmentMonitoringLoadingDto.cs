using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentLoadings.Queries
{
	public class GarmentMonitoringLoadingDto
	{
		public GarmentMonitoringLoadingDto()
		{
		}

		public Guid Id { get; internal set; }
		public string roJob { get; internal set; }
		public string article { get; internal set; }
		public double qtyOrder { get; internal set; }
		public double stock { get; internal set; }
		public double cuttingQtyPcs { get; internal set; }
		public double loadingQtyPcs { get; internal set; }
		public string uomUnit { get; internal set; }
		public string style { get; internal set; }
		public string buyerCode { get; internal set; }
		public double remainQty { get; internal set; }
		public decimal price { get; internal set; }
		public decimal nominal { get; internal set; }
		public GarmentMonitoringLoadingDto(GarmentMonitoringLoadingDto garmentMonitoring)
		{
			Id = garmentMonitoring.Id;
			roJob = garmentMonitoring.roJob;
			article = garmentMonitoring.article;
			qtyOrder = garmentMonitoring.qtyOrder;
			stock = garmentMonitoring.stock;
			cuttingQtyPcs = garmentMonitoring.cuttingQtyPcs;
			loadingQtyPcs = garmentMonitoring.loadingQtyPcs;
			uomUnit = garmentMonitoring.uomUnit;
			remainQty = garmentMonitoring.remainQty;
			style = garmentMonitoring.style;
			buyerCode = garmentMonitoring.buyerCode;
			price = garmentMonitoring.price;
			nominal = garmentMonitoring.nominal;
		}
	}
}
