using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings.ValueObjects
{
	public class GarmentMonitoringPrepareValueObject : ValueObject
	{
		public GarmentMonitoringPrepareValueObject()
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
		public double stock { get; internal set; }
		public double receipt { get; internal set; }
		public double mainFabricExpenditure { get; internal set; }
		public double nonMainFabricExpenditure { get; internal set; }
		public double expenditure { get; internal set; }
		public double aval { get; internal set; }
		public double remainQty { get; internal set; }
		public GarmentMonitoringPrepareValueObject(Guid prepareItemId, string roJob, string article, string buyerCode, string productCode,string uomUnit, string roAsal, string remark,double stock, double receipt, double mainFabricExpenditure, double nonMainFabricExpenditure, double expenditure, double aval, double remainingqty)
		{
			Id = prepareItemId;
			this.roJob = roJob;
			this.article = article;
			this.buyerCode = buyerCode;
			this.productCode = productCode;
		    this.uomUnit = uomUnit;
			this.roAsal = roAsal;
			this.remark = remark;
			this.stock = stock;
			this.receipt = receipt;
			this.mainFabricExpenditure = mainFabricExpenditure;
			this.nonMainFabricExpenditure = nonMainFabricExpenditure;
			this.expenditure = expenditure;
			this.aval = aval;
			this.remainQty = remainingqty;

		}

		protected override IEnumerable<object> GetAtomicValues()
		{
			throw new NotImplementedException();
		}
	}
}
