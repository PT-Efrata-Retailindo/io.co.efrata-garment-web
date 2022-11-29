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
using System.Linq;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System.Net.Http;
using System.Text;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleAvalComponents.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleFinishingIns.Repositories;
using Manufactures.Domain.GarmentSample.SampleExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.GarmentMonitoringSampleStockFlows.Queries
{
	public class GetXlsMonitoringSampleStockFlowQueryHandler : IQueryHandler<GetXlsMonitoringSampleStockFlowQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentSampleCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentSampleCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentSampleCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentSampleCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentSampleCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentSampleCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentSampleSewingInRepository garmentSewingInRepository;
		private readonly IGarmentSampleSewingInItemRepository garmentSewingInItemRepository;
		private readonly IGarmentSampleAvalComponentRepository garmentAvalComponentRepository;
		private readonly IGarmentSampleAvalComponentItemRepository garmentAvalComponentItemRepository;
		private readonly IGarmentSampleSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSampleSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentSampleFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentSampleFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentSampleFinishingInRepository garmentFinishingInRepository;
		private readonly IGarmentSampleFinishingInItemRepository garmentFinishingInItemRepository;
		private readonly IGarmentSampleExpenditureGoodRepository garmentExpenditureGoodRepository;
		private readonly IGarmentSampleExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
		private readonly IGarmentComodityPriceRepository garmentComodityPriceRepository;
		private readonly IGarmentSamplePreparingRepository garmentPreparingRepository;
        private readonly IGarmentSamplePreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentSampleRequestRepository garmentSampleRequestRepository;
		private readonly IGarmentSampleRequestProductRepository garmentSampleRequestProductRepository;

		public GetXlsMonitoringSampleStockFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
            garmentCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSampleCuttingOutDetailRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
			garmentSewingInRepository = storage.GetRepository<IGarmentSampleSewingInRepository>();
			garmentSewingInItemRepository = storage.GetRepository<IGarmentSampleSewingInItemRepository>();
			garmentAvalComponentRepository = storage.GetRepository<IGarmentSampleAvalComponentRepository>();
			garmentAvalComponentItemRepository = storage.GetRepository<IGarmentSampleAvalComponentItemRepository>();
			garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
			garmentFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
			garmentFinishingInRepository = storage.GetRepository<IGarmentSampleFinishingInRepository>();
			garmentFinishingInItemRepository = storage.GetRepository<IGarmentSampleFinishingInItemRepository>();
			garmentExpenditureGoodRepository = storage.GetRepository<IGarmentSampleExpenditureGoodRepository>();
			garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentSampleExpenditureGoodItemRepository>();
			garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentSamplePreparingItemRepository>();
			garmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
			garmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
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
			public double QtyLoading { get; internal set; }
			public double PriceLoading { get; internal set; }
			public double QtyLoadingInTransfer { get; internal set; }
			public double PriceLoadingInTransfer { get; internal set; }
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
			public double OtherQty { get; internal set; }
			public double OtherPrice { get; internal set; }
			public double SampleQty { get; internal set; }
			public double SamplePrice { get; internal set; }
			public double MDQty { get; internal set; }
			public double MDPrice { get; internal set; }

            public double LocalQty { get; internal set; }
            public double LocalPrice { get; internal set; }
            public double NCSQty { get; internal set; }
            public double NCSPrice { get; internal set; }

            public double ExpenditureGoodRemainingQty { get; internal set; }
			public double ExpenditureGoodRemainingPrice { get; internal set; }
			public double ExpenditureGoodAdj { get; internal set; }
			public double ExpenditureGoodAdjPrice { get; internal set; }
			public double EndBalanceExpenditureGood { get; internal set; }
			public double EndBalanceExpenditureGoodPrice { get; internal set; }
			public double ExpenditureGoodInTransfer { get; internal set; }
			public double ExpenditureGoodInTransferPrice { get; internal set; }

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
		public async Task<MemoryStream> Handle(GetXlsMonitoringSampleStockFlowQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
			dateFrom.AddHours(7);
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
			dateTo = dateTo.AddHours(7); DateTimeOffset dateFareNew = dateTo.AddDays(1);


            var sumbasicPrice = (from a in (from prep in garmentPreparingRepository.Query
                                            where (request.ro == null || (request.ro != null && request.ro != "" && prep.RONo == request.ro))
                                            select new { prep.RONo,prep.Identity})
                                            join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentSamplePreparingId
                                 
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

            var queryGroup = (from a in (from aa in garmentCuttingOutRepository.Query
                                         where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro))
                                         select new { aa.RONo, aa.ComodityId, aa.ComodityName, aa.Article })
                              select new { BasicPrice = (from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault(),
                                  FareNew = (from aa in garmentComodityPriceRepository.Query where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && a.ComodityId == aa.ComodityId && aa.Date == dateFareNew select aa.Price).FirstOrDefault(),
                                  Fare = (from aa in garmentComodityPriceRepository.Query where aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && a.ComodityId == aa.ComodityId && aa.IsValid == true select aa.Price).FirstOrDefault(),
                                  Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName,
                                  FC = (from cost in sumFCs where cost.RO == a.RONo select cost.FC / cost.Count).FirstOrDefault() }).Distinct();




			var QueryCuttingOut = (from a in (from aa in garmentCuttingOutRepository.Query
											  where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId == aa.UnitFromId
											  select new { aa.RONo, aa.Identity, aa.CuttingOutDate, aa.CuttingOutType })
								   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
								   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CuttingOutItemId

								   select new
								   {
									   BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
									   BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
									   Ro = a.RONo,
									   QtyCuttingOut = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
									   PriceCuttingOut = a.CuttingOutDate >= dateFrom ? c.Price : 0,
								   }).GroupBy(x => x.Ro, (key, group) => new monitoringView
								   {
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
									   BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
									   BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
									   Ro = key,
									   ExpenditureGoodRetur = 0,
									   ExpenditureGoodReturPrice = 0,
									   QtyCuttingOut = group.Sum(x => x.QtyCuttingOut),
									   PriceCuttingOut = group.Sum(x => x.PriceCuttingOut),
									   ExportQty = 0,
									   ExportPrice = 0,
									   SampleQty = 0,
									   SamplePrice = 0,
									   MDQty = 0,
									   MDPrice = 0,
                                       LocalQty = 0,
                                       LocalPrice = 0,
                                       NCSQty = 0,
                                       NCSPrice = 0,
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
													where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == (request.unit == 0 ? aa.UnitFromId : request.unit) && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SUBKON"
													select new { aa.RONo, aa.Identity, aa.CuttingOutDate, aa.CuttingOutType })
										 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
										 join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CuttingOutItemId
										 select new
										 {

											 BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
											 Ro = a.RONo,
											 BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
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
											 BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
											 Ro = key,
											 BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
											 FC = 0,
											 QtyCuttingsubkon = group.Sum(x => x.QtyCuttingsubkon),
											 PriceCuttingsubkon = group.Sum(x => x.PriceCuttingsubkon),
											 ExpenditureGoodRetur = 0,
											 ExpenditureGoodReturPrice = 0,
											 ExportQty = 0,
											 ExportPrice = 0,
											 SampleQty = 0,
											 SamplePrice = 0,
											 MDQty = 0,
											 MDPrice = 0,
                                             LocalQty = 0,
                                             LocalPrice = 0,
                                             NCSQty = 0,
                                             NCSPrice = 0,
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
													  where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitFromId == (request.unit == 0 ? aa.UnitFromId : request.unit) && aa.CuttingOutDate <= dateTo && aa.CuttingOutType == "SEWING" && aa.UnitId != aa.UnitFromId
													  select new { aa.RONo, aa.Identity, aa.CuttingOutType, aa.CuttingOutDate })
										   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
										   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CuttingOutItemId
										   select new
										   {
											   BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
											   BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
											   Ro = a.RONo,
											   QtyCuttingTransfer = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
											   PriceCuttingTransfer = a.CuttingOutDate >= dateFrom ? c.Price : 0,
										   }).GroupBy(x => x.Ro, (key, group) => new monitoringView
										   {
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
											   BeginingBalanceCuttingQty = group.Sum(x => x.BeginingBalanceCuttingQty),
											   BeginingBalanceCuttingPrice = group.Sum(x => x.BeginingBalanceCuttingPrice),
											   Ro = key,
											   QtyCuttingTransfer = group.Sum(x => x.QtyCuttingTransfer),
											   PriceCuttingTransfer = group.Sum(x => x.PriceCuttingTransfer),
											   ExpenditureGoodRetur = 0,
											   ExpenditureGoodReturPrice = 0,
											   ExportQty = 0,
											   ExportPrice = 0,
											   SampleQty = 0,
											   SamplePrice = 0,
											   MDQty = 0,
											   MDPrice = 0,
                                               LocalQty = 0,
                                               LocalPrice = 0,
                                               NCSQty = 0,
                                               NCSPrice = 0,
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
											 where aa.CuttingType != "Non Main Fabric" && (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.CuttingInDate <= dateTo
											 select new { aa.RONo, aa.Identity, aa.CuttingInDate })
								  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
								  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
								  select new
								  {

									  BeginingBalanceCuttingQty = a.CuttingInDate < dateFrom ? c.CuttingInQuantity : 0,
									  BeginingBalanceCuttingPrice = a.CuttingInDate < dateFrom ? c.Price : 0,
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
									  SampleQty = 0,
									  SamplePrice = 0,
									  MDQty = 0,
									  MDPrice = 0,
                                      LocalQty = 0,
                                      LocalPrice = 0,
                                      NCSQty = 0,
                                      NCSPrice = 0,
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
												  where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.Date <= dateTo && aa.SampleAvalComponentType == "SEWING"
												  select new { aa.RONo, aa.Identity, aa.Date, aa.SampleAvalComponentType })
									   join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.SampleAvalComponentId
									   select new
									   {
										   Ro = a.RONo,
										   AvalSewing = a.Date >= dateFrom ? b.Quantity : 0,
										   AvalSewingPrice = a.Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
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
										   BeginingBalanceCuttingQty = 0,//a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
										   BeginingBalanceCuttingPrice = 0,//a.Date < dateFrom && a.Date > dateBalance  ? -Convert.ToDouble(b.Price) : 0,
										   Ro = key,
										   QtyCuttingIn = 0,
										   PriceCuttingIn = 0,
										   AvalSewing = group.Sum(x => x.AvalSewing),
										   AvalSewingPrice = group.Sum(x => x.AvalSewingPrice),
										   ExpenditureGoodRetur = 0,
										   ExpenditureGoodReturPrice = 0,
										   ExportQty = 0,
										   ExportPrice = 0,
										   SampleQty = 0,
										   SamplePrice = 0,
										   MDQty = 0,
										   MDPrice = 0,
                                           LocalQty = 0,
                                           LocalPrice = 0,
                                           NCSQty = 0,
                                           NCSPrice = 0,
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
												   where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.Date <= dateTo && aa.SampleAvalComponentType == "CUTTING"
												   select new { aa.RONo, aa.Identity, aa.Date, aa.SampleAvalComponentType })
										join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.SampleAvalComponentId
										select new
										{
											Ro = a.RONo,
											AvalCutting = a.Date >= dateFrom ? b.Quantity : 0,
											AvalCuttingPrice = a.Date >= dateFrom ? Convert.ToDouble(b.Price) : 0
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
											BeginingBalanceCuttingQty = 0,//a.Date < dateFrom && a.Date > dateBalance ? -b.Quantity : 0,
											BeginingBalanceCuttingPrice = 0,// a.Date < dateFrom && a.Date > dateBalance ? -Convert.ToDouble(b.Price) : 0,
											Ro = key,
											AvalCutting = group.Sum(x => x.AvalCutting),
											AvalCuttingPrice = group.Sum(x => x.AvalCuttingPrice),
											ExpenditureGoodRetur = 0,
											ExpenditureGoodReturPrice = 0,
											ExportQty = 0,
											ExportPrice = 0,
											SampleQty = 0,
											SamplePrice = 0,
											MDQty = 0,
											MDPrice = 0,
                                            LocalQty = 0,
                                            LocalPrice = 0,
                                            NCSQty = 0,
                                            NCSPrice = 0,
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

			var QuerySewingIn = (from a in (from aa in garmentSewingInRepository.Query
											where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.SewingInDate <= dateTo
											select new { aa.RONo, aa.Identity, aa.SewingInDate, aa.SewingFrom })
								 join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
								 select new
								 {
									 BeginingBalanceSewingQty = (a.SewingInDate < dateFrom && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Quantity : 0,
									 BeginingBalanceSewingPrice = (a.SewingInDate < dateFrom && a.SewingFrom != "SEWING" /*&& a.SewingFrom == "FINISHING"*/) ? b.Price : 0,
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
									 SampleQty = 0,
									 SamplePrice = 0,
									 MDQty = 0,
									 MDPrice = 0,
                                     LocalQty = 0,
                                     LocalPrice = 0,
                                     NCSQty = 0,
                                     NCSPrice = 0,
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
											 where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.SewingOutDate <= dateTo
											 select new { aa.RONo, aa.Identity, aa.SewingOutDate, aa.SewingTo, aa.UnitToId, aa.UnitId })
								  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SampleSewingOutId

								  select new
								  {

									  FinishingTransferExpenditure = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0,
									  FinishingTransferExpenditurePrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0,
									  FinishingInTransferQty = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
									  FinishingInTransferPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
									  BeginingBalanceFinishingQty = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0,
									  BeginingBalanceFinishingPrice = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0,
									  BeginingBalanceSewingQty = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Quantity : 0 - ((a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Quantity : 0) + ((a.SewingOutDate < dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Quantity : 0),
									  BeginingBalanceSewingPrice = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Price : 0 - ((a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? -b.Price : 0) + ((a.SewingOutDate < dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == (request.unit == 0 ? a.UnitToId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == (request.unit == 0 ? a.UnitId : request.unit)) ? b.Price : 0),

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
									  FinishingTransferExpenditure = group.Sum(x => x.FinishingTransferExpenditure),
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
									  SampleQty = 0,
									  SamplePrice = 0,
									  MDQty = 0,
									  MDPrice = 0,
                                      LocalQty = 0,
                                      LocalPrice = 0,
                                      NCSQty = 0,
                                      NCSPrice = 0,
                                      OtherQty = 0,
									  OtherPrice = 0,
									  ExpenditureGoodInTransfer = 0,
									  ExpenditureGoodInTransferPrice = 0,
									  BeginingBalanceCuttingQty = 0,
									  BeginingBalanceCuttingPrice = 0,
									  BeginingBalanceLoadingQty = 0,
									  BeginingBalanceLoadingPrice = 0
								  });



			var QueryFinishingIn = (from a in (from aa in garmentFinishingInRepository.Query
											   where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingInDate <= dateTo
											   select new { aa.RONo, aa.Identity, aa.FinishingInDate, aa.FinishingInType })
									join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
									select new
									{

										BeginingBalanceSubconQty = (a.FinishingInDate < dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
										BeginingBalanceSubconPrice = (a.FinishingInDate < dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
										BeginingBalanceFinishingQty = (a.FinishingInDate < dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
										BeginingBalanceFinishingPrice = (a.FinishingInDate < dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
										FinishingInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
										FinishingInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
										SubconInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
										SubconInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
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
										BeginingBalanceSubconQty = group.Sum(x => x.BeginingBalanceSubconQty),
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
										SampleQty = 0,
										SamplePrice = 0,
										MDQty = 0,
										MDPrice = 0,
                                        LocalQty = 0,
                                        LocalPrice = 0,
                                        NCSQty = 0,
                                        NCSPrice = 0,
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
												where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "GUDANG JADI"
												select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
									 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									 join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
									 join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
									 select new
									 {

										 BeginingBalanceFinishingQty = (a.FinishingOutDate < dateFrom && d.FinishingInType != "PEMBELIAN") ? -b.Quantity : 0,
										 BeginingBalanceFinishingPrice = (a.FinishingOutDate < dateFrom && d.FinishingInType != "PEMBELIAN") ? -b.Price : 0,
										 BeginingBalanceExpenditureGood = ((a.FinishingOutDate < dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0) + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0),
										 BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate < dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Price : 0 + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0),
										 BeginingBalanceSubconQty = (a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? -b.Quantity : 0,
										 BeginingBalanceSubconPrice = (a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? -b.Price : 0,

										 FinishingOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
										 FinishingOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Price : 0,
										 SubconOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
										 SubconOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0,
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
										 FinishingReturQty = 0,
										 FinishingReturPrice = 0,
										 QtyLoadingInTransfer = 0,
										 PriceLoadingInTransfer = 0,
										 BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
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
										 SampleQty = 0,
										 SamplePrice = 0,
										 MDQty = 0,
										 MDPrice = 0,
                                         LocalQty = 0,
                                         LocalPrice = 0,
                                         NCSQty = 0,
                                         NCSPrice = 0,
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
															 where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId != aa.UnitToId && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "GUDANG JADI" && aa.UnitToId == (request.unit == 0 ? aa.UnitToId : request.unit)
															 select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo })
												  join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
												  join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
												  join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
												  select new
												  {

													  Ro = a.RONo,
													  ExpenditureGoodInTransfer = (a.FinishingOutDate >= dateFrom) ? b.Quantity : 0,
													  ExpenditureGoodInTransferPrice = (a.FinishingOutDate >= dateFrom) ? b.Price : 0,
													  BeginingBalanceExpenditureGood = (a.FinishingOutDate < dateFrom) ? b.Quantity : 0,
													  BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate < dateFrom) ? b.Price : 0,

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
													  SampleQty = 0,
													  SamplePrice = 0,
													  MDQty = 0,
													  MDPrice = 0,
                                                      LocalQty = 0,
                                                      LocalPrice = 0,
                                                      NCSQty = 0,
                                                      NCSPrice = 0,
                                                      OtherQty = 0,
													  OtherPrice = 0,
													  QtyLoadingInTransfer = 0,
													  PriceLoadingInTransfer = 0,
													  ExpenditureGoodInTransfer = group.Sum(x => x.ExpenditureGoodInTransfer),
													  ExpenditureGoodInTransferPrice = group.Sum(x => x.ExpenditureGoodInTransferPrice),
													  BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
													  BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
													  BeginingBalanceCuttingQty = 0,
													  BeginingBalanceCuttingPrice = 0,
													  BeginingBalanceLoadingQty = 0,
													  BeginingBalanceLoadingPrice = 0
												  });


			var QueryFinishingRetur = (from a in (from aa in garmentFinishingOutRepository.Query
												  where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.FinishingOutDate <= dateTo && aa.FinishingTo == "SEWING"
												  select new { aa.RONo, aa.Identity, aa.FinishingOutDate, aa.FinishingTo, aa.UnitId, aa.UnitToId })
									   join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									   join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
									   join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
									   select new
									   {

										   BeginingBalanceFinishingQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate < dateFrom && a.UnitId == a.UnitToId) ? -b.Quantity : 0,
										   BeginingBalanceFinishingPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate < dateFrom && a.UnitId == a.UnitToId) ? -b.Price : 0,
										   FinishingReturQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitToId == a.UnitToId) ? b.Quantity : 0,
										   FinishingReturPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitToId == a.UnitToId) ? b.Price : 0,
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
										   SubconOutQty = 0,
										   QtyLoadingInTransfer = 0,
										   PriceLoadingInTransfer = 0,
										   SubconOutPrice = 0,
										   BeginingBalanceFinishingQty = group.Sum(x => x.BeginingBalanceFinishingQty),
										   BeginingBalanceFinishingPrice = group.Sum(x => x.BeginingBalanceFinishingPrice),
										   FinishingReturQty = group.Sum(x => x.FinishingReturQty),
										   FinishingReturPrice = group.Sum(x => x.FinishingReturPrice),
										   Ro = key,
										   ExpenditureGoodRetur = 0,
										   ExpenditureGoodReturPrice = 0,
										   ExportQty = 0,
										   ExportPrice = 0,
										   SampleQty = 0,
										   SamplePrice = 0,
										   MDQty = 0,
										   MDPrice = 0,
                                           LocalQty = 0,
                                           LocalPrice = 0,
                                           NCSQty = 0,
                                           NCSPrice = 0,
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
													where (request.ro == null || (request.ro != null && request.ro != "" && aa.RONo == request.ro)) && aa.UnitId == (request.unit == 0 ? aa.UnitId : request.unit) && aa.ExpenditureDate <= dateTo
													select new { aa.RONo, aa.Identity, aa.ExpenditureDate, aa.ExpenditureType })
										 join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
										 select new
										 {

											 BeginingBalanceExpenditureGood = a.ExpenditureDate < dateFrom ? -b.Quantity : 0,
											 BeginingBalanceExpenditureGoodPrice = a.ExpenditureDate < dateFrom ? -b.Price : 0,
											 ExportQty = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Quantity : 0,
											 ExportPrice = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Price : 0,
											 SampleQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "ARSIP SAMPLE")) ? b.Quantity : 0,
											 SamplePrice = (a.ExpenditureDate >= dateFrom & (a.ExpenditureType == "ARSIP SAMPLE")) ? b.Price : 0,
											 MDQuantity = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "ARSIP MD")) ? b.Quantity : 0,
											 MDPrice = (a.ExpenditureDate >= dateFrom & (a.ExpenditureType == "ARSIP MD")) ? b.Price : 0,

                                             LocalQty = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "PENGIRIMAN LOKAL") ? b.Quantity : 0,
                                             LocalPrice = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "PENGIRIMAN LOKAL") ? b.Price : 0,
                                             NCSQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "EXPORT (NON COMMERCIAL SAMPLE)")) ? b.Quantity : 0,
                                             NCSPrice = (a.ExpenditureDate >= dateFrom & (a.ExpenditureType == "EXPORT (NON COMMERCIAL SAMPLE)")) ? b.Price : 0,

                                             OtherQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Quantity : 0,
											 OtherPrice = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Price : 0,
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
											 BeginingBalanceExpenditureGood = group.Sum(x => x.BeginingBalanceExpenditureGood),
											 BeginingBalanceExpenditureGoodPrice = group.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
											 ExportQty = group.Sum(x => x.ExportQty),
											 ExportPrice = group.Sum(x => x.ExportPrice),
											 SampleQty = group.Sum(x => x.SampleQty),
											 SamplePrice = group.Sum(x => x.SamplePrice),
											 MDQty = group.Sum(x => x.MDQuantity),
											 MDPrice = group.Sum(x => x.MDPrice),

                                             LocalQty = group.Sum(x => x.LocalQty),
                                             LocalPrice = group.Sum(x => x.LocalPrice),
                                             NCSQty = group.Sum(x => x.NCSQty),
                                             NCSPrice = group.Sum(x => x.NCSPrice),

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

			var queryNow = QueryCuttingIn

				.Union(QueryCuttingOut)
				.Union(QueryCuttingOutSubkon)
				.Union(QueryCuttingOutTransfer)
				.Union(QueryAvalCompCutting)
				.Union(QueryAvalCompSewing)
				.Union(QuerySewingIn)
				.Union(QuerySewingOut)
				.Union(QueryFinishingIn)
				.Union(QueryFinishingOut)
				.Union(QueryFinishingRetur)
				.Union(QueryExpenditureGoods)
				.Union(QueryExpenditureGoodInTransfer)
				.AsEnumerable();

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
								a.OtherQty,
								a.OtherPrice,
								a.SampleQty,
								a.SamplePrice,
								a.MDQty,
								a.MDPrice,
                                a.LocalQty,
                                a.LocalPrice,
                                a.NCSQty,
                                a.NCSPrice,
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
					otherqty = group.Sum(s => s.OtherQty),
					otherprice = group.Sum(s => s.OtherPrice),
					sampleQty = group.Sum(s => s.SampleQty),
					samplePrice = group.Sum(s => s.SamplePrice),
					mdQty = group.Sum(s => s.MDQty),
					mdPrice = group.Sum(s => s.MDPrice),

                    lclQty = group.Sum(s => s.LocalQty),
                    lclPrice = group.Sum(s => s.LocalPrice),
                    ncsQty = group.Sum(s => s.NCSQty),
                    ncsPrice = group.Sum(s => s.NCSPrice),

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

			GarmentMonitoringSampleStockFlowListViewModel garmentMonitoringSampleFlow = new GarmentMonitoringSampleStockFlowListViewModel();
			List<GarmentMonitoringSampleStockFlowDto> monitoringDtos = new List<GarmentMonitoringSampleStockFlowDto>();

			var ros = querySum.Select(x => x.ro).Distinct().ToArray();

			var dtosPrice = (from a in querySum

							 select new GarmentMonitoringSampleStockFlowDto
							 {
								 Article = a.article,
								 Ro = a.ro,
								 FC = Math.Round(Convert.ToDouble(a.fc), 2),
								 Fare = a.fare,
								 BasicPrice = Math.Round(Convert.ToDouble((from aa in sumbasicPrice where aa.RO == a.ro select aa.BasicPrice / aa.Count).FirstOrDefault()), 2) * Convert.ToDouble((from cost in sumFCs where cost.RO == a.ro select cost.FC / cost.Count).FirstOrDefault()),
								 BeginingBalanceCuttingQty = a.begining < 0 ? 0 : a.begining,
								 QtyCuttingTransfer = Math.Round(a.qtyCuttingTransfer, 2),
								 QtyCuttingsubkon = Math.Round(a.qtCuttingSubkon, 2),
								 QtyCuttingIn = Math.Round(a.qtyCuttingIn, 2),
								 QtyCuttingOut = Math.Round(a.qtycutting, 2),
								 Comodity = a.comodity,
								 AvalCutting = Math.Round(a.qtyavalcut, 2),
								 AvalSewing = Math.Round(a.qtyavalsew, 2),
								 EndBalancCuttingeQty = Math.Round(a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew, 2) < 0 ? 0 : Math.Round(a.begining + a.qtyCuttingIn - a.qtycutting - a.qtyCuttingTransfer - a.qtCuttingSubkon - a.qtyavalcut - a.qtyavalsew, 2),
								 BeginingBalanceLoadingQty = Math.Round(a.beginingloading, 2) < 0 ? 0 : Math.Round(a.beginingloading, 2),
								 QtyLoadingIn = Math.Round(a.qtyLoadingIn, 2),
								 QtyLoadingInTransfer = Math.Round(a.qtyloadingInTransfer, 2),
								 QtyLoading = Math.Round(a.qtyloading, 2),
								 QtyLoadingAdjs = Math.Round(a.qtyLoadingAdj, 2),
								 EndBalanceLoadingQty = (Math.Round(a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj, 2)) < 0 ? 0 : (Math.Round(a.beginingloading + a.qtyLoadingIn + a.qtyloadingInTransfer - a.qtyloading - a.qtyLoadingAdj, 2)),
								 BeginingBalanceSewingQty = Math.Round(a.beginingSewing, 2),
								 QtySewingIn = Math.Round(a.sewingIn, 2),
								 QtySewingOut = Math.Round(a.sewingout, 2),
								 QtySewingInTransfer = Math.Round(a.sewingintransfer, 2),
								 QtySewingRetur = Math.Round(a.sewingretur, 2),
								 WipSewingOut = Math.Round(a.wipsewing, 2),
								 WipFinishingOut = Math.Round(a.wipfinishing, 2),
								 QtySewingAdj = Math.Round(a.sewingadj, 2),
								 EndBalanceSewingQty = Math.Round(a.beginingSewing + a.sewingIn - a.sewingout + a.sewingintransfer - a.wipsewing - a.wipfinishing - a.sewingretur - a.sewingadj, 2),
								 BeginingBalanceFinishingQty = Math.Round(a.beginingbalanceFinishing, 2),
								 FinishingInExpenditure = Math.Round(a.finishingout + a.subconout, 2),
								 FinishingInQty = Math.Round(a.finishingin, 2),
								 FinishingOutQty = Math.Round(a.finishingout, 2),
								 BeginingBalanceSubconQty = Math.Round(a.beginingbalancesubcon, 2),
								 SubconInQty = Math.Round(a.subconIn, 2),
								 SubconOutQty = Math.Round(a.subconout, 2),
								 EndBalanceSubconQty = Math.Round(a.beginingbalancesubcon + a.subconIn - a.subconout, 2),
								 FinishingInTransferQty = Math.Round(a.finishingintransfer, 2),
								 FinishingReturQty = Math.Round(a.finishinigretur, 2),
								 FinishingAdjQty = Math.Round(a.finishingadj, 2),
								 BeginingBalanceExpenditureGood = Math.Round(a.beginingBalanceExpenditureGood, 2),
								 EndBalanceFinishingQty = Math.Round(a.beginingbalanceFinishing + a.finishingin + a.finishingintransfer - a.finishingout - a.finishingadj - a.finishinigretur, 2),
								 ExportQty = Math.Round(a.exportQty, 2),
								 SampleQty = Math.Round(a.sampleQty, 2),
								 MDQty = Math.Round(a.mdQty, 2),
                                 LocalQty = Math.Round(a.lclQty, 2),
                                 NCSQty = Math.Round(a.ncsQty, 2),
                                 OtherQty = Math.Round(a.otherqty, 2),
								 ExpenditureGoodAdj = Math.Round(a.expendAdj, 2),
								 ExpenditureGoodRetur = Math.Round(a.expendRetur, 2),
								 ExpenditureGoodInTransfer = Math.Round(a.expenditureInTransfer, 2),
								 EndBalanceExpenditureGood = Math.Round(a.beginingBalanceExpenditureGood + a.finishingout + a.subconout + a.expendRetur + a.expenditureInTransfer - a.exportQty - a.otherqty - a.sampleQty - a.mdQty - a.lclQty - a.ncsQty, 2),

							 }).ToList();


			var dtos = (from a in dtosPrice
						select new GarmentMonitoringSampleStockFlowDto
						{
							Article = a.Article,
							Ro = a.Ro,
							FC = Math.Round(Convert.ToDouble(a.FC), 2),
							Fare = a.Fare,
							BasicPrice = Convert.ToDouble(a.BasicPrice),

							BeginingBalanceCuttingQty = a.BeginingBalanceCuttingQty < 0 ? 0 : a.BeginingBalanceCuttingQty,
							BeginingBalanceCuttingPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.BeginingBalanceCuttingQty, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.BeginingBalanceCuttingQty, 2),
							QtyCuttingTransfer = Math.Round(a.QtyCuttingTransfer, 2),
							PriceCuttingTransfer = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.QtyCuttingTransfer, 2),
							QtyCuttingsubkon = Math.Round(a.QtyCuttingsubkon, 2),
							PriceCuttingsubkon = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.QtyCuttingsubkon, 2),
							QtyCuttingIn = Math.Round(a.QtyCuttingIn, 2),
							PriceCuttingIn = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.QtyCuttingIn, 2),
							QtyCuttingOut = Math.Round(a.QtyCuttingOut, 2),
							PriceCuttingOut = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.QtyCuttingOut, 2),
							Comodity = a.Comodity,
							AvalCutting = Math.Round(a.AvalCutting, 2),
							AvalCuttingPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.AvalCutting, 2),
							AvalSewing = Math.Round(a.AvalSewing, 2),
							AvalSewingPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.AvalSewing, 2),
							EndBalancCuttingeQty = Math.Round(a.BeginingBalanceCuttingQty + a.QtyCuttingIn - a.QtyCuttingIn - a.QtyCuttingTransfer - a.QtyCuttingsubkon - a.AvalCutting - a.AvalSewing, 2) < 0 ? 0 : Math.Round(a.BeginingBalanceCuttingQty + a.QtyCuttingIn - a.QtyCuttingIn - a.QtyCuttingTransfer - a.QtyCuttingsubkon - a.AvalCutting - a.AvalSewing, 2),
							EndBalancCuttingePrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * (a.BeginingBalanceCuttingQty + a.QtyCuttingIn - a.QtyCuttingIn - a.QtyCuttingTransfer - a.QtyCuttingsubkon - a.AvalCutting - a.AvalSewing), 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * (a.BeginingBalanceCuttingQty + a.QtyCuttingIn - a.QtyCuttingIn - a.QtyCuttingTransfer - a.QtyCuttingsubkon - a.AvalCutting - a.AvalSewing), 2),
							BeginingBalanceLoadingQty = Math.Round(a.BeginingBalanceLoadingQty, 2) < 0 ? 0 : Math.Round(a.BeginingBalanceLoadingQty, 2),
							BeginingBalanceLoadingPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.BeginingBalanceLoadingQty, 2) < 0 ? 0 : Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.BeginingBalanceLoadingQty, 2),
							QtyLoadingIn = Math.Round(a.QtyLoadingIn, 2),
							PriceLoadingIn = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.QtyLoadingIn, 2),
							QtyLoadingInTransfer = Math.Round(a.QtyLoadingInTransfer, 2),
							PriceLoadingInTransfer = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.QtyLoadingInTransfer, 2),
							QtyLoading = Math.Round(a.QtyLoading, 2),
							PriceLoading = Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * a.QtyLoading, 2),
							EndBalanceLoadingQty = (Math.Round(a.BeginingBalanceLoadingQty + a.QtyLoadingIn + a.QtyLoadingInTransfer - a.QtyLoading, 2)) < 0 ? 0 : (Math.Round(a.BeginingBalanceLoadingQty + a.QtyLoadingIn + a.QtyLoadingInTransfer - a.QtyLoading, 2)),
							EndBalanceLoadingPrice = (Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * (a.BeginingBalanceLoadingQty + a.QtyLoadingIn + a.QtyLoadingInTransfer - a.QtyLoading), 2)) < 0 ? 0 : (Math.Round(((Convert.ToDouble(a.Fare) * 0.25) + Convert.ToDouble(a.BasicPrice)) * (a.BeginingBalanceLoadingQty + a.QtyLoadingIn + a.QtyLoadingInTransfer - a.QtyLoading), 2)),
							BeginingBalanceSewingQty = Math.Round(a.BeginingBalanceSewingQty, 2),
							BeginingBalanceSewingPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * a.BeginingBalanceSewingQty, 2),
							QtySewingIn = Math.Round(a.QtySewingIn, 2),
							PriceSewingIn = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * a.QtySewingIn, 2),
							QtySewingOut = Math.Round(a.QtySewingOut, 2),
							PriceSewingOut = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * a.QtySewingOut, 2),
							QtySewingInTransfer = Math.Round(a.QtySewingInTransfer, 2),
							PriceSewingInTransfer = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * a.QtySewingInTransfer, 2),
							QtySewingRetur = Math.Round(a.QtySewingRetur, 2),
							PriceSewingRetur = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * a.QtySewingRetur, 2),
							WipSewingOut = Math.Round(a.WipSewingOut, 2),
							WipSewingOutPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * a.WipSewingOut, 2),
							WipFinishingOut = Math.Round(a.WipFinishingOut, 2),
							WipFinishingOutPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * a.WipFinishingOut, 2),
							QtySewingAdj = Math.Round(a.QtySewingAdj, 2),
							PriceSewingAdj = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * a.QtySewingAdj, 2),
							EndBalanceSewingQty = Math.Round(a.BeginingBalanceSewingQty + a.QtySewingIn - a.QtySewingOut + a.QtySewingInTransfer - a.WipSewingOut - a.WipFinishingOut - a.QtySewingRetur - a.QtySewingAdj, 2),
							EndBalanceSewingPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.5) + Convert.ToDouble(a.BasicPrice)) * Math.Round(a.BeginingBalanceSewingQty + a.QtySewingIn - a.QtySewingOut + a.QtySewingInTransfer - a.WipSewingOut - a.WipFinishingOut - a.QtySewingRetur - a.QtySewingAdj, 2), 2),
							BeginingBalanceFinishingQty = Math.Round(a.BeginingBalanceFinishingQty, 2),
							BeginingBalanceFinishingPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.BeginingBalanceFinishingQty, 2),
							FinishingInExpenditure = Math.Round(a.FinishingOutQty + a.SubconOutQty, 2),
							FinishingInExpenditurepPrice = Math.Round((((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.FinishingOutQty) + (((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.SubconOutQty), 2),
							FinishingInQty = Math.Round(a.FinishingInQty, 2),
							FinishingInPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.FinishingInQty, 2),
							FinishingOutQty = Math.Round(a.FinishingOutQty, 2),
							FinishingOutPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.FinishingOutQty, 2),
							BeginingBalanceSubconQty = Math.Round(a.BeginingBalanceSubconQty, 2),
							BeginingBalanceSubconPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.BeginingBalanceSubconQty, 2),
							SubconInQty = Math.Round(a.SubconInQty, 2),
							SubconInPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.SubconInQty, 2),
							SubconOutQty = Math.Round(a.SubconOutQty, 2),
							SubconOutPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.SubconOutQty, 2),
							EndBalanceSubconQty = Math.Round(a.BeginingBalanceSubconQty + a.SubconInQty - a.SubconOutQty, 2),
							EndBalanceSubconPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * (a.BeginingBalanceSubconQty + a.SubconInQty - a.SubconOutQty), 2),
							FinishingInTransferQty = Math.Round(a.FinishingInTransferQty, 2),
							FinishingInTransferPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.FinishingInTransferQty, 2),
							FinishingReturQty = Math.Round(a.FinishingReturQty, 2),
							FinishingReturPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.FinishingReturQty, 2),
							FinishingAdjQty = Math.Round(a.FinishingAdjQty, 2),
							FinishingAdjPRice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * a.FinishingAdjQty, 2),
							BeginingBalanceExpenditureGood = Math.Round(a.BeginingBalanceExpenditureGood, 2),
							BeginingBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.BeginingBalanceExpenditureGood, 2),
							EndBalanceFinishingQty = Math.Round(a.BeginingBalanceFinishingQty + a.FinishingInQty + a.FinishingInTransferQty - a.FinishingOutQty - a.FinishingAdjQty - a.FinishingReturQty, 2),
							EndBalanceFinishingPrice = Math.Round(((Convert.ToDouble(a.Fare) * 0.75) + Convert.ToDouble(a.BasicPrice)) * (a.BeginingBalanceFinishingQty + a.FinishingInQty + a.FinishingInTransferQty - a.FinishingOutQty - a.FinishingAdjQty - a.FinishingReturQty), 2),
							ExportQty = Math.Round(a.ExportQty, 2),
							ExportPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.ExportQty, 2),
							SampleQty = Math.Round(a.SampleQty, 2),
							SamplePrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.SampleQty, 2),
							MDQty = Math.Round(a.MDQty, 2),
							MDPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.MDQty, 2),

                            LocalQty = Math.Round(a.LocalQty, 2),
                            LocalPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.LocalQty, 2),

                            NCSQty = Math.Round(a.NCSQty, 2),
                            NCSPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.NCSQty, 2),

                            OtherQty = Math.Round(a.OtherQty, 2),
							OtherPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.OtherQty, 2),
							ExpenditureGoodAdj = Math.Round(a.ExpenditureGoodAdj, 2),
							ExpenditureGoodAdjPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.ExpenditureGoodAdj, 2),
							ExpenditureGoodRetur = Math.Round(a.ExpenditureGoodRetur, 2),
							ExpenditureGoodReturPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.ExpenditureGoodRetur, 2),
							ExpenditureGoodInTransfer = Math.Round(a.ExpenditureGoodInTransfer, 2),
							ExpenditureGoodInTransferPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * a.ExpenditureGoodInTransfer, 2),
							EndBalanceExpenditureGood = Math.Round(a.BeginingBalanceExpenditureGood + a.FinishingOutQty + a.SubconOutQty + a.ExpenditureGoodRetur + a.ExpenditureGoodInTransfer - a.ExportQty - a.OtherQty - a.SampleQty - a.MDQty - a.LocalQty - a.NCSQty, 2),
							EndBalanceExpenditureGoodPrice = Math.Round(((Convert.ToDouble(a.Fare)) + Convert.ToDouble(a.BasicPrice)) * (a.BeginingBalanceExpenditureGood + a.FinishingOutQty + a.SubconOutQty + a.ExpenditureGoodRetur + a.ExpenditureGoodInTransfer - a.ExportQty - a.OtherQty - a.SampleQty - a.MDQty - a.LocalQty - a.NCSQty), 2),
							FareNew = a.FareNew,
							CuttingNew = Math.Round(a.FareNew * Convert.ToDecimal(a.BeginingBalanceCuttingQty + a.QtyCuttingIn - a.QtyCuttingOut - a.QtyCuttingTransfer - a.QtyCuttingsubkon - a.AvalCutting - a.AvalSewing), 2),
							LoadingNew = Math.Round(a.FareNew * Convert.ToDecimal(a.BeginingBalanceLoadingQty + a.QtyLoadingIn - a.QtyLoading - a.QtyLoadingAdjs), 2),
							SewingNew = Math.Round(a.FareNew * Convert.ToDecimal(a.BeginingBalanceSewingQty + a.QtySewingIn - a.QtySewingOut + a.QtySewingInTransfer - a.WipSewingOut - a.WipFinishingOut - a.QtySewingRetur - a.QtySewingAdj), 2),
							FinishingNew = Math.Round(a.FareNew * Convert.ToDecimal(a.BeginingBalanceFinishingQty + a.FinishingInQty + a.FinishingInTransferQty - a.FinishingOutQty - a.FinishingAdjQty - a.FinishingReturQty), 2),
							ExpenditureNew = Math.Round(a.FareNew * Convert.ToDecimal(a.BeginingBalanceExpenditureGood + a.FinishingOutQty + a.SubconOutQty + a.ExpenditureGoodRetur + a.ExpenditureGoodInTransfer - a.ExportQty - a.OtherQty - a.SampleQty - a.ExpenditureGoodAdj), 2),
							SubconNew = Math.Round(a.FareNew * Convert.ToDecimal(a.BeginingBalanceSubconQty + a.SubconInQty - a.SubconOutQty), 2)
						}).ToList();


			var data = from a in dtos
					   where a.BeginingBalanceCuttingQty > 0 || a.QtyCuttingIn > 0 || a.QtyCuttingOut > 0 || a.QtyCuttingsubkon > 0 || a.QtyCuttingTransfer > 0 || a.EndBalancCuttingeQty > 0 ||
						//a.BeginingBalanceLoadingQty > 0 || a.QtyLoading > 0 || a.QtyLoadingAdjs > 0 || a.QtyLoadingIn > 0 || a.QtyLoadingInTransfer > 0 || a.EndBalanceLoadingQty > 0 ||
						a.BeginingBalanceSewingQty > 0 || a.QtySewingAdj > 0 || a.QtySewingIn > 0 || a.QtySewingInTransfer > 0 || a.QtySewingOut > 0 || a.QtySewingRetur > 0 || a.WipSewingOut > 0 || a.WipFinishingOut > 0 || a.EndBalanceSewingQty > 0 
						//a.BeginingBalanceSubconQty > 0 || a.EndBalanceSubconQty > 0 || a.SubconInQty > 0 || a.SubconOutQty > 0 
						|| a.AvalCutting > 0 || a.AvalSewing > 0 ||
						a.BeginingBalanceFinishingQty > 0 || a.FinishingAdjQty > 0 || a.FinishingInExpenditure > 0 || a.FinishingInQty > 0 || a.FinishingInTransferQty > 0 || a.FinishingOutQty > 0 || a.FinishingReturQty > 0 ||
						a.BeginingBalanceExpenditureGood > 0 || a.ExpenditureGoodAdj > 0 || a.ExpenditureGoodInTransfer > 0 || a.ExpenditureGoodRemainingQty > 0 || a.ExpenditureGoodRetur > 0 || a.EndBalanceExpenditureGood > 0
					   select a;

			//var data2 = data.Count();

			var roList = (from a in data
						  select a.Ro).Distinct().ToList();


			//var costcalgroup = costCalculation.data.GroupBy(x => new { x.ro, }, (key, group) => new CostCalViewModel
			//{
			//    buyerCode = group.FirstOrDefault().buyerCode,
			//    comodityName = group.FirstOrDefault().comodityName,
			//    hours = group.FirstOrDefault().hours,
			//    qtyOrder = group.FirstOrDefault().qtyOrder,
			//    ro = key.ro
			//}).ToList();

			//var costcal2 = costCalculation.data.Distinct().Count();
			var sampleRequest = (from a in garmentSampleRequestRepository.Query
								 join b in garmentSampleRequestProductRepository.Query
								 on a.Identity equals b.SampleRequestId
								 select new { a.RONoSample, a.BuyerCode, b.Quantity, a.ComodityName })
							   .GroupBy(x => new { x.RONoSample, x.BuyerCode, x.ComodityName }, (key, group) => new
							   {
								   ro = key.RONoSample,
								   qtyOrder = group.Sum(s => s.Quantity),
								   buyer = key.BuyerCode,
								   comodity = key.ComodityName

							   });

			var dataend = (from item in data
							   // join b in costcalgroup on item.Ro equals b.ro
						   select new GarmentMonitoringSampleStockFlowDto
						   {
							   Article = item.Article,
							   Ro = item.Ro,
							   FC = item.FC,
							   Fare = item.Fare,
							   BasicPrice = item.BasicPrice,
							   BuyerCode = item.BuyerCode == null ? (from a in sampleRequest where a.ro == item.Ro select a.buyer).FirstOrDefault() : item.BuyerCode,
							   Comodity = item.Comodity == null ? (from a in sampleRequest where a.ro == item.Ro select a.comodity).FirstOrDefault() : item.Comodity,
							   QtyOrder = (from a in sampleRequest where a.ro == item.Ro select a.qtyOrder).FirstOrDefault(),
							   //Hours = item.Hours == 0 ? b.hours : item.Hours,
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
							   SampleQty = item.SampleQty,
							   SamplePrice = item.SamplePrice,
							   MDQty = item.MDQty,
							   MDPrice = item.MDPrice,

                               LocalQty = item.LocalQty,
                               LocalPrice = item.LocalPrice,
                               NCSQty = item.NCSQty,
                               NCSPrice = item.NCSPrice,

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


			//garmentMonitoringSampleFlow.garmentMonitorings = data.ToList();
			//garmentMonitoringSampleFlow.count = data.Count();
			garmentMonitoringSampleFlow.garmentMonitorings = dataend.ToList();
            garmentMonitoringSampleFlow.count = dataend.Count();
    //        double PriceUsageTotal=0,MaterialUsageTotal=0,BeginingBalanceCuttingQtyTotal =0, BeginingBalanceCuttingPriceTotal=0, QtyCuttingInTotal =0, PriceCuttingInTotal=0, QtyCuttingOutTotal=0, PriceCuttingOutTotal=0, QtyCuttingTransferTotal=0 , PriceCuttingTransferTotal=0, QtyCuttingsubkonTotal= 0 , PriceCuttingsubkonTotal=0,AvalCuttingTotal=0, PriceAvalCuttingTotal =0, AvalSewingTotal =0 , PriceAvalSewingTotal=0, EndBalanceCuttingeQtyTotal =0,
				//EndBalanceCuttingePriceTotal =0, BeginingBalanceLoadingQtyTotal =0, BeginingBalanceLoadingPriceTotal =0,QtyLoadingInTotal=0, QtyLoadingInTransferTotal =0, PriceLoadingInTransferTotal =0, PriceLoadingInTotal=0, QtyLoadingTotal=0, PriceLoadingTotal=0, QtyLoadingAdjsTotal =0 , PriceLoadingAdjsTotal=0,EndBalanceLoadingQtyTotal =0, EndBalanceLoadingPriceTotal =0, QtySewingInTotal=0, PriceSewingInTotal=0, QtySewingOutTotal=0, PriceSewingOutTotal=0,	QtySewingInTransferTotal=0, PriceSewingInTransferTotal=0, WipSewingOutTotal = 0, PriceWipSewingOutTotal =0, WipFinishingOutTotal=0, PriceWipFinishingOutTotal=0,
				//	QtySewingReturTotal =0, PriceSewingReturTotal =0, QtySewingAdjTotal=0, PriceSewingAdjTotal=0, EndBalanceSewingQtyTotal=0, EndBalanceSewingPriceTotal =0, 
				//	BeginingBalanceFinishingQtyTotal=0, BeginingBalanceFinishingPriceTotal=0, FinishingInQtyTotal=0, FinishingInPriceTotal=0, BeginingBalanceSubconQtyTotal=0, 
				//	BeginingBalanceSubconPriceTotal=0, SubconInQtyTotal =0, SubconInPriceTotal =0, SubconOutQtyTotal=0, SubconOutPriceTotal=0,
				//	EndBalanceSubconQtyTotal=0, EndBalanceSubconPriceTotal=0, FinishingOutQtyTotal=0, FinishingOutPriceTotal=0, FinishingInTransferQtyTotal=0, FinishingInTransferPriceTotal=0,
				//	FinishingAdjQtyTotal=0, FinishingAdjPriceTotal=0,  EndBalanceFinishingQtyTotal=0, EndBalanceFinishingPriceTotal =0, BeginingBalanceExpenditureGoodTotal=0, PriceBeginingBalanceExpenditureGoodTotal=0, FinishingInExpenditureTotal =0, 
				//	PriceFinishingInExpenditureTotal=0, ExpenditureGoodInTransferTotal=0, PriceExpenditureGoodInTransferTotal =0, ExpenditureGoodReturTotal=0, PriceExpenditureGoodReturTotal =0,
				//	ExportQtyTotal=0, ExportPriceTotal=0, OtherQtyTotal=0, OtherPriceTotal=0, SampleQtyTotal =0, SamplePriceTotal =0, ExpenditureGoodAdjTotal=0, PriceExpenditureGoodAdjTotal =0, EndBalanceExpenditureGoodTotal=0, PriceEndBalanceExpenditureGoodTotal =0,
				//	CuttingNew =0, LoadingNew =0, SewingNew=0, FinishingNew=0 , ExpenditureNew =0,subconNew = 0, BeginingBalanceSewingQtyTotal =0, BeginingBalanceSewingPriceTotal=0, FinishingReturQtyTotal=0, PriceFinishingReturTotal =0;
			
			 
			//foreach (var item in data)
			//{
   //             MaterialUsageTotal += item.MaterialUsage;
   //             PriceUsageTotal += item.PriceUsage;
			//	QtySewingReturTotal += item.QtySewingRetur;
			//	PriceSewingReturTotal += item.PriceSewingRetur;
			//	QtySewingAdjTotal += item.QtySewingAdj;
			//	PriceSewingAdjTotal += item.PriceSewingAdj;
			//	BeginingBalanceSewingQtyTotal += item.BeginingBalanceSewingQty;
			//	BeginingBalanceSewingPriceTotal += item.BeginingBalanceSewingPrice;
			//	EndBalanceSewingQtyTotal += item.EndBalanceSewingQty;
			//	EndBalanceSewingPriceTotal += item.EndBalanceSewingPrice;
			//	BeginingBalanceFinishingQtyTotal += item.BeginingBalanceFinishingQty;
			//	BeginingBalanceFinishingPriceTotal += item.BeginingBalanceFinishingPrice;
			//	FinishingInQtyTotal += item.FinishingInQty;
			//	FinishingInPriceTotal += item.FinishingInPrice;
			//	BeginingBalanceSubconQtyTotal += item.BeginingBalanceSubconQty;
			//	BeginingBalanceSubconPriceTotal += item.BeginingBalanceSubconPrice;
			//	SubconInQtyTotal += item.SubconInQty;
			//	SubconInPriceTotal += item.SubconInPrice;
			//	SubconOutQtyTotal += item.SubconOutQty;
			//	SubconOutPriceTotal += item.SubconOutPrice;
			//	EndBalanceSubconQtyTotal += item.EndBalanceSubconQty;
			//	EndBalanceSubconPriceTotal += item.EndBalanceSubconPrice;
			//	FinishingOutQtyTotal += item.FinishingOutQty;
			//	FinishingOutPriceTotal += item.FinishingOutPrice;
			//	FinishingInTransferQtyTotal += item.FinishingInTransferQty;
			//	FinishingInTransferPriceTotal += item.FinishingInTransferPrice;
			//	FinishingAdjQtyTotal += item.FinishingAdjQty;
			//	FinishingAdjPriceTotal += item.FinishingAdjPRice;
			//	EndBalanceFinishingQtyTotal += item.EndBalanceFinishingQty;
			//	EndBalanceFinishingPriceTotal += item.EndBalanceFinishingPrice;
			//	BeginingBalanceExpenditureGoodTotal += item.BeginingBalanceExpenditureGood;
			//	PriceBeginingBalanceExpenditureGoodTotal += item.BeginingBalanceExpenditureGoodPrice;
			//	FinishingInExpenditureTotal += item.FinishingInExpenditure;
   //             PriceFinishingInExpenditureTotal += (item.FinishingInExpenditure * Convert.ToDouble(item.Fare) + (item.FinishingInExpenditure * item.BasicPrice));
			//	ExpenditureGoodInTransferTotal += item.ExpenditureGoodInTransfer;
			//	PriceExpenditureGoodInTransferTotal += item.ExpenditureGoodInTransferPrice;
			//	ExpenditureGoodReturTotal += item.ExpenditureGoodRetur;
			//	PriceExpenditureGoodReturTotal += item.ExpenditureGoodReturPrice;
			//	ExportQtyTotal += item.ExportQty;
			//	ExportPriceTotal += item.ExportPrice;
			//	OtherQtyTotal += item.OtherQty;
			//	OtherPriceTotal += item.OtherPrice;
			//	SampleQtyTotal += item.SampleQty;
			//	SamplePriceTotal += item.SamplePrice;
			//	ExpenditureGoodAdjTotal += item.ExpenditureGoodAdj;
			//	PriceExpenditureGoodAdjTotal += item.ExpenditureGoodAdjPrice;
			//	EndBalanceExpenditureGoodTotal += item.EndBalanceExpenditureGood;
			//	PriceEndBalanceExpenditureGoodTotal += item.EndBalanceExpenditureGoodPrice;
			//	CuttingNew += Convert.ToDouble(item.CuttingNew);
			//	LoadingNew += Convert.ToDouble(item.LoadingNew);
			//	SewingNew += Convert.ToDouble(item.SewingNew) ;
			//	FinishingNew += Convert.ToDouble(item.FinishingNew);
			//	ExpenditureNew += Convert.ToDouble(item.ExpenditureNew);
			//	subconNew += Convert.ToDouble(item.SubconNew);
			//	BeginingBalanceCuttingQtyTotal += item.BeginingBalanceCuttingQty;
			//	BeginingBalanceCuttingPriceTotal += item.BeginingBalanceCuttingPrice;
			//	QtyCuttingInTotal += item.QtyCuttingIn;
			//	PriceCuttingInTotal += item.PriceCuttingIn;
			//	QtyCuttingOutTotal += item.QtyCuttingOut;
			//	PriceCuttingOutTotal += item.PriceCuttingOut;
			//	QtyCuttingsubkonTotal += item.QtyCuttingsubkon;
			//	PriceCuttingsubkonTotal += item.PriceCuttingsubkon;
			//	QtyCuttingTransferTotal += item.QtyCuttingTransfer;
			//	PriceCuttingTransferTotal += item.PriceCuttingTransfer;
			//	AvalCuttingTotal += item.AvalCutting;
			//	PriceAvalCuttingTotal += item.AvalCuttingPrice;
			//	AvalSewingTotal += item.AvalSewing;
			//	PriceAvalSewingTotal += item.AvalSewingPrice;
			//	EndBalanceCuttingeQtyTotal += item.EndBalancCuttingeQty;
			//	EndBalanceCuttingePriceTotal += item.EndBalancCuttingePrice;
			//	BeginingBalanceLoadingQtyTotal += item.BeginingBalanceLoadingQty;
			//	BeginingBalanceLoadingPriceTotal += item.BeginingBalanceLoadingPrice;
			//	QtyLoadingInTotal += item.QtyLoadingIn;
			//	PriceLoadingInTotal += item.PriceLoadingIn;
			//	QtyLoadingInTransferTotal += item.QtyLoadingInTransfer;
			//	PriceLoadingInTransferTotal += item.PriceLoadingInTransfer;
			//	QtyLoadingTotal += item.QtyLoading;
			//	PriceLoadingTotal += item.PriceLoading;
			//	QtyLoadingAdjsTotal += item.QtyLoadingAdjs;
			//	PriceLoadingAdjsTotal += item.PriceLoadingAdjs;
			//	EndBalanceLoadingQtyTotal += item.EndBalanceLoadingQty;
			//	EndBalanceLoadingPriceTotal += item.EndBalanceLoadingPrice;
			//	QtySewingInTotal += item.QtySewingIn;
			//	PriceSewingInTotal += item.PriceSewingIn;
			//	QtySewingOutTotal += item.QtySewingOut;
			//	PriceSewingOutTotal += item.PriceSewingOut;
			//	QtySewingInTransferTotal += item.QtySewingInTransfer;
			//	PriceSewingInTransferTotal += item.PriceSewingInTransfer;
			//	WipSewingOutTotal += item.WipSewingOut;
			//	PriceWipSewingOutTotal += item.WipSewingOutPrice;
			//	WipFinishingOutTotal += item.WipFinishingOut;
			//	WipFinishingOutTotal += item.WipFinishingOutPrice;
			//	PriceWipFinishingOutTotal += item.WipFinishingOutPrice;
			//	FinishingReturQtyTotal += item.FinishingReturQty;
			//	PriceFinishingReturTotal += item.FinishingReturPrice;
			

			//}
			monitoringDtos = dataend.ToList();
   //         GarmentMonitoringSampleStockFlowDto total = new GarmentMonitoringSampleStockFlowDto()
   //         {
   //             MaterialUsage = dataend.Sum(MaterialUsage),
   //             PriceUsage = PriceUsageTotal,
			//	BeginingBalanceCuttingQty = BeginingBalanceCuttingQtyTotal,
			//	BeginingBalanceCuttingPrice = BeginingBalanceCuttingPriceTotal,
			//	QtyCuttingTransfer = QtyCuttingTransferTotal,
			//	PriceCuttingTransfer = PriceCuttingTransferTotal,
			//	QtyCuttingsubkon = QtyCuttingsubkonTotal,
			//	PriceCuttingsubkon = PriceCuttingsubkonTotal,
			//	QtyCuttingIn = QtyCuttingInTotal,
			//	PriceCuttingIn = PriceCuttingInTotal,
			//	QtyCuttingOut = QtyCuttingOutTotal,
			//	PriceCuttingOut = PriceCuttingOutTotal,
			//	AvalCutting = AvalCuttingTotal,
			//	AvalCuttingPrice = PriceAvalCuttingTotal,
			//	AvalSewing = AvalSewingTotal,
			//	AvalSewingPrice = PriceAvalSewingTotal,
			//	EndBalancCuttingeQty = EndBalanceCuttingeQtyTotal,
			//	EndBalancCuttingePrice = EndBalanceCuttingePriceTotal,
			//	BeginingBalanceLoadingQty = BeginingBalanceLoadingQtyTotal,
			//	BeginingBalanceLoadingPrice = BeginingBalanceLoadingPriceTotal,
			//	QtyLoadingIn = QtyLoadingInTotal,
			//	PriceLoadingIn = PriceLoadingInTotal,
			//	QtyLoadingInTransfer = QtyLoadingInTransferTotal,
			//	PriceLoadingInTransfer = PriceLoadingInTransferTotal,
			//	QtyLoading = QtyLoadingTotal,
			//	PriceLoading = PriceLoadingTotal,
			//	QtyLoadingAdjs = QtyLoadingAdjsTotal,
			//	PriceLoadingAdjs = PriceLoadingAdjsTotal,
			//	EndBalanceLoadingQty = EndBalanceLoadingQtyTotal,
			//	EndBalanceLoadingPrice = EndBalanceLoadingPriceTotal,
			//	BeginingBalanceSewingQty = BeginingBalanceSewingQtyTotal,
			//	BeginingBalanceSewingPrice = BeginingBalanceSewingPriceTotal,
			//	QtySewingIn = QtySewingInTotal,
			//	PriceSewingIn = PriceSewingInTotal,
			//	QtySewingOut = QtySewingOutTotal,
			//	PriceSewingOut = PriceSewingOutTotal,
			//	QtySewingInTransfer = QtySewingInTransferTotal,
			//	PriceSewingInTransfer = PriceSewingInTransferTotal,
			//	QtySewingRetur = QtySewingReturTotal,
			//	PriceSewingRetur = PriceSewingReturTotal,
			//	WipSewingOut =WipSewingOutTotal ,
			//	WipSewingOutPrice = PriceWipSewingOutTotal,
			//	WipFinishingOut = WipFinishingOutTotal,
			//	WipFinishingOutPrice = PriceWipFinishingOutTotal,
			//	QtySewingAdj = QtySewingAdjTotal,
			//	PriceSewingAdj = PriceSewingAdjTotal,
			//	EndBalanceSewingQty = EndBalanceSewingQtyTotal,
			//	EndBalanceSewingPrice = EndBalanceSewingPriceTotal,
			//	BeginingBalanceFinishingQty = BeginingBalanceFinishingQtyTotal,
			//	BeginingBalanceFinishingPrice = BeginingBalanceFinishingPriceTotal,
			//	FinishingInExpenditure = FinishingInExpenditureTotal,
			//	FinishingInExpenditurepPrice = PriceFinishingInExpenditureTotal,
			//	FinishingInQty = FinishingInQtyTotal,
			//	FinishingInPrice = FinishingInPriceTotal,
			//	FinishingOutQty = FinishingOutQtyTotal,
			//	FinishingOutPrice = FinishingOutPriceTotal,
			//	BeginingBalanceSubconQty = BeginingBalanceSubconQtyTotal,
			//	BeginingBalanceSubconPrice = BeginingBalanceSubconPriceTotal,
			//	SubconInQty = SubconInQtyTotal,
			//	SubconInPrice = SubconInPriceTotal,
			//	SubconOutQty = SubconOutQtyTotal,
			//	SubconOutPrice = SubconOutPriceTotal,
			//	EndBalanceSubconQty = EndBalanceSubconQtyTotal,
			//	EndBalanceSubconPrice = EndBalanceSubconPriceTotal,
			//	FinishingInTransferQty = FinishingInTransferQtyTotal,
			//	FinishingInTransferPrice = FinishingInTransferPriceTotal,
			//	FinishingReturQty = FinishingReturQtyTotal,
			//	FinishingReturPrice = PriceFinishingReturTotal,
			//	FinishingAdjQty = FinishingAdjQtyTotal,
			//	FinishingAdjPRice = FinishingAdjPriceTotal,
			//	BeginingBalanceExpenditureGood = BeginingBalanceExpenditureGoodTotal,
			//	BeginingBalanceExpenditureGoodPrice = PriceBeginingBalanceExpenditureGoodTotal,
			//	EndBalanceFinishingQty = EndBalanceFinishingQtyTotal,
			//	EndBalanceFinishingPrice = EndBalanceFinishingPriceTotal,
			//	ExportQty = ExportQtyTotal,
			//	ExportPrice = ExportPriceTotal,
			//	SampleQty = SampleQtyTotal,
			//	SamplePrice = SamplePriceTotal,
			//	OtherQty = OtherQtyTotal,
			//	OtherPrice = OtherPriceTotal,
			//	ExpenditureGoodAdj = ExpenditureGoodAdjTotal,
			//	ExpenditureGoodAdjPrice = PriceExpenditureGoodAdjTotal,
			//	ExpenditureGoodRetur = ExpenditureGoodReturTotal,
			//	ExpenditureGoodReturPrice = PriceExpenditureGoodReturTotal,
			//	ExpenditureGoodInTransfer = ExpenditureGoodInTransferTotal,
			//	ExpenditureGoodInTransferPrice = PriceExpenditureGoodInTransferTotal,
			//	EndBalanceExpenditureGood = EndBalanceExpenditureGoodTotal,
			//	EndBalanceExpenditureGoodPrice = PriceEndBalanceExpenditureGoodTotal,
				 
			//	CuttingNew = Convert.ToDecimal(CuttingNew),
			//	LoadingNew = Convert.ToDecimal(LoadingNew),
			//	SewingNew = Convert.ToDecimal(SewingNew),
			//	FinishingNew = Convert.ToDecimal(FinishingNew),
			//	ExpenditureNew = Convert.ToDecimal(ExpenditureNew),
			//	SubconNew = Convert.ToDecimal(subconNew)
			//};
            GarmentMonitoringSampleStockFlowDto total = new GarmentMonitoringSampleStockFlowDto()
            {
                MaterialUsage = dataend.Sum(x=>x.MaterialUsage),
                PriceUsage = dataend.Sum(x => x.PriceUsage),
                BeginingBalanceCuttingQty = dataend.Sum(x => x.BeginingBalanceCuttingQty),
                BeginingBalanceCuttingPrice = dataend.Sum(x => x.BeginingBalanceCuttingPrice),
                QtyCuttingTransfer = dataend.Sum(x => x.QtyCuttingTransfer),
                PriceCuttingTransfer = dataend.Sum(x => x.PriceCuttingTransfer),
                QtyCuttingsubkon = dataend.Sum(x => x.QtyCuttingsubkon),
                PriceCuttingsubkon = dataend.Sum(x => x.PriceCuttingsubkon),
                QtyCuttingIn = dataend.Sum(x => x.QtyCuttingIn),
                PriceCuttingIn = dataend.Sum(x => x.PriceCuttingIn),
                QtyCuttingOut = dataend.Sum(x => x.QtyCuttingOut),
                PriceCuttingOut = dataend.Sum(x => x.PriceCuttingOut),
                AvalCutting = dataend.Sum(x => x.AvalCutting),
                AvalCuttingPrice = dataend.Sum(x => x.AvalCuttingPrice),
                AvalSewing = dataend.Sum(x => x.AvalSewing),
                AvalSewingPrice = dataend.Sum(x => x.AvalSewingPrice),
                EndBalancCuttingeQty = dataend.Sum(x => x.EndBalancCuttingeQty),
                EndBalancCuttingePrice = dataend.Sum(x => x.EndBalancCuttingePrice),
                BeginingBalanceLoadingQty = dataend.Sum(x => x.BeginingBalanceLoadingQty),
                BeginingBalanceLoadingPrice = dataend.Sum(x => x.BeginingBalanceLoadingPrice),
                QtyLoadingIn = dataend.Sum(x => x.QtyLoadingIn),
                PriceLoadingIn = dataend.Sum(x => x.PriceLoadingIn),
                QtyLoadingInTransfer = dataend.Sum(x => x.QtyLoadingInTransfer),
                PriceLoadingInTransfer = dataend.Sum(x => x.PriceLoadingInTransfer),
                QtyLoading = dataend.Sum(x => x.QtyLoading),
                PriceLoading = dataend.Sum(x => x.PriceLoading),
                QtyLoadingAdjs = dataend.Sum(x => x.QtyLoadingAdjs),
                PriceLoadingAdjs = dataend.Sum(x => x.PriceLoadingAdjs),
                EndBalanceLoadingQty = dataend.Sum(x => x.EndBalanceLoadingQty),
                EndBalanceLoadingPrice = dataend.Sum(x => x.EndBalanceLoadingPrice),
                BeginingBalanceSewingQty = dataend.Sum(x => x.BeginingBalanceSewingQty),
                BeginingBalanceSewingPrice = dataend.Sum(x => x.BeginingBalanceSewingPrice),
                QtySewingIn = dataend.Sum(x => x.QtySewingIn),
                PriceSewingIn = dataend.Sum(x => x.PriceSewingIn),
                QtySewingOut = dataend.Sum(x => x.QtySewingOut),
                PriceSewingOut = dataend.Sum(x => x.PriceSewingOut),
                QtySewingInTransfer = dataend.Sum(x => x.QtySewingInTransfer),
                PriceSewingInTransfer = dataend.Sum(x => x.PriceSewingInTransfer),
                QtySewingRetur = dataend.Sum(x => x.QtySewingRetur),
                PriceSewingRetur = dataend.Sum(x => x.PriceSewingRetur),
                WipSewingOut = dataend.Sum(x => x.WipSewingOut),
                WipSewingOutPrice = dataend.Sum(x => x.WipSewingOutPrice),
                WipFinishingOut = dataend.Sum(x => x.WipFinishingOut),
                WipFinishingOutPrice = dataend.Sum(x => x.WipFinishingOutPrice),
                QtySewingAdj = dataend.Sum(x => x.QtySewingAdj),
                PriceSewingAdj = dataend.Sum(x => x.PriceSewingAdj),
                EndBalanceSewingQty = dataend.Sum(x => x.EndBalanceSewingQty),
                EndBalanceSewingPrice = dataend.Sum(x => x.EndBalanceSewingPrice),
                BeginingBalanceFinishingQty = dataend.Sum(x => x.BeginingBalanceFinishingQty),
                BeginingBalanceFinishingPrice = dataend.Sum(x => x.BeginingBalanceFinishingPrice),
                FinishingInExpenditure = dataend.Sum(x => x.FinishingInExpenditure),
                FinishingInExpenditurepPrice = dataend.Sum(x => x.FinishingInExpenditurepPrice),
                FinishingInQty = dataend.Sum(x => x.FinishingInQty),
                FinishingInPrice = dataend.Sum(x => x.FinishingInPrice),
                FinishingOutQty = dataend.Sum(x => x.FinishingOutQty),
                FinishingOutPrice = dataend.Sum(x => x.FinishingOutPrice),
                BeginingBalanceSubconQty = dataend.Sum(x => x.BeginingBalanceSubconQty),
                BeginingBalanceSubconPrice = dataend.Sum(x => x.BeginingBalanceSubconPrice),
                SubconInQty = dataend.Sum(x => x.SubconInQty),
                SubconInPrice = dataend.Sum(x => x.SubconInPrice),
                SubconOutQty = dataend.Sum(x => x.SubconOutQty),
                SubconOutPrice = dataend.Sum(x => x.SubconOutPrice),
                EndBalanceSubconQty = dataend.Sum(x => x.EndBalanceSubconQty),
                EndBalanceSubconPrice = dataend.Sum(x => x.EndBalanceSubconPrice),
                FinishingInTransferQty = dataend.Sum(x => x.FinishingInTransferQty),
                FinishingInTransferPrice = dataend.Sum(x => x.FinishingInTransferPrice),
                FinishingReturQty = dataend.Sum(x => x.FinishingReturQty),
                FinishingReturPrice = dataend.Sum(x => x.FinishingReturPrice),
                FinishingAdjQty = dataend.Sum(x => x.FinishingAdjQty),
                FinishingAdjPRice = dataend.Sum(x => x.FinishingAdjPRice),
                BeginingBalanceExpenditureGood = dataend.Sum(x => x.BeginingBalanceExpenditureGood),
                BeginingBalanceExpenditureGoodPrice = dataend.Sum(x => x.BeginingBalanceExpenditureGoodPrice),
                EndBalanceFinishingQty = dataend.Sum(x => x.EndBalanceFinishingQty),
                EndBalanceFinishingPrice = dataend.Sum(x => x.EndBalanceFinishingPrice),
                ExportQty = dataend.Sum(x => x.ExportQty),
                ExportPrice = dataend.Sum(x => x.ExportPrice),
                SampleQty = dataend.Sum(x => x.SampleQty),
                SamplePrice = dataend.Sum(x => x.SamplePrice),
                OtherQty = dataend.Sum(x => x.OtherQty),
                OtherPrice = dataend.Sum(x => x.OtherPrice),
				MDQty = dataend.Sum(x => x.MDQty),
				MDPrice = dataend.Sum(x => x.MDPrice),

                LocalQty = dataend.Sum(x => x.LocalQty),
                LocalPrice = dataend.Sum(x => x.LocalPrice),
                NCSQty = dataend.Sum(x => x.NCSQty),
                NCSPrice = dataend.Sum(x => x.NCSPrice),

                ExpenditureGoodAdj = dataend.Sum(x => x.ExpenditureGoodAdj),
                ExpenditureGoodAdjPrice = dataend.Sum(x => x.ExpenditureGoodAdjPrice),
                ExpenditureGoodRetur = dataend.Sum(x => x.ExpenditureGoodRetur),
                ExpenditureGoodReturPrice = dataend.Sum(x => x.ExpenditureGoodReturPrice),
                ExpenditureGoodInTransfer = dataend.Sum(x => x.ExpenditureGoodInTransfer),
                ExpenditureGoodInTransferPrice = dataend.Sum(x => x.ExpenditureGoodInTransferPrice),
                EndBalanceExpenditureGood = dataend.Sum(x => x.EndBalanceExpenditureGood),
                EndBalanceExpenditureGoodPrice = dataend.Sum(x => x.EndBalanceExpenditureGoodPrice),

                CuttingNew = dataend.Sum(x => x.CuttingNew),
                LoadingNew = dataend.Sum(x => x.LoadingNew),
                SewingNew = dataend.Sum(x => x.SewingNew),
                FinishingNew = dataend.Sum(x => x.FinishingNew),
                ExpenditureNew = dataend.Sum(x => x.ExpenditureNew),
                SubconNew = dataend.Sum(x => x.SubconNew)
            };
            monitoringDtos.Add(total);
		
		garmentMonitoringSampleFlow.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();
			
			if (request.type != "bookkeeping")
			{
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING6", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING7", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING8", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING2", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING3", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING4", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING5", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING5", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING6", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING7", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING8", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING5", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING6", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING7", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING8", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING9", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING10", DataType = typeof(string) });
				//reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI7", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI8", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI9", DataType = typeof(string) });
                //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI8", DataType = typeof(string) });
                //reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI9", DataType = typeof(string) });

                reportDataTable.Rows.Add("", "", "", "",
				"Saldo Awal WIP Cutting", "Cutting In (WIP Cutting)", "Cutting Out / HP",  "Aval Komponen dari Cutting", "Aval Komponen dari Sewing", "Saldo Akhir WIP Cutting",
				
				"Saldo Awal WIP Sewing", "Sewing In (WIP Sewing)", "Sewing Out (WIP Finishing)",  "Retur ke Cutting",  "Saldo Akhir WIP Sewing",
				"Saldo Awal WIP Finishing", "Finishing In (WIP Finishing)", "Finishing Out (WIP BJ)", "Retur ke Sewing", "Saldo Akhir WIP Finishing",
                "Saldo Awal Barang Jadi", "Barang Jadi In/ (WIP BJ)", "Pengiriman Export", "Pengiriman Gudang Sisa", "Pengiriman Arsip Sample", "Pengiriman Arsip MD", "Pengiriman Lokal", "Pengiriman Export NCS", "Saldo Akhir Barang Jadi"
                );
			}
			else
			{

				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TARIF", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HARGA BAHAN BAKU", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI14", DataType = typeof(string) });

                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI15", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI16", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI17", DataType = typeof(string) });
                reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI18", DataType = typeof(string) });

                reportDataTable.Rows.Add("", "", "", "", "", "",  
									"Saldo Awal WIP Cutting","", "Cutting In (WIP Cutting)","", "Cutting Out","",  "Aval Komponen dari Cutting","", "Aval Komponen dari Sewing","", "Saldo Akhir WIP Cutting","",
									"Saldo Awal WIP Sewing","", "Sewing In (WIP Sewing)","", "Sewing Out (WIP Finishing)","", "Retur ke Cutting","", "Saldo Akhir WIP Sewing","",
									"Saldo Awal WIP Finishing","", "Finishing In (WIP Finishing)","", "Finishing Out (WIP BJ)", "", "Retur ke Sewing", "",  "Saldo Akhir WIP Finishing","",
                                    "Saldo Awal Barang Jadi", "", "Barang Jadi In/ (WIP BJ)", "", "Pengiriman Export", "", "Pengiriman Gudang Sisa", "", "Pengiriman Arsip Sample", "", "Pengiriman Arsip Md", "", "Pengiriman Lokal", "", "Pengiriman Export NCS", "", "Saldo Akhir Barang Jadi", ""
                                 );
				reportDataTable.Rows.Add("", "", "", "", "", "", 
									"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
									"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
									"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
                                    "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga"
                                );
				
			}
			int counter = 6;

			foreach (var report in garmentMonitoringSampleFlow.garmentMonitorings)
			{
				if (request.type != "bookkeeping")
				{
					reportDataTable.Rows.Add(report.Ro, report.Article, report.Comodity, report.QtyOrder,
					report.BeginingBalanceCuttingQty, report.QtyCuttingIn, report.QtyCuttingOut, report.AvalCutting, report.AvalSewing, report.EndBalancCuttingeQty,
					report.BeginingBalanceSewingQty, report.QtySewingIn, report.QtySewingOut,  report.QtySewingRetur,  report.EndBalanceSewingQty,
					report.BeginingBalanceFinishingQty, report.FinishingInQty, report.FinishingOutQty,  report.FinishingReturQty, report.EndBalanceFinishingQty,
					report.BeginingBalanceExpenditureGood, report.FinishingInExpenditure,  report.ExportQty, report.OtherQty, report.SampleQty, report.MDQty, report.LocalQty, report.NCSQty, report.EndBalanceExpenditureGood);
					counter++;
				}
				else
				{
					reportDataTable.Rows.Add(report.Ro, report.Article, report.Comodity, report.QtyOrder,report.Fare,report.BasicPrice,
					report.BeginingBalanceCuttingQty,report.BeginingBalanceCuttingPrice, report.QtyCuttingIn,report.PriceCuttingIn ,report.QtyCuttingOut, report.PriceCuttingOut,report.AvalCutting,report.AvalCuttingPrice, report.AvalSewing,report.AvalSewingPrice, report.EndBalancCuttingeQty,report.EndBalancCuttingePrice,
					report.BeginingBalanceSewingQty,report.BeginingBalanceSewingPrice, report.QtySewingIn,report.PriceSewingIn, report.QtySewingOut,report.PriceSewingOut, report.QtySewingRetur,report.PriceSewingRetur, report.EndBalanceSewingQty,report.EndBalanceSewingPrice,
					report.BeginingBalanceFinishingQty,report.BeginingBalanceFinishingPrice, report.FinishingInQty,report.FinishingInPrice, report.FinishingOutQty,report.FinishingOutPrice, report.FinishingReturQty, report.FinishingReturPrice,report.EndBalanceFinishingQty,report.EndBalanceFinishingPrice,
					report.BeginingBalanceExpenditureGood,report.BeginingBalanceExpenditureGoodPrice, report.FinishingInExpenditure,report.FinishingInExpenditurepPrice, report.ExportQty,report.ExportPrice, report.OtherQty,report.OtherPrice, report.SampleQty,report.SamplePrice, report.MDQty,report.MDPrice, report.LocalQty, report.LocalPrice, report.NCSQty, report.NCSPrice, report.EndBalanceExpenditureGood,report.EndBalanceExpenditureGoodPrice);
					 
					counter++;
				}
			}
            var _unitName = (from a in garmentFinishingOutRepository.Query
                             where a.UnitId == request.unit
                             select a.UnitName).FirstOrDefault();
            using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				
				if (request.type != "bookkeeping")
				{
					worksheet.Cells["A1"].Value = "Report Produksi";
					
					worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
					worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
					worksheet.Cells["A" + 1 + ":AC" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 2 + ":AC" + 2 + ""].Merge = true;
					worksheet.Cells["A" + 3 + ":AC" + 3 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":AC" + 3 + ""].Style.Font.Size = 15;
					worksheet.Cells["A" + 1 + ":AC" + 3 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 1 + ":AC" + 6 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 1 + ":AC" + 6 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
					worksheet.Cells["E" + 5 + ":AC" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

					worksheet.Cells["E" + 5 + ":J" + 5 + ""].Merge = true;
					worksheet.Cells["K" + 5 + ":O" + 5 + ""].Merge = true;
					worksheet.Cells["P" + 5 + ":T" + 5 + ""].Merge = true;
					worksheet.Cells["U" + 5 + ":AC" + 5 + ""].Merge = true;
				 
					worksheet.Cells["A" + counter + ":D" + counter + ""].Merge = true;
					worksheet.Cells["A" + 5 + ":AC" + 6 + ""].Style.Font.Bold = true;
					worksheet.Cells["E" + 6 + ":AC" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					worksheet.Cells["A" + 5 + ":AC" + counter + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AC" + counter + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AC" + counter + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":AC" + counter + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + counter + ":AC" + counter + ""].Style.Font.Bold = true;
                    foreach (var cell in worksheet.Cells["D" + 7 + ":AC" + (counter + 1) + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
					worksheet.Cells.AutoFitColumns();
                }
				else
				{
					worksheet.Cells["A1"].Value = "Report Produksi"; 
					worksheet.Cells["A2"].Value = "Periode " + dateFrom.ToString("dd-MM-yyyy") + " s/d " + dateTo.ToString("dd-MM-yyyy");
					worksheet.Cells["A3"].Value = "Konfeksi " + _unitName;
					worksheet.Cells["A" + 1 + ":BD" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 2 + ":BD" + 2 + ""].Merge = true;
					worksheet.Cells["A" + 3 + ":BD" + 3 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":BD" + 3 + ""].Style.Font.Size = 15;
					worksheet.Cells["A" + 1 + ":BD" + 3 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 1 + ":BD" + 3 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 1 + ":BD" + 3 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["A5"].LoadFromDataTable(reportDataTable, true);
					worksheet.Cells["E" + 5 + ":L" + 5 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["G" + 5 + ":R" + 5 + ""].Merge = true;
					worksheet.Cells["S" + 5 + ":AB" + 5 + ""].Merge = true;
					worksheet.Cells["AC" + 5 + ":AL" + 5 + ""].Merge = true;
					worksheet.Cells["AM" + 5 + ":BD" + 5 + ""].Merge = true;
					
					worksheet.Cells["G" + 6 + ":H" + 6 + ""].Merge = true;
					worksheet.Cells["I" + 6 + ":J" + 6 + ""].Merge = true;
					worksheet.Cells["K" + 6 + ":L" + 6 + ""].Merge = true;
					worksheet.Cells["M" + 6 + ":N" + 6 + ""].Merge = true;
					worksheet.Cells["O" + 6 + ":P" + 6 + ""].Merge = true;
					worksheet.Cells["Q" + 6 + ":R" + 6 + ""].Merge = true;
					worksheet.Cells["S" + 6 + ":T" + 6 + ""].Merge = true;
					worksheet.Cells["U" + 6 + ":V" + 6 + ""].Merge = true;
					worksheet.Cells["W" + 6 + ":X" + 6 + ""].Merge = true;
					worksheet.Cells["Y" + 6 + ":Z" + 6 + ""].Merge = true;
					worksheet.Cells["AA" + 6 + ":AB" + 6 + ""].Merge = true;
					worksheet.Cells["AC" + 6 + ":AD" + 6 + ""].Merge = true;
					worksheet.Cells["AE" + 6 + ":AF" + 6 + ""].Merge = true;
					worksheet.Cells["AG" + 6 + ":AH" + 6 + ""].Merge = true;
					worksheet.Cells["AI" + 6 + ":AJ" + 6 + ""].Merge = true;
					worksheet.Cells["AK" + 6 + ":AL" + 6 + ""].Merge = true;
					worksheet.Cells["AM" + 6 + ":AN" + 6 + ""].Merge = true;
					worksheet.Cells["AO" + 6 + ":AP" + 6 + ""].Merge = true;
					worksheet.Cells["AQ" + 6 + ":AR" + 6 + ""].Merge = true;
					worksheet.Cells["AS" + 6 + ":AT" + 6 + ""].Merge = true;
					worksheet.Cells["AU" + 6 + ":AV" + 6 + ""].Merge = true;
					worksheet.Cells["AW" + 6 + ":AX" + 6 + ""].Merge = true;
					worksheet.Cells["AY" + 6 + ":AZ" + 6 + ""].Merge = true;
                    worksheet.Cells["BA" + 6 + ":BB" + 6 + ""].Merge = true;
                    worksheet.Cells["BC" + 6 + ":BD" + 6 + ""].Merge = true;


                    worksheet.Cells["A" + (counter + 1) + ":F" + (counter + 1) + ""].Merge = true;
					worksheet.Cells["A" + 5 + ":BD" + 7 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + (counter + 1) + ":BD" + (counter + 1) + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 5 + ":BD" + 7 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 5 + ":BD" + 6 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["E" + 8 + ":BD" + (counter + 1) + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					
					worksheet.Cells["A" + 5 + ":BD" + (counter + 1) + ""].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":BD" + (counter + 1) + ""].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":BD" + (counter + 1) + ""].Style.Border.Left.Style = ExcelBorderStyle.Thin;
					worksheet.Cells["A" + 5 + ":BD" + (counter + 1) + ""].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    foreach (var cell in worksheet.Cells["E" + 8 + ":BD" + (counter + 1) + ""])
                    {
                        cell.Value = Convert.ToDecimal(cell.Value);
                    }
					worksheet.Cells.AutoFitColumns();


				}
				var stream = new MemoryStream();

				package.SaveAs(stream);

				return stream;
			}
		}
	}
}