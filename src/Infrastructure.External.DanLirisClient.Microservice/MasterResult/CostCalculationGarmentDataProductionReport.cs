using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
	public class CostCalculationGarmentDataProductionReport
	{
	
		public IList<CostCalViewModel> data { get; set; }
		public CostCalculationGarmentDataProductionReport()
		{
			data = new List<CostCalViewModel>();
		}


		public class CostCalViewModel
		{
			public string comodityName { get; set; }
			public string ro { get; set; }
			public string buyerCode { get; set; }
			public double hours { get; set; }
			public double qtyOrder { get; set; }

		}
	}
}
