using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.MonitoringSampleStockFlow
{
	public class GarmentMonitoringSampleStockReadModel : ReadModelBase
	{
		public GarmentMonitoringSampleStockReadModel(Guid identity) : base(identity)
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
		public double OtherQty { get; internal set; }
		public double OtherPrice { get; internal set; }
		public double SampleQty { get; internal set; }
		public double SamplePrice { get; internal set; }
		public double FinishingInExpenditure { get; internal set; }
		public double FinishingInExpenditurepPrice { get; internal set; }

		public double ExpenditureGoodRemainingQty { get; internal set; }
		public double ExpenditureGoodRemainingPrice { get; internal set; }
		public double ExpenditureGoodAdj { get; internal set; }
		public double ExpenditureGoodAdjPrice { get; internal set; }
		public double EndBalanceExpenditureGood { get; internal set; }
		public double EndBalanceExpenditureGoodPrice { get; internal set; }
		public decimal FareNew { get; internal set; }
		public decimal CuttingNew { get; internal set; }
		public decimal LoadingNew { get; internal set; }
		public decimal SewingNew { get; internal set; }
		public decimal FinishingNew { get; internal set; }
		public decimal ExpenditureNew
		{
			get; internal set;
		}
	}
}
