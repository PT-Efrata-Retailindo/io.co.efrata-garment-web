using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.SamplePreparings.Queries.GetMonitoringPrepareSample
{
	public class GarmentMonitoringSamplePrepareDto
	{
		public GarmentMonitoringSamplePrepareDto()
		{
		}

		public Guid Id { get; internal set; }
		public string roNo { get; internal set; }
		public string article { get; internal set; }
		public string productCode { get; internal set; }
		public string remark { get; internal set; }
		public string uomUnit { get; internal set; }
		public string roSource { get; internal set; }
		public string designColor { get; internal set; }
		public double stock { get; internal set; }
		public double receipt { get; internal set; }
		public double mainFabricExpenditure { get; internal set; }
		public double nonMainFabricExpenditure { get; internal set; }
		public double deliveryReturn { get; internal set; }
		public double aval { get; internal set; }
		public double remainQty { get; internal set; }
		public double nominal { get; internal set; }
		public decimal price { get; internal set; }
		public GarmentMonitoringSamplePrepareDto(GarmentMonitoringSamplePrepareDto garmentMonitoringPrepareSampleDto)
		{
			Id = garmentMonitoringPrepareSampleDto.Id;
			roNo = garmentMonitoringPrepareSampleDto.roNo;
			article = garmentMonitoringPrepareSampleDto.article;
			productCode = garmentMonitoringPrepareSampleDto.productCode;
			remark = garmentMonitoringPrepareSampleDto.remark;
			uomUnit = garmentMonitoringPrepareSampleDto.uomUnit;
			roSource = garmentMonitoringPrepareSampleDto.roSource;
			designColor = garmentMonitoringPrepareSampleDto.designColor;
			stock = garmentMonitoringPrepareSampleDto.stock;
			receipt = garmentMonitoringPrepareSampleDto.receipt;
			mainFabricExpenditure = garmentMonitoringPrepareSampleDto.mainFabricExpenditure;
			nonMainFabricExpenditure = garmentMonitoringPrepareSampleDto.nonMainFabricExpenditure;
			deliveryReturn = garmentMonitoringPrepareSampleDto.deliveryReturn;
			aval = garmentMonitoringPrepareSampleDto.aval;
			remainQty = garmentMonitoringPrepareSampleDto.remainQty;
			 
		}
	}
}
