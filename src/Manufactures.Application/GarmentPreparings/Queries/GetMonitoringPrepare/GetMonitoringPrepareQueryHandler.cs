using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using Manufactures.Domain.GarmentPreparings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.External.DanLirisClient.Microservice;
using Newtonsoft.Json;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using ExtCore.Data.Abstractions;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.ExpenditureROResult;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalProducts.Repositories;
using Manufactures.Domain.GarmentDeliveryReturns.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;

namespace Manufactures.Application.GarmentPreparings.Queries.GetMonitoringPrepare
{
	public class GetMonitoringPrepareQueryHandler : IQueryHandler<GetMonitoringPrepareQuery, GarmentMonitoringPrepareListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;

		private readonly IGarmentPreparingRepository garmentPreparingRepository;
		private readonly IGarmentPreparingItemRepository garmentPreparingItemRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentAvalProductRepository garmentAvalProductRepository;
		private readonly IGarmentAvalProductItemRepository garmentAvalProductItemRepository;
		private readonly IGarmentDeliveryReturnRepository garmentDeliveryReturnRepository;
		private readonly IGarmentDeliveryReturnItemRepository garmentDeliveryReturnItemRepository;

		public GetMonitoringPrepareQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentPreparingRepository = storage.GetRepository<IGarmentPreparingRepository>();
			garmentPreparingItemRepository = storage.GetRepository<IGarmentPreparingItemRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentAvalProductRepository = storage.GetRepository<IGarmentAvalProductRepository>();
			garmentAvalProductItemRepository = storage.GetRepository<IGarmentAvalProductItemRepository>();
			garmentDeliveryReturnRepository = storage.GetRepository<IGarmentDeliveryReturnRepository>();
			garmentDeliveryReturnItemRepository = storage.GetRepository<IGarmentDeliveryReturnItemRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}

		public async Task<ExpenditureROResult> GetExpenditureById(List<int> id, string token)
		{
			List<ExpenditureROViewModel> expenditureRO = new List<ExpenditureROViewModel>();

			ExpenditureROResult expenditureROResult = new ExpenditureROResult();
			foreach (var item in id)
			{
				var garmentUnitExpenditureNoteUri = PurchasingDataSettings.Endpoint + $"garment-unit-expenditure-notes/ro-asal/{item}";
				var httpResponse = await _http.GetAsync(garmentUnitExpenditureNoteUri, token);

				if (httpResponse.IsSuccessStatusCode)
				{
					var a = await httpResponse.Content.ReadAsStringAsync();
					Dictionary<string, object> keyValues = JsonConvert.DeserializeObject<Dictionary<string, object>>(a);
					var b = keyValues.GetValueOrDefault("data");

					var expenditure = JsonConvert.DeserializeObject<ExpenditureROViewModel>(keyValues.GetValueOrDefault("data").ToString());
					ExpenditureROViewModel expenditureROViewModel = new ExpenditureROViewModel
					{
						ROAsal = expenditure.ROAsal,
						DetailExpenditureId = expenditure.DetailExpenditureId,
						BuyerCode=expenditure.BuyerCode
					};
					expenditureRO.Add(expenditureROViewModel);
				}
				else
				{
					//await GetExpenditureById(id, token);
				}
			}
			expenditureROResult.data = expenditureRO;
			return expenditureROResult;
		}

		class monitoringView
		{
			public string roJob { get; set; }
			public string article { get; set; }
			public string buyerCode { get; set; }
			public string productCode { get; set; }
			public string uomUnit { get; set; }
			public string roAsal { get; set; }
			public string remark { get; set; }
			public double stock { get; set; }
			public double receipt { get; set; }
			public double mainFabricExpenditure { get; set; }
			public double nonMainFabricExpenditure { get; set; }
			public double expenditure { get; set; }
			public double aval { get; set; }
			public double remainQty { get; set; }
			public decimal price { get; set; }
			public Guid prepareItemid { get; set; }
		}
		//OLD VIEW MODEL
		//class ViewBasicPrices
		//{
		//	public string RO { get; internal set; }
		//	public decimal BasicPrice { get; internal set; }
		//	public int Count { get; internal set; }
		//}

		//NEW VIEW MODEL
		class ViewBasicPrices
		{
			public string RO { get; internal set; }
			public decimal Total { get; internal set; }
		}
		public async Task<GarmentMonitoringPrepareListViewModel> Handle(GetMonitoringPrepareQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom);
            dateFrom.AddHours(7);
            DateTimeOffset dateTo = new DateTimeOffset(request.dateTo);
			dateTo = dateTo.AddHours(7);

			var QueryMutationPrepareNow = from a in (from aa in garmentPreparingRepository.Query
													 where aa.UnitId == request.unit && aa.ProcessDate.Value.AddHours(7) <= dateTo
													 select new
													 {
														 aa.Identity,
														 aa.Article,
														 aa.BuyerCode,
														 aa.RONo,
														 aa.ProcessDate
													 })
										  join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
										  select new
										  {
											  Buyer = a.BuyerCode,
											  RO = a.RONo,
											  Articles = a.Article,
											  Id = a.Identity,
											  DetailExpend = b.UENItemId,
											  Processdate = a.ProcessDate
										  };

			//OLD QUERY
			//      var sumbasicPrice = (from a in ( from aa in garmentPreparingRepository.Query
			//                                       where aa.UnitId == request.unit 
			//					 select new { 
			//						 aa.Identity,
			//						 aa.RONo
			//					 })
			//                           join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
			//                           select new { 
			//			 a.RONo, 
			//			 b.BasicPrice 
			//		 })
			//.GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
			//{
			//	RO = key.RONo,
			//	BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
			//	Count = group.Count()
			//});

			//NEW QUERY
			var sumbasicPrice = (from a in (from aa in garmentPreparingRepository.Query
											where aa.UnitId == request.unit
											select new
											{
												aa.Identity,
												aa.RONo
											})
								 join b in garmentPreparingItemRepository.Query on a.Identity equals b.GarmentPreparingId
								 select new
								 {
									 a.RONo,
									 b.BasicPrice
								 })
					   .GroupBy(x => new { x.RONo }, (key, group) => new ViewBasicPrices
					   {
						   RO = key.RONo,
						   Total = Convert.ToDecimal(group.Sum(s => s.BasicPrice) / group.Count())
						   //BasicPrice = Convert.ToDecimal(group.Sum(s => s.BasicPrice)),
						   //Count = group.Count()
					   });


			var QueryMutationPrepareItemsROASAL = (from a in QueryMutationPrepareNow
												   join b in garmentPreparingItemRepository.Query on a.Id equals b.GarmentPreparingId
												   where b.UENItemId == a.DetailExpend
												   select new
												   {
													   article = a.Articles,
													   roJob = a.RO,
													   buyerCode = a.Buyer,
													   price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RO select aa.Total).FirstOrDefault()),
													   prepareitemid = b.Identity,
													   roasal = b.ROSource
												   });

			//	OLD QUERY
			//   var QueryCuttingDONow = from a in (from data in garmentCuttingInRepository.Query 
			//		   where  data.UnitId == request.unit && data.CuttingInDate <= dateTo 
			//		   select new { 
			//			   data.RONo, 
			//			   data.Identity, 
			//			   data.CuttingInDate, 
			//			   data.CuttingType 
			//		   })
			//                           join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
			//                           join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
			//                           select new monitoringView { 
			//	prepareItemid = c.PreparingItemId, 
			//	price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), 
			//	expenditure = 0, 
			//	aval = 0, 
			//	uomUnit = "", 
			//	stock = a.CuttingInDate < dateFrom ? -c.PreparingQuantity : 0, 
			//	nonMainFabricExpenditure = a.CuttingType == "Non Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0, 
			//	mainFabricExpenditure = a.CuttingType == "Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0, 
			//	remark = c.DesignColor, 
			//	receipt = 0, 
			//	productCode = c.ProductCode, 
			//	remainQty = 0 
			//};

			//NEW QUERY
			var QueryCuttingDONow = from a in (from data in garmentCuttingInRepository.Query
											   where data.UnitId == request.unit && data.CuttingInDate.AddHours(7) <= dateTo
											   select new
											   {
												   data.RONo,
												   data.Identity,
												   data.CuttingInDate,
												   data.CuttingType
											   })
									join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
									join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
									join d in garmentPreparingItemRepository.Query on c.PreparingItemId equals d.Identity
									select new monitoringView
									{
										prepareItemid = c.PreparingItemId,
										price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault()),
										expenditure = 0,
										aval = 0,
										uomUnit = "",
										stock = a.CuttingInDate.AddHours(7) < dateFrom ? -c.PreparingQuantity : 0,
										nonMainFabricExpenditure = a.CuttingType == "Non Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0,
										mainFabricExpenditure = a.CuttingType == "Main Fabric" && (a.CuttingInDate >= dateFrom) ? c.PreparingQuantity : 0,
										remark = d.DesignColor,
										receipt = 0,
										productCode = c.ProductCode,
										remainQty = 0
									};

			var QueryMutationPrepareItemNow = (from d in QueryMutationPrepareNow
											   join e in garmentPreparingItemRepository.Query on d.Id equals e.GarmentPreparingId
											   where e.UENItemId == d.DetailExpend
											   select new monitoringView
											   {
												   prepareItemid = e.Identity,
												   price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == d.RO select aa.Total).FirstOrDefault()),
												   uomUnit = "",
												   stock = d.Processdate.Value.AddHours(7) < dateFrom ? e.Quantity : 0,
												   mainFabricExpenditure = 0,
												   nonMainFabricExpenditure = 0,
												   remark = e.DesignColor,
												   receipt = (d.Processdate.Value.AddHours(7) >= dateFrom ? e.Quantity : 0),
												   productCode = e.ProductCode,
												   remainQty = e.RemainingQuantity
											   }).Distinct();

			//OLD QUERY
			//     var QueryAval = from a in (from data in garmentAvalProductRepository.Query where  data.AvalDate <= dateTo select new { data.Identity, data.RONo, data.AvalDate })
			//                                join b in garmentAvalProductItemRepository.Query on a.Identity equals b.APId
			//                     join c in garmentPreparingItemRepository.Query on Guid.Parse(b.PreparingItemId) equals c.Identity
			//                     join d in (from data in garmentPreparingRepository.Query where data.UnitId == request.unit select new { data.Identity, data.RONo }) on c.GarmentPreparingId equals d.Identity
			//                     select new monitoringView { 
			//	prepareItemid = c.Identity, 
			//	price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.BasicPrice / aa.Count).FirstOrDefault()), 
			//	expenditure = 0, 
			//	aval = a.AvalDate >= dateFrom ? b.Quantity : 0, 
			//	uomUnit = "", 
			//	stock = a.AvalDate < dateFrom ? -b.Quantity : 0, 
			//	mainFabricExpenditure = 0, 
			//	nonMainFabricExpenditure = 0, 
			//	remark = b.DesignColor, 
			//	receipt = 0, 
			//	productCode = b.ProductCode, 
			//	remainQty = 0 
			//};

			//NEW QUERY
			var QueryAval = from a in (from data in garmentAvalProductRepository.Query
									   where data.AvalDate <= dateTo
									   select new
									   {
										   data.Identity,
										   data.RONo,
										   data.AvalDate
									   })
							join b in garmentAvalProductItemRepository.Query on a.Identity equals b.APId
							join c in garmentPreparingItemRepository.Query on Guid.Parse(b.PreparingItemId) equals c.Identity
							join d in (from data in garmentPreparingRepository.Query
									   where data.UnitId == request.unit
									   select new
									   {
										   data.Identity,
										   data.RONo
									   }) on c.GarmentPreparingId equals d.Identity
							select new monitoringView
							{
								prepareItemid = c.Identity,
								price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault()),
								expenditure = 0,
								aval = a.AvalDate >= dateFrom ? b.Quantity : 0,
								uomUnit = "",
								stock = a.AvalDate < dateFrom ? -b.Quantity : 0,
								mainFabricExpenditure = 0,
								nonMainFabricExpenditure = 0,
								remark = c.DesignColor,
								receipt = 0,
								productCode = b.ProductCode,
								remainQty = 0
							};

			var QueryDRPrepare = from a in (from data in garmentDeliveryReturnRepository.Query
											where data.ReturnDate <= dateTo && data.UnitId == request.unit
                                            && data.StorageName.Contains("GUDANG BAHAN BAKU")
											select new
											{
												data.RONo,
												data.Identity,
												data.ReturnDate
											})
								 join b in (from bb in garmentDeliveryReturnItemRepository.Query
											where bb.PreparingItemId != "00000000-0000-0000-0000-000000000000"
											select new
											{
												bb.PreparingItemId,
												bb.DRId,
												bb.Quantity,
												bb.ProductCode,
												bb.DesignColor
											}) on a.Identity equals b.DRId
								 select new
								 {
									 a.RONo,
									 b.PreparingItemId,
									 b.Quantity,
									 b.DesignColor,
									 a.ReturnDate,
									 b.ProductCode
								 };

			var QueryDeliveryReturn = from a in QueryDRPrepare
									  join c in garmentPreparingItemRepository.Query
									  on Guid.Parse(a.PreparingItemId) equals (c.Identity)
									  select new monitoringView
									  {
										  prepareItemid = c.Identity,
										  price = Convert.ToDecimal((from aa in sumbasicPrice where aa.RO == a.RONo select aa.Total).FirstOrDefault()),
										  expenditure = a.ReturnDate >= dateFrom ? a.Quantity : 0,
										  aval = 0,
										  uomUnit = "",
										  stock = a.ReturnDate < dateFrom ? -a.Quantity : 0,
										  mainFabricExpenditure = 0,
										  nonMainFabricExpenditure = 0,
										  remark = c.DesignColor,
										  receipt = 0,
										  productCode = a.ProductCode,
										  remainQty = 0
									  };

			var queryNow = (from a in (QueryMutationPrepareItemNow
							.Union(QueryCuttingDONow)
							.Union(QueryAval)
							.Union(QueryDeliveryReturn)
							.AsEnumerable())
						   join b in QueryMutationPrepareItemsROASAL
						   on a.prepareItemid equals b.prepareitemid
						   select new { a, b }).Distinct();


			var querySum = queryNow.GroupBy(x => new { x.a.price, x.b.roasal, x.b.roJob, x.b.article, x.b.buyerCode, x.a.productCode, x.a.remark }, (key, group) => new
			{
				ROAsal = key.roasal,
				ROJob = key.roJob,
				stock = group.Sum(s => s.a.stock),
				ProductCode = key.productCode,
				Article = key.article,
				buyer = key.buyerCode,
				Remark = key.remark,
				Price = key.price,
				mainFabricExpenditure = group.Sum(s => s.a.mainFabricExpenditure),
				nonmainFabricExpenditure = group.Sum(s => s.a.nonMainFabricExpenditure),
				receipt = group.Sum(s => s.a.receipt),
				Aval = group.Sum(s => s.a.aval),
				drQty = group.Sum(s => s.a.expenditure)
			}).Where(s => s.Price > 0).OrderBy(s => s.ROJob);


			GarmentMonitoringPrepareListViewModel garmentMonitoringPrepareListViewModel = new GarmentMonitoringPrepareListViewModel();
			List<GarmentMonitoringPrepareDto> monitoringPrepareDtos = new List<GarmentMonitoringPrepareDto>();
			foreach (var item in querySum)
			{
				GarmentMonitoringPrepareDto garmentMonitoringPrepareDto = new GarmentMonitoringPrepareDto()
				{
					article = item.Article,
					roJob = item.ROJob,
					productCode = item.ProductCode,
					roAsal = item.ROAsal,
					uomUnit = "MT",
					remainQty = Math.Round(item.stock + item.receipt - item.nonmainFabricExpenditure - item.mainFabricExpenditure - item.Aval - item.drQty, 2),
					stock = Math.Round(item.stock, 2),
					remark = item.Remark,
					receipt = Math.Round(item.receipt, 2),
					aval = Math.Round(item.Aval, 2),
					nonMainFabricExpenditure = Math.Round(item.nonmainFabricExpenditure, 2),
					mainFabricExpenditure = Math.Round(item.mainFabricExpenditure, 2),
					expenditure = Math.Round(item.drQty, 2),
					price = Math.Round(item.Price, 2),
					buyerCode = item.buyer,
					nominal = (item.stock + item.receipt - item.nonmainFabricExpenditure - item.mainFabricExpenditure - item.Aval - item.drQty) * Convert.ToDouble(item.Price)

				};
				monitoringPrepareDtos.Add(garmentMonitoringPrepareDto);
			}
			var datas = from aa in monitoringPrepareDtos
						where Math.Round(aa.stock, 2) > 0 || Math.Round(aa.receipt, 2) > 0 || Math.Round(aa.aval, 2) > 0 || Math.Round(aa.mainFabricExpenditure, 2) > 0 || Math.Round(aa.nonMainFabricExpenditure, 2) > 0 || Math.Round(aa.remainQty, 2) > 0
						select aa;
			monitoringPrepareDtos = datas.ToList();
			garmentMonitoringPrepareListViewModel.garmentMonitorings = monitoringPrepareDtos;

			return garmentMonitoringPrepareListViewModel;
		}
	}
}