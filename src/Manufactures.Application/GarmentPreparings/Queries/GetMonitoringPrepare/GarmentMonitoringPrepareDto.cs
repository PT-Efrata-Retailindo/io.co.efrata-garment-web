using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare
{
	public class GarmentMonitoringPrepareDto
	{
		public GarmentMonitoringPrepareDto()
		{
		}

		public Guid Id { get; internal set; }
		public string roJob { get; internal set; }
		public string article { get; internal set; }
		public string buyerCode { get; internal set; }
		public string productCode { get; internal set; }
		public string uomUnit { get; internal set; }
		public string roAsal { get; internal set; }
		public string remark { get; internal set; }
		public decimal price { get; internal set; }
		public double stock { get; internal set; }
		public double receipt { get; internal set; }
		public double mainFabricExpenditure { get; internal set; }
		public double nonMainFabricExpenditure { get; internal set; }
		public double expenditure { get; internal set; }
		public double aval { get; internal set; }
		public double remainQty { get; internal set; }
		public double nominal { get; internal set; }
		public GarmentMonitoringPrepareDto(GarmentMonitoringPrepareDto garmentMonitoringPrepareDto)
		{
			Id = garmentMonitoringPrepareDto.Id;
			roJob = garmentMonitoringPrepareDto.roJob;
			article = garmentMonitoringPrepareDto.article;
			buyerCode = garmentMonitoringPrepareDto.buyerCode;
			productCode = garmentMonitoringPrepareDto.productCode;
			uomUnit = garmentMonitoringPrepareDto.uomUnit;
			roAsal = garmentMonitoringPrepareDto.roAsal;
			remark = garmentMonitoringPrepareDto.remark;
			stock = garmentMonitoringPrepareDto.stock;
			receipt = garmentMonitoringPrepareDto.receipt;
			mainFabricExpenditure = garmentMonitoringPrepareDto.mainFabricExpenditure;
			nonMainFabricExpenditure = garmentMonitoringPrepareDto.nonMainFabricExpenditure;
			expenditure = garmentMonitoringPrepareDto.expenditure;
			aval = garmentMonitoringPrepareDto.aval;
			remainQty = garmentMonitoringPrepareDto.remainQty;
			nominal = garmentMonitoringPrepareDto.nominal;
		}
	}
}
