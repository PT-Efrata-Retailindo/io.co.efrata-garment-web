using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries
{
	public class GarmentMonitoringProductionStockFlowDto
	{
		public GarmentMonitoringProductionStockFlowDto()
		{
		}

		public string Ro { get; internal set; }
		public string BuyerCode { get; internal set; }
		public string Article { get; internal set; }
		public string Comodity { get; internal set; }
		public double QtyOrder { get; internal set; }
		public double BasicPrice { get; internal set; }
		public decimal Fare { get; internal set; }
		public double FC { get; internal set; }
		public double Hours { get; internal set; }
		public double BeginingBalanceCuttingQty { get; internal set; }
		public double BeginingBalanceCuttingPrice { get; internal set; }
		public double QtyCuttingIn { get; internal set; }
		public double PriceCuttingIn { get; internal set; }
		public double QtyCuttingOut { get; internal set; }
		public double PriceCuttingOut { get; internal set; }
		public double QtyCuttingTransfer { get; internal set; }
		public double PriceCuttingTransfer { get; internal set; }
		public double QtyCuttingsubkon { get; internal set; }
		public double PriceCuttingsubkon { get; internal set; }
		public double AvalCutting { get; internal set; }
		public double AvalCuttingPrice { get; internal set; }
		public double AvalSewing { get; internal set; }
		public double AvalSewingPrice { get; internal set; }
		public double EndBalancCuttingeQty { get; internal set; }
		public double EndBalancCuttingePrice { get; internal set; }
		public double BeginingBalanceLoadingQty { get; internal set; }
		public double BeginingBalanceLoadingPrice { get; internal set; }
		public double QtyLoadingIn { get; internal set; }
		public double PriceLoadingIn { get; internal set; }
		public double QtyLoadingInTransfer { get; internal set; }
		public double PriceLoadingInTransfer { get; internal set; }
		public double QtyLoading { get; internal set; }
		public double PriceLoading { get; internal set; }
		public double QtyLoadingAdjs { get; internal set; }
		public double PriceLoadingAdjs { get; internal set; }
		public double EndBalanceLoadingQty { get; internal set; }
		public double EndBalanceLoadingPrice { get; internal set; }
		public double BeginingBalanceSewingQty { get; internal set; }
		public double BeginingBalanceSewingPrice { get; internal set; }
		public double QtySewingIn { get; internal set; }
		public double PriceSewingIn { get; internal set; }
		public double QtySewingOut { get; internal set; }
		public double PriceSewingOut { get; internal set; }
		public double QtySewingInTransfer { get; internal set; }
		public double PriceSewingInTransfer { get; internal set; }
		public double WipSewingOut { get; internal set; }
		public double WipSewingOutPrice { get; internal set; }
		public double WipFinishingOut { get; internal set; }
		public double WipFinishingOutPrice { get; internal set; }
		public double QtySewingRetur { get; internal set; }
		public double PriceSewingRetur { get; internal set; }
		public double QtySewingAdj { get; internal set; }
		public double PriceSewingAdj { get; internal set; }
		public double EndBalanceSewingQty { get; internal set; }
		public double EndBalanceSewingPrice { get; internal set; }
		public double BeginingBalanceFinishingQty { get; internal set; }
		public double BeginingBalanceFinishingPrice { get; internal set; }
		public double FinishingInQty { get; internal set; }
		public double FinishingInPrice { get; internal set; }
		public double BeginingBalanceSubconQty { get; internal set; }
		public double BeginingBalanceSubconPrice { get; internal set; }
		public double SubconInQty { get; internal set; }
		public double SubconInPrice { get; internal set; }
		public double SubconOutQty { get; internal set; }
		public double SubconOutPrice { get; internal set; }
		public double EndBalanceSubconQty { get; internal set; }
		public double EndBalanceSubconPrice { get; internal set; }
		public double FinishingOutQty { get; internal set; }
		public double FinishingOutPrice { get; internal set; }
		public double FinishingInTransferQty { get; internal set; }
		public double FinishingInTransferPrice { get; internal set; }
		public double FinishingAdjQty { get; internal set; }
		public double FinishingAdjPRice { get; internal set; }
		public double FinishingReturQty { get; internal set; }
		public double FinishingReturPrice { get; internal set; }
		public double EndBalanceFinishingQty { get; internal set; }
		public double EndBalanceFinishingPrice { get; internal set; }
		public double BeginingBalanceExpenditureGood { get; internal set; }
		public double BeginingBalanceExpenditureGoodPrice { get; internal set; }
		public double FinishingTransferExpenditure { get; internal set; }
		public double FinishingTransferExpenditurePrice { get; internal set; }
		public double ExpenditureGoodRetur { get; internal set; }
		public double ExpenditureGoodReturPrice { get; internal set; }
		public double ExportQty { get; internal set; }
		public double ExportPrice { get; internal set; }
		public double LocalQty { get; internal set; }
		public double LocalPrice { get; internal set; }
		public double OtherQty { get; internal set; }
		public double OtherPrice { get; internal set; }
		public double SampleQty { get; internal set; }
		public double SamplePrice { get; internal set; }
		public double FinishingInExpenditure { get; internal set; }
		public double FinishingInExpenditurepPrice { get; internal set; }
		public double ExpenditureGoodRemainingQty { get; internal set; }
		public double ExpenditureGoodRemainingPrice { get; internal set; }
		public double ExpenditureGoodInTransfer { get; internal set; }
		public double ExpenditureGoodInTransferPrice { get; internal set; }
		public double ExpenditureGoodAdj { get; internal set; }
		public double ExpenditureGoodAdjPrice { get; internal set; }
		public double EndBalanceExpenditureGood { get; internal set; }
		public double EndBalanceExpenditureGoodPrice { get; internal set; }
		public decimal FareNew { get; internal set; }
		public decimal CuttingNew { get; internal set; }
		public decimal LoadingNew { get; internal set; }
		public decimal SewingNew { get; internal set; }
		public decimal FinishingNew { get; internal set; }
		public decimal ExpenditureNew { get; internal set; }
		public decimal SubconNew { get; internal set; }
        public double MaterialUsage { get; internal set; }
        public double PriceUsage { get; internal set; }

        public GarmentMonitoringProductionStockFlowDto(GarmentMonitoringProductionStockFlowDto flowDto)
		{
			 
			this.Ro = flowDto.Ro;
			this.BuyerCode = flowDto.BuyerCode;
			this.Article = flowDto.Article;
			this.Comodity = flowDto.Comodity;
			this.QtyOrder = flowDto.QtyOrder;
			this.BeginingBalanceCuttingQty = flowDto.BeginingBalanceCuttingQty;
			this.BeginingBalanceCuttingPrice = flowDto.BeginingBalanceCuttingPrice;
			this.QtyCuttingIn = flowDto.QtyCuttingIn;
			this.PriceCuttingIn = flowDto.PriceCuttingIn;
			this.QtyCuttingOut = flowDto.QtyCuttingOut;
			this.PriceCuttingOut = flowDto.PriceCuttingOut;
			this.QtyCuttingTransfer = flowDto.QtyCuttingTransfer;
			this.PriceCuttingTransfer = flowDto.PriceCuttingTransfer;
			this.QtyCuttingsubkon = flowDto.QtyCuttingsubkon;
			this.PriceCuttingsubkon = flowDto.PriceCuttingsubkon;
			this.AvalCutting = flowDto.AvalCutting;
			this.AvalCuttingPrice = flowDto.AvalCuttingPrice;
			this.AvalSewing = flowDto.AvalSewing;
			this.AvalSewingPrice = flowDto.AvalSewingPrice;
			this.EndBalancCuttingeQty = flowDto.EndBalancCuttingeQty;
			this.EndBalancCuttingePrice = flowDto.EndBalancCuttingePrice;
			this.BeginingBalanceLoadingQty = flowDto.BeginingBalanceLoadingQty;
			this.BeginingBalanceLoadingPrice = flowDto.BeginingBalanceLoadingPrice;
			this.QtyLoading = flowDto.QtyLoading;
			this.PriceLoading = flowDto.PriceLoading;
			this.QtyLoadingAdjs = flowDto.QtyLoadingAdjs;
			this.PriceLoadingAdjs = flowDto.PriceLoadingAdjs;
			this.EndBalanceLoadingQty = flowDto.EndBalanceLoadingQty;
			this.EndBalanceLoadingPrice = flowDto.EndBalanceLoadingPrice;
			this.BeginingBalanceSewingQty = flowDto.BeginingBalanceSewingQty;
			this.BeginingBalanceSewingPrice = flowDto.BeginingBalanceSewingPrice;
			this.QtySewingIn = flowDto.QtySewingIn;
			this.PriceSewingIn = flowDto.PriceSewingIn;
			this.QtySewingOut = flowDto.QtySewingOut;
			this.PriceSewingOut = flowDto.PriceSewingOut;
			this.QtySewingAdj = flowDto.QtySewingAdj;
			this.PriceSewingAdj = flowDto.PriceSewingAdj;
			this.QtySewingInTransfer = flowDto.QtySewingInTransfer;
			this.PriceSewingInTransfer = flowDto.PriceSewingInTransfer;
			this.WipSewingOut = flowDto.WipSewingOut;
			this.WipSewingOutPrice = flowDto.WipSewingOutPrice;
			this.WipFinishingOut = flowDto.WipFinishingOut;
			this.WipFinishingOutPrice = flowDto.WipFinishingOutPrice;
			this.QtySewingRetur = flowDto.QtySewingRetur;
			this.PriceSewingRetur = flowDto.PriceSewingRetur;
			this.QtySewingAdj = flowDto.QtySewingAdj;
			this.PriceSewingAdj = flowDto.PriceSewingAdj;
			this.EndBalanceSewingQty = flowDto.EndBalanceSewingQty;
			this.EndBalanceSewingPrice = flowDto.EndBalanceSewingPrice;
			this.BeginingBalanceFinishingQty = flowDto.BeginingBalanceFinishingQty;
			this.BeginingBalanceFinishingPrice = flowDto.BeginingBalanceFinishingPrice;
			this.FinishingInQty = flowDto.FinishingInQty;
			this.FinishingInPrice = flowDto.FinishingInPrice;
			this.FinishingAdjQty = flowDto.FinishingAdjQty;
			this.FinishingAdjPRice = flowDto.FinishingAdjPRice;
			this.FinishingInTransferQty = flowDto.FinishingInTransferQty;
			this.FinishingInTransferPrice = flowDto.FinishingInTransferPrice;
			this.FinishingOutQty = flowDto.FinishingOutQty;
			this.FinishingOutPrice = flowDto.FinishingOutPrice;
			this.FinishingReturQty = flowDto.FinishingReturQty;
			this.FinishingReturPrice = flowDto.FinishingReturPrice;
			this.EndBalanceSubconQty = flowDto.EndBalanceSubconQty;
			this.EndBalanceSubconPrice = flowDto.EndBalanceSubconPrice;
			this.EndBalanceFinishingQty = flowDto.EndBalanceFinishingQty;
			this.EndBalanceFinishingPrice = flowDto.EndBalanceFinishingPrice;
			this.SubconInQty = flowDto.SubconInQty;
			this.SubconInPrice = flowDto.SubconInPrice;
			this.SubconOutQty = flowDto.SubconOutQty;
			this.SubconOutPrice = flowDto.SubconOutPrice;
			this.BeginingBalanceExpenditureGood = flowDto.BeginingBalanceExpenditureGood;
			this.BeginingBalanceExpenditureGoodPrice = flowDto.BeginingBalanceExpenditureGoodPrice;
			this.ExpenditureGoodRetur = flowDto.ExpenditureGoodRetur;
			this.ExpenditureGoodRemainingPrice = flowDto.ExpenditureGoodRemainingPrice;
			this.SampleQty = flowDto.SampleQty;
			this.SamplePrice = flowDto.SamplePrice;
			this.ExportQty = flowDto.ExportQty;
			this.ExportPrice = flowDto.ExportPrice;
			this.ExpenditureGoodAdj = flowDto.ExpenditureGoodAdj;
			this.ExpenditureGoodAdjPrice = flowDto.ExpenditureGoodAdjPrice;
			this.EndBalanceExpenditureGood = flowDto.EndBalanceExpenditureGood;
			this.EndBalanceExpenditureGoodPrice = flowDto.EndBalanceExpenditureGoodPrice;
			this.FinishingInExpenditure = flowDto.FinishingInExpenditure;
			this.FinishingInExpenditurepPrice = flowDto.FinishingInExpenditurepPrice;
			this.BuyerCode = flowDto.BuyerCode;
			this.Hours = flowDto.Hours;
			this.Fare = flowDto.Fare;
			this.FareNew = flowDto.FareNew;
			this.CuttingNew = flowDto.CuttingNew;
			this.SewingNew = flowDto.SewingNew;
			this.LoadingNew = flowDto.LoadingNew;
			this.FinishingNew = flowDto.FinishingNew;
			this.ExpenditureNew = flowDto.ExpenditureNew;
            this.MaterialUsage = flowDto.MaterialUsage;
            this.PriceUsage = flowDto.PriceUsage;
			
		}
		
	}
}
