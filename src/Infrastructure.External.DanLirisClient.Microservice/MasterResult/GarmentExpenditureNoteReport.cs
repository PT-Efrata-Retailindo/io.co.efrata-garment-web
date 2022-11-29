using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.External.DanLirisClient.Microservice.MasterResult
{
	public class GarmentExpenditureNoteReport
	{
		public IList<UENViewModel> data { get; set; }

		public GarmentExpenditureNoteReport()
		{
			data = new List<UENViewModel>();
		}

		public class UENViewModel
        {
            public long UENId { get; set; }
            public string UENNo { get; set; }
            public DateTimeOffset UENDate { get; set; }
            public string UnitRequestName { get; set; }
            public string UnitSenderName { get; set; }
            public string FabricType { get; set; }
            public string RONo { get; set; }
            public double Quntity { get; set; }
            public string UOMUnit { get; set; }
        }       
    }
}
