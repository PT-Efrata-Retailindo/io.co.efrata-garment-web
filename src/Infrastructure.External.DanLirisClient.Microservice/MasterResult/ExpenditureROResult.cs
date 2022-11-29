using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
	public class ExpenditureROResult : BaseResult
	{
		public IList<ExpenditureROViewModel> data { get; set; }
		public ExpenditureROResult()
		{
			data = new List<ExpenditureROViewModel>();
		}
		

		public class ExpenditureROViewModel
		{
			public int DetailExpenditureId { get; set; }

			public string ROAsal { get; set; }
			public string BuyerCode { get; set; }
		}
	}
}
