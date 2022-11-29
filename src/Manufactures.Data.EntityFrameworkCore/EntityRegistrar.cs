// Copyright © 2017 Dmitry Sikorsky. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ExtCore.Data.EntityFramework;
using Manufactures.Data.EntityFrameworkCore.GarmentAdjustments.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentAvalComponents.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentAvalProducts.Configs;
//using Manufactures.Data.EntityFrameworkCore.GarmentBalanceStockProductions.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentComodityPrices.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentCuttingAdjustments.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentCuttingIns.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentCuttingOuts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentDeliveryReturns.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoodReturns.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentExpenditureGoods.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentFinishedGoodStocks.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentFinishingIns.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentFinishingOuts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentLoadings.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentMonitoringProductionStockFlows.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentPreparings.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalComponents.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleAvalProducts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingIns.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleCuttingOuts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleDeliveryReturns.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleExpenditureGoods.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishedGoodStocks.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingIns.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleFinishingOuts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SamplePreparings.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleRequests.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingIns.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleSewingOuts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSample.SampleStocks.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentScrapClassifications.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentScrapTransactions.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentSewingDOs.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSewingIns.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSewingOuts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.CustomsOuts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconCuttings.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconFabricWashes.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconSewings.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentServiceSubconShrinkagePanels.Config;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconContracts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.GarmentSubconCustomsIns.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.SubconDeliveryLetterOuts.Cofigs;
using Manufactures.Data.EntityFrameworkCore.GarmentSubconCuttingOuts.Configs;
using Manufactures.Data.EntityFrameworkCore.GarmentSubcon.InvoicePackingList.Configs;
using Microsoft.EntityFrameworkCore;

namespace Manufactures.Data.EntityFrameworkCore
{
	public class EntityRegistrar : IEntityRegistrar
	{
		public void RegisterEntities(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new GarmentPreparingConfig());
			modelBuilder.ApplyConfiguration(new GarmentPreparingItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentAvalProductConfig());
			modelBuilder.ApplyConfiguration(new GarmentAvalProductItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentCuttingInConfig());
			modelBuilder.ApplyConfiguration(new GarmentCuttingInItemConfig());
			modelBuilder.ApplyConfiguration(new GarmentCuttingInDetailConfig());

			modelBuilder.ApplyConfiguration(new GarmentDeliveryReturnConfig());
			modelBuilder.ApplyConfiguration(new GarmentDeliveryReturnItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentCuttingOutConfig());
			modelBuilder.ApplyConfiguration(new GarmentCuttingOutItemConfig());
			modelBuilder.ApplyConfiguration(new GarmentCuttingOutDetailConfig());

			modelBuilder.ApplyConfiguration(new GarmentSewingDOConfig());
			modelBuilder.ApplyConfiguration(new GarmentSewingDOItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentLoadingConfig());
			modelBuilder.ApplyConfiguration(new GarmentLoadingItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentSewingInConfig());
			modelBuilder.ApplyConfiguration(new GarmentSewingInItemConfig());
			//Enhance Jason Aug 2021
			modelBuilder.ApplyConfiguration(new SewingInHomeListViewConfig());

			modelBuilder.ApplyConfiguration(new GarmentSewingOutConfig());
			modelBuilder.ApplyConfiguration(new GarmentSewingOutItemConfig());
			modelBuilder.ApplyConfiguration(new GarmentSewingOutDetailConfig());

			modelBuilder.ApplyConfiguration(new GarmentFinishingInConfig());
			modelBuilder.ApplyConfiguration(new GarmentFinishingInItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentComodityPriceConfig());

			modelBuilder.ApplyConfiguration(new GarmentAvalComponentConfig());
			modelBuilder.ApplyConfiguration(new GarmentAvalComponentItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentSubconCuttingConfig());
			modelBuilder.ApplyConfiguration(new GarmentSubconCuttingRelationConfig());

			modelBuilder.ApplyConfiguration(new GarmentFinishingOutConfig());
			modelBuilder.ApplyConfiguration(new GarmentFinishingOutItemConfig());
			modelBuilder.ApplyConfiguration(new GarmentFinishingOutDetailConfig());


			modelBuilder.ApplyConfiguration(new GarmentFinishedGoodStockConfig());
			modelBuilder.ApplyConfiguration(new GarmentFinishedGoodStockHistoryConfig());


			modelBuilder.ApplyConfiguration(new GarmentScrapClassificationConfig());
			modelBuilder.ApplyConfiguration(new GarmentScrapTransactionConfig());
			modelBuilder.ApplyConfiguration(new GarmentScrapTransactionItemConfig());
			modelBuilder.ApplyConfiguration(new GarmentScrapDestinationConfig());
			modelBuilder.ApplyConfiguration(new GarmentScrapSourceConfig());
			modelBuilder.ApplyConfiguration(new GarmentScrapStockConfig());

			modelBuilder.ApplyConfiguration(new GarmentAdjustmentConfig());
			modelBuilder.ApplyConfiguration(new GarmentAdjustmentItemConfig());

            modelBuilder.ApplyConfiguration(new GarmentExpenditureGoodConfig());
            modelBuilder.ApplyConfiguration(new GarmentExpenditureGoodItemConfig());

