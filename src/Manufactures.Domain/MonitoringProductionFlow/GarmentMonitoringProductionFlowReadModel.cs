using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.MonitoringProductionFlow
{
	public class GarmentMonitoringProductionFlowReadModel : ReadModelBase
	{
		public GarmentMonitoringProductionFlowReadModel(Guid identity) : base(identity)
		{

		}

		public string Ro { get; internal set; }
		public string BuyerCode { get; internal set; }
		public string Article { get; internal set; }
		public string Comodity { get; internal set; }
		public double QtyOrder { get; internal set; }
		public string Size { get; internal set; }
		public double QtyCutting { get; internal set; }
		public double QtyLoading { get; internal set; }
		public double QtySewing { get; internal set; }
		public double QtyFinishing { get; internal set; }
		public double Wip { get; internal set; }
	}
}
