using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.MonitoringSampleStockFlow
{
	public class GarmentMonitoringSampleStocFlow : AggregateRoot<GarmentMonitoringSampleStocFlow, GarmentMonitoringSampleStockReadModel>
	{
		public string Ro { get; private set; }
		public string BuyerCode { get; private set; }
		public string Article { get; private set; }
		public string Comodity { get; private set; }
		public double QtyOrder { get; private set; }
		public double BasicPrice { get; private set; }
		public decimal Fare { get; private set; }
		public double FC { get; private set; }
		public double Hours { get; private set; }
		public double BeginingBalanceCuttingQty { get; private set; }
		public double BeginingBalanceCuttingPrice { get; private set; }
		public double QtyCuttingIn { get; private set; }
		public double PriceCuttingIn { get; private set; }
		public double QtyCuttingOut { get; private set; }
		public double PriceCuttingOut { get; private set; }
		public double QtyCuttingTransfer { get; private set; }
		public double PriceCuttingTransfer { get; private set; }
		public double QtyCuttingsubkon { get; private set; }
		public double PriceCuttingsubkon { get; private set; }
		public double AvalCutting { get; private set; }
		public double AvalCuttingPrice { get; private set; }
		public double AvalSewing { get; private set; }
		public double AvalSewingPrice { get; private set; }
		public double EndBalancCuttingeQty { get; private set; }
		public double EndBalancCuttingePrice { get; private set; }
		public double BeginingBalanceLoadingQty { get; private set; }
		public double BeginingBalanceLoadingPrice { get; private set; }
		public double QtyLoadingIn { get; private set; }
		public double PriceLoadingIn { get; private set; }
		public double QtyLoading { get; private set; }
		public double PriceLoading { get; private set; }
		public double QtyLoadingAdjs { get; private set; }
		public double PriceLoadingAdjs { get; private set; }
		public double EndBalanceLoadingQty { get; private set; }
		public double EndBalanceLoadingPrice { get; private set; }
		public double BeginingBalanceSewingQty { get; private set; }
		public double BeginingBalanceSewingPrice { get; private set; }
		public double QtySewingIn { get; private set; }
		public double PriceSewingIn { get; private set; }
		public double QtySewingOut { get; private set; }
		public double PriceSewingOut { get; private set; }
		public double QtySewingInTransfer { get; private set; }
		public double PriceSewingInTransfer { get; private set; }
		public double WipSewingOut { get; private set; }
		public double WipSewingOutPrice { get; private set; }
		public double WipFinishingOut { get; private set; }
		public double WipFinishingOutPrice { get; private set; }
		public double QtySewingRetur { get; private set; }
		public double PriceSewingRetur { get; private set; }
		public double QtySewingAdj { get; private set; }
		public double PriceSewingAdj { get; private set; }
		public double EndBalanceSewingQty { get; private set; }
		public double EndBalanceSewingPrice { get; private set; }
		public double BeginingBalanceFinishingQty { get; private set; }
		public double BeginingBalanceFinishingPrice { get; private set; }
		public double FinishingInQty { get; private set; }
		public double FinishingInPrice { get; private set; }
		public double BeginingBalanceSubconQty { get; private set; }
		public double BeginingBalanceSubconPrice { get; private set; }
		public double SubconInQty { get; private set; }
		public double SubconInPrice { get; private set; }
		public double SubconOutQty { get; private set; }
		public double SubconOutPrice { get; private set; }
		public double EndBalanceSubconQty { get; private set; }
		public double EndBalanceSubconPrice { get; private set; }
		public double FinishingOutQty { get; private set; }
		public double FinishingOutPrice { get; private set; }
		public double FinishingInTransferQty { get; private set; }
		public double FinishingInTransferPrice { get; private set; }
		public double FinishingAdjQty { get; private set; }
		public double FinishingAdjPRice { get; private set; }
		public double FinishingReturQty { get; private set; }
		public double FinishingReturPrice { get; private set; }
		public double EndBalanceFinishingQty { get; private set; }
		public double EndBalanceFinishingPrice { get; private set; }
		public double BeginingBalanceExpenditureGood { get; private set; }
		public double BeginingBalanceExpenditureGoodPrice { get; private set; }
		public double FinishingTransferExpenditure { get; private set; }
		public double FinishingTransferExpenditurePrice { get; private set; }
		public double ExpenditureGoodRetur { get; private set; }
		public double ExpenditureGoodReturPrice { get; private set; }
		public double ExportQty { get; private set; }
		public double ExportPrice { get; private set; }
		public double OtherQty { get; private set; }
		public double OtherPrice { get; private set; }
		public double SampleQty { get; private set; }
		public double SamplePrice { get; private set; }
		public double FinishingInExpenditure { get; private set; }
		public double FinishingInExpenditurepPrice { get; private set; }

		public double ExpenditureGoodRemainingQty { get; private set; }
		public double ExpenditureGoodRemainingPrice { get; private set; }
		public double ExpenditureGoodAdj { get; private set; }
		public double ExpenditureGoodAdjPrice { get; private set; }
		public double EndBalanceExpenditureGood { get; private set; }
		public double EndBalanceExpenditureGoodPrice { get; private set; }
		public decimal FareNew { get; private set; }
		public decimal CuttingNew { get; private set; }
		public decimal LoadingNew { get; private set; }
		public decimal SewingNew { get; private set; }
		public decimal FinishingNew { get; private set; }
		public decimal ExpenditureNew { get; private set; }
		protected override GarmentMonitoringSampleStocFlow GetEntity()
		{
			return this;
		}
		public GarmentMonitoringSampleStocFlow(
		 string Ro,
		 string BuyerCode,
		 string Article,
		 string Comodity,
		 double QtyOrder,
		 double BasicPrice,
		 decimal Fare,
		 double FC,
		 double Hours,
		 double BeginingBalanceCuttingQty,
		 double BeginingBalanceCuttingPrice,
		 double QtyCuttingIn,
		 double PriceCuttingIn,
		 double QtyCuttingOut,
		 double PriceCuttingOut,
		 double QtyCuttingTransfer,
		 double PriceCuttingTransfer,
		 double QtyCuttingsubkon,
		 double PriceCuttingsubkon,
		 double AvalCutting,
		 double AvalCuttingPrice,
		 double AvalSewing,
		 double AvalSewingPrice,
		 double EndBalancCuttingeQty,
		 double EndBalancCuttingePrice,
		 double BeginingBalanceLoadingQty,
		 double BeginingBalanceLoadingPrice,
		 double QtyLoadingIn,
		 double PriceLoadingIn,
		 double QtyLoading,
		 double PriceLoading,
		 double QtyLoadingAdjs,
		 double PriceLoadingAdjs,
		 double EndBalanceLoadingQty,
		 double EndBalanceLoadingPrice,
		 double BeginingBalanceSewingQty,
		 double BeginingBalanceSewingPrice,
		 double QtySewingIn,
		 double PriceSewingIn,
		 double QtySewingOut,
		 double PriceSewingOut,
		 double QtySewingInTransfer,
		 double PriceSewingInTransfer,
		 double WipSewingOut,
		 double WipSewingOutPrice,
		 double WipFinishingOut,
		 double WipFinishingOutPrice,
		 double QtySewingRetur,
		 double PriceSewingRetur,
		 double QtySewingAdj,
		 double PriceSewingAdj,
		 double EndBalanceSewingQty,
		 double EndBalanceSewingPrice,
		 double BeginingBalanceFinishingQty,
		 double BeginingBalanceFinishingPrice,
		 double FinishingInQty,
		 double FinishingInPrice,
		 double BeginingBalanceSubconQty,
		 double BeginingBalanceSubconPrice,
		 double SubconInQty,
		 double SubconInPrice,
		 double SubconOutQty,
		 double SubconOutPrice,
		 double EndBalanceSubconQty,
		 double EndBalanceSubconPrice,
		 double FinishingOutQty,
		 double FinishingOutPrice,
		 double FinishingInTransferQty,
		 double FinishingInTransferPrice,
		 double FinishingAdjQty,
		 double FinishingAdjPRice,
		 double FinishingReturQty,
		 double FinishingReturPrice,
		 double EndBalanceFinishingQty,
		 double EndBalanceFinishingPrice,
		 double BeginingBalanceExpenditureGood,
		 double BeginingBalanceExpenditureGoodPrice,
		 double FinishingTransferExpenditure,
		 double FinishingTransferExpenditurePrice,
		 double ExpenditureGoodRetur,
		 double ExpenditureGoodReturPrice,
		 double ExportQty,
		 double ExportPrice,
		 double OtherQty,
		 double OtherPrice,
		 double SampleQty,
		 double SamplePrice,
		 double FinishingInExpenditure,
		 double FinishingInExpenditurepPrice,
		 Guid identity,
		 double ExpenditureGoodRemainingQty,
		 double ExpenditureGoodRemainingPrice,
		 double ExpenditureGoodAdj,
		 double ExpenditureGoodAdjPrice,
		 double EndBalanceExpenditureGood,
		 double EndBalanceExpenditureGoodPrice,
		 decimal FareNew,
		 decimal CuttingNew,
		 decimal LoadingNew,
		 decimal SewingNew,
		 decimal FinishingNew,
		 decimal ExpenditureNew) : base(identity)
		{
			this.Ro = Ro;
			this.BuyerCode = BuyerCode;
			this.Article = Article;
			this.Comodity = Comodity;
			this.QtyOrder = QtyOrder;
			this.BasicPrice = BasicPrice;
			this.Fare = Fare;
			this.FC = FC;
			this.Hours = Hours;
			this.BeginingBalanceCuttingQty = BeginingBalanceCuttingQty;
			this.BeginingBalanceCuttingPrice = BeginingBalanceCuttingPrice;
			this.QtyCuttingIn = QtyCuttingIn;
			this.PriceCuttingIn = PriceCuttingIn;
			this.QtyCuttingOut = QtyCuttingOut;
			this.PriceCuttingOut = PriceCuttingOut;
			this.QtyCuttingTransfer = QtyCuttingTransfer;
			this.PriceCuttingTransfer = PriceCuttingTransfer;
			this.QtyCuttingsubkon = QtyCuttingsubkon;
			this.PriceCuttingsubkon = PriceCuttingsubkon;
			this.AvalCutting = AvalCutting;
			this.AvalCuttingPrice = AvalCuttingPrice;
			this.AvalSewing = AvalSewing;
			this.AvalSewingPrice = AvalSewingPrice;
			this.EndBalancCuttingeQty = EndBalancCuttingeQty;
			this.EndBalancCuttingePrice = EndBalancCuttingePrice;
			this.BeginingBalanceLoadingQty = BeginingBalanceLoadingQty;
			this.BeginingBalanceLoadingPrice = BeginingBalanceLoadingPrice;
			this.QtyLoadingIn = QtyLoadingIn;
			this.PriceLoadingIn = PriceLoadingIn;
			this.QtyLoading = QtyLoading;
			this.PriceLoading = PriceLoading;
			this.QtyLoadingAdjs = QtyLoadingAdjs;
			this.PriceLoadingAdjs = PriceLoadingAdjs;
			this.EndBalanceLoadingQty = EndBalanceLoadingQty;
			this.EndBalanceLoadingPrice = EndBalanceLoadingPrice;
			this.BeginingBalanceSewingQty = BeginingBalanceSewingQty;
			this.BeginingBalanceSewingPrice = BeginingBalanceSewingPrice;
			this.QtySewingIn = QtySewingIn;
			this.PriceSewingIn = PriceSewingIn;
			this.QtySewingOut = QtySewingOut;
			this.PriceSewingOut = PriceSewingOut;
			this.QtySewingInTransfer = QtySewingInTransfer;
			this.PriceSewingInTransfer = PriceSewingInTransfer;
			this.WipSewingOut = WipSewingOut;
			this.WipSewingOutPrice = WipSewingOutPrice;
			this.WipFinishingOut = WipFinishingOut;
			this.WipFinishingOutPrice = WipFinishingOutPrice;
			this.QtySewingRetur = QtySewingRetur;
			this.PriceSewingRetur = PriceSewingRetur;
			this.QtySewingAdj = QtySewingAdj;
			this.PriceSewingAdj = PriceSewingAdj;
			this.EndBalanceSewingQty = EndBalanceSewingQty;
			this.EndBalanceSewingPrice = EndBalanceSewingPrice;
			this.BeginingBalanceFinishingQty = BeginingBalanceFinishingQty;
			this.BeginingBalanceFinishingPrice = BeginingBalanceFinishingPrice;
			this.FinishingInQty = FinishingInQty;
			this.FinishingInPrice = FinishingInPrice;
			this.BeginingBalanceSubconQty = BeginingBalanceSubconQty;
			this.BeginingBalanceSubconPrice = BeginingBalanceSubconPrice;
			this.SubconInQty = SubconInQty;
			this.SubconInPrice = SubconInPrice;
			this.SubconOutQty = SubconOutQty;
			this.SubconOutPrice = SubconOutPrice;
			this.EndBalanceSubconQty = EndBalanceSubconQty;
			this.EndBalanceSubconPrice = EndBalanceSubconPrice;
			this.FinishingOutQty = FinishingOutQty;
			this.FinishingOutPrice = FinishingOutPrice;
			this.FinishingInTransferQty = FinishingInTransferQty;
			this.FinishingInTransferPrice = FinishingInTransferPrice;
			this.FinishingAdjQty = FinishingAdjQty;
			this.FinishingAdjPRice = FinishingAdjPRice;
			this.FinishingReturQty = FinishingReturQty;
			this.FinishingReturPrice = FinishingReturPrice;
			this.EndBalanceFinishingQty = EndBalanceFinishingQty;
			this.EndBalanceFinishingPrice = EndBalanceFinishingPrice;
			this.BeginingBalanceExpenditureGood = BeginingBalanceExpenditureGood;
			this.BeginingBalanceExpenditureGoodPrice = BeginingBalanceExpenditureGoodPrice;
			this.FinishingTransferExpenditure = FinishingTransferExpenditure;
			this.FinishingTransferExpenditurePrice = FinishingTransferExpenditurePrice;
			this.ExpenditureGoodRetur = ExpenditureGoodRetur;
			this.ExpenditureGoodReturPrice = ExpenditureGoodReturPrice;
			this.ExportQty = ExportQty;
			this.ExportPrice = ExportPrice;
			this.OtherQty = OtherQty;
			this.OtherPrice = OtherPrice;
			this.SampleQty = SampleQty;
			this.SamplePrice = SamplePrice;
			this.FinishingInExpenditure = FinishingInExpenditure;
			this.FinishingInExpenditurepPrice = FinishingInExpenditurepPrice;

			this.ExpenditureGoodRemainingQty = ExpenditureGoodRemainingQty;
			this.ExpenditureGoodRemainingPrice = ExpenditureGoodRemainingPrice;
			this.ExpenditureGoodAdj = ExpenditureGoodAdj;
			this.ExpenditureGoodAdjPrice = ExpenditureGoodAdjPrice;
			this.EndBalanceExpenditureGood = EndBalanceExpenditureGood;
			this.EndBalanceExpenditureGoodPrice = EndBalanceExpenditureGoodPrice;
			this.FareNew = FareNew;
			this.CuttingNew = CuttingNew;
			this.LoadingNew = LoadingNew;
			this.SewingNew = SewingNew;
			this.FinishingNew = FinishingNew;
			this.ExpenditureNew = ExpenditureNew;


		}
		public GarmentMonitoringSampleStocFlow(GarmentMonitoringSampleStockReadModel readModel) : base(readModel)
		{
			this.Ro = readModel.Ro;
			this.BuyerCode = readModel.BuyerCode;
			this.Article = readModel.Article;
			this.Comodity = readModel.Comodity;
			this.QtyOrder = readModel.QtyOrder;
			this.BasicPrice = readModel.BasicPrice;
			this.Fare = readModel.Fare;
			this.FC = readModel.FC;
			this.Hours = readModel.Hours;
			this.BeginingBalanceCuttingQty = readModel.BeginingBalanceCuttingQty;
			this.BeginingBalanceCuttingPrice = readModel.BeginingBalanceCuttingPrice;
			this.QtyCuttingIn = readModel.QtyCuttingIn;
			this.PriceCuttingIn = readModel.PriceCuttingIn;
			this.QtyCuttingOut = readModel.QtyCuttingOut;
			this.PriceCuttingOut = readModel.PriceCuttingOut;
			this.QtyCuttingTransfer = readModel.QtyCuttingTransfer;
			this.PriceCuttingTransfer = readModel.PriceCuttingTransfer;
			this.QtyCuttingsubkon = readModel.QtyCuttingsubkon;
			this.PriceCuttingsubkon = readModel.PriceCuttingsubkon;
			this.AvalCutting = readModel.AvalCutting;
			this.AvalCuttingPrice = readModel.AvalCuttingPrice;
			this.AvalSewing = readModel.AvalSewing;
			this.AvalSewingPrice = readModel.AvalSewingPrice;
			this.EndBalancCuttingeQty = readModel.EndBalancCuttingeQty;
			this.EndBalancCuttingePrice = readModel.EndBalancCuttingePrice;
			this.BeginingBalanceLoadingQty = readModel.BeginingBalanceLoadingQty;
			this.BeginingBalanceLoadingPrice = readModel.BeginingBalanceLoadingPrice;
			this.QtyLoadingIn = readModel.QtyLoadingIn;
			this.PriceLoadingIn = readModel.PriceLoadingIn;
			this.QtyLoading = readModel.QtyLoading;
			this.PriceLoading = readModel.PriceLoading;
			this.QtyLoadingAdjs = readModel.QtyLoadingAdjs;
			this.PriceLoadingAdjs = readModel.PriceLoadingAdjs;
			this.EndBalanceLoadingQty = readModel.EndBalanceLoadingQty;
			this.EndBalanceLoadingPrice = readModel.EndBalanceLoadingPrice;
			this.BeginingBalanceSewingQty = readModel.BeginingBalanceSewingQty;
			this.BeginingBalanceSewingPrice = readModel.BeginingBalanceSewingPrice;
			this.QtySewingIn = readModel.QtySewingIn;
			this.PriceSewingIn = readModel.PriceSewingIn;
			this.QtySewingOut = readModel.QtySewingOut;
			this.PriceSewingOut = readModel.PriceSewingOut;
			this.QtySewingInTransfer = readModel.QtySewingInTransfer;
			this.PriceSewingInTransfer = readModel.PriceSewingInTransfer;
			this.WipSewingOut = readModel.WipSewingOut;
			this.WipSewingOutPrice = readModel.WipSewingOutPrice;
			this.WipFinishingOut = readModel.WipFinishingOut;
			this.WipFinishingOutPrice = readModel.WipFinishingOutPrice;
			this.QtySewingRetur = readModel.QtySewingRetur;
			this.PriceSewingRetur = readModel.PriceSewingRetur;
			this.QtySewingAdj = readModel.QtySewingAdj;
			this.PriceSewingAdj = readModel.PriceSewingAdj;
			this.EndBalanceSewingQty = readModel.EndBalanceSewingQty;
			this.EndBalanceSewingPrice = readModel.EndBalanceSewingPrice;
			this.BeginingBalanceFinishingQty = readModel.BeginingBalanceFinishingQty;
			this.BeginingBalanceFinishingPrice = readModel.BeginingBalanceFinishingPrice;
			this.FinishingInQty = readModel.FinishingInQty;
			this.FinishingInPrice = readModel.FinishingInPrice;
			this.BeginingBalanceSubconQty = readModel.BeginingBalanceSubconQty;
			this.BeginingBalanceSubconPrice = readModel.BeginingBalanceSubconPrice;
			this.SubconInQty = readModel.SubconInQty;
			this.SubconInPrice = readModel.SubconInPrice;
			this.SubconOutQty = readModel.SubconOutQty;
			this.SubconOutPrice = readModel.SubconOutPrice;
			this.EndBalanceSubconQty = readModel.EndBalanceSubconQty;
			this.EndBalanceSubconPrice = readModel.EndBalanceSubconPrice;
			this.FinishingOutQty = readModel.FinishingOutQty;
			this.FinishingOutPrice = readModel.FinishingOutPrice;
			this.FinishingInTransferQty = readModel.FinishingInTransferQty;
			this.FinishingInTransferPrice = readModel.FinishingInTransferPrice;
			this.FinishingAdjQty = readModel.FinishingAdjQty;
			this.FinishingAdjPRice = readModel.FinishingAdjPRice;
			this.FinishingReturQty = readModel.FinishingReturQty;
			this.FinishingReturPrice = readModel.FinishingReturPrice;
			this.EndBalanceFinishingQty = readModel.EndBalanceFinishingQty;
			this.EndBalanceFinishingPrice = readModel.EndBalanceFinishingPrice;
			this.BeginingBalanceExpenditureGood = readModel.BeginingBalanceExpenditureGood;
			this.BeginingBalanceExpenditureGoodPrice = readModel.BeginingBalanceExpenditureGoodPrice;
			this.FinishingTransferExpenditure = readModel.FinishingTransferExpenditure;
			this.FinishingTransferExpenditurePrice = readModel.FinishingTransferExpenditurePrice;
			this.ExpenditureGoodRetur = readModel.ExpenditureGoodRetur;
			this.ExpenditureGoodReturPrice = readModel.ExpenditureGoodReturPrice;
			this.ExportQty = readModel.ExportQty;
			this.ExportPrice = readModel.ExportPrice;
			this.OtherQty = readModel.OtherQty;
			this.OtherPrice = readModel.OtherPrice;
			this.SampleQty = readModel.SampleQty;
			this.SamplePrice = readModel.SamplePrice;
			this.FinishingInExpenditure = readModel.FinishingInExpenditure;
			this.FinishingInExpenditurepPrice = readModel.FinishingInExpenditurepPrice;

			this.ExpenditureGoodRemainingQty = readModel.ExpenditureGoodRemainingQty;
			this.ExpenditureGoodRemainingPrice = readModel.ExpenditureGoodRemainingPrice;
			this.ExpenditureGoodAdj = readModel.ExpenditureGoodAdj;
			this.ExpenditureGoodAdjPrice = readModel.ExpenditureGoodAdjPrice;
			this.EndBalanceExpenditureGood = readModel.EndBalanceExpenditureGood;
			this.EndBalanceExpenditureGoodPrice = readModel.EndBalanceExpenditureGoodPrice;
			this.FareNew = readModel.FareNew;
			this.CuttingNew = readModel.CuttingNew;
			this.LoadingNew = readModel.LoadingNew;
			this.SewingNew = readModel.SewingNew;
			this.FinishingNew = readModel.FinishingNew;
			this.ExpenditureNew = readModel.ExpenditureNew;
		}
	}
}
