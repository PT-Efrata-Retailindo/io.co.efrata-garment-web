using Infrastructure.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.MonitoringProductionStockFlow
{
    public class GarmentBalanceMonitoringProductionStocFlow : AggregateRoot<GarmentBalanceMonitoringProductionStocFlow, GarmentBalanceMonitoringProductionStockReadModel>
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
        public double BeginingBalanceLoadingQty { get; private set; }
        public double BeginingBalanceLoadingPrice { get; private set; }
        public double BeginingBalanceSewingQty { get; private set; }
        public double BeginingBalanceSewingPrice { get; private set; }
        public double BeginingBalanceFinishingQty { get; private set; }
        public double BeginingBalanceFinishingPrice { get; private set; }
        public double BeginingBalanceSubconQty { get; private set; }
        public double BeginingBalanceSubconPrice { get; private set; }
        public double BeginingBalanceExpenditureGood { get; private set; }
        public double BeginingBalanceExpenditureGoodPrice { get; private set; }
        
        protected override GarmentBalanceMonitoringProductionStocFlow GetEntity()
        {
            return this;
        }
        public GarmentBalanceMonitoringProductionStocFlow(
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
         double BeginingBalanceLoadingQty,
         double BeginingBalanceLoadingPrice,
         double BeginingBalanceSewingQty,
         double BeginingBalanceSewingPrice,
         double BeginingBalanceFinishingQty,
         double BeginingBalanceFinishingPrice,
         double BeginingBalanceSubconQty,
         double BeginingBalanceSubconPrice,
         double BeginingBalanceExpenditureGood,
         double BeginingBalanceExpenditureGoodPrice,
          Guid identity
         ) : base(identity)
        {
            this.Identity = identity;
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
            this.BeginingBalanceLoadingQty = BeginingBalanceLoadingQty;
            this.BeginingBalanceLoadingPrice = BeginingBalanceLoadingPrice;
            this.BeginingBalanceSewingQty = BeginingBalanceSewingQty;
            this.BeginingBalanceSewingPrice = BeginingBalanceSewingPrice;
            this.BeginingBalanceFinishingQty = BeginingBalanceFinishingQty;
            this.BeginingBalanceFinishingPrice = BeginingBalanceFinishingPrice;
            this.BeginingBalanceSubconQty = BeginingBalanceSubconQty;
            this.BeginingBalanceSubconPrice = BeginingBalanceSubconPrice;
            this.BeginingBalanceExpenditureGood = BeginingBalanceExpenditureGood;
            this.BeginingBalanceExpenditureGoodPrice = BeginingBalanceExpenditureGoodPrice;

            ReadModel = new GarmentBalanceMonitoringProductionStockReadModel(Identity)
            {
                Ro = Ro,
                BuyerCode = BuyerCode,
                Article = Article,
                Comodity = Comodity,
                QtyOrder = QtyOrder,
                BasicPrice = BasicPrice,
                Fare = Fare,
                FC = FC,
                Hours = Hours,
                BeginingBalanceCuttingQty = BeginingBalanceCuttingQty,
                BeginingBalanceCuttingPrice = BeginingBalanceCuttingPrice,
                BeginingBalanceLoadingQty = BeginingBalanceLoadingQty,
                BeginingBalanceLoadingPrice = BeginingBalanceLoadingPrice,
                BeginingBalanceSewingQty = BeginingBalanceSewingQty,
                BeginingBalanceSewingPrice = BeginingBalanceSewingPrice,
                BeginingBalanceFinishingQty = BeginingBalanceFinishingQty,
                BeginingBalanceFinishingPrice = BeginingBalanceFinishingPrice,
                BeginingBalanceSubconQty = BeginingBalanceSubconQty,
                BeginingBalanceSubconPrice = BeginingBalanceSubconPrice,
                BeginingBalanceExpenditureGood = BeginingBalanceExpenditureGood,
                BeginingBalanceExpenditureGoodPrice = BeginingBalanceExpenditureGoodPrice


            };

    }
        public GarmentBalanceMonitoringProductionStocFlow(GarmentBalanceMonitoringProductionStockReadModel readModel) : base(readModel)
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
            this.BeginingBalanceLoadingQty = readModel.BeginingBalanceLoadingQty;
            this.BeginingBalanceLoadingPrice = readModel.BeginingBalanceLoadingPrice;
            this.BeginingBalanceSewingQty = readModel.BeginingBalanceSewingQty;
            this.BeginingBalanceSewingPrice = readModel.BeginingBalanceSewingPrice;
            this.BeginingBalanceFinishingQty = readModel.BeginingBalanceFinishingQty;
            this.BeginingBalanceFinishingPrice = readModel.BeginingBalanceFinishingPrice;
            this.BeginingBalanceSubconQty = readModel.BeginingBalanceSubconQty;
            this.BeginingBalanceSubconPrice = readModel.BeginingBalanceSubconPrice;
            this.BeginingBalanceExpenditureGood = readModel.BeginingBalanceExpenditureGood;
            this.BeginingBalanceExpenditureGoodPrice = readModel.BeginingBalanceExpenditureGoodPrice;
            
        }
    }
}
