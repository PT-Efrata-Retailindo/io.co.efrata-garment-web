using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Linq;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System.Net.Http;
using System.Text;
using Manufactures.Domain.MonitoringProductionStockFlow;

namespace Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries
{
	public class GetGarmentMonitoringProductionStockFlowQueryHandler : IQueryHandler<GetMonitoringProductionStockFlowQuery, GarmentMonitoringProductionStockFlowListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
		private readonly IGarmentSewingInRepository garmentSewingInRepository;
		private readonly IGarmentSewingInItemRepository garmentSewingInItemRepository;
		private readonly IGarmentAvalComponentRepository garmentAvalComponentRepository;
		private readonly IGarmentAvalComponentItemRepository garmentAvalComponentItemRepository;
		private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
		private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentFinishingInRepository garmentFinishingInRepository;
		private readonly IGarmentFinishingInItemRepository garmentFinishingInItemRepository;
		private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
		private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
		private readonly IGarmentExpenditureGoodReturnRepository garmentExpenditureGoodReturnRepository;
		private readonly IGarmentExpenditureGoodReturnItemRepository garmentExpenditureGoodReturnItemRepository;
		private readonly IGarmentSewingDORepository garmentSewingDORepository;
		private readonly IGarmentSewingDOItemRepository garmentSewingDOItemRepository;
		private readonly IGarmentComodityPriceRepository garmentComodityPriceRepository;
		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
        private readonly IGarmentBalanceMonitoringProductionStockFlowRepository garmentBalanceProductionStockRepository;

        public GetGarmentMonitoringProductionStockFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
            garmentBalanceProductionStockRepository = storage.GetRepository<IGarmentBalanceMonitoringProductionStockFlowRepository>();
            garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
			garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
			garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
			garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
			garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
			garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
			garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
			garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
			garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
			garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
			garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
			garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
			garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
			garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}

		class monitoringView
		{
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
			public double FinishingAdjPrice { get; internal set; }
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
			public double ExpenditureGoodRemainingQty { get; internal set; }
			public double ExpenditureGoodRemainingPrice { get; internal set; }
			public double ExpenditureGoodAdj { get; internal set; }
			public double ExpenditureGoodAdjPrice { get; internal set; }
			public double EndBalanceExpenditureGood { get; internal set; }
			public double EndBalanceExpenditureGoodPrice { get; internal set; }
			public double ExpenditureGoodInTransfer { get; internal set; }
			public double ExpenditureGoodInTransferPrice { get; internal set; }


		}
	
		public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
		{
            CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

            var listRO = string.Join(",", ro.Distinct());
            var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/";

            var httpContent = new StringContent(JsonConvert.SerializeObject(listRO), Encoding.UTF8, "application/json");

            var httpResponse = await _http.SendAsync(HttpMethod.Get, costCalculationUri, token, httpContent);

            var freeRO = new List<string>();

            if (httpResponse.IsSuccessStatusCode)
            {
                var contentString = await httpResponse.Content.ReadAsStringAsync();
                Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
                var dataString = content.GetValueOrDefault("data").ToString();
                var listData = JsonConvert.DeserializeObject<List<CostCalViewModel>>(dataString);

                foreach (var item in ro)
                {
                    var data = listData.SingleOrDefault(s => s.ro == item);
                    if (data != null)
                    {
                        costCalculationGarmentDataProductionReport.data.Add(data);
                    }
                    else
                    {
                        freeRO.Add(item);
                    }
                }
            }
            else
            {
                var err = await httpResponse.Content.ReadAsStringAsync();

            }

            return costCalculationGarmentDataProductionReport;
        }

		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal BasicPrice { get; internal set; }
			public int Count { get; internal set; }
		}
		class ViewFC
		{
			public string RO { get; internal set; }
			public double FC { get; internal set; }
			public double Count { get; internal set; }
		}



		public async Task<GarmentMonitoringProductionStockFlowListViewModel> Handle(GetMonitoringProductionStockFlowQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));
            DateTimeOffset dateBalance = (from a in garmentBalanceProductionStockRepository.Query.OrderByDescending(s => s.CreatedDate)
                                          select a.CreatedDate).FirstOrDefault();
            DateTimeOffset dateFareNew = dateTo.AddDays(1);

            var sumbasicPrice = (from a in (from prep in garmentPreparingRepository.Query
                                            where (request.ro == null || (request.ro != null && request.ro != "" && prep.RONo == request.ro))
                                            select new { prep.RONo, prep.Identity })
                                 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId

                                 select new { a.RONo, b.BasicPrice })
                         .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
                         {
                             RO = key.RONo,
                             BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
                             Count = group.Count()
                         });

            var sumFCs = (from a in garmentCuttingInRepository.Query
                          where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.CuttingType == "Main Fabric" &&
                          a.CuttingInDate <= dateTo
                          join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                          join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                          select new { a.FC, a.RONo, FCs= Convert.ToDouble( c.CuttingInQuantity  * a.FC),c.CuttingInQuantity}) 
                         .GroupBy(x => new { x.RONo }, (key, group) => new ViewFC
                         {
                             RO = key.RONo,
                             FC = group.Sum(s => (s.FCs)),
                             Count = group.Sum(s =>  s.CuttingInQuantity)
                         });


            var queryGroup = (from a in ( from aa in garmentCuttingOutRepository.Query
                                          where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro))
                                          select new {aa.RONo,aa.ComodityId,aa.ComodityName, aa.Article })
							  select new { BasicPrice = (from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault(),
                                  FareNew = (from aa in garmentComodityPriceRepository.Query where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && a.ComodityId == aa.ComodityId && aa.Date ==dateFareNew select aa.Price).FirstOrDefault(),
                                  Fare = (from aa in garmentComodityPriceRepository.Query where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && a.ComodityId == aa.ComodityId && aa.IsValid == true select aa.Price).FirstOrDefault(),
                                  Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName,
                                  FC = (from cost in sumFCs where cost.RO == a.RONo select cost.FC / cost.Count).FirstOrDefault() }).Distinct();


            var queryBalance = (from a in (from aa in garmentBalanceProductionStockRepository.Query 
                                           where (request.ro == null || (request.ro != null && request.ro != "" && aa.Ro == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.UnitId == aa.UnitId
                                           select aa)
                                  
                                where a.CreatedDate < dateFrom && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit) //&& a.RoJob == "2010810"
                                select new monitoringView {
                                            QtyCuttingIn = 0,
                                            PriceCuttingIn = 0,
                                            QtySewingIn = 0,
                                            PriceSewingIn = 0,
                                            QtyCuttingTransfer = 0,
                                            PriceCuttingTransfer = 0,
                                            QtyCuttingsubkon = 0,
                                            PriceCuttingsubkon = 0,
                                            AvalCutting = 0,
                                            AvalCuttingPrice = 0,
                                            AvalSewing = 0,
                                            AvalSewingPrice = 0,
                                            QtyLoading = 0,
                                            PriceLoading = 0,
                                            QtyLoadingAdjs = 0,
                                            PriceLoadingAdjs = 0,
                                            QtySewingOut = 0,
                                            PriceSewingOut = 0,
                                            QtySewingAdj = 0,
                                            PriceSewingAdj = 0,
                                            WipSewingOut = 0,
                                            WipSewingOutPrice = 0,
                                            WipFinishingOut = 0,
                                            WipFinishingOutPrice = 0,
                                            QtySewingRetur = 0,
                                            PriceSewingRetur = 0,
                                            QtySewingInTransfer = 0,
                                            PriceSewingInTransfer = 0,
                                            FinishingInQty = 0,
                                            FinishingInPrice = 0,
                                            SubconInQty = 0,
                                            SubconInPrice = 0,
                                            FinishingAdjQty = 0,
                                            FinishingAdjPrice = 0,
                                            FinishingTransferExpenditure = 0,
                                            FinishingTransferExpenditurePrice = 0,
                                            FinishingInTransferQty = 0,
                                            FinishingInTransferPrice = 0,
                                            FinishingOutQty = 0,
                                            FinishingOutPrice = 0,
                                            FinishingReturQty = 0,
                                            FinishingReturPrice = 0,
                                            SubconOutQty = 0,
                                            SubconOutPrice = 0,
                                            //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
                                            //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
                                            BeginingBalanceLoadingQty = a.BeginingBalanceLoadingQty,
                                            BeginingBalanceLoadingPrice = a.BeginingBalanceLoadingPrice,
                                            BeginingBalanceCuttingQty = a.BeginingBalanceCuttingQty,
                                            BeginingBalanceCuttingPrice = a.BeginingBalanceCuttingPrice,
                                            BeginingBalanceFinishingQty = a.BeginingBalanceFinishingQty,
                                            BeginingBalanceFinishingPrice = a.BeginingBalanceFinishingPrice,
                                            BeginingBalanceSewingQty = a.BeginingBalanceSewingQty,
                                            BeginingBalanceSewingPrice = a.BeginingBalanceSewingPrice,
                                            BeginingBalanceExpenditureGood= a.BeginingBalanceExpenditureGood,
                                            BeginingBalanceExpenditureGoodPrice= a.BeginingBalanceExpenditureGoodPrice,
                                            BeginingBalanceSubconQty = a.BeginingBalanceSubconQty,
                                            BeginingBalanceSubconPrice = a.BeginingBalanceSubconPrice,
                                            Ro = a.Ro,
                                            ExpenditureGoodRetur = 0,
                                            ExpenditureGoodReturPrice = 0,
                                            QtyCuttingOut =  0,
                                            PriceCuttingOut =  0,
                                            ExportQty = 0,
                                            ExportPrice = 0,
                                            LocalQty = 0,
                                            LocalPrice = 0,
                                            SampleQty = 0,
                                            SamplePrice = 0,
                                            OtherQty = 0,
                                            OtherPrice = 0,
                                            QtyLoadingInTransfer = 0,
                                            PriceLoadingInTransfer = 0,
                                            ExpenditureGoodInTransfer = 0,
                                            ExpenditureGoodInTransferPrice = 0
                                           
                                        });

            var QueryCuttingOut = (from a in (from aa in garmentCuttingOutRepository.Query
                                              where aa.CuttingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId == aa.UnitFromId
                                              select new { aa.RONo, aa.Identity, aa.CuttingOutDate, aa.CuttingOutType })
                                   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId

                                   select new
                                   {
                                       BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.CuttingOutQuantity : 0,
                                       BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.Price : 0,
                                       Ro = a.RONo,
                                       QtyCuttingOut = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
                                       PriceCuttingOut = a.CuttingOutDate >= dateFrom ? c.Price : 0,
                                   }).GroupBy(x=>x.Ro, (key, group) => new monitoringView {
                                       QtyCuttingIn = 0,
                                       PriceCuttingIn = 0,
                                       QtySewingIn = 0,
                                       PriceSewingIn = 0,
                                       QtyCuttingTransfer = 0,
                                       PriceCuttingTransfer = 0,
                                       QtyCuttingsubkon = 0,
                                       PriceCuttingsubkon = 0,
                                       AvalCutting = 0,
                                       AvalCuttingPrice = 0,
                                       AvalSewing = 0,
                                       AvalSewingPrice = 0,
                                       QtyLoading = 0,
                                       PriceLoading = 0,
                                       QtyLoadingAdjs = 0,
                                       PriceLoadingAdjs = 0,
                                       QtySewingOut = 0,
                                       PriceSewingOut = 0,
                                       QtySewingAdj = 0,
                                       PriceSewingAdj = 0,
                                       WipSewingOut = 0,
                                       WipSewingOutPrice = 0,
                                       WipFinishingOut = 0,
                                       WipFinishingOutPrice = 0,
                                       QtySewingRetur = 0,
                                       PriceSewingRetur = 0,
                                       QtySewingInTransfer = 0,
                                       PriceSewingInTransfer = 0,
                                       FinishingInQty = 0,
                                       FinishingInPrice = 0,
                                       SubconInQty = 0,
                                       SubconInPrice = 0,
                                       FinishingAdjQty = 0,
                                       FinishingAdjPrice = 0,
                                       FinishingTransferExpenditure = 0,
                                       FinishingTransferExpenditurePrice = 0,
                                       FinishingInTransferQty = 0,
                                       FinishingInTransferPrice = 0,
                                       FinishingOutQty = 0,
                                       FinishingOutPrice = 0,
                                       FinishingReturQty = 0,
                                       FinishingReturPrice = 0,
                                       SubconOutQty = 0,
                                       SubconOutPrice = 0,
                                       BeginingBalanceLoadingQty = 0,
                                       BeginingBalanceLoadingPrice = 0,
                                       BeginingBalanceCuttingQty = group.Sum(x=>x.BeginingBalanceCuttingQty),
                                       BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
                                       Ro = key,
                                       ExpenditureGoodRetur = 0,
                                       ExpenditureGoodReturPrice = 0,
                                       QtyCuttingOut = group.Sum(x => x.QtyCuttingOut),
                                       PriceCuttingOut = group.Sum(x => x.PriceCuttingOut),
                                       ExportQty = 0,
                                       ExportPrice = 0,
                                       LocalQty = 0,
                                       LocalPrice = 0,
                                       SampleQty = 0,
                                       SamplePrice = 0,
                                       OtherQty = 0,
                                       OtherPrice = 0,
                                       QtyLoadingInTransfer = 0,
                                       PriceLoadingInTransfer = 0,
                                       ExpenditureGoodInTransfer = 0,
                                       ExpenditureGoodInTransferPrice = 0,
                                       BeginingBalanceFinishingQty = 0,
                                       BeginingBalanceFinishingPrice = 0

                                   });

            var QueryCuttingOutSubkon = (from a in (from aa in garmentCuttingOutRepository.Query
                                                    where aa.CuttingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == (request.unit == 0 ? aa.UnitFromId : request.unit) && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SUBKON"
                                                    select new { aa.RONo, aa.Identity, aa.CuttingOutDate, aa.CuttingOutType })
                                         join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                         join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
                                         select new
                                         {
                                            
                                             BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.CuttingOutQuantity : 0,
                                             Ro = a.RONo,
                                             BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.Price : 0,
                                             QtyCuttingsubkon = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
                                             PriceCuttingsubkon = a.CuttingOutDate >= dateFrom ? c.Price : 0,
                                         }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                         {
                                             QtyCuttingIn = 0,
                                             PriceCuttingIn = 0,
                                             QtySewingIn = 0,
                                             PriceSewingIn = 0,
                                             QtyCuttingOut = 0,
                                             PriceCuttingOut = 0,
                                             QtyCuttingTransfer = 0,
                                             PriceCuttingTransfer = 0,
                                             AvalCutting = 0,
                                             AvalCuttingPrice = 0,
                                             AvalSewing = 0,
                                             AvalSewingPrice = 0,
                                             QtyLoading = 0,
                                             PriceLoading = 0,
                                             QtyLoadingAdjs = 0,
                                             PriceLoadingAdjs = 0,
                                             QtySewingOut = 0,
                                             PriceSewingOut = 0,
                                             QtySewingAdj = 0,
                                             PriceSewingAdj = 0,
                                             WipSewingOut = 0,
                                             WipSewingOutPrice = 0,
                                             WipFinishingOut = 0,
                                             WipFinishingOutPrice = 0,
                                             QtySewingRetur = 0,
                                             PriceSewingRetur = 0,
                                             QtySewingInTransfer = 0,
                                             PriceSewingInTransfer = 0,
                                             FinishingInQty = 0,
                                             FinishingInPrice = 0,
                                             SubconInQty = 0,
                                             SubconInPrice = 0,
                                             FinishingAdjQty = 0,
                                             FinishingAdjPrice = 0,
                                             FinishingTransferExpenditure = 0,
                                             FinishingTransferExpenditurePrice = 0,
                                             FinishingInTransferQty = 0,
                                             FinishingInTransferPrice = 0,
                                             FinishingOutQty = 0,
                                             FinishingOutPrice = 0,
                                             FinishingReturQty = 0,
                                             FinishingReturPrice = 0,
                                             SubconOutQty = 0,
                                             SubconOutPrice = 0,
                                             BeginingBalanceLoadingQty = 0,
                                             BeginingBalanceLoadingPrice = 0,
                                             BeginingBalanceCuttingQty = group.Sum(x=>x.BeginingBalanceCuttingQty),
                                             Ro = key,
                                             BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
                                             FC = 0,
                                             QtyCuttingsubkon = group.Sum(x => x.QtyCuttingsubkon),
                                             PriceCuttingsubkon = group.Sum(x => x.PriceCuttingsubkon),
                                             ExpenditureGoodRetur = 0,
                                             ExpenditureGoodReturPrice = 0,
                                             ExportQty = 0,
                                             ExportPrice = 0,
                                             LocalQty = 0,
                                             LocalPrice = 0,
                                             SampleQty = 0,
                                             SamplePrice = 0,
                                             OtherQty = 0,
                                             OtherPrice = 0,
                                             QtyLoadingInTransfer = 0,
                                             PriceLoadingInTransfer = 0,
                                             ExpenditureGoodInTransfer = 0,
                                             ExpenditureGoodInTransferPrice = 0,
                                             BeginingBalanceFinishingQty = 0,
                                             BeginingBalanceFinishingPrice = 0

                                         });

            var QueryCuttingOutTransfer = (from a in (from aa in garmentCuttingOutRepository.Query
                                                      where aa.CuttingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == (request.unit == 0 ? aa.UnitFromId : request.unit) && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId != aa.UnitFromId
                                                      select new { aa.RONo, aa.Identity, aa.CuttingOutType, aa.CuttingOutDate })
                                           join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
                                           join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
                                           select new 
                                           {
                                               BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.CuttingOutQuantity : 0,
                                               BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom && a.CuttingOutDate > dateBalance ? -c.Price : 0,
                                               Ro = a.RONo,
                                               QtyCuttingTransfer = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
                                               PriceCuttingTransfer = a.CuttingOutDate >= dateFrom ? c.Price : 0,                                               
                                           }).GroupBy(x=>x.Ro,(key,group) => new monitoringView {
                                               QtyCuttingIn = 0,
                                               PriceCuttingIn = 0,
                                               QtySewingIn = 0,
                                               PriceSewingIn = 0,
                                               QtyCuttingOut = 0,
                                               PriceCuttingOut = 0,
                                               QtyCuttingsubkon = 0,
                                               PriceCuttingsubkon = 0,
                                               AvalCutting = 0,
                                               AvalCuttingPrice = 0,
                                               AvalSewing = 0,
                                               AvalSewingPrice = 0,
                                               QtyLoading = 0,
                                               PriceLoading = 0,
                                               QtyLoadingAdjs = 0,
                                               PriceLoadingAdjs = 0,
                                               QtySewingOut = 0,
                                               PriceSewingOut = 0,
                                               QtySewingAdj = 0,
                                               PriceSewingAdj = 0,
                                               WipSewingOut = 0,
                                               WipSewingOutPrice = 0,
                                               WipFinishingOut = 0,
                                               WipFinishingOutPrice = 0,
                                               QtySewingRetur = 0,
                                               PriceSewingRetur = 0,
                                               QtySewingInTransfer = 0,
                                               PriceSewingInTransfer = 0,
                                               FinishingInQty = 0,
                                               FinishingInPrice = 0,
                                               SubconInQty = 0,
                                               SubconInPrice = 0,
                                               FinishingAdjQty = 0,
                                               FinishingAdjPrice = 0,
                                               FinishingTransferExpenditure = 0,
                                               FinishingTransferExpenditurePrice = 0,
                                               FinishingInTransferQty = 0,
                                               FinishingInTransferPrice = 0,
                                               FinishingOutQty = 0,
                                               FinishingOutPrice = 0,
                                               FinishingReturQty = 0,
                                               FinishingReturPrice = 0,
                                               SubconOutQty = 0,
                                               SubconOutPrice = 0,
                                               //BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
                                               //BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
                                               BeginingBalanceLoadingQty = 0,
                                               BeginingBalanceLoadingPrice = 0,
                                               BeginingBalanceCuttingQty = group.Sum(x=>x.BeginingBalanceCuttingQty),
                                               BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
                                               Ro = key,
                                               QtyCuttingTransfer = group.Sum(x => x.QtyCuttingTransfer),
                                               PriceCuttingTransfer = group.Sum(x => x.PriceCuttingTransfer),
                                               ExpenditureGoodRetur = 0,
                                               ExpenditureGoodReturPrice = 0,
                                               ExportQty = 0,
                                               ExportPrice = 0,
                                               LocalQty = 0,
                                               LocalPrice = 0,
                                               SampleQty = 0,
                                               SamplePrice = 0,
                                               OtherQty = 0,
                                               OtherPrice = 0,
                                               QtyLoadingInTransfer = 0,
                                               PriceLoadingInTransfer = 0,
                                               ExpenditureGoodInTransfer = 0,
                                               ExpenditureGoodInTransferPrice = 0,
                                               BeginingBalanceFinishingQty = 0,
                                               BeginingBalanceFinishingPrice = 0

                                           });

            var QueryCuttingIn = (from a in (from aa in garmentCuttingInRepository.Query
                                             where aa.CuttingInDate >= dateBalance && aa.CuttingType != "Non Main Fabric" && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.CuttingInDate <= dateTo
                                             select new { aa.RONo, aa.Identity, aa.CuttingInDate })
                                  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
                                  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
                                  select new
                                  {

                                      BeginingBalanceCuttingQty = a.CuttingInDate < dateFrom && a.CuttingInDate > dateBalance ? c.CuttingInQuantity : 0,
                                      BeginingBalanceCuttingPrice = a.CuttingInDate < dateFrom && a.CuttingInDate > dateBalance ? c.Price : 0,
                                      Ro = a.RONo,
                                      QtyCuttingIn = a.CuttingInDate >= dateFrom ? c.CuttingInQuantity : 0,
                                      PriceCuttingIn = a.CuttingInDate >= dateFrom ? c.Price : 0,

                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                  {
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingTransfer = 0,
                                      PriceCuttingTransfer = 0,
                                      QtyCuttingsubkon = 0,
                                      PriceCuttingsubkon = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtyLoadingAdjs = 0,
                                      PriceLoadingAdjs = 0,
                                      QtySewingOut = 0,
                                      PriceSewingOut = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      WipSewingOut = 0,
                                      WipSewingOutPrice = 0,
                                      WipFinishingOut = 0,
                                      WipFinishingOutPrice = 0,
                                      QtySewingRetur = 0,
                                      PriceSewingRetur = 0,
                                      QtySewingInTransfer = 0,
                                      PriceSewingInTransfer = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingTransferExpenditure = 0,
                                      FinishingTransferExpenditurePrice = 0,
                                      FinishingInTransferQty = 0,
                                      FinishingInTransferPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
                                      BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
                                      Ro = key,
                                      QtyCuttingIn = group.Sum(x => x.QtyCuttingIn),
                                      PriceCuttingIn = group.Sum(x => x.PriceCuttingIn),
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      ExportQty = 0,
                                      ExportPrice = 0,
                                      LocalQty = 0,
                                      LocalPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceLoadingQty = 0,
                                      BeginingBalanceLoadingPrice = 0,
                                      BeginingBalanceFinishingQty = 0,
                                      BeginingBalanceFinishingPrice = 0
                                  });

            var QueryAvalCompSewing = (from a in (from aa in garmentAvalComponentRepository.Query
                                                 where aa.Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.Date <= dateTo && aa.AvalComponentType == "SEWING"
                                                 select new { aa.RONo, aa.Identity, aa.Date, aa.AvalComponentType })
                                      join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                                      select new
                                      {
                                          Ro = a.RONo,
                                          AvalSewing = a.Date >= dateFrom ? b.Quantity : 0,
                                          AvalSewingPrice = a.Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
                                          BeginingBalanceCuttingQty = a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,

                                      }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                          QtySewingIn = 0,
                                          PriceSewingIn = 0,
                                          QtyCuttingOut = 0,
                                          PriceCuttingOut = 0,
                                          QtyCuttingTransfer = 0,
                                          PriceCuttingTransfer = 0,
                                          QtyCuttingsubkon = 0,
                                          PriceCuttingsubkon = 0,
                                          AvalCutting = 0,
                                          AvalCuttingPrice = 0,
                                          QtyLoading = 0,
                                          PriceLoading = 0,
                                          QtyLoadingAdjs = 0,
                                          PriceLoadingAdjs = 0,
                                          QtySewingOut = 0,
                                          PriceSewingOut = 0,
                                          QtySewingAdj = 0,
                                          PriceSewingAdj = 0,
                                          WipSewingOut = 0,
                                          WipSewingOutPrice = 0,
                                          WipFinishingOut = 0,
                                          WipFinishingOutPrice = 0,
                                          QtySewingRetur = 0,
                                          PriceSewingRetur = 0,
                                          QtySewingInTransfer = 0,
                                          PriceSewingInTransfer = 0,
                                          FinishingInQty = 0,
                                          FinishingInPrice = 0,
                                          SubconInQty = 0,
                                          SubconInPrice = 0,
                                          FinishingAdjQty = 0,
                                          FinishingAdjPrice = 0,
                                          FinishingTransferExpenditure = 0,
                                          FinishingTransferExpenditurePrice = 0,
                                          FinishingInTransferQty = 0,
                                          FinishingInTransferPrice = 0,
                                          FinishingOutQty = 0,
                                          FinishingOutPrice = 0,
                                          FinishingReturQty = 0,
                                          FinishingReturPrice = 0,
                                          SubconOutQty = 0,
                                          SubconOutPrice = 0,
                                          BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty), //0,a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
                                          BeginingBalanceCuttingPrice = 0,//a.Date < dateFrom && a.Date > dateBalance  ? -Convert.ToDouble(b.Price) : 0,
                                          Ro = key,
                                          QtyCuttingIn = 0,
                                          PriceCuttingIn = 0,
                                          AvalSewing = group.Sum(x=>x.AvalSewing),
                                          AvalSewingPrice = group.Sum(x => x.AvalSewingPrice),
                                          ExpenditureGoodRetur = 0,
                                          ExpenditureGoodReturPrice = 0,
                                          ExportQty = 0,
                                          ExportPrice = 0,
                                          LocalQty = 0,
                                          LocalPrice = 0,
                                          SampleQty = 0,
                                          SamplePrice = 0,
                                          OtherQty = 0,
                                          OtherPrice = 0,
                                          QtyLoadingInTransfer = 0,
                                          PriceLoadingInTransfer = 0,
                                          ExpenditureGoodInTransfer = 0,
                                          ExpenditureGoodInTransferPrice = 0,
                                          BeginingBalanceLoadingQty = 0,
                                          BeginingBalanceLoadingPrice = 0,
                                          BeginingBalanceFinishingQty = 0,
                                          BeginingBalanceFinishingPrice = 0
                                      });

            var QueryAvalCompCutting = (from a in (from aa in garmentAvalComponentRepository.Query
                                                  where aa.Date >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.Date <= dateTo && aa.AvalComponentType == "CUTTING"
                                                  select new { aa.RONo, aa.Identity, aa.Date, aa.AvalComponentType })
                                       join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
                                       select new
                                       {
                                           Ro = a.RONo,
                                           AvalCutting = a.Date >= dateFrom ? b.Quantity : 0,
                                           AvalCuttingPrice = a.Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
                                           BeginingBalanceCuttingQty = a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
                                           //BeginingBalanceCuttingPrice = a.Date < dateFrom && a.Date > dateBalance ? -Convert.ToDouble(b.Price) : 0
                                       }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                           QtyCuttingIn = 0,
                                           PriceCuttingIn = 0,
                                           QtySewingIn = 0,
                                           PriceSewingIn = 0,
                                           QtyCuttingOut = 0,
                                           PriceCuttingOut = 0,
                                           QtyCuttingTransfer = 0,
                                           PriceCuttingTransfer = 0,
                                           QtyCuttingsubkon = 0,
                                           PriceCuttingsubkon = 0,
                                           AvalSewing = 0,
                                           AvalSewingPrice = 0,
                                           QtyLoading = 0,
                                           PriceLoading = 0,
                                           QtyLoadingAdjs = 0,
                                           PriceLoadingAdjs = 0,
                                           QtySewingOut = 0,
                                           PriceSewingOut = 0,
                                           QtySewingAdj = 0,
                                           PriceSewingAdj = 0,
                                           WipSewingOut = 0,
                                           WipSewingOutPrice = 0,
                                           WipFinishingOut = 0,
                                           WipFinishingOutPrice = 0,
                                           QtySewingRetur = 0,
                                           PriceSewingRetur = 0,
                                           QtySewingInTransfer = 0,
                                           PriceSewingInTransfer = 0,
                                           FinishingInQty = 0,
                                           FinishingInPrice = 0,
                                           SubconInQty = 0,
                                           SubconInPrice = 0,
                                           FinishingAdjQty = 0,
                                           FinishingAdjPrice = 0,
                                           FinishingTransferExpenditure = 0,
                                           FinishingTransferExpenditurePrice = 0,
                                           FinishingInTransferQty = 0,
                                           FinishingInTransferPrice = 0,
                                           FinishingOutQty = 0,
                                           FinishingOutPrice = 0,
                                           FinishingReturQty = 0,
                                           FinishingReturPrice = 0,
                                           SubconOutQty = 0,
                                           SubconOutPrice = 0,
                                           BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty), //0,a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
                                           BeginingBalanceCuttingPrice = 0, //a.Date < dateFrom && a.Date > dateBalance ? -Convert.ToDouble(b.Price) : 0,
                                           Ro = key,
                                           AvalCutting = group.Sum(x=>x.AvalCutting),
                                           AvalCuttingPrice = group.Sum(x => x.AvalCuttingPrice),
                                           ExpenditureGoodRetur = 0,
                                           ExpenditureGoodReturPrice = 0,
                                           ExportQty = 0,
                                           ExportPrice = 0,
                                           LocalQty = 0,
                                           LocalPrice = 0,
                                           SampleQty = 0,
                                           SamplePrice = 0,
                                           OtherQty = 0,
                                           OtherPrice = 0,
                                           QtyLoadingInTransfer = 0,
                                           PriceLoadingInTransfer = 0,
                                           ExpenditureGoodInTransfer = 0,
                                           ExpenditureGoodInTransferPrice = 0,
                                           BeginingBalanceLoadingQty = 0,
                                           BeginingBalanceLoadingPrice = 0,
                                           BeginingBalanceFinishingQty = 0,
                                           BeginingBalanceFinishingPrice = 0
                                       });

            var QuerySewingDO = (from a in (from aa in garmentSewingDORepository.Query
                                            where aa.SewingDODate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.UnitFromId == aa.UnitId && aa.SewingDODate <= dateTo
                                            select new { aa.RONo, aa.Identity, aa.SewingDODate })
                                 join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
                                 select new
                                 {

                                     QtyLoadingIn = a.SewingDODate >= dateFrom ? b.Quantity : 0,
                                     PriceLoadingIn = a.SewingDODate >= dateFrom ? b.Price : 0,
                                     BeginingBalanceLoadingQty = (a.SewingDODate < dateFrom && a.SewingDODate > dateBalance) ? b.Quantity : 0,
                                     BeginingBalanceLoadingPrice = (a.SewingDODate < dateFrom && a.SewingDODate > dateBalance) ? b.Price : 0,
                                     Ro = a.RONo,

                                 }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                 {
                                     QtyCuttingIn = 0,
                                     PriceCuttingIn = 0,
                                     QtySewingIn = 0,
                                     PriceSewingIn = 0,
                                     QtyCuttingOut = 0,
                                     PriceCuttingOut = 0,
                                     QtyCuttingTransfer = 0,
                                     PriceCuttingTransfer = 0,
                                     QtyCuttingsubkon = 0,
                                     PriceCuttingsubkon = 0,
                                     AvalCutting = 0,
                                     AvalCuttingPrice = 0,
                                     AvalSewing = 0,
                                     AvalSewingPrice = 0,
                                     QtyLoading = 0,
                                     PriceLoading = 0,
                                     QtyLoadingAdjs = 0,
                                     PriceLoadingAdjs = 0,
                                     QtySewingOut = 0,
                                     PriceSewingOut = 0,
                                     QtySewingAdj = 0,
                                     PriceSewingAdj = 0,
                                     WipSewingOut = 0,
                                     WipSewingOutPrice = 0,
                                     WipFinishingOut = 0,
                                     WipFinishingOutPrice = 0,
                                     QtySewingRetur = 0,
                                     PriceSewingRetur = 0,
                                     QtySewingInTransfer = 0,
                                     PriceSewingInTransfer = 0,
                                     FinishingInQty = 0,
                                     FinishingInPrice = 0,
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     FinishingAdjPrice = 0,
                                     FinishingTransferExpenditure = 0,
                                     FinishingTransferExpenditurePrice = 0,
                                     FinishingInTransferQty = 0,
                                     FinishingInTransferPrice = 0,
                                     FinishingOutQty = 0,
                                     FinishingOutPrice = 0,
                                     FinishingReturQty = 0,
                                     FinishingReturPrice = 0,
                                     SubconOutQty = 0,
                                     SubconOutPrice = 0,
                                     QtyLoadingIn = group.Sum(x => x.QtyLoadingIn),
                                     PriceLoadingIn = group.Sum(x => x.PriceLoadingIn),
                                     BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty),
                                     BeginingBalanceLoadingPrice = group.Sum(x => x.BeginingBalanceLoadingPrice),
                                     Ro = key,
                                     ExpenditureGoodRetur = 0,
                                     ExpenditureGoodReturPrice = 0,
                                     ExportQty = 0,
                                     ExportPrice = 0,
                                     LocalQty = 0,
                                     LocalPrice = 0,
                                     SampleQty = 0,
                                     SamplePrice = 0,
                                     OtherQty = 0,
                                     OtherPrice = 0,
                                     QtyLoadingInTransfer = 0,
                                     PriceLoadingInTransfer = 0,
                                     ExpenditureGoodInTransfer = 0,
                                     ExpenditureGoodInTransferPrice = 0,
                                     BeginingBalanceCuttingQty = 0,
                                     BeginingBalanceCuttingPrice = 0,
                                     BeginingBalanceFinishingQty = 0,
                                     BeginingBalanceFinishingPrice = 0
                                 });

            var QueryLoadingInTransfer = (from a in (from aa in garmentSewingDORepository.Query
                                                     where aa.SewingDODate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.UnitFromId != aa.UnitId && aa.SewingDODate <= dateTo
                                                     select new { aa.RONo, aa.Identity, aa.SewingDODate })
                                          join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
                                          select new
                                          {

                                              QtyLoadingInTransfer = a.SewingDODate >= dateFrom ? b.Quantity : 0,
                                              PriceLoadingInTransfer = a.SewingDODate >= dateFrom ? b.Price : 0,
                                              BeginingBalanceLoadingQty = (a.SewingDODate < dateFrom && a.SewingDODate > dateBalance) ? b.Quantity : 0,
                                              BeginingBalanceLoadingPrice = (a.SewingDODate < dateFrom && a.SewingDODate > dateBalance) ? b.Price : 0,
                                              Ro = a.RONo,

                                          }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                          {
                                              QtyCuttingIn = 0,
                                              PriceCuttingIn = 0,
                                              QtySewingIn = 0,
                                              PriceSewingIn = 0,
                                              QtyCuttingOut = 0,
                                              PriceCuttingOut = 0,
                                              QtyCuttingTransfer = 0,
                                              PriceCuttingTransfer = 0,
                                              QtyCuttingsubkon = 0,
                                              PriceCuttingsubkon = 0,
                                              AvalCutting = 0,
                                              AvalCuttingPrice = 0,
                                              AvalSewing = 0,
                                              AvalSewingPrice = 0,
                                              QtyLoading = 0,
                                              PriceLoading = 0,
                                              QtyLoadingAdjs = 0,
                                              PriceLoadingAdjs = 0,
                                              QtySewingOut = 0,
                                              PriceSewingOut = 0,
                                              QtySewingAdj = 0,
                                              PriceSewingAdj = 0,
                                              WipSewingOut = 0,
                                              WipSewingOutPrice = 0,
                                              WipFinishingOut = 0,
                                              WipFinishingOutPrice = 0,
                                              QtySewingRetur = 0,
                                              PriceSewingRetur = 0,
                                              QtySewingInTransfer = 0,
                                              PriceSewingInTransfer = 0,
                                              FinishingInQty = 0,
                                              FinishingInPrice = 0,
                                              SubconInQty = 0,
                                              SubconInPrice = 0,
                                              FinishingAdjQty = 0,
                                              FinishingAdjPrice = 0,
                                              FinishingTransferExpenditure = 0,
                                              FinishingTransferExpenditurePrice = 0,
                                              FinishingInTransferQty = 0,
                                              FinishingInTransferPrice = 0,
                                              FinishingOutQty = 0,
                                              FinishingOutPrice = 0,
                                              FinishingReturQty = 0,
                                              FinishingReturPrice = 0,
                                              SubconOutQty = 0,
                                              SubconOutPrice = 0,
                                              QtyLoadingInTransfer = group.Sum(x => x.QtyLoadingInTransfer),
                                              PriceLoadingInTransfer = group.Sum(x => x.PriceLoadingInTransfer),
                                              BeginingBalanceLoadingQty = group.Sum(x => x.BeginingBalanceLoadingQty),
                                              BeginingBalanceLoadingPrice = group.Sum(x => x.BeginingBalanceLoadingPrice),
                                              Ro = key,
                                              ExpenditureGoodRetur = 0,
                                              ExpenditureGoodReturPrice = 0,
                                              ExportQty = 0,
                                              ExportPrice = 0,
                                              LocalQty = 0,
                                              LocalPrice = 0,
                                              SampleQty = 0,
                                              SamplePrice = 0,
                                              OtherQty = 0,
                                              OtherPrice = 0,
                                              ExpenditureGoodInTransfer = 0,
                                              ExpenditureGoodInTransferPrice = 0,
                                              BeginingBalanceCuttingQty = 0,
                                              BeginingBalanceCuttingPrice = 0,
                                              BeginingBalanceFinishingQty = 0,
                                              BeginingBalanceFinishingPrice = 0
                                          });

            var QueryLoading = (from a in (from aa in garmentLoadingRepository.Query
                                          where aa.LoadingDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.LoadingDate <= dateTo
                                          select new { aa.RONo, aa.Identity, aa.LoadingDate, aa.UnitId, aa.UnitFromId })
                               join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
                               select new
                               {
                                   BeginingBalanceLoadingQty = a.LoadingDate < dateFrom && a.UnitId == a.UnitFromId && a.LoadingDate > dateBalance ? -b.Quantity : 0,
                                   BeginingBalanceLoadingPrice = a.LoadingDate < dateFrom && a.UnitId == a.UnitFromId && a.LoadingDate > dateBalance ? -b.Price : 0,
                                   Ro = a.RONo,
                                   QtyLoading = a.LoadingDate >= dateFrom && a.UnitId == a.UnitFromId ? b.Quantity : 0,
                                   PriceLoading = a.LoadingDate >= dateFrom && a.UnitId == a.UnitFromId ? b.Price : 0,
                                   
                               }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                   QtyCuttingIn = 0,
                                   PriceCuttingIn = 0,
                                   QtySewingIn = 0,
                                   PriceSewingIn = 0,
                                   QtyCuttingOut = 0,
                                   PriceCuttingOut = 0,
                                   QtyCuttingTransfer = 0,
                                   PriceCuttingTransfer = 0,
                                   QtyCuttingsubkon = 0,
                                   PriceCuttingsubkon = 0,
                                   AvalCutting = 0,
                                   AvalCuttingPrice = 0,
                                   AvalSewing = 0,
                                   AvalSewingPrice = 0,
                                   QtyLoadingAdjs = 0,
                                   PriceLoadingAdjs = 0,
                                   QtySewingOut = 0,
                                   PriceSewingOut = 0,
                                   QtySewingAdj = 0,
                                   PriceSewingAdj = 0,
                                   WipSewingOut = 0,
                                   WipSewingOutPrice = 0,
                                   WipFinishingOut = 0,
                                   WipFinishingOutPrice = 0,
                                   QtySewingRetur = 0,
                                   PriceSewingRetur = 0,
                                   QtySewingInTransfer = 0,
                                   PriceSewingInTransfer = 0,
                                   FinishingInQty = 0,
                                   FinishingInPrice = 0,
                                   SubconInQty = 0,
                                   SubconInPrice = 0,
                                   FinishingAdjQty = 0,
                                   FinishingAdjPrice = 0,
                                   FinishingTransferExpenditure = 0,
                                   FinishingTransferExpenditurePrice = 0,
                                   FinishingInTransferQty = 0,
                                   FinishingInTransferPrice = 0,
                                   FinishingOutQty = 0,
                                   FinishingOutPrice = 0,
                                   FinishingReturQty = 0,
                                   FinishingReturPrice = 0,
                                   SubconOutQty = 0,
                                   SubconOutPrice = 0,
                                   QtyLoadingInTransfer = 0,
                                   PriceLoadingInTransfer = 0,
                                   // BeginingBalanceSewingQty = a.LoadingDate < dateFrom ? b.Quantity : 0,
                                   //BeginingBalanceSewingPrice = a.LoadingDate < dateFrom ? b.Price : 0,
                                   BeginingBalanceSewingQty = 0,
                                   BeginingBalanceSewingPrice = 0,
                                   BeginingBalanceLoadingQty = group.Sum(x=>x.BeginingBalanceLoadingQty),
                                   BeginingBalanceLoadingPrice = group.Sum(x => x.BeginingBalanceLoadingPrice),
                                   Ro = key,
                                   QtyLoading = group.Sum(x => x.QtyLoading),
                                   PriceLoading = group.Sum(x => x.PriceLoading),
                                   ExpenditureGoodRetur = 0,
                                   ExpenditureGoodReturPrice = 0,
                                   ExportQty = 0,
                                   ExportPrice = 0,
                                   LocalQty = 0,
                                   LocalPrice = 0,
                                   SampleQty = 0,
                                   SamplePrice = 0,
                                   OtherQty = 0,
                                   OtherPrice = 0,
                                   ExpenditureGoodInTransfer = 0,
                                   ExpenditureGoodInTransferPrice = 0,
                                   BeginingBalanceCuttingQty = 0,
                                   BeginingBalanceCuttingPrice = 0,
                                   BeginingBalanceFinishingQty = 0,
                                   BeginingBalanceFinishingPrice = 0
                               });

            var QueryLoadingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
                                             where aa.AdjustmentDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "LOADING"
                                             select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                  join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                  select new 
                                  {
                                      
                                      BeginingBalanceLoadingQty = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                      BeginingBalanceLoadingPrice = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Price : 0,
                                      Ro = a.RONo,
                                      QtyLoadingAdjs = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                      PriceLoadingAdjs = a.AdjustmentDate >= dateFrom ? b.Price : 0,
                                      
                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                      QtyCuttingIn = 0,
                                      PriceCuttingIn = 0,
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingTransfer = 0,
                                      PriceCuttingTransfer = 0,
                                      QtyCuttingsubkon = 0,
                                      PriceCuttingsubkon = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtySewingOut = 0,
                                      PriceSewingOut = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      WipSewingOut = 0,
                                      WipSewingOutPrice = 0,
                                      WipFinishingOut = 0,
                                      WipFinishingOutPrice = 0,
                                      QtySewingRetur = 0,
                                      PriceSewingRetur = 0,
                                      QtySewingInTransfer = 0,
                                      PriceSewingInTransfer = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingTransferExpenditure = 0,
                                      FinishingTransferExpenditurePrice = 0,
                                      FinishingInTransferQty = 0,
                                      FinishingInTransferPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      BeginingBalanceLoadingQty = group.Sum(x=>x.BeginingBalanceLoadingQty),
                                      BeginingBalanceLoadingPrice = group.Sum(x => x.BeginingBalanceLoadingPrice),
                                      Ro = key,
                                      QtyLoadingAdjs = group.Sum(x => x.QtyLoadingAdjs),
                                      PriceLoadingAdjs = group.Sum(x => x.PriceLoadingAdjs),
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      ExportQty = 0,
                                      ExportPrice = 0,
                                      LocalQty = 0,
                                      LocalPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceCuttingQty = 0,
                                      BeginingBalanceCuttingPrice = 0,
                                      BeginingBalanceFinishingQty = 0,
                                      BeginingBalanceFinishingPrice = 0
                                  });

            var QuerySewingIn = (from a in (from aa in garmentSewingInRepository.Query
                                            where aa.SewingInDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.SewingInDate <= dateTo
                                            select new { aa.RONo, aa.Identity, aa.SewingInDate, aa.SewingFrom })
                                 join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
                                 select new
                                 {
                                     BeginingBalanceSewingQty = (a.SewingInDate < dateFrom && a.SewingInDate > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Quantity : 0,
                                     BeginingBalanceSewingPrice = (a.SewingInDate < dateFrom && a.SewingInDate > dateBalance && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Price : 0,
                                     QtySewingIn = (a.SewingInDate >= dateFrom) && a.SewingFrom != "SEWING" ? b.Quantity : 0,
                                     PriceSewingIn = (a.SewingInDate >= dateFrom) && a.SewingFrom != "SEWING" ? b.Price : 0,
                                     Ro = a.RONo

                                 }).GroupBy(x => x.Ro, (key, group) => new monitoringView
                                 {
                                     QtyCuttingIn = 0,
                                     PriceCuttingIn = 0,
                                     QtyCuttingOut = 0,
                                     PriceCuttingOut = 0,
                                     QtyCuttingTransfer = 0,
                                     PriceCuttingTransfer = 0,
                                     QtyCuttingsubkon = 0,
                                     PriceCuttingsubkon = 0,
                                     AvalCutting = 0,
                                     AvalCuttingPrice = 0,
                                     AvalSewing = 0,
                                     AvalSewingPrice = 0,
                                     QtyLoading = 0,
                                     PriceLoading = 0,
                                     QtyLoadingAdjs = 0,
                                     PriceLoadingAdjs = 0,
                                     QtySewingOut = 0,
                                     PriceSewingOut = 0,
                                     QtySewingAdj = 0,
                                     PriceSewingAdj = 0,
                                     WipSewingOut = 0,
                                     WipSewingOutPrice = 0,
                                     WipFinishingOut = 0,
                                     WipFinishingOutPrice = 0,
                                     QtySewingRetur = 0,
                                     PriceSewingRetur = 0,
                                     QtySewingInTransfer = 0,
                                     PriceSewingInTransfer = 0,
                                     FinishingInQty = 0,
                                     FinishingInPrice = 0,
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     FinishingAdjPrice = 0,
                                     FinishingTransferExpenditure = 0,
                                     FinishingTransferExpenditurePrice = 0,
                                     FinishingInTransferQty = 0,
                                     FinishingInTransferPrice = 0,
                                     FinishingOutQty = 0,
                                     FinishingOutPrice = 0,
                                     FinishingReturQty = 0,
                                     FinishingReturPrice = 0,
                                     SubconOutQty = 0,
                                     SubconOutPrice = 0,
                                     BeginingBalanceSewingQty = group.Sum(x => x.BeginingBalanceSewingQty),
                                     BeginingBalanceSewingPrice = group.Sum(x => x.BeginingBalanceSewingPrice),
                                     QtySewingIn = group.Sum(x => x.QtySewingIn),
                                     PriceSewingIn = group.Sum(x => x.PriceSewingIn),
                                     Ro = key,
                                     ExpenditureGoodRetur = 0,
                                     ExpenditureGoodReturPrice = 0,
                                     ExportQty = 0,
                                     ExportPrice = 0,
                                     LocalQty = 0,
                                     LocalPrice = 0,
                                     SampleQty = 0,
                                     SamplePrice = 0,
                                     OtherQty = 0,
                                     OtherPrice = 0,
                                     QtyLoadingInTransfer = 0,
                                     PriceLoadingInTransfer = 0,
                                     ExpenditureGoodInTransfer = 0,
                                     ExpenditureGoodInTransferPrice = 0,
                                     BeginingBalanceCuttingQty = 0,
                                     BeginingBalanceCuttingPrice = 0,
                                     BeginingBalanceLoadingQty = 0,
                                     BeginingBalanceLoadingPrice = 0,
                                     BeginingBalanceFinishingQty = 0,
                                     BeginingBalanceFinishingPrice = 0
                                 });

            var QuerySewingOut = (from a in (from aa in garmentSewingOutRepository.Query
                                             where aa.SewingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.SewingOutDate <= dateTo
                                             select new { aa.RONo, aa.Identity, aa.SewingOutDate, aa.SewingTo, aa.UnitToId, aa.UnitId })
                                  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId

                                  select new 
                                  {
                                      
                                      FinishingTransferExpenditure = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      FinishingTransferExpenditurePrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      FinishingInTransferQty = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      FinishingInTransferPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      BeginingBalanceFinishingQty = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      BeginingBalanceFinishingPrice = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      BeginingBalanceSewingQty = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Quantity : 0 - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Quantity : 0) + ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0),
                                      BeginingBalanceSewingPrice = (a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Price : 0 - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Price : 0) + ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingOutDate > dateBalance && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0),

                                      QtySewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      PriceSewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      QtySewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
                                      PriceSewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
                                      WipSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      WipSewingOutPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      WipFinishingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      WipFinishingOutPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      QtySewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
                                      PriceSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
                                      
                                      Ro = a.RONo,
                                      

                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                      QtyCuttingIn = 0,
                                      PriceCuttingIn = 0,
                                      QtySewingIn = 0,
                                      PriceSewingIn = 0,
                                      QtyCuttingOut = 0,
                                      PriceCuttingOut = 0,
                                      QtyCuttingTransfer = 0,
                                      PriceCuttingTransfer = 0,
                                      AvalCutting = 0,
                                      AvalCuttingPrice = 0,
                                      AvalSewing = 0,
                                      AvalSewingPrice = 0,
                                      QtyLoading = 0,
                                      PriceLoading = 0,
                                      QtyLoadingAdjs = 0,
                                      PriceLoadingAdjs = 0,
                                      QtySewingAdj = 0,
                                      PriceSewingAdj = 0,
                                      FinishingInQty = 0,
                                      FinishingInPrice = 0,
                                      SubconInQty = 0,
                                      SubconInPrice = 0,
                                      FinishingAdjQty = 0,
                                      FinishingAdjPrice = 0,
                                      FinishingOutQty = 0,
                                      FinishingOutPrice = 0,
                                      FinishingReturQty = 0,
                                      FinishingReturPrice = 0,
                                      SubconOutQty = 0,
                                      SubconOutPrice = 0,
                                      FinishingTransferExpenditure = group.Sum(x=>x.FinishingTransferExpenditure),
                                      FinishingTransferExpenditurePrice = group.Sum(x => x.FinishingTransferExpenditurePrice),
                                      FinishingInTransferQty = group.Sum(x => x.FinishingInTransferQty),
                                      FinishingInTransferPrice = group.Sum(x => x.FinishingInTransferPrice),
                                      BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                      BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
                                      BeginingBalanceSewingQty = group.Sum(x => x.BeginingBalanceSewingQty),
                                      BeginingBalanceSewingPrice = group.Sum(x => x.BeginingBalanceSewingPrice),
                                      QtySewingRetur = group.Sum(x => x.QtySewingRetur),
                                      PriceSewingRetur = group.Sum(x => x.PriceSewingRetur),
                                      QtySewingInTransfer = group.Sum(x => x.QtySewingInTransfer),
                                      PriceSewingInTransfer = group.Sum(x => x.PriceSewingInTransfer),
                                      WipSewingOut = group.Sum(x => x.WipSewingOut),
                                      WipSewingOutPrice = group.Sum(x => x.WipSewingOutPrice),
                                      WipFinishingOut = group.Sum(x => x.WipFinishingOut),
                                      WipFinishingOutPrice = group.Sum(x => x.WipFinishingOutPrice),
                                      QtySewingOut = group.Sum(x => x.QtySewingOut),
                                      PriceSewingOut = group.Sum(x => x.PriceSewingOut),
                                      //BeginingBalanceExpenditureGood = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Quantity : 0,
                                      //BeginingBalanceExpenditureGoodPrice = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Price : 0,
                                      BeginingBalanceExpenditureGood = 0,
                                      BeginingBalanceExpenditureGoodPrice = 0,
                                      Ro = key,
                                      ExpenditureGoodRetur = 0,
                                      ExpenditureGoodReturPrice = 0,
                                      QtyLoadingInTransfer = 0,
                                      PriceLoadingInTransfer = 0,
                                      ExportQty = 0,
                                      ExportPrice = 0,
                                      LocalQty = 0,
                                      LocalPrice = 0,
                                      SampleQty = 0,
                                      SamplePrice = 0,
                                      OtherQty = 0,
                                      OtherPrice = 0,
                                      ExpenditureGoodInTransfer = 0,
                                      ExpenditureGoodInTransferPrice = 0,
                                      BeginingBalanceCuttingQty = 0,
                                      BeginingBalanceCuttingPrice = 0,
                                      BeginingBalanceLoadingQty = 0,
                                      BeginingBalanceLoadingPrice = 0
                                  });

            var QuerySewingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
                                            where aa.AdjustmentDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "SEWING"
                                            select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                 join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                 select new
                                 {
                                     
                                     BeginingBalanceSewingQty = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                     BeginingBalanceSewingPrice = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Price : 0,
                                     Ro = a.RONo,
                                     QtySewingAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                     PriceSewingAdj = a.AdjustmentDate >= dateFrom ? b.Price : 0
                                 }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                     QtyCuttingIn = 0,
                                     PriceCuttingIn = 0,
                                     QtySewingIn = 0,
                                     PriceSewingIn = 0,
                                     QtyCuttingOut = 0,
                                     PriceCuttingOut = 0,
                                     QtyCuttingTransfer = 0,
                                     PriceCuttingTransfer = 0,
                                     QtyCuttingsubkon = 0,
                                     PriceCuttingsubkon = 0,
                                     AvalCutting = 0,
                                     AvalCuttingPrice = 0,
                                     AvalSewing = 0,
                                     AvalSewingPrice = 0,
                                     QtyLoading = 0,
                                     PriceLoading = 0,
                                     QtyLoadingAdjs = 0,
                                     PriceLoadingAdjs = 0,
                                     QtySewingOut = 0,
                                     PriceSewingOut = 0,
                                     WipSewingOut = 0,
                                     WipSewingOutPrice = 0,
                                     WipFinishingOut = 0,
                                     WipFinishingOutPrice = 0,
                                     QtySewingRetur = 0,
                                     PriceSewingRetur = 0,
                                     QtySewingInTransfer = 0,
                                     PriceSewingInTransfer = 0,
                                     FinishingInQty = 0,
                                     FinishingInPrice = 0,
                                     SubconInQty = 0,
                                     SubconInPrice = 0,
                                     FinishingAdjQty = 0,
                                     FinishingAdjPrice = 0,
                                     FinishingTransferExpenditure = 0,
                                     FinishingTransferExpenditurePrice = 0,
                                     FinishingInTransferQty = 0,
                                     FinishingInTransferPrice = 0,
                                     FinishingOutQty = 0,
                                     FinishingOutPrice = 0,
                                     FinishingReturQty = 0,
                                     FinishingReturPrice = 0,
                                     SubconOutQty = 0,
                                     SubconOutPrice = 0,
                                     BeginingBalanceSewingQty = group.Sum(x=>x.BeginingBalanceSewingQty),
                                     BeginingBalanceSewingPrice = group.Sum(x => x.BeginingBalanceSewingPrice),
                                     Ro = key,
                                     QtySewingAdj = group.Sum(x => x.QtySewingAdj),
                                     PriceSewingAdj = group.Sum(x => x.PriceSewingAdj),
                                     ExpenditureGoodRetur = 0,
                                     ExpenditureGoodReturPrice = 0,
                                     ExportQty = 0,
                                     ExportPrice = 0,
                                     LocalQty = 0,
                                     LocalPrice = 0,
                                     SampleQty = 0,
                                     SamplePrice = 0,
                                     OtherQty = 0,
                                     OtherPrice = 0,
                                     QtyLoadingInTransfer = 0,
                                     PriceLoadingInTransfer = 0,
                                     ExpenditureGoodInTransfer = 0,
                                     ExpenditureGoodInTransferPrice = 0,
                                     BeginingBalanceCuttingQty = 0,
                                     BeginingBalanceCuttingPrice = 0,
                                     BeginingBalanceLoadingQty = 0,
                                     BeginingBalanceLoadingPrice = 0,
                                     BeginingBalanceFinishingQty = 0,
                                     BeginingBalanceFinishingPrice = 0
                                 });

            var QueryFinishingIn = (from a in (from aa in garmentFinishingInRepository.Query
                                               where aa.FinishingInDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingInDate <= dateTo
                                               select new { aa.RONo, aa.Identity, aa.FinishingInDate, aa.FinishingInType })
                                    join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
                                    select new
                                    {                                    
                                        
                                        BeginingBalanceSubconQty = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                        BeginingBalanceSubconPrice = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                        BeginingBalanceFinishingQty = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                        BeginingBalanceFinishingPrice = (a.FinishingInDate < dateFrom && a.FinishingInDate > dateBalance && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                        FinishingInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                        FinishingInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                        SubconInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                        SubconInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                        Ro = a.RONo,
                                        
                                    }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                        QtyCuttingIn = 0,
                                        PriceCuttingIn = 0,
                                        QtySewingIn = 0,
                                        PriceSewingIn = 0,
                                        QtyCuttingOut = 0,
                                        PriceCuttingOut = 0,
                                        QtyCuttingTransfer = 0,
                                        PriceCuttingTransfer = 0,
                                        QtyCuttingsubkon = 0,
                                        PriceCuttingsubkon = 0,
                                        AvalCutting = 0,
                                        AvalCuttingPrice = 0,
                                        AvalSewing = 0,
                                        AvalSewingPrice = 0,
                                        QtyLoading = 0,
                                        PriceLoading = 0,
                                        QtyLoadingAdjs = 0,
                                        PriceLoadingAdjs = 0,
                                        QtySewingOut = 0,
                                        PriceSewingOut = 0,
                                        QtySewingAdj = 0,
                                        PriceSewingAdj = 0,
                                        WipSewingOut = 0,
                                        WipSewingOutPrice = 0,
                                        WipFinishingOut = 0,
                                        WipFinishingOutPrice = 0,
                                        QtySewingRetur = 0,
                                        PriceSewingRetur = 0,
                                        QtySewingInTransfer = 0,
                                        PriceSewingInTransfer = 0,
                                        FinishingAdjQty = 0,
                                        FinishingAdjPrice = 0,
                                        FinishingTransferExpenditure = 0,
                                        FinishingTransferExpenditurePrice = 0,
                                        FinishingInTransferQty = 0,
                                        FinishingInTransferPrice = 0,
                                        FinishingOutQty = 0,
                                        FinishingOutPrice = 0,
                                        FinishingReturQty = 0,
                                        FinishingReturPrice = 0,
                                        SubconOutQty = 0,
                                        SubconOutPrice = 0,
                                        QtyLoadingInTransfer = 0,
                                        PriceLoadingInTransfer = 0,
                                        BeginingBalanceSubconQty = group.Sum(x=>x.BeginingBalanceSubconQty),
                                        BeginingBalanceSubconPrice = group.Sum(x => x.BeginingBalanceSubconPrice),
                                        BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
                                        BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
                                        FinishingInQty = group.Sum(x => x.FinishingInQty),
                                        FinishingInPrice = group.Sum(x => x.FinishingInPrice),
                                        SubconInQty = group.Sum(x => x.SubconInQty),
                                        SubconInPrice = group.Sum(x => x.SubconInPrice),
                                        Ro = key,
                                        ExpenditureGoodRetur = 0,
                                        ExpenditureGoodReturPrice = 0,
                                        ExportQty = 0,
                                        ExportPrice = 0,
                                        LocalQty = 0,
                                        LocalPrice = 0,
                                        SampleQty = 0,
                                        SamplePrice = 0,
                                        OtherQty = 0,
                                        OtherPrice = 0,
                                        ExpenditureGoodInTransfer = 0,
                                        ExpenditureGoodInTransferPrice = 0,
                                        BeginingBalanceCuttingQty = 0,
                                        BeginingBalanceCuttingPrice = 0,
                                        BeginingBalanceLoadingQty = 0,
                                        BeginingBalanceLoadingPrice = 0
                                    });

            var QueryFinishingOut = (from a in (from aa in garmentFinishingOutRepository.Query
                                                where aa.FinishingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "GUDANG JADI"
                                                select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
                                     join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                     join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                     join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                     select new 
                                     {
                                         
                                         BeginingBalanceFinishingQty = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? -b.Quantity : 0,
                                         BeginingBalanceFinishingPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? -b.Price : 0,
                                         BeginingBalanceExpenditureGood = ((a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0) + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0),
                                         BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType != "PEMBELIAN") ? b.Price : 0 + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0),
                                         BeginingBalanceSubconQty = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Quantity : 0,
                                         BeginingBalanceSubconPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && d.FinishingInType == "PEMBELIAN") ? -b.Price : 0,

                                         FinishingOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
                                         FinishingOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Price : 0,
                                         SubconOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
                                         SubconOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0,
                                         Ro = a.RONo,
                                         

                                     }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                         QtyCuttingIn = 0,
                                         PriceCuttingIn = 0,
                                         QtySewingIn = 0,
                                         PriceSewingIn = 0,
                                         QtyCuttingOut = 0,
                                         PriceCuttingOut = 0,
                                         QtyCuttingTransfer = 0,
                                         PriceCuttingTransfer = 0,
                                         QtyCuttingsubkon = 0,
                                         PriceCuttingsubkon = 0,
                                         AvalCutting = 0,
                                         AvalCuttingPrice = 0,
                                         AvalSewing = 0,
                                         AvalSewingPrice = 0,
                                         QtyLoading = 0,
                                         PriceLoading = 0,
                                         QtyLoadingAdjs = 0,
                                         PriceLoadingAdjs = 0,
                                         QtySewingOut = 0,
                                         PriceSewingOut = 0,
                                         QtySewingAdj = 0,
                                         PriceSewingAdj = 0,
                                         WipSewingOut = 0,
                                         WipSewingOutPrice = 0,
                                         WipFinishingOut = 0,
                                         WipFinishingOutPrice = 0,
                                         QtySewingRetur = 0,
                                         PriceSewingRetur = 0,
                                         QtySewingInTransfer = 0,
                                         PriceSewingInTransfer = 0,
                                         FinishingInQty = 0,
                                         FinishingInPrice = 0,
                                         SubconInQty = 0,
                                         SubconInPrice = 0,
                                         FinishingAdjQty = 0,
                                         FinishingAdjPrice = 0,
                                         FinishingTransferExpenditure = 0,
                                         FinishingTransferExpenditurePrice = 0,
                                         FinishingInTransferQty = 0,
                                         FinishingInTransferPrice = 0,
                                         FinishingReturQty = 0,
                                         FinishingReturPrice = 0,
                                         QtyLoadingInTransfer = 0,
                                         PriceLoadingInTransfer = 0,
                                         BeginingBalanceFinishingQty = group.Sum(x=>x.BeginingBalanceFinishingQty),
                                         BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
                                         BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                         BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                                         BeginingBalanceSubconQty = group.Sum(x => x.BeginingBalanceSubconQty),
                                         BeginingBalanceSubconPrice = group.Sum(x => x.BeginingBalanceSubconPrice),
                                         FinishingOutQty = group.Sum(x => x.FinishingOutQty),
                                         FinishingOutPrice = group.Sum(x => x.FinishingOutPrice),
                                         SubconOutQty = group.Sum(x => x.SubconOutQty),
                                         SubconOutPrice = group.Sum(x => x.SubconOutPrice),
                                         Ro = key,
                                         ExpenditureGoodRetur = 0,
                                         ExpenditureGoodReturPrice = 0,
                                         ExportQty = 0,
                                         ExportPrice = 0,
                                         LocalQty = 0,
                                         LocalPrice = 0,
                                         SampleQty = 0,
                                         SamplePrice = 0,
                                         OtherQty = 0,
                                         OtherPrice = 0,
                                         ExpenditureGoodInTransfer = 0,
                                         ExpenditureGoodInTransferPrice = 0,
                                         BeginingBalanceCuttingQty = 0,
                                         BeginingBalanceCuttingPrice = 0,
                                         BeginingBalanceLoadingQty = 0,
                                         BeginingBalanceLoadingPrice = 0
                                     });

            var QueryExpenditureGoodInTransfer = (from a in (from aa in garmentFinishingOutRepository.Query
                                                             where aa.FinishingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId != aa.UnitToId && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "GUDANG JADI" && aa.UnitToId == (request.unit == 0 ? aa.UnitToId : request.unit)
                                                             select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
                                                  join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                                  join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                                  join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                                  select new 
                                                  {
                                                      
                                                      Ro = a.RONo,
                                                      ExpenditureGoodInTransfer = (a.FinishingOutDate >= dateFrom) ? b.Quantity : 0,
                                                      ExpenditureGoodInTransferPrice = (a.FinishingOutDate >= dateFrom) ? b.Price : 0,
                                                      BeginingBalanceExpenditureGood = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance) ? b.Quantity : 0,
                                                      BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance) ? b.Price : 0,

                                                  }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                                      QtyCuttingIn = 0,
                                                      PriceCuttingIn = 0,
                                                      QtySewingIn = 0,
                                                      PriceSewingIn = 0,
                                                      QtyCuttingOut = 0,
                                                      PriceCuttingOut = 0,
                                                      QtyCuttingTransfer = 0,
                                                      PriceCuttingTransfer = 0,
                                                      QtyCuttingsubkon = 0,
                                                      PriceCuttingsubkon = 0,
                                                      AvalCutting = 0,
                                                      AvalCuttingPrice = 0,
                                                      AvalSewing = 0,
                                                      AvalSewingPrice = 0,
                                                      QtyLoading = 0,
                                                      PriceLoading = 0,
                                                      QtyLoadingAdjs = 0,
                                                      PriceLoadingAdjs = 0,
                                                      QtySewingOut = 0,
                                                      PriceSewingOut = 0,
                                                      QtySewingAdj = 0,
                                                      PriceSewingAdj = 0,
                                                      WipSewingOut = 0,
                                                      WipSewingOutPrice = 0,
                                                      WipFinishingOut = 0,
                                                      WipFinishingOutPrice = 0,
                                                      QtySewingRetur = 0,
                                                      PriceSewingRetur = 0,
                                                      QtySewingInTransfer = 0,
                                                      PriceSewingInTransfer = 0,
                                                      FinishingInQty = 0,
                                                      FinishingInPrice = 0,
                                                      SubconInQty = 0,
                                                      SubconInPrice = 0,
                                                      FinishingAdjQty = 0,
                                                      FinishingAdjPrice = 0,
                                                      FinishingTransferExpenditure = 0,
                                                      FinishingTransferExpenditurePrice = 0,
                                                      FinishingInTransferQty = 0,
                                                      FinishingInTransferPrice = 0,
                                                      FinishingReturQty = 0,
                                                      FinishingReturPrice = 0,
                                                      BeginingBalanceFinishingQty = 0,
                                                      BeginingBalanceFinishingPrice = 0,
                                                      FinishingOutQty = 0,
                                                      FinishingOutPrice = 0,
                                                      SubconOutQty = 0,
                                                      SubconOutPrice = 0,
                                                      Ro = key,
                                                      ExpenditureGoodRetur = 0,
                                                      ExpenditureGoodReturPrice = 0,
                                                      ExportQty = 0,
                                                      ExportPrice = 0,
                                                      LocalQty = 0,
                                                      LocalPrice = 0,
                                                      SampleQty = 0,
                                                      SamplePrice = 0,
                                                      OtherQty = 0,
                                                      OtherPrice = 0,
                                                      QtyLoadingInTransfer = 0,
                                                      PriceLoadingInTransfer = 0,
                                                      ExpenditureGoodInTransfer = group.Sum(x=>x.ExpenditureGoodInTransfer),
                                                      ExpenditureGoodInTransferPrice = group.Sum(x => x.ExpenditureGoodInTransferPrice),
                                                      BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
                                                      BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                                                      BeginingBalanceCuttingQty = 0,
                                                      BeginingBalanceCuttingPrice = 0,
                                                      BeginingBalanceLoadingQty = 0,
                                                      BeginingBalanceLoadingPrice = 0
                                                  });

            var QueryFinishingAdj = (from a in (from aa in garmentAdjustmentRepository.Query
                                               where aa.AdjustmentDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "FINISHING"
                                               select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                    join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                    select new 
                                    {
                                        BeginingBalanceFinishingQty = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                        BeginingBalanceFinishingPrice = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Price : 0,
                                        FinishingAdjQty = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                        FinishingAdjPrice = a.AdjustmentDate >= dateFrom ? b.Price : 0,
                                        Ro = a.RONo
                                    }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                        QtyCuttingIn = 0,
                                        PriceCuttingIn = 0,
                                        QtySewingIn = 0,
                                        PriceSewingIn = 0,
                                        QtyCuttingOut = 0,
                                        PriceCuttingOut = 0,
                                        QtyCuttingTransfer = 0,
                                        PriceCuttingTransfer = 0,
                                        QtyCuttingsubkon = 0,
                                        PriceCuttingsubkon = 0,
                                        AvalCutting = 0,
                                        AvalCuttingPrice = 0,
                                        AvalSewing = 0,
                                        AvalSewingPrice = 0,
                                        QtyLoading = 0,
                                        PriceLoading = 0,
                                        QtyLoadingAdjs = 0,
                                        PriceLoadingAdjs = 0,
                                        QtySewingOut = 0,
                                        PriceSewingOut = 0,
                                        QtySewingAdj = 0,
                                        PriceSewingAdj = 0,
                                        WipSewingOut = 0,
                                        WipSewingOutPrice = 0,
                                        WipFinishingOut = 0,
                                        WipFinishingOutPrice = 0,
                                        QtySewingRetur = 0,
                                        PriceSewingRetur = 0,
                                        QtySewingInTransfer = 0,
                                        PriceSewingInTransfer = 0,
                                        FinishingInQty = 0,
                                        FinishingInPrice = 0,
                                        SubconInQty = 0,
                                        SubconInPrice = 0,
                                        FinishingTransferExpenditure = 0,
                                        FinishingTransferExpenditurePrice = 0,
                                        FinishingInTransferQty = 0,
                                        FinishingInTransferPrice = 0,
                                        QtyLoadingInTransfer = 0,
                                        PriceLoadingInTransfer = 0,
                                        BeginingBalanceFinishingQty = group.Sum(x=>x.BeginingBalanceFinishingQty),
                                        BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
                                        FinishingAdjQty = group.Sum(x => x.FinishingAdjQty),
                                        FinishingAdjPrice = group.Sum(x => x.FinishingAdjPrice),
                                        FinishingOutQty = 0,
                                        FinishingOutPrice = 0,
                                        FinishingReturQty = 0,
                                        FinishingReturPrice = 0,
                                        SubconOutQty = 0,
                                        SubconOutPrice = 0,
                                        Ro = key,
                                        ExpenditureGoodRetur = 0,
                                        ExpenditureGoodReturPrice = 0,
                                        ExportQty = 0,
                                        ExportPrice = 0,
                                        LocalQty = 0,
                                        LocalPrice = 0,
                                        SampleQty = 0,
                                        SamplePrice = 0,
                                        OtherQty = 0,
                                        OtherPrice = 0,
                                        ExpenditureGoodInTransfer = 0,
                                        ExpenditureGoodInTransferPrice = 0,
                                        BeginingBalanceCuttingQty = 0,
                                        BeginingBalanceCuttingPrice = 0,
                                        BeginingBalanceLoadingQty = 0,
                                        BeginingBalanceLoadingPrice = 0
                                    });

            var QueryFinishingRetur = (from a in (from aa in garmentFinishingOutRepository.Query
                                                  where aa.FinishingOutDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "SEWING"
                                                  select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo, aa.UnitId, aa.UnitToId })
                                       join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
                                       join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
                                       join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
                                       select new 
                                       {
                                           
                                           BeginingBalanceFinishingQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && a.UnitId == a.UnitToId) ? -b.Quantity : 0,
                                           BeginingBalanceFinishingPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate < dateFrom && a.FinishingOutDate > dateBalance && a.UnitId == a.UnitToId) ? -b.Price : 0,
                                           FinishingReturQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitToId == a.UnitToId) ? b.Quantity : 0,
                                           FinishingReturPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitToId == a.UnitToId) ? b.Price : 0,
                                           Ro = a.RONo,
                                           
                                       }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                           QtyCuttingIn = 0,
                                           PriceCuttingIn = 0,
                                           QtySewingIn = 0,
                                           PriceSewingIn = 0,
                                           QtyCuttingOut = 0,
                                           PriceCuttingOut = 0,
                                           QtyCuttingTransfer = 0,
                                           PriceCuttingTransfer = 0,
                                           QtyCuttingsubkon = 0,
                                           PriceCuttingsubkon = 0,
                                           AvalCutting = 0,
                                           AvalCuttingPrice = 0,
                                           AvalSewing = 0,
                                           AvalSewingPrice = 0,
                                           QtyLoading = 0,
                                           PriceLoading = 0,
                                           QtyLoadingAdjs = 0,
                                           PriceLoadingAdjs = 0,
                                           QtySewingOut = 0,
                                           PriceSewingOut = 0,
                                           QtySewingAdj = 0,
                                           PriceSewingAdj = 0,
                                           WipSewingOut = 0,
                                           WipSewingOutPrice = 0,
                                           WipFinishingOut = 0,
                                           WipFinishingOutPrice = 0,
                                           QtySewingRetur = 0,
                                           PriceSewingRetur = 0,
                                           QtySewingInTransfer = 0,
                                           PriceSewingInTransfer = 0,
                                           FinishingInQty = 0,
                                           FinishingInPrice = 0,
                                           SubconInQty = 0,
                                           SubconInPrice = 0,
                                           FinishingAdjQty = 0,
                                           FinishingAdjPrice = 0,
                                           FinishingTransferExpenditure = 0,
                                           FinishingTransferExpenditurePrice = 0,
                                           FinishingInTransferQty = 0,
                                           FinishingInTransferPrice = 0,
                                           FinishingOutQty = 0,
                                           FinishingOutPrice = 0,
                                           SubconOutQty = 0,
                                           QtyLoadingInTransfer = 0,
                                           PriceLoadingInTransfer = 0,
                                           SubconOutPrice = 0,
                                           BeginingBalanceFinishingQty = group.Sum(x=>x.BeginingBalanceFinishingQty),
                                           BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
                                           FinishingReturQty = group.Sum(x => x.FinishingReturQty),
                                           FinishingReturPrice = group.Sum(x => x.FinishingReturPrice),
                                           Ro = key,
                                           ExpenditureGoodRetur = 0,
                                           ExpenditureGoodReturPrice = 0,
                                           ExportQty = 0,
                                           ExportPrice = 0,
                                           LocalQty = 0,
                                           LocalPrice = 0,
                                           SampleQty = 0,
                                           SamplePrice = 0,
                                           OtherQty = 0,
                                           OtherPrice = 0,
                                           ExpenditureGoodInTransfer = 0,
                                           ExpenditureGoodInTransferPrice = 0,
                                           BeginingBalanceCuttingQty = 0,
                                           BeginingBalanceCuttingPrice = 0,
                                           BeginingBalanceLoadingQty = 0,
                                           BeginingBalanceLoadingPrice = 0
                                       });

            var QueryExpenditureGoods = (from a in (from aa in garmentExpenditureGoodRepository.Query
                                                    where aa.ExpenditureDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.ExpenditureDate <= dateTo
                                                    select new { aa.RONo, aa.Identity, aa.ExpenditureDate, aa.ExpenditureType })
                                         join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
                                         select new 
                                         {
                                             
                                             BeginingBalanceExpenditureGood = a.ExpenditureDate < dateFrom && a.ExpenditureDate > dateBalance ? -b.Quantity : 0,
                                             BeginingBalanceExpenditureGoodPrice = a.ExpenditureDate < dateFrom && a.ExpenditureDate > dateBalance ? -b.Price : 0,
                                             ExportQty = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Quantity : 0,
                                             ExportPrice = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Price : 0,
                                             SampleQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "LAIN-LAIN")) ? b.Quantity : 0,
                                             SamplePrice = (a.ExpenditureDate >= dateFrom & (a.ExpenditureType == "LAIN-LAIN")) ? b.Price : 0,
                                             LocalQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "LOKAL")) ? b.Quantity : 0,
                                             LocalPrice = (a.ExpenditureDate >= dateFrom & (a.ExpenditureType == "LOKAL")) ? b.Price : 0,
                                             OtherQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Quantity : 0,
                                             OtherPrice = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Price : 0,
                                             Ro = a.RONo,
                                             
                                         }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                             QtyCuttingIn = 0,
                                             PriceCuttingIn = 0,
                                             QtySewingIn = 0,
                                             PriceSewingIn = 0,
                                             QtyCuttingOut = 0,
                                             PriceCuttingOut = 0,
                                             QtyCuttingTransfer = 0,
                                             PriceCuttingTransfer = 0,
                                             QtyCuttingsubkon = 0,
                                             PriceCuttingsubkon = 0,
                                             QtyLoadingInTransfer = 0,
                                             PriceLoadingInTransfer = 0,
                                             AvalCutting = 0,
                                             AvalCuttingPrice = 0,
                                             AvalSewing = 0,
                                             AvalSewingPrice = 0,
                                             QtyLoading = 0,
                                             PriceLoading = 0,
                                             QtyLoadingAdjs = 0,
                                             PriceLoadingAdjs = 0,
                                             QtySewingOut = 0,
                                             PriceSewingOut = 0,
                                             QtySewingAdj = 0,
                                             PriceSewingAdj = 0,
                                             WipSewingOut = 0,
                                             WipSewingOutPrice = 0,
                                             WipFinishingOut = 0,
                                             WipFinishingOutPrice = 0,
                                             QtySewingRetur = 0,
                                             PriceSewingRetur = 0,
                                             QtySewingInTransfer = 0,
                                             PriceSewingInTransfer = 0,
                                             FinishingInQty = 0,
                                             FinishingInPrice = 0,
                                             SubconInQty = 0,
                                             SubconInPrice = 0,
                                             FinishingAdjQty = 0,
                                             FinishingAdjPrice = 0,
                                             FinishingTransferExpenditure = 0,
                                             FinishingTransferExpenditurePrice = 0,
                                             FinishingInTransferQty = 0,
                                             FinishingInTransferPrice = 0,
                                             FinishingOutQty = 0,
                                             FinishingOutPrice = 0,
                                             FinishingReturQty = 0,
                                             FinishingReturPrice = 0,
                                             SubconOutQty = 0,
                                             SubconOutPrice = 0,
                                             BeginingBalanceExpenditureGood = group.Sum(x=>x.BeginingBalanceExpenditureGood),
                                             BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                                             ExportQty = group.Sum(x => x.ExportQty),
                                             ExportPrice = group.Sum(x => x.ExportPrice),
                                             LocalQty = group.Sum(x => x.LocalQty),
                                             LocalPrice = group.Sum(x => x.LocalPrice),
                                             SampleQty = group.Sum(x => x.SampleQty),
                                             SamplePrice = group.Sum(x => x.SamplePrice),
                                             OtherQty = group.Sum(x => x.OtherQty),
                                             OtherPrice = group.Sum(x => x.OtherPrice),
                                             Ro = key,
                                             ExpenditureGoodRetur = 0,
                                             ExpenditureGoodReturPrice = 0,
                                             ExpenditureGoodInTransfer = 0,
                                             ExpenditureGoodInTransferPrice = 0,
                                             BeginingBalanceCuttingQty = 0,
                                             BeginingBalanceCuttingPrice = 0,
                                             BeginingBalanceLoadingQty = 0,
                                             BeginingBalanceLoadingPrice = 0,
                                             BeginingBalanceFinishingQty = 0,
                                             BeginingBalanceFinishingPrice = 0
                                         });

            var QueryExpenditureGoodsAdj = (from a in (from aa in garmentAdjustmentRepository.Query
                                                      where aa.AdjustmentDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.AdjustmentDate <= dateTo && aa.AdjustmentType == "BARANG JADI"
                                                      select new { aa.RONo, aa.Identity, aa.AdjustmentDate })
                                           join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
                                           select new 
                                           {
                                               
                                               BeginingBalanceExpenditureGood = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Quantity : 0,
                                               BeginingBalanceExpenditureGoodPrice = a.AdjustmentDate < dateFrom && a.AdjustmentDate > dateBalance ? -b.Price : 0,
                                               ExpenditureGoodAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
                                               ExpenditureGoodAdjPrice = a.AdjustmentDate >= dateFrom ? b.Price : 0,
                                               Ro = a.RONo,
                                               
                                           }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                               QtyCuttingIn = 0,
                                               PriceCuttingIn = 0,
                                               QtySewingIn = 0,
                                               PriceSewingIn = 0,
                                               QtyCuttingOut = 0,
                                               PriceCuttingOut = 0,
                                               QtyCuttingTransfer = 0,
                                               PriceCuttingTransfer = 0,
                                               QtyCuttingsubkon = 0,
                                               PriceCuttingsubkon = 0,
                                               AvalCutting = 0,
                                               AvalCuttingPrice = 0,
                                               AvalSewing = 0,
                                               AvalSewingPrice = 0,
                                               QtyLoading = 0,
                                               PriceLoading = 0,
                                               QtyLoadingAdjs = 0,
                                               PriceLoadingAdjs = 0,
                                               QtyLoadingInTransfer = 0,
                                               PriceLoadingInTransfer = 0,
                                               QtySewingOut = 0,
                                               PriceSewingOut = 0,
                                               QtySewingAdj = 0,
                                               PriceSewingAdj = 0,
                                               WipSewingOut = 0,
                                               WipSewingOutPrice = 0,
                                               WipFinishingOut = 0,
                                               WipFinishingOutPrice = 0,
                                               QtySewingRetur = 0,
                                               PriceSewingRetur = 0,
                                               QtySewingInTransfer = 0,
                                               PriceSewingInTransfer = 0,
                                               FinishingInQty = 0,
                                               FinishingInPrice = 0,
                                               SubconInQty = 0,
                                               SubconInPrice = 0,
                                               FinishingAdjQty = 0,
                                               FinishingAdjPrice = 0,
                                               FinishingTransferExpenditure = 0,
                                               FinishingTransferExpenditurePrice = 0,
                                               FinishingInTransferQty = 0,
                                               FinishingInTransferPrice = 0,
                                               FinishingOutQty = 0,
                                               FinishingOutPrice = 0,
                                               FinishingReturQty = 0,
                                               FinishingReturPrice = 0,
                                               SubconOutQty = 0,
                                               SubconOutPrice = 0,
                                               BeginingBalanceExpenditureGood = group.Sum(x=>x.BeginingBalanceExpenditureGood),
                                               BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                                               ExpenditureGoodAdj = group.Sum(x => x.ExpenditureGoodAdj),
                                               ExpenditureGoodAdjPrice = group.Sum(x => x.ExpenditureGoodAdjPrice),
                                               Ro = key,
                                               ExpenditureGoodRetur = 0,
                                               ExpenditureGoodReturPrice = 0,
                                               ExportQty = 0,
                                               ExportPrice = 0,
                                               LocalQty = 0,
                                               LocalPrice = 0,
                                               SampleQty = 0,
                                               SamplePrice = 0,
                                               OtherQty = 0,
                                               OtherPrice = 0,
                                               ExpenditureGoodInTransfer = 0,
                                               ExpenditureGoodInTransferPrice = 0,
                                               BeginingBalanceCuttingQty = 0,
                                               BeginingBalanceCuttingPrice = 0,
                                               BeginingBalanceLoadingQty = 0,
                                               BeginingBalanceLoadingPrice = 0,
                                               BeginingBalanceFinishingQty = 0,
                                               BeginingBalanceFinishingPrice = 0
                                           });

            var QueryExpenditureGoodRetur = (from a in (from aa in garmentExpenditureGoodReturnRepository.Query
                                                       where aa.ReturDate >= dateBalance && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.ReturDate <= dateTo
                                                       select new { aa.RONo, aa.Identity, aa.ReturDate })
                                            join b in garmentExpenditureGoodReturnItemRepository.Query on a.Identity equals b.ReturId
                                            select new monitoringView
                                            {
                                                
                                                BeginingBalanceExpenditureGood = a.ReturDate < dateFrom && a.ReturDate > dateBalance ? b.Quantity : 0,
                                                BeginingBalanceExpenditureGoodPrice = a.ReturDate < dateFrom && a.ReturDate > dateBalance ? b.Price : 0,
                                                ExpenditureGoodRetur = a.ReturDate >= dateFrom ? b.Quantity : 0,
                                                ExpenditureGoodReturPrice = a.ReturDate >= dateFrom ? b.Price : 0,
                                                Ro = a.RONo,
                                                
                                            }).GroupBy(x => x.Ro, (key, group) => new monitoringView {
                                                QtyCuttingIn = 0,
                                                PriceCuttingIn = 0,
                                                QtySewingIn = 0,
                                                PriceSewingIn = 0,
                                                QtyCuttingOut = 0,
                                                PriceCuttingOut = 0,
                                                QtyCuttingTransfer = 0,
                                                PriceCuttingTransfer = 0,
                                                QtyLoadingInTransfer = 0,
                                                PriceLoadingInTransfer = 0,
                                                QtyCuttingsubkon = 0,
                                                PriceCuttingsubkon = 0,
                                                AvalCutting = 0,
                                                AvalCuttingPrice = 0,
                                                AvalSewing = 0,
                                                AvalSewingPrice = 0,
                                                QtyLoading = 0,
                                                PriceLoading = 0,
                                                QtyLoadingAdjs = 0,
                                                PriceLoadingAdjs = 0,
                                                QtySewingOut = 0,
                                                PriceSewingOut = 0,
                                                QtySewingAdj = 0,
                                                PriceSewingAdj = 0,
                                                WipSewingOut = 0,
                                                WipSewingOutPrice = 0,
                                                WipFinishingOut = 0,
                                                WipFinishingOutPrice = 0,
                                                QtySewingRetur = 0,
                                                PriceSewingRetur = 0,
                                                QtySewingInTransfer = 0,
                                                PriceSewingInTransfer = 0,
                                                FinishingInQty = 0,
                                                FinishingInPrice = 0,
                                                SubconInQty = 0,
                                                SubconInPrice = 0,
                                                FinishingAdjQty = 0,
                                                FinishingAdjPrice = 0,
                                                FinishingTransferExpenditure = 0,
                                                FinishingTransferExpenditurePrice = 0,
                                                FinishingInTransferQty = 0,
                                                FinishingInTransferPrice = 0,
                                                FinishingOutQty = 0,
                                                FinishingOutPrice = 0,
                                                FinishingReturQty = 0,
                                                FinishingReturPrice = 0,
                                                SubconOutQty = 0,
                                                SubconOutPrice = 0,
                                                BeginingBalanceExpenditureGood = group.Sum(x=>x.BeginingBalanceExpenditureGood),
                                                BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                                                ExpenditureGoodRetur = group.Sum(x => x.ExpenditureGoodRetur),
                                                ExpenditureGoodReturPrice = group.Sum(x => x.ExpenditureGoodReturPrice),
                                                Ro = key,
                                                ExportQty = 0,
                                                ExportPrice = 0,
                                                LocalQty = 0,
                                                LocalPrice = 0,
                                                SampleQty = 0,
                                                SamplePrice = 0,
                                                OtherQty = 0,
                                                OtherPrice = 0,
                                                ExpenditureGoodInTransfer = 0,
                                                ExpenditureGoodInTransferPrice = 0,
                                                BeginingBalanceCuttingQty = 0,
                                                BeginingBalanceCuttingPrice = 0,
                                                BeginingBalanceLoadingQty = 0,
                                                BeginingBalanceLoadingPrice = 0,
                                                BeginingBalanceFinishingQty = 0,
                                                BeginingBalanceFinishingPrice = 0
                                            });
            var queryNow = QueryCuttingIn
                .Union(queryBalance)
				.Union(QueryCuttingOut)
				.Union(QueryCuttingOutSubkon)
				.Union(QueryCuttingOutTransfer)
				.Union(QueryAvalCompCutting)
				.Union(QueryAvalCompSewing)
				.Union(QuerySewingDO)
				.Union(QueryLoading)
				.Union(QueryLoadingAdj)
				.Union(QuerySewingIn)
				.Union(QuerySewingOut)
				.Union(QuerySewingAdj)
				.Union(QueryFinishingIn)
				.Union(QueryFinishingOut)
				.Union(QueryFinishingAdj)
				.Union(QueryFinishingRetur)
				.Union(QueryExpenditureGoods)
				.Union(QueryExpenditureGoodsAdj)
				.Union(QueryExpenditureGoodRetur)
				.Union(QueryExpenditureGoodInTransfer)
				.Union(QueryLoadingInTransfer)
				.AsEnumerable();

            var queryNow2 = queryNow.Where(x => x.BeginingBalanceCuttingQty != 0).AsEnumerable();

            //queryNow = queryNow.Where(x => Convert.ToInt32(x.Ro.Substring(0, 2)) > 19).AsEnumerable();
            
            var querySum = (from a in queryNow
							join b in queryGroup on a.Ro equals b.Ro
							select new
							{
								b.Article,
								b.Comodity,
								b.FC,
								b.BasicPrice,
								b.Fare,
								b.FareNew,
								a.Ro,
								a.BeginingBalanceCuttingQty,
								a.BeginingBalanceCuttingPrice,
								a.QtyCuttingIn,
								a.PriceCuttingIn,
								a.QtyCuttingOut,
								a.PriceCuttingOut,
								a.QtyCuttingTransfer,
								a.PriceCuttingTransfer,
								a.QtyCuttingsubkon,
								a.PriceCuttingsubkon,
								a.AvalCutting,
								a.AvalCuttingPrice,
								a.AvalSewing,
								a.AvalSewingPrice,
								a.BeginingBalanceLoadingQty,
								a.BeginingBalanceLoadingPrice,
								a.QtyLoadingIn,
								a.PriceLoadingIn,
								a.QtyLoading,
								a.PriceLoading,
								a.QtyLoadingAdjs,
								a.PriceLoadingAdjs,
								a.BeginingBalanceSewingQty,
								a.BeginingBalanceSewingPrice,
								a.QtySewingIn,
								a.PriceSewingIn,
								a.QtySewingOut,
								a.PriceSewingOut,
								a.QtySewingInTransfer,
								a.PriceSewingInTransfer,
								a.WipSewingOut,
								a.WipSewingOutPrice,
								a.WipFinishingOut,
								a.WipFinishingOutPrice,
								a.QtySewingRetur,
								a.PriceSewingRetur,
								a.QtySewingAdj,
								a.PriceSewingAdj,
								a.BeginingBalanceFinishingQty,
								a.BeginingBalanceFinishingPrice,
								a.FinishingInQty,
								a.FinishingInPrice,
								a.BeginingBalanceSubconQty,
								a.BeginingBalanceSubconPrice,
								a.SubconInQty,
								a.SubconInPrice,
								a.SubconOutQty,
								a.SubconOutPrice,
								a.FinishingOutQty,
								a.FinishingOutPrice,
								a.FinishingInTransferQty,
								a.FinishingInTransferPrice,
								a.FinishingAdjQty,
								a.FinishingAdjPrice,
								a.FinishingReturQty,
								a.FinishingReturPrice,
								a.BeginingBalanceExpenditureGood,
								a.BeginingBalanceExpenditureGoodPrice,
								a.ExpenditureGoodRetur,
								a.ExpenditureGoodReturPrice,
								a.ExportQty,
								a.ExportPrice,
                                a.LocalQty,
                                a.LocalPrice,
                                a.OtherQty,
								a.OtherPrice,
								a.SampleQty,
								a.SamplePrice,
								a.ExpenditureGoodAdj,
								a.ExpenditureGoodAdjPrice,
								a.ExpenditureGoodInTransfer,
								a.ExpenditureGoodInTransferPrice,
								a.QtyLoadingInTransfer,
								a.PriceLoadingInTransfer
							})
				.GroupBy(x => new { x.FareNew, x.Fare, x.BasicPrice, x.FC, x.Ro, x.Article, x.Comodity }, (key, group) => new
				{
					ro = key.Ro,
					article = key.Article,
					comodity = key.Comodity,
					fc = key.FC,
					fare = key.Fare,
					farenew = key.FareNew,
					basicprice = key.BasicPrice,
					qtycutting = group.Sum(s => s.QtyCuttingOut),
					priceCuttingOut = group.Sum(s => s.PriceCuttingOut),
					qtCuttingSubkon = group.Sum(s => s.QtyCuttingsubkon),
					priceCuttingSubkon = group.Sum(s => s.PriceCuttingsubkon),
					qtyCuttingTransfer = group.Sum(s => s.QtyCuttingTransfer),
					priceCuttingTransfer = group.Sum(s => s.PriceCuttingTransfer),
					qtyCuttingIn = group.Sum(s => s.QtyCuttingIn),
					priceCuttingIn = group.Sum(s => s.PriceCuttingIn),
					begining = group.Sum(s => s.BeginingBalanceCuttingQty),
					beginingcuttingPrice = group.Sum(s => s.BeginingBalanceCuttingPrice),
					qtyavalsew = group.Sum(s => s.AvalSewing),
					priceavalsew = group.Sum(s => s.AvalSewingPrice),
					qtyavalcut = group.Sum(s => s.AvalCutting),
					priceavalcut = group.Sum(s => s.AvalCuttingPrice),
					beginingloading = group.Sum(s => s.BeginingBalanceLoadingQty),
					beginingloadingPrice = group.Sum(s => s.BeginingBalanceLoadingPrice),
					qtyLoadingIn = group.Sum(s => s.QtyLoadingIn),
					priceLoadingIn = group.Sum(s => s.PriceLoadingIn),
					qtyloading = group.Sum(s => s.QtyLoading),
					priceloading = group.Sum(s => s.PriceLoading),
					qtyLoadingAdj = group.Sum(s => s.QtyLoadingAdjs),
					priceLoadingAdj = group.Sum(s => s.PriceLoadingAdjs),
					beginingSewing = group.Sum(s => s.BeginingBalanceSewingQty),
					beginingSewingPrice = group.Sum(s => s.BeginingBalanceSewingPrice),
					sewingIn = group.Sum(s => s.QtySewingIn),
					sewingInPrice = group.Sum(s => s.PriceSewingIn),
					sewingintransfer = group.Sum(s => s.QtySewingInTransfer),
					sewingintransferPrice = group.Sum(s => s.PriceSewingInTransfer),
					sewingout = group.Sum(s => s.QtySewingOut),
					sewingoutPrice = group.Sum(s => s.PriceSewingOut),
					sewingretur = group.Sum(s => s.QtySewingRetur),
					sewingreturPrice = group.Sum(s => s.PriceSewingRetur),
					wipsewing = group.Sum(s => s.WipSewingOut),
					wipsewingPrice = group.Sum(s => s.WipSewingOutPrice),
					wipfinishing = group.Sum(s => s.WipFinishingOut),
					wipfinishingPrice = group.Sum(s => s.WipFinishingOutPrice),
					sewingadj = group.Sum(s => s.QtySewingAdj),
					sewingadjPrice = group.Sum(s => s.PriceSewingAdj),
					finishingin = group.Sum(s => s.FinishingInQty),
					finishinginPrice = group.Sum(s => s.FinishingInPrice),
					finishingintransfer = group.Sum(s => s.FinishingInTransferQty),
					finishingintransferPrice = group.Sum(s => s.FinishingInTransferPrice),
					finishingadj = group.Sum(s => s.FinishingAdjQty),
					finishingadjPrice = group.Sum(s => s.FinishingAdjPrice),
					finishingout = group.Sum(s => s.FinishingOutQty),
					finishingoutPrice = group.Sum(s => s.FinishingOutPrice),
					finishinigretur = group.Sum(s => s.FinishingReturQty),
					finishinigreturPrice = group.Sum(s => s.FinishingReturPrice),
					beginingbalanceFinishing = group.Sum(s => s.BeginingBalanceFinishingQty),
					beginingbalanceFinishingPrice = group.Sum(s => s.BeginingBalanceFinishingPrice),
					beginingbalancesubcon = group.Sum(s => s.BeginingBalanceSubconQty),
					beginingbalancesubconPrice = group.Sum(s => s.BeginingBalanceSubconPrice),
					subconIn = group.Sum(s => s.SubconInQty),
					subconInPrice = group.Sum(s => s.SubconInPrice),
					subconout = group.Sum(s => s.SubconOutQty),
					subconoutPrice = group.Sum(s => s.SubconOutPrice),
					exportQty = group.Sum(s => s.ExportQty),
					exportPrice = group.Sum(s => s.ExportPrice),
                    localQty = group.Sum(s => s.LocalQty),
                    localPrice = group.Sum(s => s.LocalPrice),
                    otherqty = group.Sum(s => s.OtherQty),
					otherprice = group.Sum(s => s.OtherPrice),
					sampleQty = group.Sum(s => s.SampleQty),
					samplePrice = group.Sum(s => s.SamplePrice),
					expendAdj = group.Sum(s => s.ExpenditureGoodAdj),
					expendAdjPrice = group.Sum(s => s.ExpenditureGoodAdjPrice),
					expendRetur = group.Sum(s => s.ExpenditureGoodRetur),
					expendReturPrice = group.Sum(s => s.ExpenditureGoodReturPrice),
					//finishinginqty =group.Sum(s=>s.FinishingInQty)
					beginingBalanceExpenditureGood = group.Sum(s => s.BeginingBalanceExpenditureGood),
					beginingBalanceExpenditureGoodPrice = group.Sum(s => s.BeginingBalanceExpenditureGoodPrice),
					expenditureInTransfer = group.Sum(s => s.ExpenditureGoodInTransfer),
					expenditureInTransferPrice = group.Sum(s => s.ExpenditureGoodInTransferPrice),
					qtyloadingInTransfer = group.Sum(s => s.QtyLoadingInTransfer),
					priceloadingInTransfer = group.Sum(s => s.PriceLoadingInTransfer)
                    
				}).ToList();

			GarmentMonitoringProductionStockFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringProductionStockFlowListViewModel();
			List<GarmentMonitoringProductionStockFlowDto> monitoringDtos = new List<GarmentMonitoringProductionStockFlowDto>();

            var ros = querySum.Select(x => x.ro).Distinct().ToArray();

            var BasicPrices = (from a in sumbasicPrice
                               join b in sumFCs on a.RO equals b.RO into sumFCes
                               from bb in sumFCes.DefaultIfEmpty()
                               join c in queryBalance on a.RO equals c.Ro into queryBalances
                               from cc in queryBalances.DefaultIfEmpty()
                               where ros.Contains(a.RO)
                               select new
                               {
                                   BasicPrice = 
                                   //Math.Round(Convert.ToDouble(a.BasicPrice / a.Count), 2) 
                                   //* 
                                   Math.Round(Convert.ToDouble((bb.FC / bb.Count)) == 0 ? cc.BasicPrice : 
                                   Math.Round(Convert.ToDouble(a.BasicPrice / a.Count), 2) * Convert.ToDouble(bb.FC / bb.Count),2),
                                   realization = a.RO
                               }).ToList();

            var dtos = (from a in querySum
                        join b in BasicPrices on a.ro equals b.realization
                        select new GarmentMonitoringProductionStockFlowDto
                        {
                            Article = a.article,
                            Ro = a.ro,
                            FC = Math.Round(Convert.ToDouble(a.fc), 2),
                            Fare = a.fare,
                            BasicPrice = b.BasicPrice,

                            BeginingBalanceCuttingQty = a.begining < 0 ? 0 : a.begining,
                            BeginingBalanceCuttingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.begining, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.begining, 2),
                            QtyCuttingTransfer = Math.Round(a.qtyCuttingTransfer, 2),
                            PriceCuttingTransfer = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyCuttingTransfer, 2),
                            QtyCuttingsubkon = Math.Round(a.qtCuttingSubkon, 2),
                            PriceCuttingsubkon = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtCuttingSubkon, 2),
                            QtyCuttingIn = Math.Round(a.qtyCuttingIn, 2),
                            PriceCuttingIn = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyCuttingIn, 2),
                            QtyCuttingOut = Math.Round(a.qtycutting, 2),
                            PriceCuttingOut = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtycutting, 2),
                            Comodity = a.comodity,
                            AvalCutting = Math.Round(a.qtyavalcut, 2),
                            AvalCuttingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyavalcut, 2),
                            AvalSewing = Math.Round(a.qtyavalsew, 2),
                            AvalSewingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyavalsew, 2),
                            EndBalancCuttingeQty = Math.Round(a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew, 2) < 0 ? 0 : Math.Round(a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew, 2),
                            EndBalancCuttingePrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * (a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew), 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * (a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew), 2),
                            BeginingBalanceLoadingQty = Math.Round(a.beginingloading, 2) < 0 ? 0 : Math.Round(a.beginingloading, 2),
                            BeginingBalanceLoadingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.beginingloading, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.beginingloading, 2),
                            QtyLoadingIn = Math.Round(a.qtyLoadingIn, 2),
                            PriceLoadingIn = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyLoadingIn, 2),
                            QtyLoadingInTransfer = Math.Round(a.qtyloadingInTransfer, 2),
                            PriceLoadingInTransfer = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyloadingInTransfer, 2),
                            QtyLoading = Math.Round(a.qtyloading, 2),
                            PriceLoading = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyloading, 2),
                            QtyLoadingAdjs = Math.Round(a.qtyLoadingAdj, 2),
                            PriceLoadingAdjs = Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * a.qtyLoadingAdj, 2),
                            EndBalanceLoadingQty = (Math.Round(a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj, 2)) < 0 ? 0 : (Math.Round(a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj, 2)),
                            EndBalanceLoadingPrice = (Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * (a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj), 2)) < 0 ? 0 : (Math.Round(((Convert.ToDouble(a.fare) * 0.25) + b.BasicPrice) * (a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj), 2)),
                            BeginingBalanceSewingQty = Math.Round(a.beginingSewing, 2),
                            BeginingBalanceSewingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.beginingSewing, 2),
                            QtySewingIn = Math.Round(a.sewingIn, 2),
                            PriceSewingIn = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingIn, 2),
                            QtySewingOut = Math.Round(a.sewingout, 2),
                            PriceSewingOut = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingout, 2),
                            QtySewingInTransfer = Math.Round(a.sewingintransfer, 2),
                            PriceSewingInTransfer = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingintransfer, 2),
                            QtySewingRetur = Math.Round(a.sewingretur, 2),
                            PriceSewingRetur = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingretur, 2),
                            WipSewingOut = Math.Round(a.wipsewing, 2),
                            WipSewingOutPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.wipsewing, 2),
                            WipFinishingOut = Math.Round(a.wipfinishing, 2),
                            WipFinishingOutPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.wipfinishing, 2),
                            QtySewingAdj = Math.Round(a.sewingadj, 2),
                            PriceSewingAdj = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * a.sewingadj, 2),
                            EndBalanceSewingQty = Math.Round(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.wipsewing - a.wipfinishing - a.sewingretur - a.sewingadj, 2),
                            EndBalanceSewingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.5) + b.BasicPrice) * Math.Round(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.wipsewing - a.wipfinishing - a.sewingretur - a.sewingadj, 2), 2),
                            BeginingBalanceFinishingQty = Math.Round(a.beginingbalanceFinishing, 2),
                            BeginingBalanceFinishingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.beginingbalanceFinishing, 2),
                            FinishingInExpenditure = Math.Round(a.finishingout + a.subconout, 2),
                            FinishingInExpenditurepPrice = Math.Round((((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingout) + (((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.subconout), 2),
                            FinishingInQty = Math.Round(a.finishingin, 2),
                            FinishingInPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingin, 2),
                            FinishingOutQty = Math.Round(a.finishingout, 2),
                            FinishingOutPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingout, 2),
                            BeginingBalanceSubconQty = Math.Round(a.beginingbalancesubcon, 2),
                            BeginingBalanceSubconPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.beginingbalancesubcon, 2),
                            SubconInQty = Math.Round(a.subconIn, 2),
                            SubconInPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.subconIn, 2),
                            SubconOutQty = Math.Round(a.subconout, 2),
                            SubconOutPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.subconout, 2),
                            EndBalanceSubconQty = Math.Round(a.beginingbalancesubcon + a.subconIn - a.subconout, 2),
                            EndBalanceSubconPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * (a.beginingbalancesubcon + a.subconIn - a.subconout), 2),
                            FinishingInTransferQty = Math.Round(a.finishingintransfer, 2),
                            FinishingInTransferPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingintransfer, 2),
                            FinishingReturQty = Math.Round(a.finishinigretur, 2),
                            FinishingReturPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishinigretur, 2),
                            FinishingAdjQty = Math.Round(a.finishingadj, 2),
                            FinishingAdjPRice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * a.finishingadj, 2),
                            BeginingBalanceExpenditureGood = Math.Round(a.beginingBalanceExpenditureGood, 2),
                            BeginingBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.beginingBalanceExpenditureGood, 2),
                            EndBalanceFinishingQty = Math.Round(a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur, 2),
                            EndBalanceFinishingPrice = Math.Round(((Convert.ToDouble(a.fare) * 0.75) + b.BasicPrice) * (a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur), 2),
                            ExportQty = Math.Round(a.exportQty, 2),
                            ExportPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.exportQty, 2),
                            LocalQty = Math.Round(a.localQty, 2),
                            LocalPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.localQty, 2),
                            SampleQty = Math.Round(a.sampleQty, 2),
                            SamplePrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.sampleQty, 2),
                            OtherQty = Math.Round(a.otherqty, 2),
                            OtherPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.otherqty, 2),
                            ExpenditureGoodAdj = Math.Round(a.expendAdj, 2),
                            ExpenditureGoodAdjPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.expendAdj, 2),
                            ExpenditureGoodRetur = Math.Round(a.expendRetur, 2),
                            ExpenditureGoodReturPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.expendRetur, 2),
                            ExpenditureGoodInTransfer = Math.Round(a.expenditureInTransfer, 2),
                            ExpenditureGoodInTransferPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * a.expenditureInTransfer, 2),
                            EndBalanceExpenditureGood = Math.Round(a.beginingBalanceExpenditureGood + a.finishingout + a.subconout + a.expendRetur + a.expenditureInTransfer - a.exportQty - a.localQty - a.otherqty - a.sampleQty - a.expendAdj, 2),
                            EndBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(a.fare)) + b.BasicPrice) * (a.beginingBalanceExpenditureGood + a.finishingout + a.subconout + a.expendRetur + a.expenditureInTransfer - a.exportQty - a.localQty - a.otherqty - a.sampleQty - a.expendAdj), 2),
                            FareNew = a.farenew,
                            CuttingNew = Math.Round(a.farenew * Convert.ToDecimal(a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew), 2),
                            LoadingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingloading + a.qtyLoadingIn - a.qtyloading - a.qtyLoadingAdj), 2),
                            SewingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.wipsewing - a.wipfinishing - a.sewingretur - a.sewingadj), 2),
                            FinishingNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur), 2),
                            ExpenditureNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingBalanceExpenditureGood + a.finishingout + a.subconout + a.expendRetur + a.expenditureInTransfer - a.exportQty - a.localQty - a.otherqty - a.sampleQty - a.expendAdj), 2),
                            SubconNew = Math.Round(a.farenew * Convert.ToDecimal(a.beginingbalancesubcon + a.subconIn - a.subconout), 2)
                        }).ToList();


            //foreach (var item in querySum.OrderBy(s => s.ro))
            //{

            //    var fc = Math.Round(Convert.ToDouble(item.fc), 2);
            //var basicPrice = Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == item.ro select aa.BasicPrice / aa.Count).FirstOrDefault()), 2)
            //* Convert.ToDouble((from cost in sumFCs where cost.RO == item.ro select cost.FC / cost.Count).FirstOrDefault()) == 0 ?
            //Convert.ToDouble((from a in queryBalance.ToList() where a.Ro == item.ro select a.BasicPrice).FirstOrDefault()) :
            //Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == item.ro select aa.BasicPrice / aa.Count).FirstOrDefault()), 2)
            //* Convert.ToDouble((from cost in sumFCs where cost.RO == item.ro select cost.FC / cost.Count).FirstOrDefault());
            //Math.Round(Convert.ToDouble(item.basicprice) * fc, 2);

            //    GarmentMonitoringProductionStockFlowDto garmentMonitoringDto = new GarmentMonitoringProductionStockFlowDto()
            //    {
            //        Article = item.article,
            //        Ro = item.ro,
            //        FC = fc,
            //        Fare = item.fare,
            //        BasicPrice = basicPrice,

            //        BeginingBalanceCuttingQty = item.begining < 0 ? 0 : item.begining,
            //        BeginingBalanceCuttingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.begining, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.begining, 2),
            //        QtyCuttingTransfer = Math.Round(item.qtyCuttingTransfer, 2),
            //        PriceCuttingTransfer = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyCuttingTransfer, 2),
            //        QtyCuttingsubkon = Math.Round(item.qtCuttingSubkon, 2),
            //        PriceCuttingsubkon = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtCuttingSubkon, 2),
            //        QtyCuttingIn = Math.Round(item.qtyCuttingIn, 2),
            //        PriceCuttingIn = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyCuttingIn, 2),
            //        QtyCuttingOut = Math.Round(item.qtycutting, 2),
            //        PriceCuttingOut = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtycutting, 2),
            //        Comodity = item.comodity,
            //        AvalCutting = Math.Round(item.qtyavalcut, 2),
            //        AvalCuttingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyavalcut, 2),
            //        AvalSewing = Math.Round(item.qtyavalsew, 2),
            //        AvalSewingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyavalsew, 2),
            //        EndBalancCuttingeQty = Math.Round(item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon /*- item.qtyavalcut - item.qtyavalsew*/, 2) < 0 ? 0 : Math.Round(item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon /* - item.qtyavalcut - item.qtyavalsew*/, 2),
            //        EndBalancCuttingePrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * (item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon /*- item.qtyavalcut - item.qtyavalsew*/), 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * (item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon /*- item.qtyavalcut - item.qtyavalsew*/), 2),
            //        BeginingBalanceLoadingQty = Math.Round(item.beginingloading, 2) < 0 ? 0 : Math.Round(item.beginingloading, 2),
            //        BeginingBalanceLoadingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.beginingloading, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.beginingloading, 2),
            //        QtyLoadingIn = Math.Round(item.qtyLoadingIn, 2),
            //        PriceLoadingIn = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyLoadingIn, 2),
            //        QtyLoadingInTransfer = Math.Round(item.qtyloadingInTransfer, 2),
            //        PriceLoadingInTransfer = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyloadingInTransfer, 2),
            //        QtyLoading = Math.Round(item.qtyloading, 2),
            //        PriceLoading = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyloading, 2),
            //        QtyLoadingAdjs = Math.Round(item.qtyLoadingAdj, 2),
            //        PriceLoadingAdjs = Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * item.qtyLoadingAdj, 2),
            //        EndBalanceLoadingQty = (Math.Round(item.beginingloading + item.qtyLoadingIn + item.qtyloadingInTransfer - item.qtyloading - item.qtyLoadingAdj, 2)) < 0 ? 0 : (Math.Round(item.beginingloading + item.qtyLoadingIn + item.qtyloadingInTransfer - item.qtyloading - item.qtyLoadingAdj, 2)),
            //        EndBalanceLoadingPrice = (Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * (item.beginingloading + item.qtyLoadingIn + item.qtyloadingInTransfer - item.qtyloading - item.qtyLoadingAdj), 2)) < 0 ? 0 : (Math.Round(((Convert.ToDouble(item.fare) * 0.25) + basicPrice) * (item.beginingloading + item.qtyLoadingIn + item.qtyloadingInTransfer - item.qtyloading - item.qtyLoadingAdj), 2)),
            //        BeginingBalanceSewingQty = Math.Round(item.beginingSewing, 2),
            //        BeginingBalanceSewingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.beginingSewing, 2),
            //        QtySewingIn = Math.Round(item.sewingIn, 2),
            //        PriceSewingIn = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingIn, 2),
            //        QtySewingOut = Math.Round(item.sewingout, 2),
            //        PriceSewingOut = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingout, 2),
            //        QtySewingInTransfer = Math.Round(item.sewingintransfer, 2),
            //        PriceSewingInTransfer = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingintransfer, 2),
            //        QtySewingRetur = Math.Round(item.sewingretur, 2),
            //        PriceSewingRetur = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingretur, 2),
            //        WipSewingOut = Math.Round(item.wipsewing, 2),
            //        WipSewingOutPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.wipsewing, 2),
            //        WipFinishingOut = Math.Round(item.wipfinishing, 2),
            //        WipFinishingOutPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.wipfinishing, 2),
            //        QtySewingAdj = Math.Round(item.sewingadj, 2),
            //        PriceSewingAdj = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * item.sewingadj, 2),
            //        EndBalanceSewingQty = Math.Round(item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj, 2),
            //        EndBalanceSewingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.5) + basicPrice) * Math.Round(item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj, 2), 2),
            //        BeginingBalanceFinishingQty = Math.Round(item.beginingbalanceFinishing, 2),
            //        BeginingBalanceFinishingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.beginingbalanceFinishing, 2),
            //        FinishingInExpenditure = Math.Round(item.finishingout + item.subconout, 2),
            //        FinishingInExpenditurepPrice = Math.Round((((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingout) + (((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.subconout), 2),
            //        FinishingInQty = Math.Round(item.finishingin, 2),
            //        FinishingInPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingin, 2),
            //        FinishingOutQty = Math.Round(item.finishingout, 2),
            //        FinishingOutPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingout, 2),
            //        BeginingBalanceSubconQty = Math.Round(item.beginingbalancesubcon, 2),
            //        BeginingBalanceSubconPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.beginingbalancesubcon, 2),
            //        SubconInQty = Math.Round(item.subconIn, 2),
            //        SubconInPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.subconIn, 2),
            //        SubconOutQty = Math.Round(item.subconout, 2),
            //        SubconOutPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.subconout, 2),
            //        EndBalanceSubconQty = Math.Round(item.beginingbalancesubcon + item.subconIn - item.subconout, 2),
            //        EndBalanceSubconPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * (item.beginingbalancesubcon + item.subconIn - item.subconout), 2),
            //        FinishingInTransferQty = Math.Round(item.finishingintransfer, 2),
            //        FinishingInTransferPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingintransfer, 2),
            //        FinishingReturQty = Math.Round(item.finishinigretur, 2),
            //        FinishingReturPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishinigretur, 2),
            //        FinishingAdjQty = Math.Round(item.finishingadj, 2),
            //        FinishingAdjPRice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * item.finishingadj, 2),
            //        BeginingBalanceExpenditureGood = Math.Round(item.beginingBalanceExpenditureGood, 2),
            //        BeginingBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.beginingBalanceExpenditureGood, 2),
            //        EndBalanceFinishingQty = Math.Round(item.beginingbalanceFinishing + item.finishingin + item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur, 2),
            //        EndBalanceFinishingPrice = Math.Round(((Convert.ToDouble(item.fare) * 0.75) + basicPrice) * (item.beginingbalanceFinishing + item.finishingin + item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur), 2),
            //        ExportQty = Math.Round(item.exportQty, 2),
            //        ExportPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.exportQty, 2),
            //        SampleQty = Math.Round(item.sampleQty, 2),
            //        SamplePrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.sampleQty, 2),
            //        OtherQty = Math.Round(item.otherqty, 2),
            //        OtherPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.otherqty, 2),
            //        ExpenditureGoodAdj = Math.Round(item.expendAdj, 2),
            //        ExpenditureGoodAdjPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.expendAdj, 2),
            //        ExpenditureGoodRetur = Math.Round(item.expendRetur, 2),
            //        ExpenditureGoodReturPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.expendRetur, 2),
            //        ExpenditureGoodInTransfer = Math.Round(item.expenditureInTransfer, 2),
            //        ExpenditureGoodInTransferPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * item.expenditureInTransfer, 2),
            //        EndBalanceExpenditureGood = Math.Round(item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur + item.expenditureInTransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj, 2),
            //        EndBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(item.fare)) + basicPrice) * (item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur + item.expenditureInTransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj), 2),
            //        FareNew = item.farenew,
            //        CuttingNew = Math.Round(item.farenew * Convert.ToDecimal(item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon - item.qtyavalcut - item.qtyavalsew), 2),
            //        LoadingNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingloading + item.qtyLoadingIn - item.qtyloading - item.qtyLoadingAdj), 2),
            //        SewingNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj), 2),
            //        FinishingNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingbalanceFinishing + item.finishingin + item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur), 2),
            //        ExpenditureNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur + item.expenditureInTransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj), 2),
            //        SubconNew = Math.Round(item.farenew * Convert.ToDecimal(item.beginingbalancesubcon + item.subconIn - item.subconout), 2)
            //    };
            //    monitoringDtos.Add(garmentMonitoringDto);
            //}

            //List<string> ro = new List<string> { 2020890,2020968,2021141,2020891,2020894,1920056,1923018,1950051,1950052 }

            //var data = from a in monitoringDtos
            //           where a.BeginingBalanceCuttingQty > 0 || a.QtyCuttingIn > 0 || a.QtyCuttingOut > 0 || a.QtyCuttingsubkon > 0 || a.QtyCuttingTransfer > 0 || a.EndBalancCuttingeQty > 0 ||
            //            a.BeginingBalanceLoadingQty > 0 || a.QtyLoading > 0 || a.QtyLoadingAdjs > 0 || a.QtyLoadingIn > 0 || a.QtyLoadingInTransfer > 0 || a.EndBalanceLoadingQty > 0 ||
            //            a.BeginingBalanceSewingQty > 0 || a.QtySewingAdj > 0 || a.QtySewingIn > 0 || a.QtySewingInTransfer > 0 || a.QtySewingOut > 0 || a.QtySewingRetur > 0 || a.WipSewingOut > 0 || a.WipFinishingOut > 0 || a.EndBalanceSewingQty > 0 ||
            //            a.BeginingBalanceSubconQty > 0 || a.EndBalanceSubconQty > 0 || a.SubconInQty > 0 || a.SubconOutQty > 0 || a.AvalCutting > 0 || a.AvalSewing > 0 ||
            //            a.BeginingBalanceFinishingQty > 0 || a.FinishingAdjQty > 0 || a.FinishingInExpenditure > 0 || a.FinishingInQty > 0 || a.FinishingInTransferQty > 0 || a.FinishingOutQty > 0 || a.FinishingReturQty > 0 ||
            //            a.BeginingBalanceExpenditureGood > 0 || a.ExpenditureGoodAdj > 0 || a.ExpenditureGoodInTransfer > 0 || a.ExpenditureGoodRemainingQty > 0 || a.ExpenditureGoodRetur > 0 || a.EndBalanceExpenditureGood > 0
            //           select a;

            var data = from a in dtos
                       where a.BeginingBalanceCuttingQty > 0 || a.QtyCuttingIn > 0 || a.QtyCuttingOut > 0 || a.QtyCuttingsubkon > 0 || a.QtyCuttingTransfer > 0 || a.EndBalancCuttingeQty > 0 ||
                        a.BeginingBalanceLoadingQty > 0 || a.QtyLoading > 0 || a.QtyLoadingAdjs > 0 || a.QtyLoadingIn > 0 || a.QtyLoadingInTransfer > 0 || a.EndBalanceLoadingQty > 0 ||
                        a.BeginingBalanceSewingQty > 0 || a.QtySewingAdj > 0 || a.QtySewingIn > 0 || a.QtySewingInTransfer > 0 || a.QtySewingOut > 0 || a.QtySewingRetur > 0 || a.WipSewingOut > 0 || a.WipFinishingOut > 0 || a.EndBalanceSewingQty > 0 ||
                        a.BeginingBalanceSubconQty > 0 || a.EndBalanceSubconQty > 0 || a.SubconInQty > 0 || a.SubconOutQty > 0 || a.AvalCutting > 0 || a.AvalSewing > 0 ||
                        a.BeginingBalanceFinishingQty > 0 || a.FinishingAdjQty > 0 || a.FinishingInExpenditure > 0 || a.FinishingInQty > 0 || a.FinishingInTransferQty > 0 || a.FinishingOutQty > 0 || a.FinishingReturQty > 0 ||
                        a.BeginingBalanceExpenditureGood > 0 || a.ExpenditureGoodAdj > 0 || a.ExpenditureGoodInTransfer > 0 || a.ExpenditureGoodRemainingQty > 0 || a.ExpenditureGoodRetur > 0 || a.EndBalanceExpenditureGood > 0
                       select a;

            //var data2 = data.Count();

            var roList = (from a in data
                          select a.Ro).Distinct().ToList();
            var roBalance = from a in garmentBalanceProductionStockRepository.Query
                            select new CostCalViewModel { comodityName = a.Comodity, buyerCode = a.BuyerCode, hours = a.Hours, qtyOrder = a.QtyOrder, ro = a.Ro };

            CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(roList, request.token);

            foreach (var item in roBalance)
            {
                costCalculation.data.Add(item);
            }

            var costcalgroup = costCalculation.data.GroupBy(x => new { x.ro, }, (key, group) => new CostCalViewModel
            {
                buyerCode = group.FirstOrDefault().buyerCode,
                comodityName = group.FirstOrDefault().comodityName,
                hours = group.FirstOrDefault().hours,
                qtyOrder = group.FirstOrDefault().qtyOrder,
                ro = key.ro
            }).ToList();

            //var costcal2 = costCalculation.data.Distinct().Count();


            var dataend = (from item in data
                           join b in costcalgroup on item.Ro equals b.ro
                           select new GarmentMonitoringProductionStockFlowDto
                           {
                               Article = item.Article,
                               Ro = item.Ro,
                               FC = item.FC,
                               Fare = item.Fare,
                               BasicPrice = item.BasicPrice,
                               BuyerCode = item.BuyerCode == null ? b.buyerCode : item.BuyerCode,
                               Comodity = item.Comodity == null ? b.comodityName : item.Comodity,
                               QtyOrder = b.qtyOrder,
                               Hours = item.Hours == 0 ? b.hours : item.Hours,
                               BeginingBalanceCuttingQty = item.BeginingBalanceCuttingQty,
                               BeginingBalanceCuttingPrice = item.BeginingBalanceCuttingPrice,
                               QtyCuttingTransfer = item.QtyCuttingTransfer,
                               PriceCuttingTransfer = item.PriceCuttingTransfer,
                               QtyCuttingsubkon = item.QtyCuttingsubkon,
                               PriceCuttingsubkon = item.PriceCuttingsubkon,
                               QtyCuttingIn = item.QtyCuttingIn,
                               PriceCuttingIn = item.PriceCuttingIn,
                               QtyCuttingOut = item.QtyCuttingOut,
                               PriceCuttingOut = item.PriceCuttingOut,
                               AvalCutting = item.AvalCutting,
                               AvalCuttingPrice = item.AvalCuttingPrice,
                               AvalSewing = item.AvalSewing,
                               AvalSewingPrice = item.AvalSewingPrice,
                               EndBalancCuttingeQty = item.EndBalancCuttingeQty,
                               EndBalancCuttingePrice = item.EndBalancCuttingePrice,
                               BeginingBalanceLoadingQty = item.BeginingBalanceLoadingQty,
                               BeginingBalanceLoadingPrice = item.BeginingBalanceLoadingPrice,
                               QtyLoadingIn = item.QtyLoadingIn,
                               PriceLoadingIn = item.PriceLoadingIn,
                               QtyLoadingInTransfer = item.QtyLoadingInTransfer,
                               PriceLoadingInTransfer = item.PriceLoadingInTransfer,
                               QtyLoading = item.QtyLoading,
                               PriceLoading = item.PriceLoading,
                               QtyLoadingAdjs = item.QtyLoadingAdjs,
                               PriceLoadingAdjs = item.PriceLoadingAdjs,
                               EndBalanceLoadingQty = item.EndBalanceLoadingQty,
                               EndBalanceLoadingPrice = item.EndBalanceLoadingPrice,
                               BeginingBalanceSewingQty = item.BeginingBalanceSewingQty,
                               BeginingBalanceSewingPrice = item.BeginingBalanceSewingPrice,
                               QtySewingIn = item.QtySewingIn,
                               PriceSewingIn = item.PriceSewingIn,
                               QtySewingOut = item.QtySewingOut,
                               PriceSewingOut = item.PriceSewingOut,
                               QtySewingInTransfer = item.QtySewingInTransfer,
                               PriceSewingInTransfer = item.PriceSewingInTransfer,
                               QtySewingRetur = item.QtySewingRetur,
                               PriceSewingRetur = item.PriceSewingRetur,
                               WipSewingOut = item.WipSewingOut,
                               WipSewingOutPrice = item.WipSewingOutPrice,
                               WipFinishingOut = item.WipFinishingOut,
                               WipFinishingOutPrice = item.WipFinishingOutPrice,
                               QtySewingAdj = item.QtySewingAdj,
                               PriceSewingAdj = item.PriceSewingAdj,
                               EndBalanceSewingQty = item.EndBalanceSewingQty,
                               EndBalanceSewingPrice = item.EndBalanceSewingPrice,
                               BeginingBalanceFinishingQty = item.BeginingBalanceFinishingQty,
                               BeginingBalanceFinishingPrice = item.BeginingBalanceFinishingPrice,
                               FinishingInExpenditure = item.FinishingInExpenditure,
                               FinishingInExpenditurepPrice = item.FinishingInExpenditurepPrice,
                               FinishingInQty = item.FinishingInQty,
                               FinishingInPrice = item.FinishingInPrice,
                               FinishingOutQty = item.FinishingOutQty,
                               FinishingOutPrice = item.FinishingOutPrice,
                               BeginingBalanceSubconQty = item.BeginingBalanceSubconQty,
                               BeginingBalanceSubconPrice = item.BeginingBalanceSubconPrice,
                               SubconInQty = item.SubconInQty,
                               SubconInPrice = item.SubconInPrice,
                               SubconOutQty = item.SubconOutQty,
                               SubconOutPrice = item.SubconOutPrice,
                               EndBalanceSubconQty = item.EndBalanceSubconQty,
                               EndBalanceSubconPrice = item.EndBalanceSubconPrice,
                               FinishingInTransferQty = item.FinishingInTransferQty,
                               FinishingInTransferPrice = item.FinishingInTransferPrice,
                               FinishingReturQty = item.FinishingReturQty,
                               FinishingReturPrice = item.FinishingReturPrice,
                               FinishingAdjQty = item.FinishingAdjQty,
                               FinishingAdjPRice = item.FinishingAdjPRice,
                               BeginingBalanceExpenditureGood = item.BeginingBalanceExpenditureGood,
                               BeginingBalanceExpenditureGoodPrice = item.BeginingBalanceExpenditureGoodPrice,
                               EndBalanceFinishingQty = item.EndBalanceFinishingQty,
                               EndBalanceFinishingPrice = item.EndBalanceFinishingPrice,
                               ExportQty = item.ExportQty,
                               ExportPrice = item.ExportPrice,
                               LocalQty = item.LocalQty,
                               LocalPrice = item.LocalPrice,
                               SampleQty = item.SampleQty,
                               SamplePrice = item.SamplePrice,
                               OtherQty = item.OtherQty,
                               OtherPrice = item.OtherPrice,
                               ExpenditureGoodAdj = item.ExpenditureGoodAdj,
                               ExpenditureGoodAdjPrice = item.ExpenditureGoodAdjPrice,
                               ExpenditureGoodRetur = item.ExpenditureGoodRetur,
                               ExpenditureGoodReturPrice = item.ExpenditureGoodReturPrice,
                               ExpenditureGoodInTransfer = item.ExpenditureGoodInTransfer,
                               ExpenditureGoodInTransferPrice = item.ExpenditureGoodInTransferPrice,
                               EndBalanceExpenditureGood = item.EndBalanceExpenditureGood,
                               EndBalanceExpenditureGoodPrice = item.EndBalanceExpenditureGoodPrice,
                               FareNew = item.FareNew,
                               CuttingNew = item.CuttingNew,
                               LoadingNew = item.LoadingNew,
                               SewingNew = item.SewingNew,
                               FinishingNew = item.FinishingNew,
                               ExpenditureNew = item.ExpenditureNew,
                               SubconNew = item.SubconNew,
                               ExpenditureGoodRemainingPrice = item.ExpenditureGoodRemainingPrice,
                               ExpenditureGoodRemainingQty = item.ExpenditureGoodRemainingQty,
                               FinishingTransferExpenditure = item.FinishingTransferExpenditure,
                               FinishingTransferExpenditurePrice = item.FinishingTransferExpenditurePrice,
                               MaterialUsage = item.MaterialUsage,
                               PriceUsage = item.PriceUsage
                           }).ToList();


            //foreach (var garment in data)
            //{
            //    garment.BuyerCode = garment.BuyerCode == null ? (from cost in costCalculation.data where cost.ro == garment.Ro select cost.buyerCode).FirstOrDefault() : garment.BuyerCode;
            //    garment.Comodity = garment.Comodity == null ? (from cost in costCalculation.data where cost.ro == garment.Ro select cost.comodityName).FirstOrDefault() : garment.Comodity;
            //    garment.QtyOrder = (from cost in costCalculation.data where cost.ro == garment.Ro select cost.qtyOrder).FirstOrDefault();
            //    garment.Hours = garment.Hours == 0 ? (from cost in costCalculation.data where cost.ro == garment.Ro select cost.hours).FirstOrDefault() : garment.Hours;

            //    garment.BasicPrice = Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == garment.Ro select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDouble((from cost in sumFCs where cost.RO == garment.Ro select cost.FC / cost.Count).FirstOrDefault()) == 0 ? Convert.ToDouble((from a in queryBalance.ToList() where a.Ro == garment.Ro select a.BasicPrice).FirstOrDefault()) : Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == garment.Ro select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDouble((from cost in sumFCs where cost.RO == garment.Ro select cost.FC / cost.Count).FirstOrDefault());
            //}
            garmentMonitoringProductionFlow.garmentMonitorings = dataend.OrderBy(x=>x.Ro).ToList();
			garmentMonitoringProductionFlow.count = dataend.Count();
             
           
            return garmentMonitoringProductionFlow;
		}
	}
}