            modelBuilder.ApplyConfiguration(new GarmentExpenditureGoodReturnConfig());
            modelBuilder.ApplyConfiguration(new GarmentExpenditureGoodReturnItemConfig());

			//modelBuilder.ApplyConfiguration(new GarmentBalanceStockProductionConfig());
			modelBuilder.ApplyConfiguration(new GarmentBalanceCuttingConfig());
			modelBuilder.ApplyConfiguration(new GarmentBalanceLoadingConfig());
			modelBuilder.ApplyConfiguration(new GarmentBalanceSewingConfig());
			modelBuilder.ApplyConfiguration(new GarmentBalanceFinishingConfig());
            modelBuilder.ApplyConfiguration(new GarmentBalanceMonitoringProductionStockFlowConfig());

			//Enhance Jason Aug 2021
			modelBuilder.ApplyConfiguration(new GarmentMonitoringFinishingReportConfig());


			modelBuilder.ApplyConfiguration(new GarmentCuttingAdjustmentConfig());
            modelBuilder.ApplyConfiguration(new GarmentCuttingAdjustmentItemConfig());
            //GARMENT SUBCON
            modelBuilder.ApplyConfiguration(new GarmentSubconContractConfig());
            modelBuilder.ApplyConfiguration(new GarmentSubconContractItemConfig());

            modelBuilder.ApplyConfiguration(new GarmentServiceSubconCuttingConfig());
            modelBuilder.ApplyConfiguration(new GarmentServiceSubconCuttingItemConfig());
            modelBuilder.ApplyConfiguration(new GarmentServiceSubconCuttingDetailConfig());
            modelBuilder.ApplyConfiguration(new GarmentServiceSubconCuttingSizeConfig());

            modelBuilder.ApplyConfiguration(new GarmentServiceSubconSewingConfig());
			modelBuilder.ApplyConfiguration(new GarmentServiceSubconSewingItemConfig());
            modelBuilder.ApplyConfiguration(new GarmentServiceSubconSewingDetailConfig());

            modelBuilder.ApplyConfiguration(new GarmentSubconDeliveryLetterOutConfig());
            modelBuilder.ApplyConfiguration(new GarmentSubconDeliveryLetterOutItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentSubconCustomsInConfig());
			modelBuilder.ApplyConfiguration(new GarmentSubconCustomsInItemConfig());

            modelBuilder.ApplyConfiguration(new GarmentSubconCustomsOutConfig());
            modelBuilder.ApplyConfiguration(new GarmentSubconCustomsOutItemConfig());
			
			modelBuilder.ApplyConfiguration(new GarmentServiceSubconShrinkagePanelConfig());
			modelBuilder.ApplyConfiguration(new GarmentServiceSubconShrinkagePanelItemConfig());
			modelBuilder.ApplyConfiguration(new GarmentServiceSubconShrinkagePanelDetailConfig());

			modelBuilder.ApplyConfiguration(new GarmentServiceSubconFabricWashConfig());
			modelBuilder.ApplyConfiguration(new GarmentServiceSubconFabricWashItemConfig());
			modelBuilder.ApplyConfiguration(new GarmentServiceSubconFabricWashDetailConfig());

            //GARMENT SAMPLE
            modelBuilder.ApplyConfiguration(new GarmentSampleRequestConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleRequestProductConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleRequestSpecificationConfig());

			modelBuilder.ApplyConfiguration(new GarmentSamplePreparingConfig());
			modelBuilder.ApplyConfiguration(new GarmentSamplePreparingItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentSampleCuttingInConfig());
			modelBuilder.ApplyConfiguration(new GarmentSampleCuttingInItemConfig());
			modelBuilder.ApplyConfiguration(new GarmentSampleCuttingInDetailConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleCuttingOutConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleCuttingOutItemConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleCuttingOutDetailConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleSewingInConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleSewingInItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentSampleDeliveryReturnConfig());
			modelBuilder.ApplyConfiguration(new GarmentSampleDeliveryReturnItemConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleSewingOutConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleSewingOutItemConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleSewingOutDetailConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleFinishingInConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleFinishingInItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentSampleAvalProductConfig());
			modelBuilder.ApplyConfiguration(new GarmentSampleAvalProductItemConfig());

			modelBuilder.ApplyConfiguration(new GarmentSampleAvalComponentConfig());
			modelBuilder.ApplyConfiguration(new GarmentSampleAvalComponentItemConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleAvalComponentConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleAvalComponentItemConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleFinishingOutConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleFinishingOutItemConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleFinishingOutDetailConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleFinishedGoodStockConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleFinishedGoodStockHistoryConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleExpenditureGoodConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleExpenditureGoodItemConfig());

            modelBuilder.ApplyConfiguration(new GarmentSampleStockConfig());
            modelBuilder.ApplyConfiguration(new GarmentSampleStockHistoryConfig());

            modelBuilder.ApplyConfiguration(new GarmentMonitoringSampleFinishingReportConfig());

			modelBuilder.ApplyConfiguration(new GarmentExpenditureGoodInvoiceRelationConfig());

			modelBuilder.ApplyConfiguration(new SubconInvoicePackingListConfig());
			modelBuilder.ApplyConfiguration(new SubconInvoicePackingListItemConfig());



		}
	}
}