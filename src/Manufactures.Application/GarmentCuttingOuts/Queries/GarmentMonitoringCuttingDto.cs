using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentCuttingOuts.Queries
{
	public class GarmentMonitoringCuttingDto
	{
		public GarmentMonitoringCuttingDto()
		{
		}

		public Guid Id { get; internal set; }
		public string roJob { get; internal set; }
		public string article { get; internal set; }
		public string productCode { get; internal set; }
		public string buyerCode { get; internal set; }
		public double qtyOrder { get; internal set; }
		public string style { get; internal set; }
		public double hours { get; internal set; }
		public double stock { get; internal set; }
		public double cuttingQtyMeter { get; internal set; }
		public double cuttingQtyPcs { get; internal set; }
		public double fc { get; internal set; }
		public double expenditure { get; internal set; }
		public double remainQty	{get; internal set;	}
		public decimal price { get; internal set; }
		public decimal nominal { get; internal set; }
		public GarmentMonitoringCuttingDto(GarmentMonitoringCuttingDto garmentMonitoringCuttingDto)
		{
			Id = garmentMonitoringCuttingDto.Id;
			roJob = garmentMonitoringCuttingDto.roJob;
			article = garmentMonitoringCuttingDto.article;
			qtyOrder = garmentMonitoringCuttingDto.qtyOrder;
			productCode = garmentMonitoringCuttingDto.productCode;
			style = garmentMonitoringCuttingDto.style;
			hours = garmentMonitoringCuttingDto.hours;
			cuttingQtyMeter = garmentMonitoringCuttingDto.cuttingQtyMeter;
			stock = garmentMonitoringCuttingDto.stock;
			cuttingQtyPcs = garmentMonitoringCuttingDto.cuttingQtyPcs;
			fc = garmentMonitoringCuttingDto.fc;
			expenditure = garmentMonitoringCuttingDto.expenditure;
			remainQty = garmentMonitoringCuttingDto.remainQty;
			nominal = garmentMonitoringCuttingDto.nominal;
		}
	}
}
