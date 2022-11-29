using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings.ReadModels
{
	public class GarmentMonitoringPrepareReadModel 
	{
		
		public Guid Id { get;  set; }
		public string roJob { get;  set; }
		public string article { get;  set; }
		public string buyerCode { get;  set; }
		public string productCode { get;  set; }
		public string uomUnit { get;  set; }
		public string roAsal { get;  set; }
		public string remark { get;  set; }
		public double stock { get;  set; }
		public double receipt { get;  set; }
		public double mainFabricExpenditure { get;  set; }
		public double nonMainFabricExpenditure { get;  set; }
		public double expenditure { get;  set; }
		public double aval { get;  set; }
		public double remainQty { get;  set; }
	}
}
