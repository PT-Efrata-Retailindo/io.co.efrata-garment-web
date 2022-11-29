using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Linq;
using System.Net.Http;
using System.Text;
using Manufactures.Domain.GarmentSample.SampleFinishingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleSewingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingOuts.Repositories;
using Manufactures.Domain.GarmentSample.SampleRequests.Repositories;

namespace Manufactures.Application.GarmentSample.GarmentMonitoringSampleFlows.Queries
{
	public class GetMonitoringSampleFlowQueryHandler : IQueryHandler<GetMonitoringSampleFlowQuery, GarmentMonitoringSampleFlowListViewModel>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentSampleCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentSampleCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentSampleCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentSampleSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSampleSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentSampleSewingOutDetailRepository garmentSewingOutDetailRepository;
		private readonly IGarmentSampleFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentSampleFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentSampleFinishingOutDetailRepository garmentFinishingOutDetailRepository;
		private readonly IGarmentSampleRequestRepository garmentSampleRequestRepository;
		private readonly IGarmentSampleRequestProductRepository garmentSampleRequestProductRepository;

		public GetMonitoringSampleFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentSampleCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentSampleCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentSampleCuttingOutDetailRepository>();
			garmentSewingOutRepository = storage.GetRepository<IGarmentSampleSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSampleSewingOutItemRepository>();
			garmentSewingOutDetailRepository = storage.GetRepository<IGarmentSampleSewingOutDetailRepository>();
			garmentFinishingOutRepository = storage.GetRepository<IGarmentSampleFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentSampleFinishingOutItemRepository>();
			garmentFinishingOutDetailRepository = storage.GetRepository<IGarmentSampleFinishingOutDetailRepository>();
			garmentSampleRequestProductRepository = storage.GetRepository<IGarmentSampleRequestProductRepository>();
			garmentSampleRequestRepository = storage.GetRepository<IGarmentSampleRequestRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
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
		class monitoringView
		{
			public string Ro { get; internal set; }
			public string BuyerCode { get; internal set; }
			public string Article { get; internal set; }
			public string Comodity { get; internal set; }
			public double QtyOrder { get; internal set; }
			public string Size { get; internal set; }
			public double QtyCutting { get; internal set; }
			public double QtyLoading { get; internal set; }
			public double QtySewing { get; internal set; }
			public double QtyFinishing { get; internal set; }
			public double Wip { get; internal set; }

		}
		public async Task<GarmentMonitoringSampleFlowListViewModel> Handle(GetMonitoringSampleFlowQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset date = new DateTimeOffset(request.date );
			date.AddHours(7);
			 
			var QueryRo = (from a in garmentCuttingOutRepository.Query
									 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
									 where a.UnitFromId == request.unit && a.CuttingOutDate <= date 
									 select a.RONo).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);
			
