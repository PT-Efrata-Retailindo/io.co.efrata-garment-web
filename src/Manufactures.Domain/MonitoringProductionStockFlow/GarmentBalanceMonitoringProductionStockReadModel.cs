using Infrastructure.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.MonitoringProductionStockFlow
{
    public class GarmentBalanceMonitoringProductionStockReadModel : ReadModelBase
    {
        public GarmentBalanceMonitoringProductionStockReadModel(Guid identity) : base(identity)
        {

        }
        public int UnitId { get; internal set; }
        public string UnitCode { get; internal set; }
        public string UnitName { get; internal set; }
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
        public double BeginingBalanceLoadingQty { get; internal set; }
        public double BeginingBalanceLoadingPrice { get; internal set; }
        public double BeginingBalanceSewingQty { get; internal set; }
        public double BeginingBalanceSewingPrice { get; internal set; }
        public double BeginingBalanceFinishingQty { get; internal set; }
        public double BeginingBalanceFinishingPrice { get; internal set; }
        public double BeginingBalanceSubconQty { get; internal set; }
        public double BeginingBalanceSubconPrice { get; internal set; }
        public double BeginingBalanceExpenditureGood { get; internal set; }
        public double BeginingBalanceExpenditureGoodPrice { get; internal set; }
        
    }

}
