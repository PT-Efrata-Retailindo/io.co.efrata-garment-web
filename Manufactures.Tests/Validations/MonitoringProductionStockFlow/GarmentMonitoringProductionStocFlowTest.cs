using Manufactures.Domain.MonitoringProductionStockFlow;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Manufactures.Tests.Validations.MonitoringProductionStockFlow
{
    public class GarmentMonitoringProductionStocFlowTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            Guid id = Guid.NewGuid();
            GarmentMonitoringProductionStocFlow item = new GarmentMonitoringProductionStocFlow(
                "RO",
                "BuyerCode",
                "Article",
                "Comodity",
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                id,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1,
                1
                );
            Assert.Equal(id, item.Identity);
            Assert.Equal("RO", item.Ro);
            Assert.Equal("BuyerCode", item.BuyerCode);
            Assert.Equal("Article", item.Article);
            Assert.Equal("Comodity", item.Comodity);
            Assert.Equal(1, item.QtyOrder);
            Assert.Equal(1, item.BasicPrice);
            Assert.Equal(1, item.Fare);
            Assert.Equal(1, item.FC);
            Assert.Equal(1, item.Hours);
            Assert.Equal(1, item.BeginingBalanceCuttingQty);
            Assert.Equal(1, item.BeginingBalanceCuttingPrice);
            Assert.Equal(1, item.QtyCuttingIn);
            Assert.Equal(1, item.PriceCuttingIn);
            Assert.Equal(1, item.QtyCuttingOut);
            Assert.Equal(1, item.PriceCuttingOut);
            Assert.Equal(1, item.QtyCuttingTransfer);
            Assert.Equal(1, item.PriceCuttingTransfer);
            Assert.Equal(1, item.QtyCuttingsubkon);
            Assert.Equal(1, item.PriceCuttingsubkon);
            Assert.Equal(1, item.AvalCutting);
            Assert.Equal(1, item.AvalCuttingPrice);
            Assert.Equal(1, item.AvalSewing);
            Assert.Equal(1, item.AvalSewingPrice);
            Assert.Equal(1, item.EndBalancCuttingeQty);
            Assert.Equal(1, item.EndBalancCuttingePrice);
            Assert.Equal(1, item.BeginingBalanceLoadingQty);
            Assert.Equal(1, item.BeginingBalanceLoadingPrice);
            Assert.Equal(1, item.QtyLoadingIn);
            Assert.Equal(1, item.PriceLoadingIn);
            Assert.Equal(1, item.QtyLoading);
            Assert.Equal(1, item.PriceLoading);
            Assert.Equal(1, item.QtyLoadingAdjs);
            Assert.Equal(1, item.PriceLoadingAdjs);
            Assert.Equal(1, item.EndBalanceLoadingQty);
            Assert.Equal(1, item.EndBalanceLoadingPrice);
            Assert.Equal(1, item.BeginingBalanceSewingQty);
            Assert.Equal(1, item.BeginingBalanceSewingPrice);
            Assert.Equal(1, item.QtySewingIn);
            Assert.Equal(1, item.PriceSewingIn);
            Assert.Equal(1, item.QtySewingOut);
            Assert.Equal(1, item.PriceSewingOut);
            Assert.Equal(1, item.QtySewingInTransfer);
            Assert.Equal(1, item.PriceSewingInTransfer);
            Assert.Equal(1, item.WipSewingOut);
            Assert.Equal(1, item.WipSewingOutPrice);
            Assert.Equal(1, item.WipFinishingOut);
            Assert.Equal(1, item.WipFinishingOutPrice);
            Assert.Equal(1, item.QtySewingRetur);
            Assert.Equal(1, item.PriceSewingRetur);
            Assert.Equal(1, item.QtySewingAdj);
            Assert.Equal(1, item.PriceSewingAdj);
            Assert.Equal(1, item.EndBalanceSewingQty);
            Assert.Equal(1, item.EndBalanceSewingPrice);
            Assert.Equal(1, item.BeginingBalanceFinishingQty);
            Assert.Equal(1, item.BeginingBalanceFinishingPrice);
            Assert.Equal(1, item.FinishingInQty);
            Assert.Equal(1, item.FinishingInPrice);
            Assert.Equal(1, item.BeginingBalanceSubconQty);
            Assert.Equal(1, item.BeginingBalanceSubconPrice);
            Assert.Equal(1, item.SubconInQty);
            Assert.Equal(1, item.SubconInPrice);
            Assert.Equal(1, item.SubconOutQty);
            Assert.Equal(1, item.SubconOutPrice);
            Assert.Equal(1, item.EndBalanceSubconQty);
            Assert.Equal(1, item.EndBalanceSubconPrice);
            Assert.Equal(1, item.FinishingOutQty);
            Assert.Equal(1, item.FinishingOutPrice);
            Assert.Equal(1, item.FinishingInTransferQty);
            Assert.Equal(1, item.FinishingInTransferPrice);
            Assert.Equal(1, item.FinishingAdjQty);
            Assert.Equal(1, item.FinishingAdjPRice);
            Assert.Equal(1, item.FinishingReturQty);
            Assert.Equal(1, item.FinishingReturPrice);
            Assert.Equal(1, item.EndBalanceFinishingQty);
            Assert.Equal(1, item.EndBalanceFinishingPrice);
            Assert.Equal(1, item.BeginingBalanceExpenditureGood);
            Assert.Equal(1, item.BeginingBalanceExpenditureGoodPrice);
            Assert.Equal(1, item.FinishingTransferExpenditure);
            Assert.Equal(1, item.FinishingTransferExpenditurePrice);
            Assert.Equal(1, item.ExpenditureGoodRetur);
            Assert.Equal(1, item.ExpenditureGoodReturPrice);
            Assert.Equal(1, item.ExportQty);
            Assert.Equal(1, item.ExportPrice);
            Assert.Equal(1, item.OtherQty);
            Assert.Equal(1, item.OtherPrice);
            Assert.Equal(1, item.SampleQty);
            Assert.Equal(1, item.SamplePrice);
            Assert.Equal(1, item.FinishingInExpenditure);
            Assert.Equal(1, item.FinishingInExpenditurepPrice);
            Assert.Equal(1, item.ExpenditureGoodRemainingQty);
            Assert.Equal(1, item.ExpenditureGoodRemainingPrice);
            Assert.Equal(1, item.ExpenditureGoodAdj);
            Assert.Equal(1, item.ExpenditureGoodAdjPrice);
            Assert.Equal(1, item.EndBalanceExpenditureGood);
            Assert.Equal(1, item.EndBalanceExpenditureGoodPrice);
            Assert.Equal(1, item.FareNew);
            Assert.Equal(1, item.CuttingNew);
            Assert.Equal(1, item.LoadingNew);
            Assert.Equal(1, item.SewingNew);
            Assert.Equal(1, item.FinishingNew);
            Assert.Equal(1, item.ExpenditureNew);
            Assert.NotNull(item);
        }

        [Fact]
        public void should_success_instantiate_withModel()
        {
            Guid id = Guid.NewGuid();
            GarmentMonitoringProductionStocFlow item = new GarmentMonitoringProductionStocFlow(new GarmentMonitoringProductionStockReadModel(id));
            Assert.NotNull(item);
        }
    }
}