			var QueryCuttingOut = (from a in garmentCuttingOutRepository.Query
									 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CuttingOutId
									 join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CuttingOutItemId
									 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.CuttingOutDate <= date   
								   select new  monitoringView{ Ro = a.RONo,Article= a.Article, Comodity= a.ComodityName,BuyerCode= (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(),QtyOrder= (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(),QtyCutting= c.CuttingOutQuantity,Size= c.SizeName });
			
			var QuerySewingOutIsDifSize = from a in garmentSewingOutRepository.Query
									join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SampleSewingOutId
									join c in garmentSewingOutDetailRepository.Query on b.Identity equals c.SampleSewingOutItemId
										  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.SewingTo == "FINISHING" &&  a.UnitId == request.unit && a.SewingOutDate <= date
								    select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewing = c.Quantity, Size =c.SizeName };
			var QuerySewingOut = from a in garmentSewingOutRepository.Query
								   join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SampleSewingOutId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.SewingTo == "FINISHING" && a.UnitId == request.unit && a.SewingOutDate <= date && a.IsDifferentSize == false
								 select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtySewing = b.Quantity, Size = b.SizeName };
			var QueryFinishingOutisDifSize = from a in garmentFinishingOutRepository.Query
								 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
								 join c in garmentFinishingOutDetailRepository.Query on b.Identity equals c.FinishingOutItemId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.FinishingTo== "GUDANG JADI" && a.UnitId == request.unit && a.FinishingOutDate <= date
									select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyFinishing = c.Quantity, Size = c.SizeName };
			var QueryFinishingOut = from a in garmentFinishingOutRepository.Query
									join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.FinishingTo == "GUDANG JADI" && a.UnitId == request.unit && a.FinishingOutDate <= date && a.IsDifferentSize == false
									select new monitoringView { Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), QtyFinishing = b.Quantity, Size = b.SizeName };


			var queryNow = QueryCuttingOut.Union(QuerySewingOutIsDifSize).Union(QuerySewingOut).Union(QueryFinishingOut).Union(QueryFinishingOutisDifSize).AsEnumerable();

			var querySum = queryNow.GroupBy(x => new { x.Size,x.Ro, x.Article, x.BuyerCode, x.Comodity,x.QtyOrder }, (key, group) => new
			{
				ro = key.Ro,
				article = key.Article,
				buyer= key.BuyerCode,
				comodity= key.Comodity,
				qtyOrder= key.QtyOrder,
				qtycutting = group.Sum(s => s.QtyCutting),
				qtySewing = group.Sum(s => s.QtySewing),
				qtyLoading= group.Sum(s => s.QtyLoading),
				qtyFinishing= group.Sum(s => s.QtyFinishing),
				size= key.Size,
			});
			var querySumTotal = queryNow.GroupBy(x => new {  x.Ro, x.Article, x.BuyerCode, x.Comodity, x.QtyOrder }, (key, group) => new
			{
				ro = key.Ro,
				article = key.Article,
				buyer = key.BuyerCode,
				comodity = key.Comodity,
				qtyOrder = key.QtyOrder,
				qtycutting = group.Sum(s => s.QtyCutting),
				qtySewing = group.Sum(s => s.QtySewing),
				qtyLoading = group.Sum(s => s.QtyLoading),
				qtyFinishing = group.Sum(s => s.QtyFinishing),
				size="TOTAL"
			});

			var query = querySum.Union(querySumTotal).OrderBy(s => s.ro);
			GarmentMonitoringSampleFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringSampleFlowListViewModel();
			List<GarmentMonitoringSampleFlowDto> monitoringDtos = new List<GarmentMonitoringSampleFlowDto>();

			var sampleRequest = (from a in garmentSampleRequestRepository.Query
								 join b in garmentSampleRequestProductRepository.Query
								 on a.Identity equals b.SampleRequestId select new { a.RONoSample, a.BuyerCode, b.Quantity })
							   .GroupBy(x => new { x.RONoSample, x.BuyerCode }, (key, group) => new
							   {
								   ro = key.RONoSample,
								   qtyOrder = group.Sum(s=>s.Quantity),
								   buyer = key.BuyerCode

							   });
			foreach (var item in query)
			{
				GarmentMonitoringSampleFlowDto garmentMonitoringDto = new GarmentMonitoringSampleFlowDto()
				{
					Article = item.article,
					Ro = item.ro,
					BuyerCode = (from a in sampleRequest where a.ro == item.ro select a.buyer).FirstOrDefault(),
					QtyOrder = (from a in sampleRequest where a.ro == item.ro  select a.qtyOrder).FirstOrDefault(),
					QtyCutting = item.qtycutting,
					QtySewing = item.qtySewing,
					QtyFinishing = item.qtyFinishing,
					QtyLoading = item.qtyLoading,
					Size = item.size,
					Comodity = item.comodity,
					Wip = item.qtycutting - item.qtyFinishing
				};
				monitoringDtos.Add(garmentMonitoringDto);
			}
			garmentMonitoringProductionFlow.garmentMonitorings = monitoringDtos;

			return garmentMonitoringProductionFlow;
		}
	}
}
