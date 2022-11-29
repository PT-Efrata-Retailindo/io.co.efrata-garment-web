using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentSample.GarmentMonitoringSampleFlows.Queries
{
	public class GarmentMonitoringSampleFlowDto
	{
		public GarmentMonitoringSampleFlowDto()
		{
		}
		public Guid Id { get; internal set; }
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

		public GarmentMonitoringSampleFlowDto(GarmentMonitoringSampleFlowDto flowDto)
		{
			Id = flowDto.Id;
			this.Ro  = flowDto.Ro;
			this.BuyerCode  = flowDto.BuyerCode;
			this.Article   = flowDto.Article;
			this.Comodity  = flowDto.Comodity;
			this.QtyOrder = flowDto.QtyOrder;
			this.Size  = flowDto.Size;
			this.QtyCutting = flowDto.QtyCutting;
			this.QtyLoading = flowDto.QtyLoading;
			this.QtySewing = flowDto.QtySewing;
			this.QtyFinishing = flowDto.QtyFinishing;
			this.Wip = flowDto.Wip;
			 
		}
	}
}
