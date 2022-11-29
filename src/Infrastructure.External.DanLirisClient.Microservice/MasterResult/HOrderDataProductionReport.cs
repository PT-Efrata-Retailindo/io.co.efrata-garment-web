using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
    public class HOrderDataProductionReport
    {
        public IList<HOrderViewModel> data { get; set; }

        public HOrderDataProductionReport()
        {
            data = new List<HOrderViewModel>();
        }

        public class HOrderViewModel
        {
            public string No { get; set; }
            public string Codeby { get; set; }
            public decimal Sh_Cut { get; set; }
            public string Kode { get; set; }
            public decimal Qty { get; set; }
			public double Fc { get; set; }
		}
	}
}
